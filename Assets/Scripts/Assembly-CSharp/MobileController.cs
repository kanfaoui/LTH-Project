using UnityEngine;
using UnityEngine.UI;

public class MobileController : MonoBehaviour
{
	public static MobileController instance;

	public GameObject joyStick;

	public Image indicatorJoyStick;

	public bool joystickState;

	public bool once;

	private void Awake()
	{
		instance = this;
		joyStick.SetActive(true);
		Object.FindObjectOfType<SecondaryCamera>().joyStick = Object.FindObjectOfType<Joystick>();
		Object.FindObjectOfType<PlayerMovement>().joyStick = Object.FindObjectOfType<Joystick>();
		joyStick.SetActive(false);

		Click();

    }

	public void Click()
	{
		if (joyStick.activeInHierarchy)
		{
			joyStick.SetActive(false);
			indicatorJoyStick.color = Color.red;
			joystickState = false;
		}
		else
		{
			joyStick.SetActive(true);
			Object.FindObjectOfType<SecondaryCamera>().joyStick = Object.FindObjectOfType<Joystick>();
			indicatorJoyStick.color = Color.green;
			joystickState = true;
		}
	}
}
