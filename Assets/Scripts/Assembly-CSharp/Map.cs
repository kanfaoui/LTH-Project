using UnityEngine;
using UnityEngine.SceneManagement;

public class Map : MonoBehaviour
{
	public static Map instance;

	public GameObject mapUI;

	public void Awake()
	{
		instance = this;
	}

	public void MapUI()
	{
		if (mapUI.activeInHierarchy)
		{
			mapUI.SetActive(false);
		}
		else
		{
			if (DateManager.instance.date)
			{
				if (DateManager.instance.travelOnce <= 1)
				{
					PlayerHUD.instance.HideUIS();
					DateManager.instance.MapUI();
				}
				else
				{
					Notify.instance.NotifyPlayer("Can only change location once");
				}
				return;
			}
			PlayerHUD.instance.HideUIS();
			mapUI.SetActive(true);
			bool flag = PlayerMovement.instance != null;
		}
		AudioManager.instance.PlayUIs("Select");
	}

	public void Travel(string place)
	{
		if (Object.FindObjectOfType<PositionsManager>() != null || Object.FindObjectOfType<Intimacy>() != null)
		{
			Notify.instance.NotifyPlayer("Can't do that right now");
		}
		else if (place == "Library" && Lighting.instance.currentState == 2)
		{
			Notify.instance.NotifyPlayer("Library is close at night");
		}
		else if (place == "Beach" && Lighting.instance.currentState == 2)
		{
			Notify.instance.NotifyPlayer("Beach is close at night");
		}
		else if (!(PlayerMovement.instance == null))
		{
			PlayerMovement.instance.stayPos = false;
			mapUI.SetActive(false);
			SceneManager.LoadScene(place);
		}
//		ApplovinManager.Instance.FunctionToCall();
	}
}
