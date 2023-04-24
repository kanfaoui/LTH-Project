using Cinemachine.Utility;
using UnityEngine;

namespace Cinemachine
{
	[DocumentationSorting(DocumentationSortingAttribute.Level.UserRef)]
	[AddComponentMenu("")]
	[SaveDuringPlay]
	public class CinemachinePOV : CinemachineComponentBase
	{
		public enum RecenterTargetMode
		{
			None = 0,
			FollowTargetForward = 1,
			LookAtTargetForward = 2
		}

		public RecenterTargetMode m_RecenterTarget;

		[Tooltip("The Vertical axis.  Value is -90..90. Controls the vertical orientation")]
		[AxisStateProperty]
		public AxisState m_VerticalAxis = new AxisState(-70f, 70f, false, false, 300f, 0.1f, 0.1f, "Mouse Y", true);

		[Tooltip("Controls how automatic recentering of the Vertical axis is accomplished")]
		public AxisState.Recentering m_VerticalRecentering = new AxisState.Recentering(false, 1f, 2f);

		[Tooltip("The Horizontal axis.  Value is -180..180.  Controls the horizontal orientation")]
		[AxisStateProperty]
		public AxisState m_HorizontalAxis = new AxisState(-180f, 180f, true, false, 300f, 0.1f, 0.1f, "Mouse X", false);

		[Tooltip("Controls how automatic recentering of the Horizontal axis is accomplished")]
		public AxisState.Recentering m_HorizontalRecentering = new AxisState.Recentering(false, 1f, 2f);

		[HideInInspector]
		[Tooltip("Obsolete - no longer used")]
		public bool m_ApplyBeforeBody;

		public override bool IsValid
		{
			get
			{
				return base.enabled;
			}
		}

		public override CinemachineCore.Stage Stage
		{
			get
			{
				return CinemachineCore.Stage.Aim;
			}
		}

		public override bool RequiresUserInput
		{
			get
			{
				return true;
			}
		}

		private void OnValidate()
		{
			m_VerticalAxis.Validate();
			m_VerticalRecentering.Validate();
			m_HorizontalAxis.Validate();
			m_HorizontalRecentering.Validate();
		}

		private void OnEnable()
		{
			UpdateInputAxisProvider();
		}

		public void UpdateInputAxisProvider()
		{
			m_HorizontalAxis.SetInputAxisProvider(0, null);
			m_VerticalAxis.SetInputAxisProvider(1, null);
			if (base.VirtualCamera != null)
			{
				AxisState.IInputAxisProvider inputAxisProvider = base.VirtualCamera.GetInputAxisProvider();
				if (inputAxisProvider != null)
				{
					m_HorizontalAxis.SetInputAxisProvider(0, inputAxisProvider);
					m_VerticalAxis.SetInputAxisProvider(1, inputAxisProvider);
				}
			}
		}

		public override void PrePipelineMutateCameraState(ref CameraState state, float deltaTime)
		{
		}

		public override void MutateCameraState(ref CameraState curState, float deltaTime)
		{
			if (!IsValid)
			{
				return;
			}
			if (deltaTime >= 0f && CinemachineCore.Instance.IsLive(base.VirtualCamera))
			{
				if (m_HorizontalAxis.Update(deltaTime))
				{
					m_HorizontalRecentering.CancelRecentering();
				}
				if (m_VerticalAxis.Update(deltaTime))
				{
					m_VerticalRecentering.CancelRecentering();
				}
				Vector2 recenterTarget = GetRecenterTarget();
				m_HorizontalRecentering.DoRecentering(ref m_HorizontalAxis, deltaTime, recenterTarget.x);
				m_VerticalRecentering.DoRecentering(ref m_VerticalAxis, deltaTime, recenterTarget.y);
			}
			Quaternion quaternion = Quaternion.Euler(m_VerticalAxis.Value, m_HorizontalAxis.Value, 0f);
			Transform parent = base.VirtualCamera.transform.parent;
			if (parent != null)
			{
				quaternion = parent.rotation * quaternion;
			}
			quaternion = Quaternion.FromToRotation(Vector3.up, curState.ReferenceUp) * quaternion;
			curState.RawOrientation = quaternion;
		}

		public Vector2 GetRecenterTarget()
		{
			Transform transform = null;
			switch (m_RecenterTarget)
			{
			case RecenterTargetMode.FollowTargetForward:
				transform = base.VirtualCamera.Follow;
				break;
			case RecenterTargetMode.LookAtTargetForward:
				transform = base.VirtualCamera.LookAt;
				break;
			}
			if (transform != null)
			{
				Vector3 vector = transform.forward;
				Transform parent = base.VirtualCamera.transform.parent;
				if (parent != null)
				{
					vector = parent.rotation * vector;
				}
				Vector3 eulerAngles = Quaternion.FromToRotation(Vector3.forward, vector).eulerAngles;
				return new Vector2(NormalizeAngle(eulerAngles.y), NormalizeAngle(eulerAngles.x));
			}
			return Vector2.zero;
		}

		private static float NormalizeAngle(float angle)
		{
			return (angle + 180f) % 360f - 180f;
		}

		public override void ForceCameraPosition(Vector3 pos, Quaternion rot)
		{
			SetAxesForRotation(rot);
		}

		public override bool OnTransitionFromCamera(ICinemachineCamera fromCam, Vector3 worldUp, float deltaTime, ref CinemachineVirtualCameraBase.TransitionParams transitionParams)
		{
			m_HorizontalRecentering.DoRecentering(ref m_HorizontalAxis, -1f, 0f);
			m_VerticalRecentering.DoRecentering(ref m_VerticalAxis, -1f, 0f);
			m_HorizontalRecentering.CancelRecentering();
			m_VerticalRecentering.CancelRecentering();
			if (fromCam != null && transitionParams.m_InheritPosition && !CinemachineCore.Instance.IsLiveInBlend(base.VirtualCamera))
			{
				SetAxesForRotation(fromCam.State.RawOrientation);
				return true;
			}
			return false;
		}

		private void SetAxesForRotation(Quaternion targetRot)
		{
			Vector3 referenceUp = base.VcamState.ReferenceUp;
			Vector3 vector = Vector3.forward;
			Transform parent = base.VirtualCamera.transform.parent;
			if (parent != null)
			{
				vector = parent.rotation * vector;
			}
			m_HorizontalAxis.Value = 0f;
			m_HorizontalAxis.Reset();
			Vector3 vector2 = targetRot * Vector3.forward;
			Vector3 vector3 = vector.ProjectOntoPlane(referenceUp);
			Vector3 vector4 = vector2.ProjectOntoPlane(referenceUp);
			if (!vector3.AlmostZero() && !vector4.AlmostZero())
			{
				m_HorizontalAxis.Value = Vector3.SignedAngle(vector3, vector4, referenceUp);
			}
			m_VerticalAxis.Value = 0f;
			m_VerticalAxis.Reset();
			vector = Quaternion.AngleAxis(m_HorizontalAxis.Value, referenceUp) * vector;
			Vector3 vector5 = Vector3.Cross(referenceUp, vector);
			if (!vector5.AlmostZero())
			{
				m_VerticalAxis.Value = Vector3.SignedAngle(vector, vector2, vector5);
			}
		}
	}
}
