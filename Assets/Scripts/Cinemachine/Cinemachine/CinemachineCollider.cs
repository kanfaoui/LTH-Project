using System;
using System.Collections.Generic;
using Cinemachine.Utility;
using UnityEngine;
using UnityEngine.Serialization;

namespace Cinemachine
{
	[DocumentationSorting(DocumentationSortingAttribute.Level.UserRef)]
	[AddComponentMenu("")]
	[SaveDuringPlay]
	[ExecuteAlways]
	[DisallowMultipleComponent]
	[HelpURL("https://docs.unity3d.com/Packages/com.unity.cinemachine@2.8/manual/CinemachineCollider.html")]
	public class CinemachineCollider : CinemachineExtension
	{
		public enum ResolutionStrategy
		{
			PullCameraForward = 0,
			PreserveCameraHeight = 1,
			PreserveCameraDistance = 2
		}

		private class VcamExtraState
		{
			public Vector3 previousDisplacement;

			public bool targetObscured;

			public float occlusionStartTime;

			public List<Vector3> debugResolutionPath;

			private float m_SmoothedDistance;

			private float m_SmoothedTime;

			public void AddPointToDebugPath(Vector3 p)
			{
			}

			public float ApplyDistanceSmoothing(float distance, float smoothingTime)
			{
				if (m_SmoothedTime != 0f && smoothingTime > 0.0001f && CinemachineCore.CurrentTime - m_SmoothedTime < smoothingTime)
				{
					return Mathf.Min(distance, m_SmoothedDistance);
				}
				return distance;
			}

			public void UpdateDistanceSmoothing(float distance)
			{
				float currentTime = CinemachineCore.CurrentTime;
				if (m_SmoothedDistance == 0f || distance <= m_SmoothedDistance)
				{
					m_SmoothedDistance = distance;
					m_SmoothedTime = currentTime;
				}
			}

			public void ResetDistanceSmoothing(float smoothingTime)
			{
				if (CinemachineCore.CurrentTime - m_SmoothedTime >= smoothingTime)
				{
					m_SmoothedDistance = (m_SmoothedTime = 0f);
				}
			}
		}

		[Header("Obstacle Detection")]
		[Tooltip("Objects on these layers will be detected")]
		public LayerMask m_CollideAgainst = 1;

		[TagField]
		[Tooltip("Obstacles with this tag will be ignored.  It is a good idea to set this field to the target's tag")]
		public string m_IgnoreTag = string.Empty;

		[Tooltip("Objects on these layers will never obstruct view of the target")]
		public LayerMask m_TransparentLayers = 0;

		[Tooltip("Obstacles closer to the target than this will be ignored")]
		public float m_MinimumDistanceFromTarget = 0.1f;

		[Space]
		[Tooltip("When enabled, will attempt to resolve situations where the line of sight to the target is blocked by an obstacle")]
		[FormerlySerializedAs("m_PreserveLineOfSight")]
		public bool m_AvoidObstacles = true;

		[Tooltip("The maximum raycast distance when checking if the line of sight to this camera's target is clear.  If the setting is 0 or less, the current actual distance to target will be used.")]
		[FormerlySerializedAs("m_LineOfSightFeelerDistance")]
		public float m_DistanceLimit;

		[Tooltip("Don't take action unless occlusion has lasted at least this long.")]
		public float m_MinimumOcclusionTime;

		[Tooltip("Camera will try to maintain this distance from any obstacle.  Try to keep this value small.  Increase it if you are seeing inside obstacles due to a large FOV on the camera.")]
		public float m_CameraRadius = 0.1f;

		[Tooltip("The way in which the Collider will attempt to preserve sight of the target.")]
		public ResolutionStrategy m_Strategy = ResolutionStrategy.PreserveCameraHeight;

		[Range(1f, 10f)]
		[Tooltip("Upper limit on how many obstacle hits to process.  Higher numbers may impact performance.  In most environments, 4 is enough.")]
		public int m_MaximumEffort = 4;

		[Range(0f, 2f)]
		[Tooltip("Smoothing to apply to obstruction resolution.  Nearest camera point is held for at least this long")]
		public float m_SmoothingTime;

		[Range(0f, 10f)]
		[Tooltip("How gradually the camera returns to its normal position after having been corrected.  Higher numbers will move the camera more gradually back to normal.")]
		[FormerlySerializedAs("m_Smoothing")]
		public float m_Damping;

