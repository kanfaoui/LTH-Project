using System;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ShopForm : MonoBehaviour
{
	public GameObject button;

	public string sName;

	public string[] itemsInShop;

	public void Start()
	{
		ShopManager instance = ShopManager.instance;
		GameManager instance2 = GameManager.instance;
		Shop shop = Array.Find(instance.shops, _003CStart_003Eb__3_0);
		itemsInShop = new string[shop.itemName.Length];
		for (int i = 0; i < shop.itemName.Length; i++)
		{
			if (shop.itemName[i] != "")
			{
				if ((bool)instance2.GetItemDetails(shop.itemName[i]))
				{
					itemsInShop[i] = shop.itemName[i];
				}
			}
			else
			{
				itemsInShop[i] = null;
				Debug.Log("What the fuck is this? " + shop.itemName[i]);
			}
		}
	}

	public void OpenShop()
	{
		ShopManager instance = ShopManager.instance;
		for (int i = 0; i < instance.itemsInShop.Length; i++)
		{
			instance.itemsInShop[i] = null;
		}
		instance.itemsInShop = new string[itemsInShop.Length];
		for (int j = 0; j < itemsInShop.Length; j++)
		{
			instance.itemsInShop[j] = itemsInShop[j];
		}
		instance.ShopUI();
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.tag == "Player")
		{
			button.gameObject.SetActive(true);
		}
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		if (collision.tag == "Player")
		{
			button.gameObject.SetActive(false);
			if (ChestManager.instance.showUI.activeInHierarchy)
			{
				ChestManager.instance.ChestUI();
			}
		}
	}

	[CompilerGenerated]
	private bool _003CStart_003Eb__3_0(Shop shop)
	{
		return shop.shopName == sName;
	}
}
