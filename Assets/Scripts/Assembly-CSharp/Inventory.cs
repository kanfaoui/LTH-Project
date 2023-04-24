using System;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;

public class Inventory : MonoBehaviour
{
	public static Inventory instance;

	public GameObject inventoryUI;

	public GameObject[] selectButton;

	[Header("Consumables")]
	[Space]
	public string[] consumables;

	public ItemButtons[] cButtons;

	[Header("Gifts")]
	[Space]
	public string[] gifts;

	public ItemButtons[] gButtons;

	[Header("Gifts")]
	[Space]
	public TextMeshProUGUI itemName;

	public TextMeshProUGUI itemDesc;

	public TextMeshProUGUI consumableEGain;

	public ChestManager chestM;

	public string itemSelected;

	public int pos;

	[HideInInspector]
	public string[] sortName = new string[12];

	[HideInInspector]
	public int[] sortNumber = new int[12];

	private PlayerHUD pHud;

	public PartnerMovement partner;

	private void Awake()
	{
		pHud = PlayerHUD.instance;
		partner = UnityEngine.Object.FindObjectOfType<PartnerMovement>();
		instance = this;
	}

	public void InventoryUI()
	{
		if (!inventoryUI.activeInHierarchy)
		{
			PlayerHUD.instance.HideUIS();
			inventoryUI.SetActive(true);
			itemName.text = "";
			itemDesc.text = "";
			consumableEGain.text = "";
			ShowItems();
			SelectOption(0);
		}
		else
		{
			inventoryUI.SetActive(false);
		}
	}

	public void SelectOption(int number)
	{
		for (int i = 0; i < selectButton.Length; i++)
		{
			selectButton[i].SetActive(false);
		}
		itemName.text = "";
		itemDesc.text = "";
		consumableEGain.text = "";
		selectButton[number].SetActive(true);
		ShowItems();
		AudioManager.instance.PlayUIs("Select");
	}

	public void ShowItems()
	{
		GameManager gameManager = GameManager.instance;
		for (int i = 0; i < cButtons.Length; i++)
		{
			cButtons[i].pos = i;
			if (consumables[i] != "")
			{
				cButtons[i].image.sprite = gameManager.GetItemDetails(consumables[i]).sprite;
				cButtons[i].image.gameObject.SetActive(true);
			}
			else
			{
				cButtons[i].image.gameObject.SetActive(false);
			}
		}
		for (int j = 0; j < gButtons.Length; j++)
		{
			gButtons[j].pos = j;
			if (gifts[j] != "")
			{
				gButtons[j].image.sprite = gameManager.GetItemDetails(gifts[j]).sprite;
				gButtons[j].image.gameObject.SetActive(true);
			}
			else
			{
				gButtons[j].image.gameObject.SetActive(false);
			}
		}
	}

	public bool AddItem(string item)
	{
		GameManager gameManager = GameManager.instance;
		if (gameManager.GetItemDetails(item).itemType == Items.Type.consumable)
		{
			bool flag = true;
			for (int i = 0; i < consumables.Length; i++)
			{
				if (consumables[i] == "")
				{
					consumables[i] = item;
					flag = false;
					SelectOption(0);
					break;
				}
			}
			if (flag)
			{
				Notify.instance.NotifyPlayer("Inventory is full");
				return false;
			}
		}
		else if (gameManager.GetItemDetails(item).itemType == Items.Type.gift)
		{
			bool flag2 = true;
			for (int j = 0; j < gifts.Length; j++)
			{
				if (gifts[j] == "")
				{
					gifts[j] = item;
					flag2 = false;
					SelectOption(1);
					break;
				}
			}
			if (flag2)
			{
				Notify.instance.NotifyPlayer("Inventory is full");
				return false;
			}
		}
		ShowItems();
		return true;
	}

	public void UseItem()
	{
		GameManager gameManager = GameManager.instance;
		if (itemSelected != "" && gameManager.GetItemDetails(itemSelected).itemType == Items.Type.consumable)
		{
			pHud.energyBar.value += gameManager.GetItemDetails(itemSelected).energyGain;
			itemSelected = "";
			consumables[pos] = "";
			ShowItems();
			AudioManager.instance.PlayUIs("Select");
		}
		//ApplovinManager.Instance.FunctionToCall();
	}

	public void TransferItem()
	{
		GameManager gameManager = GameManager.instance;
		GiftManager giftManager = GiftManager.instance;
		if (itemSelected != "" && chestM != null)
		{
			Storage storage = Array.Find(chestM.storages, _003CTransferItem_003Eb__23_0);
			if (gameManager.GetItemDetails(itemSelected).itemType == Items.Type.consumable)
			{
				for (int i = 0; i < chestM.itemsStored.Length; i++)
				{
					if (chestM.itemsStored[i] == "")
					{
						chestM.itemsStored[i] = itemSelected;
						chestM.chestInUse.itemsInChest[i] = itemSelected;
						storage.itemName[i] = itemSelected;
						itemSelected = "";
						consumables[pos] = "";
						chestM.ShowItems();
						ShowItems();
						AudioManager.instance.PlayUIs("Inv_Transfer");
						return;
					}
				}
				Notify.instance.NotifyPlayer(chestM.chestInUse.chestName + " is full");
			}
			else
			{
				if (gameManager.GetItemDetails(itemSelected).itemType != Items.Type.gift)
				{
					return;
				}
				for (int j = 0; j < chestM.itemsStored.Length; j++)
				{
					if (chestM.itemsStored[j] == "")
					{
						chestM.itemsStored[j] = itemSelected;
						chestM.chestInUse.itemsInChest[j] = itemSelected;
						storage.itemName[j] = itemSelected;
						itemSelected = "";
						gifts[pos] = "";
						chestM.ShowItems();
						ShowItems();
						AudioManager.instance.PlayUIs("Inv_Transfer");
						return;
					}
				}
				Notify.instance.NotifyPlayer(chestM.chestInUse.chestName + " is full");
			}
		}
		else
		{
			if (!(itemSelected != "") || !giftManager.canvasGift.activeInHierarchy)
			{
				return;
			}
			if (gameManager.GetItemDetails(itemSelected).itemType == Items.Type.gift)
			{
				if (giftManager.gift == "")
				{
					giftManager.gift = itemSelected;
					gifts[pos] = "";
					itemSelected = "";
				}
				else
				{
					string gift = giftManager.gift;
					giftManager.gift = itemSelected;
					gifts[pos] = "";
					itemSelected = "";
					AddItem(gift);
				}
			}
			ShowItems();
			GiftManager.instance.ShowItem();
			AudioManager.instance.PlayUIs("Inv_Transfer");
		}
	}

	public void DeleteItem()
	{
		GameManager gameManager = GameManager.instance;
		if (!(itemSelected != ""))
		{
			return;
		}
		if (gameManager.GetItemDetails(itemSelected).itemType == Items.Type.consumable)
		{
			consumables[pos] = "";
		}
		else if (gameManager.GetItemDetails(itemSelected).itemType == Items.Type.gift)
		{
			if (gameManager.GetItemDetails(itemSelected).giftType == Items.GiftType.Quest)
			{
				Notify.instance.NotifyPlayer("Can't be thrown away");
				return;
			}
			gifts[pos] = "";
		}
		itemSelected = "";
		ShowItems();
		itemName.text = "";
		itemDesc.text = "";
		consumableEGain.text = "";
	}

	[CompilerGenerated]
	private bool _003CTransferItem_003Eb__23_0(Storage storage)
	{
		return storage.storageName == chestM.chestInUse.sName;
	}
}