		[Range(0f, 10f)]
		[Tooltip("How gradually the camera moves to resolve an occlusion.  Higher numbers will move the camera more gradually.")]
		public float m_DampingWhenOccluded;

		[Header("Shot Evaluation")]
		[Tooltip("If greater than zero, a higher score will be given to shots when the target is closer to this distance.  Set this to zero to disable this feature.")]
		public float m_OptimalTargetDistance;

		private const float k_PrecisionSlush = 0.001f;

		private RaycastHit[] m_CornerBuffer = new RaycastHit[4];

		private const float k_AngleThreshold = 0.1f;

		private static Collider[] s_ColliderBuffer = new Collider[5];

		public List<List<Vector3>> DebugPaths
		{
			get
			{
				List<List<Vector3>> list = new List<List<Vector3>>();
				foreach (VcamExtraState allExtraState in GetAllExtraStates<VcamExtraState>())
				{
					if (allExtraState.debugResolutionPath != null && allExtraState.debugResolutionPath.Count > 0)
					{
						list.Add(allExtraState.debugResolutionPath);
					}
				}
				return list;
			}
		}

		public bool IsTargetObscured(ICinemachineCamera vcam)
		{
			return GetExtraState<VcamExtraState>(vcam).targetObscured;
		}

		public bool CameraWasDisplaced(ICinemachineCamera vcam)
		{
			return GetCameraDisplacementDistance(vcam) > 0f;
		}

		public float GetCameraDisplacementDistance(ICinemachineCamera vcam)
		{
			return GetExtraState<VcamExtraState>(vcam).previousDisplacement.magnitude;
		}

		private void OnValidate()
		{
			m_DistanceLimit = Mathf.Max(0f, m_DistanceLimit);
			m_MinimumOcclusionTime = Mathf.Max(0f, m_MinimumOcclusionTime);
			m_CameraRadius = Mathf.Max(0f, m_CameraRadius);
			m_MinimumDistanceFromTarget = Mathf.Max(0.01f, m_MinimumDistanceFromTarget);
			m_OptimalTargetDistance = Mathf.Max(0f, m_OptimalTargetDistance);
		}

		protected override void OnDestroy()
		{
			RuntimeUtility.DestroyScratchCollider();
			base.OnDestroy();
		}

		public override float GetMaxDampTime()
		{
			return Mathf.Max(m_Damping, Mathf.Max(m_DampingWhenOccluded, m_SmoothingTime));
		}

