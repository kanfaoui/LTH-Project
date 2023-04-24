using TMPro;
using UnityEngine;

public class DialogueChoice : MonoBehaviour
{
	public TextMeshProUGUI choiceName;

	public int pos;

	public void SelectButton()
	{
		DialogueManager instance = DialogueManager.instance;
		DialogueChoice[] array = (DialogueChoice[])Object.FindObjectsOfType(typeof(DialogueChoice));
		for (int i = 0; i < array.Length; i++)
		{
			array[i].DestroyObject();
		}
		instance.choices[pos].TriggerDialogue();
	}

	public void DestroyObject()
	{
		Object.Destroy(base.gameObject);
	}
}
