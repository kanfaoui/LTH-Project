using System;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ChestManager : MonoBehaviour
{
	public static ChestManager instance;

	public GameObject showUI;

	[Header("Global Storage")]
	[Space]
	[NonReorderable]
	public Storage[] storages;

	[Header("Chest UI")]
	[Space]
	public Chest chestInUse;

	public string[] itemsStored;

	public ItemButtons[] iButtons;

	public bool storedInventoryFull;

	public string itemSelected;

	public int pos;

	private Inventory inventory;

	private void Awake()
	{
		inventory = UnityEngine.Object.FindObjectOfType<Inventory>();
		instance = this;
	}

	private void Update()
	{
	}

	public void ChestUI()
	{
		if (!showUI.activeInHierarchy)
		{
			if (!inventory.inventoryUI.gameObject.activeInHierarchy)
			{
				inventory.InventoryUI();
			}
			showUI.SetActive(true);
			ShowItems();
			inventory.chestM = this;
		}
		else
		{
			itemSelected = "";
			showUI.SetActive(false);
			inventory.chestM = null;
			if (inventory.inventoryUI.gameObject.activeInHierarchy)
			{
				inventory.InventoryUI();
			}
		}
	}

	public void ShowItems()
	{
		GameManager gameManager = GameManager.instance;
		for (int i = 0; i < iButtons.Length; i++)
		{
			iButtons[i].pos = i;
			if (itemsStored[i] != "")
			{
				iButtons[i].image.sprite = gameManager.GetItemDetails(itemsStored[i]).sprite;
				iButtons[i].image.gameObject.SetActive(true);
			}
			else
			{
				iButtons[i].image.gameObject.SetActive(false);
			}
		}
	}

	public void TransferItem()
	{
		GameManager gameManager = GameManager.instance;
		Storage storage = Array.Find(instance.storages, _003CTransferItem_003Eb__14_0);
		if (!(itemSelected != ""))
		{
			return;
		}
		if (gameManager.GetItemDetails(itemSelected).itemType == Items.Type.consumable)
		{
			for (int i = 0; i < inventory.consumables.Length; i++)
			{
				if (inventory.consumables[i] == "")
				{
					inventory.consumables[i] = itemSelected;
					itemSelected = "";
					itemsStored[pos] = "";
					chestInUse.itemsInChest[pos] = "";
					storage.itemName[pos] = "";
					inventory.ShowItems();
					inventory.SelectOption(0);
					ShowItems();
					AudioManager.instance.PlayUIs("Inv_Transfer");
					return;
				}
			}
			Debug.Log("Inventory full");
			Notify.instance.NotifyPlayer("Inventory is full");
		}
		else
		{
			if (gameManager.GetItemDetails(itemSelected).itemType != Items.Type.gift)
			{
				return;
			}
			for (int j = 0; j < inventory.gifts.Length; j++)
			{
				if (inventory.gifts[j] == "")
				{
					inventory.gifts[j] = itemSelected;
					itemSelected = "";
					itemsStored[pos] = "";
					chestInUse.itemsInChest[pos] = "";
					storage.itemName[pos] = "";
					inventory.ShowItems();
					inventory.SelectOption(1);
					ShowItems();
					AudioManager.instance.PlayUIs("Inv_Transfer");
					return;
				}
			}
			Debug.Log("Inventory full");
			Notify.instance.NotifyPlayer("Inventory is full");
		}
	}

	[CompilerGenerated]
	private bool _003CTransferItem_003Eb__14_0(Storage storage)
	{
		return storage.storageName == chestInUse.sName;
	}
}
