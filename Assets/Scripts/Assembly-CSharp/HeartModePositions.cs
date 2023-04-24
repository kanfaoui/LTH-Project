using System;
using System.Runtime.CompilerServices;
using UnityEngine;

public class HeartModePositions : MonoBehaviour
{
	[Serializable]
	public class PartnerAvail
	{
		public string pName;

		public Intimacy partnerInt;

		public PositionsManager partnerPos;
	}

	public PartnerMovement partner;

	public PositionsManager pInstant;

	public GameObject[] removedInstants;

	[Space]
	[Header("Pos Avail")]
	[NonReorderable]
	public PartnerAvail[] pAvail;

	[Space]
	[Header("H Allower")]
	public bool dayH = true;

	public bool eveH = true;

	public bool nightH = true;

	[Space]
	[Header("D Trigger")]
	public DialogueTrigger maryDeny;

	public DialogueTrigger eulDeny;

	[Space]
	[Header("Instant Dialogue")]
	public PositionsManager instantPos;

	public string partnerName;

	public DialogueTrigger instantDeny;

	public void NewStartH()
	{
		GameManager instance = GameManager.instance;
		GiftManager instance3 = GiftManager.instance;
		HeartMode instance2 = HeartMode.instance;
		int currentState = Lighting.instance.currentState;
		partner = UnityEngine.Object.FindObjectOfType<PartnerMovement>();
		if (PlayerHUD.instance.energyBar.value <= 20f)
		{
			Notify.instance.NotifyPlayer("Not enough energy");
			return;
		}
		if (UnityEngine.Object.FindObjectOfType<PartnerMovement>() == null)
		{
			Notify.instance.NotifyPlayer("You must have a partner.");
			return;
		}
		if (!partner.followPlayer)
		{
			Notify.instance.NotifyPlayer("Partner must be following you.");
			return;
		}
		if (currentState == 0 && !dayH)
		{
			if (partner.partnerName == "Mary")
			{
				maryDeny.TriggerDialogue();
			}
			else if (partner.partnerName == "Eul")
			{
				eulDeny.TriggerDialogue();
			}
			partner.anim.SetTrigger("Deny");
			return;
		}
		PartnerAvail partnerAvail = Array.Find(pAvail, _003CNewStartH_003Eb__12_0);
		if (partnerAvail == null)
		{
			Notify.instance.NotifyPlayer("No available positions");
			return;
		}
		PartnerStats partnerStats = Array.Find(instance.partners, _003CNewStartH_003Eb__12_1);
		bool flag = true;
		for (int i = 0; i < partnerStats.doneStory.Length; i++)
		{
			if (!partnerStats.doneStory[i])
			{
				flag = false;
			}
		}
		if (flag)
		{
			if (partnerAvail.partnerInt != null)
			{
				instance2.isIntimate = true;
				instance2.heartMeter.value = 0f;
				instance2.intimacy = partnerAvail.partnerInt;
				partnerAvail.partnerInt.gameObject.SetActive(true);
				partnerAvail.partnerInt.sexButton.SetActive(false);
				partnerAvail.partnerInt.undressButton.SetActive(false);
				partnerAvail.partnerInt.undressOnce = false;
				for (int j = 0; j < partnerAvail.partnerInt.stages.Length; j++)
				{
					partnerAvail.partnerInt.stages[j].SetActive(false);
				}
				if (partnerStats.doneIntimate)
				{
					partnerAvail.partnerInt.undressOnce = true;
					partnerAvail.partnerInt.undressButton.SetActive(true);
					partnerAvail.partnerInt.sexButton.SetActive(true);
				}
			}
			else
			{
				instance2.isIntimate = false;
				instance2.intimacy = null;
			}
			instance2.posManager = partnerAvail.partnerPos;
			instance2.partner = partner;
			instance2.ProceedHMode();
		}
		else
		{
			Notify.instance.NotifyPlayer("Finish all her stories");
			partner.anim.SetTrigger("Deny");
		}
	}

	public void InstantHMode()
	{
		HeartMode instance = HeartMode.instance;
		GameManager instance2 = GameManager.instance;
		if (PlayerHUD.instance.energyBar.value <= 20f)
		{
			Notify.instance.NotifyPlayer("Not enough energy");
			return;
		}
		if (UnityEngine.Object.FindObjectOfType<PartnerMovement>() != null)
		{
			Notify.instance.NotifyPlayer("There must be no partner in the area");
			return;
		}
		bool flag = true;
		for (int i = 0; i < instance2.partners.Length; i++)
		{
			for (int j = 0; j < instance2.partners[i].doneStory.Length; j++)
			{
				if (!instance2.partners[i].doneStory[j])
				{
					flag = false;
				}
			}
		}
		if (!flag)
		{
			Notify.instance.NotifyPlayer("Requires all partner's story finished");
			return;
		}
		for (int k = 0; k < removedInstants.Length; k++)
		{
			if (removedInstants[k] != null)
			{
				removedInstants[k].SetActive(false);
			}
		}
		instance.posManager = pInstant;
		instance.ProceedHMode();
		instance.isInstant = true;
		instance.hModePos = this;
	}

	public void InstantHModeDialogue()
	{
		HeartMode instance = HeartMode.instance;
		if (PlayerHUD.instance.energyBar.value <= 20f)
		{
			Notify.instance.NotifyPlayer("Not enough energy");
			return;
		}
		if (UnityEngine.Object.FindObjectOfType<PartnerMovement>() != null)
		{
			Notify.instance.NotifyPlayer("There must be no partner in the area");
			return;
		}
		PartnerStats partnerStats = Array.Find(GameManager.instance.partners, _003CInstantHModeDialogue_003Eb__14_0);
		bool flag = true;
		for (int i = 0; i < partnerStats.doneStory.Length; i++)
		{
			if (!partnerStats.doneStory[i])
			{
				flag = false;
			}
		}
		if (!flag)
		{
			instantDeny.TriggerDialogue();
			return;
		}
		for (int j = 0; j < removedInstants.Length; j++)
		{
			if (removedInstants[j] != null)
			{
				removedInstants[j].SetActive(false);
			}
		}
		instance.posManager = instantPos;
		instance.ProceedHMode();
		instance.isInstant = true;
		instance.hModePos = this;
		//ApplovinManager.Instance.FunctionToCall();
	}

	[CompilerGenerated]
	private bool _003CNewStartH_003Eb__12_0(PartnerAvail p)
	{
		return p.pName == partner.partnerName;
	}

	[CompilerGenerated]
	private bool _003CNewStartH_003Eb__12_1(PartnerStats pStats)
	{
		return pStats.partnerName == partner.partnerName;
	}

	[CompilerGenerated]
	private bool _003CInstantHModeDialogue_003Eb__14_0(PartnerStats p)
	{
		return p.partnerName == partnerName;
	}
}
