using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DateMinigame : MonoBehaviour
{
	public string gameName;

	public void Dategame(string dateGame)
	{
		GameManager instance2 = GameManager.instance;
		DateManager instance = DateManager.instance;
		if (!instance.date)
		{
			Notify.instance.NotifyPlayer("Date only");
			return;
		}
		PartnerMovement partnerMovement = UnityEngine.Object.FindObjectOfType<PartnerMovement>();
		if (!partnerMovement.followPlayer)
		{
			Notify.instance.NotifyPlayer("Partner must be following");
			return;
		}
		PlayerHUD.instance.HideUIS();
		DateChecker dateChecker = Array.Find(instance.dateCheckers, _003CDategame_003Eb__1_0);
		if (dateChecker.isDone && instance.date)
		{
			partnerMovement.anim.SetTrigger("Deny");
			return;
		}
		if (instance.date)
		{
			instance.partnerSpawn = GameManager.instance.currentCalledPartner.transform.position;
			instance.playerSpawn = Player.instance.playerPrefab.transform.position;
			instance.onMinigame = true;
		}
		dateChecker.isDone = true;
		SceneManager.LoadScene(dateGame);
	}

	[CompilerGenerated]
	private bool _003CDategame_003Eb__1_0(DateChecker dCheck)
	{
		return dCheck.dateGame == gameName;
	}
}