		protected override void PostPipelineStageCallback(CinemachineVirtualCameraBase vcam, CinemachineCore.Stage stage, ref CameraState state, float deltaTime)
		{
			if (stage == CinemachineCore.Stage.Body)
			{
				VcamExtraState extra = GetExtraState<VcamExtraState>(vcam);
				extra.targetObscured = false;
				List<Vector3> debugResolutionPath = extra.debugResolutionPath;
				if (debugResolutionPath != null)
				{
					debugResolutionPath.RemoveRange(0, extra.debugResolutionPath.Count);
				}
				if (m_AvoidObstacles)
				{
					extra.previousDisplacement = Quaternion.Euler(state.PositionDampingBypass) * extra.previousDisplacement;
					Vector3 vector = PreserveLineOfSight(ref state, ref extra);
					if (m_MinimumOcclusionTime > 0.0001f)
					{
						float currentTime = CinemachineCore.CurrentTime;
						if (vector.AlmostZero())
						{
							extra.occlusionStartTime = 0f;
						}
						else
						{
							if (extra.occlusionStartTime <= 0f)
							{
								extra.occlusionStartTime = currentTime;
							}
							if (currentTime - extra.occlusionStartTime < m_MinimumOcclusionTime)
							{
								vector = extra.previousDisplacement;
							}
						}
					}
					if (m_SmoothingTime > 0.0001f)
					{
						Vector3 vector2 = state.CorrectedPosition + vector;
						Vector3 vector3 = vector2 - state.ReferenceLookAt;
						float magnitude = vector3.magnitude;
						if (magnitude > 0.0001f)
						{
							vector3 /= magnitude;
							if (!vector.AlmostZero())
							{
								extra.UpdateDistanceSmoothing(magnitude);
							}
							magnitude = extra.ApplyDistanceSmoothing(magnitude, m_SmoothingTime);
							vector += state.ReferenceLookAt + vector3 * magnitude - vector2;
						}
					}
					if (vector.AlmostZero())
					{
						extra.ResetDistanceSmoothing(m_SmoothingTime);
					}
					Vector3 vector4 = state.CorrectedPosition + vector;
					vector += RespectCameraRadius(vector4, state.HasLookAt ? state.ReferenceLookAt : vector4);
					if (deltaTime >= 0f && base.VirtualCamera.PreviousStateIsValid)
					{
						vector = extra.previousDisplacement + Damper.Damp(vector - extra.previousDisplacement, (vector.sqrMagnitude > extra.previousDisplacement.sqrMagnitude) ? m_DampingWhenOccluded : m_Damping, deltaTime);
					}
					extra.previousDisplacement = vector;
					state.PositionCorrection += vector;
				}
			}
			if (stage != CinemachineCore.Stage.Aim)
			{
				return;
			}
			VcamExtraState extraState = GetExtraState<VcamExtraState>(vcam);
			extraState.targetObscured = IsTargetOffscreen(state) || CheckForTargetObstructions(state);
			if (extraState.targetObscured)
			{
				state.ShotQuality *= 0.2f;
			}
			if (!extraState.previousDisplacement.AlmostZero())
			{
				state.ShotQuality *= 0.8f;
			}
			float num = 0f;
			if (!(m_OptimalTargetDistance > 0f) || !state.HasLookAt)
			{
				return;
			}
			float num2 = Vector3.Magnitude(state.ReferenceLookAt - state.FinalPosition);
			if (num2 <= m_OptimalTargetDistance)
			{
				float num3 = m_OptimalTargetDistance / 2f;
				if (num2 >= num3)
				{
					num = 0.2f * (num2 - num3) / (m_OptimalTargetDistance - num3);
				}
			}
			else
			{
				num2 -= m_OptimalTargetDistance;
				float num4 = m_OptimalTargetDistance * 3f;
				if (num2 < num4)
				{
					num = 0.2f * (1f - num2 / num4);
				}
			}
			state.ShotQuality *= 1f + num;
		}

		private Vector3 PreserveLineOfSight(ref CameraState state, ref VcamExtraState extra)
		{
			Vector3 result = Vector3.zero;
			if (state.HasLookAt && (int)m_CollideAgainst != 0 && (int)m_CollideAgainst != (int)m_TransparentLayers)
			{
				Vector3 correctedPosition = state.CorrectedPosition;
				Vector3 referenceLookAt = state.ReferenceLookAt;
				RaycastHit hitInfo = default(RaycastHit);
				result = PullCameraInFrontOfNearestObstacle(correctedPosition, referenceLookAt, (int)m_CollideAgainst & ~(int)m_TransparentLayers, ref hitInfo);
				Vector3 vector = correctedPosition + result;
				if (hitInfo.collider != null)
				{
					extra.AddPointToDebugPath(vector);
					if (m_Strategy != 0)
					{
						Vector3 pushDir = correctedPosition - referenceLookAt;
						vector = PushCameraBack(vector, pushDir, hitInfo, referenceLookAt, new Plane(state.ReferenceUp, correctedPosition), pushDir.magnitude, m_MaximumEffort, ref extra);
					}
				}
				result = vector - correctedPosition;
			}
			return result;
		}

		private Vector3 PullCameraInFrontOfNearestObstacle(Vector3 cameraPos, Vector3 lookAtPos, int layerMask, ref RaycastHit hitInfo)
		{
			Vector3 result = Vector3.zero;
			Vector3 vector = cameraPos - lookAtPos;
			float magnitude = vector.magnitude;
			if (magnitude > 0.0001f)
			{
				vector /= magnitude;
				float num = Mathf.Max(m_MinimumDistanceFromTarget, 0.0001f);
				if (magnitude < num + 0.0001f)
				{
					result = vector * (num - magnitude);
				}
				else
				{
					float num2 = magnitude - num;
					if (m_DistanceLimit > 0.0001f)
					{
						num2 = Mathf.Min(m_DistanceLimit, num2);
					}
					Ray ray = new Ray(cameraPos - num2 * vector, vector);
					num2 += 0.001f;
					if (num2 > 0.0001f && RuntimeUtility.RaycastIgnoreTag(ray, out hitInfo, num2, layerMask, ref m_IgnoreTag))
					{
						float distance = Mathf.Max(0f, hitInfo.distance - 0.001f);
						result = ray.GetPoint(distance) - cameraPos;
					}
				}
			}
			return result;
		}

