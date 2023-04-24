using UnityEngine;

public class ShopManager : MonoBehaviour
{
	public static ShopManager instance;

	public GameObject showUI;

	[Header("Global Shop")]
	[Space]
	[NonReorderable]
	public Shop[] shops;

	[Header("Shop UI")]
	[Space]
	public string[] itemsInShop;

	public ShopButton[] iButtons;

	private Inventory inventory;

	private void Awake()
	{
		inventory = Object.FindObjectOfType<Inventory>();
		instance = this;
	}

	public void ShopUI()
	{
		if (!showUI.activeInHierarchy)
		{
			if (!inventory.inventoryUI.gameObject.activeInHierarchy)
			{
				inventory.InventoryUI();
			}
			showUI.SetActive(true);
			ShowShopItems();
		}
		else
		{
			showUI.SetActive(false);
			inventory.chestM = null;
			if (inventory.inventoryUI.gameObject.activeInHierarchy)
			{
				inventory.InventoryUI();
			}
		}
	}

	public void ShowShopItems()
	{
		GameManager gameManager = GameManager.instance;
		for (int i = 0; i < iButtons.Length; i++)
		{
			iButtons[i].gameObject.SetActive(false);
		}
		for (int j = 0; j < itemsInShop.Length; j++)
		{
			iButtons[j].pos = j;
			if (itemsInShop[j] != "")
			{
				iButtons[j].itemName.text = gameManager.GetItemDetails(itemsInShop[j]).itemName;
				iButtons[j].priceName.text = "$ " + gameManager.GetItemDetails(itemsInShop[j]).itemPrice;
				iButtons[j].image.sprite = gameManager.GetItemDetails(itemsInShop[j]).sprite;
				iButtons[j].gameObject.SetActive(true);
			}
		}
	}
}
