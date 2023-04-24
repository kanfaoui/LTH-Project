using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Conditional : MonoBehaviour
{
	public string qName;

	public void TriggerSound(string sound)
	{
		AudioManager.instance.PlayUIs(sound);
	}

	public void AllowCall(int toCall)
	{
		GameManager.instance.partners[toCall].canBeCalled = true;
	}

	public void SleepPlayer()
	{
		Lighting.instance.Sleep();
	}

	public void Work()
	{
		QuestManager instance = QuestManager.instance;
		Array.Find(instance.quests, _003CWork_003Eb__4_0);
		instance.UpdateQuest(qName);
		GameManager.instance.money += 100;
		PlayerHUD.instance.UpdateMoney();
		SceneManager.LoadScene("MINIGAME1");
	}

	public void ProceedQuest()
	{
		QuestManager instance = QuestManager.instance;
		Array.Find(instance.quests, _003CProceedQuest_003Eb__5_0);
		instance.UpdateQuest(qName);
	}

	public void Date()
	{
		DateManager.instance.DateArea(qName);
	}

	public void OpenMap()
	{
		DateManager instance = DateManager.instance;
		instance.MapUI();
		instance.ResetDate();
	}

	public void DateMinigame(string dateGame)
	{
		DateManager instance = DateManager.instance;
		if (instance.date)
		{
			instance.partnerSpawn = GameManager.instance.currentCalledPartner.transform.position;
			instance.playerSpawn = Player.instance.playerPrefab.transform.position;
			instance.onMinigame = true;
		}
		SceneManager.LoadScene(dateGame);
	}

	public void Destroy()
	{
		UnityEngine.Object.Destroy(base.gameObject);
	}

	[CompilerGenerated]
	private bool _003CWork_003Eb__4_0(Quest quest)
	{
		return quest.questName == qName;
	}

	[CompilerGenerated]
	private bool _003CProceedQuest_003Eb__5_0(Quest quest)
	{
		return quest.questName == qName;
	}
}
