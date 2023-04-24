using System;
using System.Runtime.CompilerServices;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass3_0
	{
		public string qName;

		internal bool _003CUpdateQuest_003Eb__0(Quest quest)
		{
			return quest.questName == qName;
		}
	}

	public static QuestManager instance;

	[NonReorderable]
	public Quest[] quests;

	private void Awake()
	{
		instance = this;
	}

	public void UpdateQuest(string qName)
	{
		_003C_003Ec__DisplayClass3_0 _003C_003Ec__DisplayClass3_ = new _003C_003Ec__DisplayClass3_0();
		_003C_003Ec__DisplayClass3_.qName = qName;
		Array.Find(quests, _003C_003Ec__DisplayClass3_._003CUpdateQuest_003Eb__0).questCurrent++;
		QuestTrigger[] array = (QuestTrigger[])UnityEngine.Object.FindObjectsOfType(typeof(QuestTrigger));
		for (int i = 0; i < array.Length; i++)
		{
			if (array[i].qName == _003C_003Ec__DisplayClass3_.qName)
			{
				array[i].UpdateScene();
			}
		}
	}
}