		private Vector3 PushCameraBack(Vector3 currentPos, Vector3 pushDir, RaycastHit obstacle, Vector3 lookAtPos, Plane startPlane, float targetDistance, int iterations, ref VcamExtraState extra)
		{
			Vector3 vector = currentPos;
			Vector3 outDir = Vector3.zero;
			if (!GetWalkingDirection(vector, pushDir, obstacle, ref outDir))
			{
				return vector;
			}
			Ray ray = new Ray(vector, outDir);
			float pushBackDistance = GetPushBackDistance(ray, startPlane, targetDistance, lookAtPos);
			if (pushBackDistance <= 0.0001f)
			{
				return vector;
			}
			float num = ClampRayToBounds(ray, pushBackDistance, obstacle.collider.bounds);
			pushBackDistance = Mathf.Min(pushBackDistance, num + 0.001f);
			RaycastHit hitInfo;
			if (RuntimeUtility.RaycastIgnoreTag(ray, out hitInfo, pushBackDistance, (int)m_CollideAgainst & ~(int)m_TransparentLayers, ref m_IgnoreTag))
			{
				float distance = hitInfo.distance - 0.001f;
				vector = ray.GetPoint(distance);
				extra.AddPointToDebugPath(vector);
				if (iterations > 1)
				{
					vector = PushCameraBack(vector, outDir, hitInfo, lookAtPos, startPlane, targetDistance, iterations - 1, ref extra);
				}
				return vector;
			}
			vector = ray.GetPoint(pushBackDistance);
			outDir = vector - lookAtPos;
			float magnitude = outDir.magnitude;
			RaycastHit hitInfo2;
			if (magnitude < 0.0001f || RuntimeUtility.RaycastIgnoreTag(new Ray(lookAtPos, outDir), out hitInfo2, magnitude - 0.001f, (int)m_CollideAgainst & ~(int)m_TransparentLayers, ref m_IgnoreTag))
			{
				return currentPos;
			}
			ray = new Ray(vector, outDir);
			extra.AddPointToDebugPath(vector);
			pushBackDistance = GetPushBackDistance(ray, startPlane, targetDistance, lookAtPos);
			if (pushBackDistance > 0.0001f)
			{
				if (!RuntimeUtility.RaycastIgnoreTag(ray, out hitInfo, pushBackDistance, (int)m_CollideAgainst & ~(int)m_TransparentLayers, ref m_IgnoreTag))
				{
					vector = ray.GetPoint(pushBackDistance);
					extra.AddPointToDebugPath(vector);
				}
				else
				{
					float distance2 = hitInfo.distance - 0.001f;
					vector = ray.GetPoint(distance2);
					extra.AddPointToDebugPath(vector);
					if (iterations > 1)
					{
						vector = PushCameraBack(vector, outDir, hitInfo, lookAtPos, startPlane, targetDistance, iterations - 1, ref extra);
					}
				}
			}
			return vector;
		}

		private bool GetWalkingDirection(Vector3 pos, Vector3 pushDir, RaycastHit obstacle, ref Vector3 outDir)
		{
			Vector3 normal = obstacle.normal;
			float num = 0.0050000004f;
			int num2 = Physics.SphereCastNonAlloc(pos, num, pushDir.normalized, m_CornerBuffer, 0f, (int)m_CollideAgainst & ~(int)m_TransparentLayers, QueryTriggerInteraction.Ignore);
			if (num2 > 1)
			{
				for (int i = 0; i < num2; i++)
				{
					if (m_CornerBuffer[i].collider == null || (m_IgnoreTag.Length > 0 && m_CornerBuffer[i].collider.CompareTag(m_IgnoreTag)))
					{
						continue;
					}
					Type type = m_CornerBuffer[i].collider.GetType();
					if (!(type == typeof(BoxCollider)) && !(type == typeof(SphereCollider)) && !(type == typeof(CapsuleCollider)))
					{
						continue;
					}
					Vector3 direction = m_CornerBuffer[i].collider.ClosestPoint(pos) - pos;
					if (direction.magnitude > 1E-05f && m_CornerBuffer[i].collider.Raycast(new Ray(pos, direction), out m_CornerBuffer[i], num))
					{
						if (!(m_CornerBuffer[i].normal - obstacle.normal).AlmostZero())
						{
							normal = m_CornerBuffer[i].normal;
						}
						break;
					}
				}
			}
			Vector3 vector = Vector3.Cross(obstacle.normal, normal);
			if (vector.AlmostZero())
			{
				vector = Vector3.ProjectOnPlane(pushDir, obstacle.normal);
			}
			else
			{
				float num3 = Vector3.Dot(vector, pushDir);
				if (Mathf.Abs(num3) < 0.0001f)
				{
					return false;
				}
				if (num3 < 0f)
				{
					vector = -vector;
				}
			}
			if (vector.AlmostZero())
			{
				return false;
			}
			outDir = vector.normalized;
			return true;
		}

