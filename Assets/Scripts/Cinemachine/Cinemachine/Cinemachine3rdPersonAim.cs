using UnityEngine;

namespace Cinemachine
{
	[AddComponentMenu("")]
	[ExecuteAlways]
	[SaveDuringPlay]
	[DisallowMultipleComponent]
	public class Cinemachine3rdPersonAim : CinemachineExtension
	{
		[Header("Aim Target Detection")]
		[Tooltip("Objects on these layers will be detected")]
		public LayerMask AimCollisionFilter;

		[TagField]
		[Tooltip("Objects with this tag will be ignored.  It is a good idea to set this field to the target's tag")]
		public string IgnoreTag = string.Empty;

		[Tooltip("How far to project the object detection ray")]
		public float AimDistance;

		[Tooltip("This 2D object will be positioned in the game view over the raycast hit point, if any, or will remain in the center of the screen if no hit point is detected.  May be null, in which case no on-screen indicator will appear")]
		public RectTransform AimTargetReticle;

		private Vector3 AimTarget { get; set; }

		private void OnValidate()
		{
			AimDistance = Mathf.Max(1f, AimDistance);
		}

		private void Reset()
		{
			AimCollisionFilter = 1;
			IgnoreTag = string.Empty;
			AimDistance = 200f;
			AimTargetReticle = null;
		}

		public override bool OnTransitionFromCamera(ICinemachineCamera fromCam, Vector3 worldUp, float deltaTime)
		{
			CinemachineCore.CameraUpdatedEvent.RemoveListener(DrawReticle);
			CinemachineCore.CameraUpdatedEvent.AddListener(DrawReticle);
			return false;
		}

		private void DrawReticle(CinemachineBrain brain)
		{
			if (!brain.IsLive(base.VirtualCamera) || brain.OutputCamera == null)
			{
				CinemachineCore.CameraUpdatedEvent.RemoveListener(DrawReticle);
			}
			else if (AimTargetReticle != null)
			{
				AimTargetReticle.position = brain.OutputCamera.WorldToScreenPoint(AimTarget);
			}
		}

		private Vector3 ComputeLookAtPoint(Vector3 camPos, Transform player)
		{
			float num = AimDistance;
			Quaternion rotation = player.rotation;
			Vector3 vector = rotation * Vector3.forward;
			Vector3 vector2 = Quaternion.Inverse(rotation) * (player.position - camPos);
			if (vector2.z > 0f)
			{
				camPos += vector * vector2.z;
				num -= vector2.z;
			}
			num = Mathf.Max(1f, num);
			RaycastHit hitInfo;
			if (!RuntimeUtility.RaycastIgnoreTag(new Ray(camPos, vector), out hitInfo, num, AimCollisionFilter, ref IgnoreTag))
			{
				return camPos + vector * num;
			}
			return hitInfo.point;
		}

		private Vector3 ComputeAimTarget(Vector3 cameraLookAt, Transform player)
		{
			Vector3 position = player.position;
			Vector3 direction = cameraLookAt - position;
			RaycastHit hitInfo;
			if (RuntimeUtility.RaycastIgnoreTag(new Ray(position, direction), out hitInfo, direction.magnitude, AimCollisionFilter, ref IgnoreTag))
			{
				return hitInfo.point;
			}
			return cameraLookAt;
		}

		protected override void PostPipelineStageCallback(CinemachineVirtualCameraBase vcam, CinemachineCore.Stage stage, ref CameraState state, float deltaTime)
		{
			if (stage == CinemachineCore.Stage.Body)
			{
				Transform follow = vcam.Follow;
				if (follow != null)
				{
					state.ReferenceLookAt = ComputeLookAtPoint(state.CorrectedPosition, follow);
					AimTarget = ComputeAimTarget(state.ReferenceLookAt, follow);
				}
			}
			if (stage == CinemachineCore.Stage.Finalize)
			{
				Vector3 forward = state.ReferenceLookAt - state.FinalPosition;
				if (forward.sqrMagnitude > 0.01f)
				{
					state.RawOrientation = Quaternion.LookRotation(forward, state.ReferenceUp);
					state.OrientationCorrection = Quaternion.identity;
				}
			}
		}
	}
}
