using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
	public Dialogue dialogue;

	public DialogueTrigger[] choices;

	public string[] choiceName;

	public bool hasChoices;

	public MonoBehaviour theScript;

	public string toInvoke;

	public void TriggerDialogue()
	{
		DialogueManager.instance.StartDialogue(dialogue, choices, hasChoices, choiceName, theScript, toInvoke);
	//	ApplovinManager.Instance.FunctionToCall();
	}
}
