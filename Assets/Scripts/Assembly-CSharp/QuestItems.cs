using UnityEngine;
using UnityEngine.UI;

public class QuestItems : MonoBehaviour
{
	public string conditionName;

	public Button button;

	public void UpdateCondition()
	{
		QuestConditions[] array = (QuestConditions[])Object.FindObjectsOfType(typeof(QuestConditions));
		for (int i = 0; i < array.Length; i++)
		{
			if (array[i].conditionName == conditionName)
			{
				array[i].UpdateCondition();
				Object.Destroy(base.gameObject);
			}
		}
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
		}
	}
}
