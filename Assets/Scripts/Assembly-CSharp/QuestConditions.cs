using UnityEngine;

public class QuestConditions : MonoBehaviour
{
	public string conditionName;

	public string qName;

	public string qGiftName;

	public QuestItems itemToSpawn;

	public int itemsInArea;

	public GameObject[] objectsToPlace;

	public void Start()
	{
		SpawnQuestItems();
	}

	public void SpawnQuestItems()
	{
		itemsInArea = objectsToPlace.Length - 1;
		for (int i = 0; i < objectsToPlace.Length; i++)
		{
			Object.Instantiate(itemToSpawn, objectsToPlace[i].transform.position, Quaternion.identity, objectsToPlace[i].transform);
		}
	}

	public void UpdateCondition()
	{
		if (itemsInArea > 0)
		{
			itemsInArea--;
		}
		else
		{
			QuestManager.instance.UpdateQuest(qName);
		}
	}

	public void GiftCondition()
	{
		GiftManager.instance.GiftQuestUI(qName, qGiftName);
	}
}
