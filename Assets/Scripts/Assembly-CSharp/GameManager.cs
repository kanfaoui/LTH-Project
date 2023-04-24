using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
	public static GameManager instance;

	public bool patreonOnly;

	public PartnerMovement currentCalledPartner;

	public GameObject[] partnerPrefab;

	public int currentDay;

	public int money;

	[Header("Partner Stats")]
	[NonReorderable]
	[Space]
	public PartnerStats[] partners;

	public Items[] referenceItems;

	public Partner[] referencePartner;

	private void Awake()
	{
		instance = this;
		Application.targetFrameRate = 60;
		Object.DontDestroyOnLoad(base.gameObject);
	}

	public void Update()
	{
	}

	public Items GetItemDetails(string itemToGrab)
	{
		for (int i = 0; i < referenceItems.Length; i++)
		{
			if (referenceItems[i].itemName == itemToGrab)
			{
				return referenceItems[i];
			}
		}
		return null;
	}

	public Partner GetPartnerDetails(string partnerName)
	{
		for (int i = 0; i < referencePartner.Length; i++)
		{
			if (referencePartner[i].pName == partnerName)
			{
				return referencePartner[i];
			}
		}
		return null;
	}

	public void SaveGame()
	{
		SaveSystem.SavePlayer(this);
		Notify.instance.NotifyPlayer("Game Saved");
	}

	public void LoadGame()
	{
		Inventory inventory = Inventory.instance;
		ChestManager chestManager = ChestManager.instance;
		QuestManager questManager = QuestManager.instance;
		Player player = Player.instance;
		PlayerData playerData = SaveSystem.LoadPlayer();
		SceneManager.LoadScene("House");
		player.playerPrefab.stayPos = true;
		player.playerPrefab.transform.position = new Vector3(0f, -4f, 0f);
		currentDay = playerData.cDay;
		PlayerHUD.instance.SetDay();
		money = playerData.money;
		PlayerHUD.instance.UpdateMoney();
		for (int i = 0; i < inventory.consumables.Length; i++)
		{
			inventory.consumables[i] = playerData.consumHelds[i];
		}
		for (int j = 0; j < inventory.gifts.Length; j++)
		{
			inventory.gifts[j] = playerData.giftHelds[j];
		}
		for (int k = 0; k < partners.Length; k++)
		{
			partners[k].numDates = playerData.dateAtt[k];
		}
		for (int l = 0; l < partners.Length; l++)
		{
			for (int m = 0; m < partners[l].likedItem.Length; m++)
			{
				partners[l].likedItem[m] = playerData.likedUnlocked[l][m];
			}
		}
		for (int n = 0; n < partners.Length; n++)
		{
			for (int num = 0; num < partners[n].doneStory.Length; num++)
			{
				partners[n].doneStory[num] = playerData.storyUnlocked[n][num];
			}
		}
		for (int num2 = 0; num2 < partners.Length; num2++)
		{
			partners[num2].canBeCalled = playerData.callUnlocked[num2];
		}
		for (int num3 = 0; num3 < partners.Length; num3++)
		{
			partners[num3].doneIntimate = playerData.intimateDone[num3];
		}
		for (int num4 = 0; num4 < chestManager.storages.Length; num4++)
		{
			for (int num5 = 0; num5 < chestManager.storages[num4].itemName.Length; num5++)
			{
				chestManager.storages[num4].itemName[num5] = playerData.itemInChest[num4][num5];
			}
		}
		Chest[] array = (Chest[])Object.FindObjectsOfType(typeof(Chest));
		for (int num6 = 0; num6 < array.Length; num6++)
		{
			array[num6].SetStorage();
		}
		for (int num7 = 0; num7 < questManager.quests.Length; num7++)
		{
			questManager.quests[num7].questCurrent = playerData.questCount[num7];
		}
	}
}