		private float GetPushBackDistance(Ray ray, Plane startPlane, float targetDistance, Vector3 lookAtPos)
		{
			float num = targetDistance - (ray.origin - lookAtPos).magnitude;
			if (num < 0.0001f)
			{
				return 0f;
			}
			if (m_Strategy == ResolutionStrategy.PreserveCameraDistance)
			{
				return num;
			}
			float enter;
			if (!startPlane.Raycast(ray, out enter))
			{
				enter = 0f;
			}
			enter = Mathf.Min(num, enter);
			if (enter < 0.0001f)
			{
				return 0f;
			}
			float num2 = Mathf.Abs(UnityVectorExtensions.Angle(startPlane.normal, ray.direction) - 90f);
			if (num2 < 0.1f)
			{
				enter = Mathf.Lerp(0f, enter, num2 / 0.1f);
			}
			return enter;
		}

		private static float ClampRayToBounds(Ray ray, float distance, Bounds bounds)
		{
			float enter;
			if (Vector3.Dot(ray.direction, Vector3.up) > 0f)
			{
				if (new Plane(Vector3.down, bounds.max).Raycast(ray, out enter) && enter > 0.0001f)
				{
					distance = Mathf.Min(distance, enter);
				}
			}
			else if (Vector3.Dot(ray.direction, Vector3.down) > 0f && new Plane(Vector3.up, bounds.min).Raycast(ray, out enter) && enter > 0.0001f)
			{
				distance = Mathf.Min(distance, enter);
			}
			if (Vector3.Dot(ray.direction, Vector3.right) > 0f)
			{
				if (new Plane(Vector3.left, bounds.max).Raycast(ray, out enter) && enter > 0.0001f)
				{
					distance = Mathf.Min(distance, enter);
				}
			}
			else if (Vector3.Dot(ray.direction, Vector3.left) > 0f && new Plane(Vector3.right, bounds.min).Raycast(ray, out enter) && enter > 0.0001f)
			{
				distance = Mathf.Min(distance, enter);
			}
			if (Vector3.Dot(ray.direction, Vector3.forward) > 0f)
			{
				if (new Plane(Vector3.back, bounds.max).Raycast(ray, out enter) && enter > 0.0001f)
				{
					distance = Mathf.Min(distance, enter);
				}
			}
			else if (Vector3.Dot(ray.direction, Vector3.back) > 0f && new Plane(Vector3.forward, bounds.min).Raycast(ray, out enter) && enter > 0.0001f)
			{
				distance = Mathf.Min(distance, enter);
			}
			return distance;
		}

