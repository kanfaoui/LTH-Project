using UnityEngine;

public class Info : MonoBehaviour
{
	public GameObject InfoBox;

	public GameObject devMessage;

	public GameObject cheats;

	public GameObject[] infoClick;

	public void Update()
	{
		if (Input.GetKeyDown(KeyCode.P))
		{
			OpenInfo();
		}
	}

	public void OpenInfo()
	{
		GiftManager instance = GiftManager.instance;
		if (InfoBox.activeInHierarchy)
		{
			InfoBox.SetActive(false);
		}
		else
		{
			PlayerHUD.instance.HideUIS();
			InfoBox.SetActive(true);
			InfoButton(0);
			if (GameManager.instance.patreonOnly)
			{
				cheats.SetActive(false);
			}
			else
			{
				cheats.SetActive(true);
			}
		}
		AudioManager.instance.PlayUIs("Select");
	}

	public void InfoButton(int number)
	{
		for (int i = 0; i < infoClick.Length; i++)
		{
			infoClick[i].SetActive(false);
		}
		infoClick[number].SetActive(true);
	}

	public void DevMessage()
	{
		if (devMessage.activeInHierarchy)
		{
			devMessage.SetActive(false);
		}
		else
		{
			devMessage.SetActive(true);
		}
	}
}
