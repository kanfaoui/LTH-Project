using System;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DateManager : MonoBehaviour
{
	public static DateManager instance;

	[Space]
	[Header("Date Checker")]
	[NonReorderable]
	public DateChecker[] dateCheckers;

	[NonReorderable]
	public DatePartner[] datePartners;

	[Space]
	[Header("Date UI")]
	public GameObject dateUI;

	public GameObject leaveButton;

	public Slider impressionBar;

	public bool date;

	public TextMeshProUGUI giftLimitText;

	public TextMeshProUGUI travelLimitText;

	public GameObject maxMeter;

	[Space]
	[Header("Map UI")]
	public GameObject mapUI;

	[Space]
	[Header("Date Values")]
	public Vector2 partnerSpawn;

	public Vector2 playerSpawn;

	public bool onMinigame;

	public int giftLimit;

	public string datePartnerName;

	public int travelOnce;

	private void Awake()
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
			mapUI.SetActive(true);
		}
	}

	public void DateArea(string areaName)
	{
		mapUI.SetActive(false);
		if (Lighting.instance.currentState != 0)
		{
			Notify.instance.NotifyPlayer("Dates must be initiated in the morning");
			return;
		}
		Map.instance.Travel(areaName);
		PlayerHUD.instance.HideUIS();
		date = true;
		onMinigame = false;
		maxMeter.SetActive(false);
		leaveButton.SetActive(true);
		travelOnce++;
		if (travelOnce == 2)
		{
			travelLimitText.text = "1/1";
		}
	}

	public void ResetDate()
	{
		datePartnerName = Statistics.instance.partnerName;
		travelOnce = 0;
		giftLimit = 0;
		travelLimitText.text = "0/1";
		giftLimitText.text = giftLimit + "/3";
		PartnerStats partnerStats = Array.Find(GameManager.instance.partners, _003CResetDate_003Eb__20_0);
		impressionBar.value = 0f;
		if (partnerStats.numDates >= 1)
		{
			impressionBar.maxValue = 26f;
		}
		else
		{
			impressionBar.maxValue = 20f;
		}
		for (int i = 0; i < dateCheckers.Length; i++)
		{
			dateCheckers[i].isDone = false;
		}
		for (int j = 0; j < datePartners.Length; j++)
		{
			if (datePartners[j].chibi != null)
			{
				datePartners[j].chibi.SetActive(false);
			}
		}
		GiftManager giftManager = GiftManager.instance;
		for (int k = 0; k < giftManager.giftsGiven.Length; k++)
		{
			giftManager.giftsGiven[k] = "";
		}
	}

	public void UpdateImpressions(int value)
	{
		maxMeter.SetActive(false);
		impressionBar.value += value;
		if (impressionBar.value <= 0f)
		{
			impressionBar.value = 0f;
		}
		if (impressionBar.value >= impressionBar.maxValue)
		{
			maxMeter.SetActive(true);
			impressionBar.value = impressionBar.maxValue;
		}
	}

	public void EnableChibi()
	{
		GameManager instance2 = GameManager.instance;
		Array.Find(datePartners, _003CEnableChibi_003Eb__22_0).chibi.SetActive(true);
	}

	public void DisableChibi()
	{
		GameManager instance2 = GameManager.instance;
		Array.Find(datePartners, _003CDisableChibi_003Eb__23_0).chibi.SetActive(false);
	}

	public void EndDate()
	{
		GameManager gameManager = GameManager.instance;
		GiftManager instance2 = GiftManager.instance;
		PlayerHUD.instance.HideUIS();
		date = false;
		onMinigame = false;
		Lighting.instance.preLighting = true;
		PartnerStats partnerStats = Array.Find(gameManager.partners, _003CEndDate_003Eb__24_0);
		DatePartner datePartner = Array.Find(datePartners, _003CEndDate_003Eb__24_1);
		DateManager instance3 = instance;
		Partner partnerDetails = gameManager.GetPartnerDetails(datePartnerName);
		for (int i = 0; i < datePartner.success.dialogue.sprite.Length; i++)
		{
			for (int j = 0; j < partnerDetails.faceSprite.Length; j++)
			{
				int num = Array.IndexOf(partnerDetails.faceSprite[j].pGiftFaceSprite, datePartner.success.dialogue.sprite[i]);
				if (num != -1)
				{
					SceneObjectSpawner sceneObjectSpawner = UnityEngine.Object.FindObjectOfType<SceneObjectSpawner>();
					if (sceneObjectSpawner.normalAttire)
					{
						datePartner.success.dialogue.sprite[i] = partnerDetails.faceSprite[0].pGiftFaceSprite[num];
					}
					else if (sceneObjectSpawner.beachAttire)
					{
						datePartner.success.dialogue.sprite[i] = partnerDetails.faceSprite[1].pGiftFaceSprite[num];
					}
				}
			}
		}
		for (int k = 0; k < datePartner.fail.dialogue.sprite.Length; k++)
		{
			for (int l = 0; l < partnerDetails.faceSprite.Length; l++)
			{
				int num2 = Array.IndexOf(partnerDetails.faceSprite[l].pGiftFaceSprite, datePartner.fail.dialogue.sprite[k]);
				if (num2 != -1)
				{
					SceneObjectSpawner sceneObjectSpawner2 = UnityEngine.Object.FindObjectOfType<SceneObjectSpawner>();
					if (sceneObjectSpawner2.normalAttire)
					{
						datePartner.fail.dialogue.sprite[k] = partnerDetails.faceSprite[0].pGiftFaceSprite[num2];
					}
					else if (sceneObjectSpawner2.beachAttire)
					{
						datePartner.fail.dialogue.sprite[k] = partnerDetails.faceSprite[1].pGiftFaceSprite[num2];
					}
				}
			}
		}
		if (impressionBar.value == impressionBar.maxValue)
		{
			partnerStats.numDates++;
			datePartner.success.TriggerDialogue();
			dateUI.SetActive(false);
		}
		else if (impressionBar.value < impressionBar.maxValue)
		{
			datePartner.fail.TriggerDialogue();
			dateUI.SetActive(false);
		}
	}

	public void Home()
	{
		Map.instance.Travel("House");
	}

	[CompilerGenerated]
	private bool _003CResetDate_003Eb__20_0(PartnerStats p)
	{
		return p.partnerName == datePartnerName;
	}

	[CompilerGenerated]
	private bool _003CEnableChibi_003Eb__22_0(DatePartner d)
	{
		return d.partnerName == datePartnerName;
	}

	[CompilerGenerated]
	private bool _003CDisableChibi_003Eb__23_0(DatePartner d)
	{
		return d.partnerName == datePartnerName;
	}

	[CompilerGenerated]
	private bool _003CEndDate_003Eb__24_0(PartnerStats p)
	{
		return p.partnerName == datePartnerName;
	}

	[CompilerGenerated]
	private bool _003CEndDate_003Eb__24_1(DatePartner d)
	{
		return d.partnerName == datePartnerName;
	}
}
