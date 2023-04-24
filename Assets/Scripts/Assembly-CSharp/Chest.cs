using System;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Chest : MonoBehaviour
{
	public GameObject button;

	public string chestName;

	public string sName;

	public string[] itemsInChest;

	public void Start()
	{
		SetStorage();
	}

	public void SetStorage()
	{
		ChestManager instance = ChestManager.instance;
		GameManager instance2 = GameManager.instance;
		Storage storage = Array.Find(instance.storages, _003CSetStorage_003Eb__5_0);
		for (int i = 0; i < storage.itemName.Length; i++)
		{
			if (storage.itemName[i] != "")
			{
				if ((bool)instance2.GetItemDetails(storage.itemName[i]))
				{
					itemsInChest[i] = storage.itemName[i];
					continue;
				}
				itemsInChest[i] = "";
				Debug.Log("big Problem what is this? " + storage.itemName[i]);
			}
			else
			{
				itemsInChest[i] = "";
			}
		}
	}

	public void OpenChest()
	{
		ChestManager instance = ChestManager.instance;
		instance.chestInUse = this;
		for (int i = 0; i < instance.itemsStored.Length; i++)
		{
			instance.itemsStored[i] = null;
		}
		for (int j = 0; j < itemsInChest.Length; j++)
		{
			instance.itemsStored[j] = itemsInChest[j];
		}
		instance.ChestUI();
		//ApplovinManager.Instance.FunctionToCall();
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
	private bool _003CSetStorage_003Eb__5_0(Storage storage)
	{
		return storage.storageName == sName;
	}
}
