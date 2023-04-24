using System;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GiftManager : MonoBehaviour
{
	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass20_0
	{
		public GameManager gameManager;

		internal bool _003CReceive_003Eb__1(PartnerStats p)
		{
			return p.partnerName == gameManager.currentCalledPartner.partnerName;
		}

		internal bool _003CReceive_003Eb__2(ItemDate i)
		{
			return i.dateName == gameManager.currentCalledPartner.partnerName;
		}

		internal bool _003CReceive_003Eb__0(GiftDialogues item)
		{
			return item.partnerName == gameManager.currentCalledPartner.partnerName;
		}
	}

	public static GiftManager instance;

	[Header("Canvas Gift")]
	[Space]
	public GameObject canvasGift;

	public Image itemImage;

	public string gift;

	public GameObject giftNameBG;

	public TextMeshProUGUI giftNameText;

	[Header("Gift Limit")]
	[Space]
	public string[] giftsGiven;

	[Header("H Dialogue")]
	[Space]
	public DialogueTrigger dTriggerMary;

	public DialogueTrigger dTriggerEul;

	public DialogueTrigger dTriggerCarol;

	public DialogueTrigger dTriggerCait;

	[Header("Gift Quest")]
	[Space]
	public string questName;

	public string questGiftName;

	private PlayerMovement player;

	private Inventory inventory;

	private PlayerHUD pHUD;

	public void Awake()
	{
		instance = this;
	}

	public void GiftUI()
	{
		inventory = Inventory.instance;
		questName = "";
		questGiftName = "";
		giftNameBG.SetActive(false);
		OpenUI();
	}

	public void GiftQuestUI(string qName, string qGiftName)
	{
		inventory = Inventory.instance;
		questName = qName;
		questGiftName = qGiftName;
		giftNameText.text = qGiftName;
		giftNameBG.SetActive(true);
		OpenUI();
	}

	private void OpenUI()
	{
		if (!canvasGift.activeInHierarchy)
		{
			if (!inventory.inventoryUI.gameObject.activeInHierarchy)
			{
				inventory.InventoryUI();
				inventory.SelectOption(1);
			}
			canvasGift.SetActive(true);
			ShowItem();
			AudioManager.instance.PlayUIs("Select");
			return;
		}
		canvasGift.SetActive(false);
		if (inventory.inventoryUI.gameObject.activeInHierarchy)
		{
			inventory.InventoryUI();
		}
		if (gift != "")
		{
			inventory.AddItem(gift);
			gift = "";
		}
	}

	public void Receive()
	{
		_003C_003Ec__DisplayClass20_0 _003C_003Ec__DisplayClass20_ = new _003C_003Ec__DisplayClass20_0();
		_003C_003Ec__DisplayClass20_.gameManager = GameManager.instance;
		DateManager dateManager = DateManager.instance;
		if (gift != "" && questGiftName == "")
		{
			Partner partnerDetails = _003C_003Ec__DisplayClass20_.gameManager.GetPartnerDetails(_003C_003Ec__DisplayClass20_.gameManager.currentCalledPartner.partnerName);
			if (_003C_003Ec__DisplayClass20_.gameManager.GetItemDetails(gift).giftType == Items.GiftType.Quest)
			{
				Notify.instance.NotifyPlayer("This is a quest item.");
				return;
			}
			if (dateManager.date)
			{
				PartnerMovement component = _003C_003Ec__DisplayClass20_.gameManager.currentCalledPartner.GetComponent<PartnerMovement>();
				for (int i = 0; i < giftsGiven.Length; i++)
				{
					if (giftsGiven[i] == gift)
					{
						component.anim.SetTrigger("Deny");
						OpenUI();
						return;
					}
				}
				PartnerStats partnerStats = Array.Find(_003C_003Ec__DisplayClass20_.gameManager.partners, _003C_003Ec__DisplayClass20_._003CReceive_003Eb__1);
				ItemDate itemDate = Array.Find(_003C_003Ec__DisplayClass20_.gameManager.GetItemDetails(gift).dateGift, _003C_003Ec__DisplayClass20_._003CReceive_003Eb__2);
				if (dateManager.giftLimit >= 3)
				{
					component.anim.SetTrigger("Deny");
					OpenUI();
					return;
				}
				for (int j = 0; j < partnerDetails.pLikes.Length; j++)
				{
					if (partnerDetails.pLikes[j] == _003C_003Ec__DisplayClass20_.gameManager.GetItemDetails(gift).itemName)
					{
						partnerStats.likedItem[j] = true;
					}
				}
				dateManager.UpdateImpressions(itemDate.dateGain);
				dateManager.giftLimit++;
				dateManager.giftLimitText.text = dateManager.giftLimit + "/3";
				for (int k = 0; k < giftsGiven.Length; k++)
				{
					if (giftsGiven[k] == "")
					{
						giftsGiven[k] = gift;
						break;
					}
				}
			}
			GiftDialogues giftDialogues = Array.Find(_003C_003Ec__DisplayClass20_.gameManager.GetItemDetails(gift).gDialogue, _003C_003Ec__DisplayClass20_._003CReceive_003Eb__0);
			for (int l = 0; l < giftDialogues.dTrigger.sprite.Length; l++)
			{
				for (int m = 0; m < partnerDetails.faceSprite.Length; m++)
				{
					int num = Array.IndexOf(partnerDetails.faceSprite[m].pGiftFaceSprite, giftDialogues.dTrigger.sprite[l]);
					if (num != -1)
					{
						SceneObjectSpawner sceneObjectSpawner = UnityEngine.Object.FindObjectOfType<SceneObjectSpawner>();
						if (sceneObjectSpawner.normalAttire)
						{
							giftDialogues.dTrigger.sprite[l] = partnerDetails.faceSprite[0].pGiftFaceSprite[num];
						}
						else if (sceneObjectSpawner.beachAttire)
						{
							giftDialogues.dTrigger.sprite[l] = partnerDetails.faceSprite[1].pGiftFaceSprite[num];
						}
					}
				}
			}
			DialogueManager.instance.StartDialogue(giftDialogues.dTrigger, null, false, null, null, null);
			gift = "";
			canvasGift.SetActive(false);
			inventory.InventoryUI();
		}
		else if (gift != "" && questGiftName != "")
		{
			if (gift == questGiftName)
			{
				QuestManager.instance.UpdateQuest(questName);
				gift = "";
				canvasGift.SetActive(false);
				inventory.InventoryUI();
			}
			else
			{
				Notify.instance.NotifyPlayer("Wrong item");
			}
		}
	}

	public void ShowItem()
	{
		GameManager gameManager = GameManager.instance;
		if (gift != "")
		{
			itemImage.gameObject.SetActive(true);
			itemImage.sprite = gameManager.GetItemDetails(gift).sprite;
		}
		else
		{
			itemImage.gameObject.SetActive(false);
		}
	}
}
