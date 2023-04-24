using System;
using System.Runtime.CompilerServices;
using UnityEngine;

public class QuestTrigger : MonoBehaviour
{
	public string qName;

	public GameObject[] cutscenes;

	private void Start()
	{
		UpdateScene();
	}

	public void UpdateScene()
	{
		Quest quest = Array.Find(QuestManager.instance.quests, _003CUpdateScene_003Eb__3_0);
		for (int i = 0; i < cutscenes.Length; i++)
		{
			if (cutscenes[i] != null)
			{
				cutscenes[i].gameObject.SetActive(false);
			}
		}
		if (quest.questCurrent < cutscenes.Length)
		{
			cutscenes[quest.questCurrent].gameObject.SetActive(true);
		}
		else
		{
			cutscenes[cutscenes.Length - 1].gameObject.SetActive(true);
		}
	}

	[CompilerGenerated]
	private bool _003CUpdateScene_003Eb__3_0(Quest quest)
	{
		return quest.questName == qName;
	}
}
