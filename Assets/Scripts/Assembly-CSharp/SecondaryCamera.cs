using Cinemachine;
using UnityEngine;
using UnityEngine.UI;

public class SecondaryCamera : MonoBehaviour
{
	public Rigidbody2D rb;

	public float panSpeed = 20f;

	public Slider zoomSlider;

	public bool unlockCamera;

	public CinemachineVirtualCamera vCam;

	public Joystick joyStick;

	private void Awake()
	{
		joyStick = Object.FindObjectOfType<Joystick>();
	}

	public void Start()
	{
		joyStick = Object.FindObjectOfType<Joystick>();
		rb = GetComponent<Rigidbody2D>();
	}

	private void FixedUpdate()
	{
		if (unlockCamera)
		{
			if (!Object.FindObjectOfType<MobileController>().joystickState)
			{
				float axisRaw = Input.GetAxisRaw("Horizontal");
				float axisRaw2 = Input.GetAxisRaw("Vertical");
				rb.velocity = new Vector2(axisRaw * panSpeed, axisRaw2 * panSpeed);
			}
			else if (Object.FindObjectOfType<MobileController>().joystickState)
			{
				float axisRaw = joyStick.Horizontal;
				float axisRaw2 = joyStick.Vertical;
				rb.velocity = new Vector2(axisRaw * panSpeed, axisRaw2 * panSpeed);
			}
		}
	}

	public void Setlevel(float sliderValue)
	{
		if (unlockCamera)
		{
			vCam.m_Lens.OrthographicSize = sliderValue;
		}
	}
}