		private Vector3 RespectCameraRadius(Vector3 cameraPos, Vector3 lookAtPos)
		{
			Vector3 vector = Vector3.zero;
			if (m_CameraRadius < 0.0001f || (int)m_CollideAgainst == 0)
			{
				return vector;
			}
			Vector3 vector2 = cameraPos - lookAtPos;
			float magnitude = vector2.magnitude;
			if (magnitude > 0.0001f)
			{
				vector2 /= magnitude;
			}
			int num = Physics.OverlapSphereNonAlloc(cameraPos, m_CameraRadius, s_ColliderBuffer, m_CollideAgainst, QueryTriggerInteraction.Ignore);
			RaycastHit hitInfo;
			if (num == 0 && (int)m_TransparentLayers != 0 && magnitude > m_MinimumDistanceFromTarget + 0.0001f)
			{
				float num2 = magnitude - m_MinimumDistanceFromTarget;
				if (RuntimeUtility.RaycastIgnoreTag(new Ray(lookAtPos + vector2 * m_MinimumDistanceFromTarget, vector2), out hitInfo, num2, m_CollideAgainst, ref m_IgnoreTag))
				{
					Collider collider = hitInfo.collider;
					if (!collider.Raycast(new Ray(cameraPos, -vector2), out hitInfo, num2))
					{
						s_ColliderBuffer[num++] = collider;
					}
				}
			}
			if ((num > 0 && magnitude == 0f) || magnitude > m_MinimumDistanceFromTarget)
			{
				SphereCollider scratchCollider = RuntimeUtility.GetScratchCollider();
				scratchCollider.radius = m_CameraRadius;
				Vector3 vector3 = cameraPos;
				for (int i = 0; i < num; i++)
				{
					Collider collider2 = s_ColliderBuffer[i];
					if (m_IgnoreTag.Length > 0 && collider2.CompareTag(m_IgnoreTag))
					{
						continue;
					}
					if (magnitude > m_MinimumDistanceFromTarget)
					{
						vector2 = vector3 - lookAtPos;
						float magnitude2 = vector2.magnitude;
						if (magnitude2 > 0.0001f)
						{
							vector2 /= magnitude2;
							Ray ray = new Ray(lookAtPos, vector2);
							if (collider2.Raycast(ray, out hitInfo, magnitude2 + m_CameraRadius))
							{
								vector3 = ray.GetPoint(hitInfo.distance) - vector2 * 0.001f;
							}
						}
					}
					Vector3 direction;
					float distance;
					if (Physics.ComputePenetration(scratchCollider, vector3, Quaternion.identity, collider2, collider2.transform.position, collider2.transform.rotation, out direction, out distance))
					{
						vector3 += direction * distance;
					}
				}
				vector = vector3 - cameraPos;
			}
			if (magnitude > 0.0001f && m_MinimumDistanceFromTarget > 0.0001f)
			{
				float num3 = Mathf.Max(m_MinimumDistanceFromTarget, m_CameraRadius) + 0.001f;
				if ((cameraPos + vector - lookAtPos).magnitude < num3)
				{
					vector = lookAtPos - cameraPos + vector2 * num3;
				}
			}
			return vector;
		}

		private bool CheckForTargetObstructions(CameraState state)
		{
			if (state.HasLookAt)
			{
				Vector3 referenceLookAt = state.ReferenceLookAt;
				Vector3 correctedPosition = state.CorrectedPosition;
				Vector3 vector = referenceLookAt - correctedPosition;
				float magnitude = vector.magnitude;
				if (magnitude < Mathf.Max(m_MinimumDistanceFromTarget, 0.0001f))
				{
					return true;
				}
				RaycastHit hitInfo;
				if (RuntimeUtility.RaycastIgnoreTag(new Ray(correctedPosition, vector.normalized), out hitInfo, magnitude - m_MinimumDistanceFromTarget, (int)m_CollideAgainst & ~(int)m_TransparentLayers, ref m_IgnoreTag))
				{
					return true;
				}
			}
			return false;
		}

		private static bool IsTargetOffscreen(CameraState state)
		{
			if (state.HasLookAt)
			{
				Vector3 vector = state.ReferenceLookAt - state.CorrectedPosition;
				vector = Quaternion.Inverse(state.CorrectedOrientation) * vector;
				if (state.Lens.Orthographic)
				{
					if (Mathf.Abs(vector.y) > state.Lens.OrthographicSize)
					{
						return true;
					}
					if (Mathf.Abs(vector.x) > state.Lens.OrthographicSize * state.Lens.Aspect)
					{
						return true;
					}
				}
				else
				{
					float num = state.Lens.FieldOfView / 2f;
					if (UnityVectorExtensions.Angle(vector.ProjectOntoPlane(Vector3.right), Vector3.forward) > num)
					{
						return true;
					}
					num = 57.29578f * Mathf.Atan(Mathf.Tan(num * (MathF.PI / 180f)) * state.Lens.Aspect);
					if (UnityVectorExtensions.Angle(vector.ProjectOntoPlane(Vector3.up), Vector3.forward) > num)
					{
						return true;
					}
				}
			}
			return false;
		}
	}
}
