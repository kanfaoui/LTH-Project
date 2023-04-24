using System;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Intimacy : MonoBehaviour
{
	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass5_0
	{
		public HeartMode hManager;

		internal bool _003CAction_003Eb__0(PartnerStats pStats)
		{
			return pStats.partnerName == hManager.partner.partnerName;
		}
	}

	public Animator anim;

	public GameObject undressButton;

	public bool undressOnce;

	public GameObject sexButton;

	public GameObject[] stages;

	public void Action(string actionName)
	{
		_003C_003Ec__DisplayClass5_0 _003C_003Ec__DisplayClass5_ = new _003C_003Ec__DisplayClass5_0();
		GameManager instance = GameManager.instance;
		_003C_003Ec__DisplayClass5_.hManager = HeartMode.instance;
		PlayerHUD instance2 = PlayerHUD.instance;
		PartnerStats partnerStats = Array.Find(instance.partners, _003C_003Ec__DisplayClass5_._003CAction_003Eb__0);
		if (instance2.energyBar.value <= 0f && !partnerStats.doneIntimate)
		{
			Notify.instance.NotifyPlayer("Not enough enengy");
			return;
		}
		anim.SetTrigger(actionName);
		if (!partnerStats.doneIntimate)
		{
			instance2.energyBar.value -= 5f;
		}
		if (!(_003C_003Ec__DisplayClass5_.hManager.heartMeter.value >= 50f) || undressOnce)
		{
			_003C_003Ec__DisplayClass5_.hManager.heartMeter.value += 10f;
			if (_003C_003Ec__DisplayClass5_.hManager.heartMeter.value >= 50f && !undressOnce)
			{
				undressButton.SetActive(true);
			}
			if (_003C_003Ec__DisplayClass5_.hManager.heartMeter.value >= 100f)
			{
				partnerStats.doneIntimate = true;
				sexButton.SetActive(true);
			}
		}
	}

	public void Undress()
	{
		undressOnce = true;
	}

	public void ProceedH()
	{
		if (PlayerHUD.instance.energyBar.value < 25f)
		{
			Notify.instance.NotifyPlayer("Not enough energy");
			return;
		}
		for (int i = 0; i < stages.Length; i++)
		{
			stages[i].SetActive(false);
		}
		HeartMode instance = HeartMode.instance;
		instance.isIntimate = false;
		base.gameObject.SetActive(false);
		instance.ProceedHMode();
	}
}
