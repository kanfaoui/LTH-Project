using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopButton : MonoBehaviour
{
	public Image image;

	public TextMeshProUGUI itemName;

	public TextMeshProUGUI priceName;

	public int pos;

	public void BuyItem()
	{
		ShopManager instance = ShopManager.instance;
		GameManager instance2 = GameManager.instance;
		Inventory instance3 = Inventory.instance;
		if (instance2.money >= instance2.GetItemDetails(instance.itemsInShop[pos]).itemPrice)
		{
			if (instance.showUI.activeInHierarchy && instance3.AddItem(instance.itemsInShop[pos]))
			{
				instance2.money -= instance2.GetItemDetails(instance.itemsInShop[pos]).itemPrice;
			}
			PlayerHUD.instance.UpdateMoney();
		}
		else
		{
			Notify.instance.NotifyPlayer("Not enough money");
		}
	}
}
