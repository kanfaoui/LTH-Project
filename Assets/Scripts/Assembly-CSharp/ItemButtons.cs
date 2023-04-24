using UnityEngine;
using UnityEngine.UI;

public class ItemButtons : MonoBehaviour
{
	public Image image;

	public int pos;

	private float doubleClick;

	private PlayerHUD pHud;

	private Inventory inventory;

	private void Start()
	{
		pHud = Object.FindObjectOfType<PlayerHUD>();
		inventory = Object.FindObjectOfType<Inventory>();
	}

	public void Update()
	{
		if (doubleClick > 0f)
		{
			doubleClick -= Time.deltaTime;
		}
	}

	public void SelectConsumable()
	{
		GameManager instance = GameManager.instance;
		inventory.itemSelected = inventory.consumables[pos];
		inventory.pos = pos;
		if (inventory.itemSelected != "")
		{
			inventory.itemName.text = instance.GetItemDetails(inventory.itemSelected).itemName;
			inventory.itemDesc.text = instance.GetItemDetails(inventory.itemSelected).itemDesc;
			inventory.consumableEGain.text = "+" + instance.GetItemDetails(inventory.itemSelected).energyGain;
		}
		if (inventory.chestM != null)
		{
			inventory.chestM.itemSelected = "";
		}
		if (doubleClick > 0f && inventory.itemSelected != "")
		{
			inventory.TransferItem();
		}
		doubleClick = 0.2f;
		AudioManager.instance.PlayUIs("Inv_Select");
	}

	public void SelectGifts()
	{
		GameManager instance = GameManager.instance;
		inventory.itemSelected = inventory.gifts[pos];
		inventory.pos = pos;
		if (inventory.itemSelected != "")
		{
			inventory.itemName.text = instance.GetItemDetails(inventory.itemSelected).itemName;
			inventory.itemDesc.text = instance.GetItemDetails(inventory.itemSelected).itemDesc;
		}
		if (inventory.chestM != null)
		{
			inventory.chestM.itemSelected = "";
		}
		if (doubleClick > 0f && inventory.itemSelected != "")
		{
			inventory.TransferItem();
		}
		doubleClick = 0.2f;
		AudioManager.instance.PlayUIs("Inv_Select");
	}

	public void SelectStoredItem()
	{
		GameManager instance = GameManager.instance;
		if (inventory.chestM != null)
		{
			inventory.chestM.itemSelected = inventory.chestM.itemsStored[pos];
			inventory.chestM.pos = pos;
			if (inventory.chestM.itemSelected != "")
			{
				inventory.itemName.text = instance.GetItemDetails(inventory.chestM.itemSelected).itemName;
				inventory.itemDesc.text = instance.GetItemDetails(inventory.chestM.itemSelected).itemDesc;
				if (instance.GetItemDetails(inventory.chestM.itemSelected).itemType == Items.Type.consumable)
				{
					inventory.consumableEGain.text = "+" + instance.GetItemDetails(inventory.chestM.itemSelected).energyGain;
				}
				else
				{
					inventory.consumableEGain.text = "";
				}
			}
			inventory.itemSelected = "";
		}
		if (doubleClick > 0f && inventory.chestM.itemSelected != "")
		{
			inventory.chestM.TransferItem();
		}
		doubleClick = 0.2f;
		AudioManager.instance.PlayUIs("Inv_Select");
	}
}
