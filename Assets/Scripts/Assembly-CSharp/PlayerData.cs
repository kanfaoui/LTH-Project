using System;

[Serializable]
public class PlayerData
{
	public int money;

	public int cDay;

	public string[] consumHelds;

	public string[] giftHelds;

	public int[] heartsAtt;

	public bool[][] likedUnlocked;

	public bool[][] storyUnlocked;

	public int[] dateAtt;

	public bool[] callUnlocked;

	public bool[] intimateDone;

	public string[][] itemInChest;

	public int[] questCount;

	public PlayerData(GameManager player)
	{
		GameManager instance = GameManager.instance;
		PlayerHUD instance5 = PlayerHUD.instance;
		cDay = instance.currentDay;
		money = instance.money;
		Inventory instance2 = Inventory.instance;
		consumHelds = new string[instance2.consumables.Length];
		giftHelds = new string[instance2.gifts.Length];
		for (int i = 0; i < instance2.consumables.Length; i++)
		{
			if (instance2.consumables[i] != "")
			{
				consumHelds[i] = instance2.consumables[i];
			}
			else
			{
				consumHelds[i] = "";
			}
		}
		for (int j = 0; j < instance2.gifts.Length; j++)
		{
			if (instance2.gifts[j] != "")
			{
				giftHelds[j] = instance2.gifts[j];
			}
			else
			{
				giftHelds[j] = "";
			}
		}
		heartsAtt = new int[instance.partners.Length];
		dateAtt = new int[instance.partners.Length];
		for (int k = 0; k < instance.partners.Length; k++)
		{
			dateAtt[k] = instance.partners[k].numDates;
		}
		likedUnlocked = new bool[instance.partners.Length][];
		for (int l = 0; l < instance.partners.Length; l++)
		{
			likedUnlocked[l] = new bool[instance.partners[l].likedItem.Length];
		}
		for (int m = 0; m < instance.partners.Length; m++)
		{
			for (int n = 0; n < instance.partners[m].likedItem.Length; n++)
			{
				likedUnlocked[m][n] = instance.partners[m].likedItem[n];
			}
		}
		storyUnlocked = new bool[instance.partners.Length][];
		for (int num = 0; num < instance.partners.Length; num++)
		{
			storyUnlocked[num] = new bool[instance.partners[num].doneStory.Length];
		}
		for (int num2 = 0; num2 < instance.partners.Length; num2++)
		{
			for (int num3 = 0; num3 < instance.partners[num2].doneStory.Length; num3++)
			{
				storyUnlocked[num2][num3] = instance.partners[num2].doneStory[num3];
			}
		}
		callUnlocked = new bool[instance.partners.Length];
		for (int num4 = 0; num4 < instance.partners.Length; num4++)
		{
			callUnlocked[num4] = instance.partners[num4].canBeCalled;
		}
		intimateDone = new bool[instance.partners.Length];
		for (int num5 = 0; num5 < instance.partners.Length; num5++)
		{
			intimateDone[num5] = instance.partners[num5].doneIntimate;
		}
		ChestManager instance3 = ChestManager.instance;
		itemInChest = new string[instance3.storages.Length][];
		for (int num6 = 0; num6 < instance3.storages.Length; num6++)
		{
			itemInChest[num6] = new string[instance3.storages[num6].itemName.Length];
		}
		for (int num7 = 0; num7 < instance3.storages.Length; num7++)
		{
			for (int num8 = 0; num8 < instance3.storages[num7].itemName.Length; num8++)
			{
				itemInChest[num7][num8] = instance3.storages[num7].itemName[num8];
			}
		}
		QuestManager instance4 = QuestManager.instance;
		questCount = new int[instance4.quests.Length];
		for (int num9 = 0; num9 < instance4.quests.Length; num9++)
		{
			questCount[num9] = instance4.quests[num9].questCurrent;
		}
	}
}
