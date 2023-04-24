using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHUD : MonoBehaviour
{
	public static PlayerHUD instance;

	private Inventory inventory;

	public GameObject[] UIs;

	public Slider energyBar;

	public TextMeshProUGUI day;

	public TextMeshProUGUI moneyText;

	private void Awake()
	{
		inventory = Object.FindObjectOfType<Inventory>();
		energyBar.value = 100f;
		instance = this;
	}

	public void Start()
	{
		SetDay();
		UpdateMoney();
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.I))
		{
			inventory.InventoryUI();
		}
	}

	public void UpdateMoney()
	{
		moneyText.text = "$ " + GameManager.instance.money;
	}

	public void ChangeDay()
	{
		GameManager gameManager = GameManager.instance;
		gameManager.currentDay++;
		if (gameManager.currentDay == 7)
		{
			gameManager.currentDay = 0;
		}
		SetDay();
	}

	public void SetDay()
	{
		GameManager gameManager = GameManager.instance;
		if (gameManager.currentDay == 0)
		{
			day.text = "Monday";
		}
		else if (gameManager.currentDay == 1)
		{
			day.text = "Tuesday";
		}
		else if (gameManager.currentDay == 2)
		{
			day.text = "Wednesday";
		}
		else if (gameManager.currentDay == 3)
		{
			day.text = "Thursday";
		}
		else if (gameManager.currentDay == 4)
		{
			day.text = "Friday";
		}
		else if (gameManager.currentDay == 5)
		{
			day.text = "Saturday";
		}
		else if (gameManager.currentDay == 6)
		{
			day.text = "Sunday";
		}
	}

	public void MaxEnergy()
	{
		energyBar.value = 100f;
	}

	public void MaxMoney()
	{
		GameManager.instance.money = 100000;
		UpdateMoney();
		Notify.instance.NotifyPlayer("Cheat activated");
	}

	public void UnlockAll()
	{
		GameManager gameManager = GameManager.instance;
		for (int i = 0; i < gameManager.partners.Length; i++)
		{
			for (int j = 0; j < gameManager.partners[i].doneStory.Length; j++)
			{
				gameManager.partners[i].doneStory[j] = true;
			}
		}
		for (int k = 0; k < gameManager.partners.Length; k++)
		{
			gameManager.partners[k].canBeCalled = true;
			gameManager.partners[k].numDates += 2;
			Notify.instance.NotifyPlayer("Cheat activated");
		}
	}

	public void HideUIS()
	{
		for (int i = 0; i < UIs.Length; i++)
		{
			UIs[i].SetActive(false);
		}
	}
}
