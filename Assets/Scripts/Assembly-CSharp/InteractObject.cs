using UnityEngine;

public class InteractObject : MonoBehaviour
{
	public Animator anim;

	public GameObject roomObject;

	public DialogueTrigger dtrigger;

	public void Interact()
	{
		Player player = Object.FindObjectOfType<Player>();
		anim.SetTrigger("Interact");
		player.playerPrefab.gameObject.SetActive(false);
		if (dtrigger != null)
		{
			dtrigger.TriggerDialogue();
		}
		AudioManager.instance.PlayUIs("Select");
	}

	public void RevealModel()
	{
		Player player = Object.FindObjectOfType<Player>();
		anim.ResetTrigger("Interact");
		player.playerPrefab.gameObject.SetActive(true);
	}

	public void Switch()
	{
		if (roomObject.activeInHierarchy)
		{
			roomObject.SetActive(false);
		}
		else
		{
			roomObject.SetActive(true);
		}
		AudioManager.instance.PlayUIs("Select");
	}

	public void InteractGrass()
	{
		PlayerMovement playerMovement = Object.FindObjectOfType<PlayerMovement>();
		anim.SetTrigger("Interact");
		playerMovement.gameObject.SetActive(false);
		Object.FindObjectOfType<Notify>().NotifyPlayer("You touch grass.");
	}

	public void Sound(string toPlay)
	{
		AudioManager.instance.PlayUIs(toPlay);
	}

	public void Talk()
	{
		dtrigger.TriggerDialogue();
	}
}
