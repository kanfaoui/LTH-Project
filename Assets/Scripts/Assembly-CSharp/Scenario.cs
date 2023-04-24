using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Scenario : MonoBehaviour
{
	public Animator anim;

	public DialogueTrigger[] dialogues;

	public int currentDialogue;

	public string partnerName;

	public int storyNum;

	public void Talk()
	{
		dialogues[currentDialogue].TriggerDialogue();
		currentDialogue++;
	}

	public void Next()
	{
		anim.SetTrigger("Next");
	}

	public void Leave()
	{
		GameManager instance = GameManager.instance;
		DialogueManager instance2 = DialogueManager.instance;
		Array.Find(instance.partners, _003CLeave_003Eb__7_0).doneStory[storyNum] = true;
		instance2.EndDialogue();
		SceneManager.LoadScene("House");
	}

	[CompilerGenerated]
	private bool _003CLeave_003Eb__7_0(PartnerStats p)
	{
		return p.partnerName == partnerName;
	}
}
