using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
	public static DialogueManager instance;

	public GameObject textCanvas;

	public GameObject textBackground;

	public TextMeshProUGUI dialogueText;

	public TextMeshProUGUI dialogueName;

	public Image dialogueSprite;

	public bool dialogueActive;

	[Header("Dialogue Details")]
	[Space]
	public int currentSentenceNumber;

	public string[] sentences;

	public string[] charName;

	public Sprite[] sprites;

	[Header("If dialogue has choices")]
	[Space]
	public bool hasChoices;

	public DialogueTrigger[] choices;

	public string[] choiceName;

	public DialogueChoice choiceButton;

	public GameObject choiceSpawner;

	private bool choiceActive;

	[Header("if there is to invoke from Script")]
	[Space]
	public MonoBehaviour script;

	public string invokeScript;

	private void Awake()
	{
		instance = this;
		sentences = new string[0];
		charName = new string[0];
		currentSentenceNumber = 0;
		choices = new DialogueTrigger[0];
		choiceName = new string[0];
		sprites = new Sprite[0];
	}

	private void LateUpdate()
	{
		if ((Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space)) && textCanvas.activeInHierarchy)
		{
			if (dialogueText.text != sentences[currentSentenceNumber - 1])
			{
				StopAllCoroutines();
				dialogueText.text = "";
				dialogueText.text = sentences[currentSentenceNumber - 1];
			}
			else if (!choiceActive)
			{
				DisplayNextSentence();
			}
		}
	}

	public void RevealBackground()
	{
		if (!textBackground.activeInHierarchy)
		{
			textBackground.SetActive(true);
		}
		else
		{
			textBackground.SetActive(false);
		}
	}

	public void StartDialogue(Dialogue dialogue, DialogueTrigger[] dChoices, bool ifChoices, string[] cName, MonoBehaviour theScript, string toInvoke)
	{
		if (dialogue.sentences.Length == 0)
		{
			EndDialogue();
		}
		if (Player.instance.playerPrefab.gameObject.activeInHierarchy)
		{
			PlayerMovement.instance.anim.SetFloat("Speed", 0f);
			PlayerMovement.instance.rb.velocity = new Vector2(0f, 0f);
			PlayerMovement.instance.movePlayer = false;
		}
		textCanvas.SetActive(true);
		hasChoices = ifChoices;
		choiceActive = false;
		dialogueActive = true;
		sentences = new string[0];
		charName = new string[0];
		sprites = new Sprite[0];
		currentSentenceNumber = 0;
		choices = new DialogueTrigger[0];
		choiceName = new string[0];
		script = null;
		invokeScript = null;
		sentences = new string[dialogue.sentences.Length];
		charName = new string[dialogue.name.Length];
		sprites = new Sprite[dialogue.sprite.Length];
		if (dChoices != null)
		{
			choices = new DialogueTrigger[dChoices.Length];
		}
		if (cName != null)
		{
			choiceName = new string[cName.Length];
		}
		script = theScript;
		invokeScript = toInvoke;
		for (int i = 0; i < dialogue.sentences.Length; i++)
		{
			sentences[i] = dialogue.sentences[i];
		}
		for (int j = 0; j < dialogue.name.Length; j++)
		{
			charName[j] = dialogue.name[j];
		}
		for (int k = 0; k < dialogue.sprite.Length; k++)
		{
			sprites[k] = dialogue.sprite[k];
		}
		if (dChoices != null)
		{
			for (int l = 0; l < dChoices.Length; l++)
			{
				choices[l] = dChoices[l];
			}
		}
		if (cName != null)
		{
			for (int m = 0; m < cName.Length; m++)
			{
				choiceName[m] = cName[m];
			}
		}
		DisplayNextSentence();
	}

	public void DisplayNextSentence()
	{
		if (currentSentenceNumber == sentences.Length)
		{
			if (!hasChoices)
			{
				EndDialogue();
				return;
			}
			choiceActive = true;
			for (int i = 0; i < choices.Length; i++)
			{
				DialogueChoice dialogueChoice = Object.Instantiate(choiceButton, choiceSpawner.transform);
				dialogueChoice.choiceName.text = choiceName[i];
				dialogueChoice.pos = i;
			}
		}
		else
		{
			StopAllCoroutines();
			StartCoroutine(TypeSentence(sentences[currentSentenceNumber]));
			dialogueName.text = charName[currentSentenceNumber];
			if (sprites[currentSentenceNumber] != null)
			{
				dialogueSprite.gameObject.SetActive(true);
				dialogueSprite.sprite = sprites[currentSentenceNumber];
			}
			else
			{
				dialogueSprite.gameObject.SetActive(false);
			}
			currentSentenceNumber++;
		}
	}

	private IEnumerator TypeSentence(string sentence)
	{
		dialogueText.text = "";
		char[] array = sentence.ToCharArray();
		foreach (char c in array)
		{
			dialogueText.text += c;
			yield return new WaitForSeconds(0.05f);
		}
	}

	public void EndDialogue()
	{
		if (Player.instance.playerPrefab.gameObject.activeInHierarchy)
		{
			PlayerMovement.instance.movePlayer = true;
		}
		if (script != null)
		{
			script.Invoke(invokeScript, 0f);
		}
		dialogueActive = false;
		textCanvas.SetActive(false);
	}
}
