using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TextAnim : MonoBehaviour
{
	public int currentSentenceNumber;

	[NonReorderable]
	[TextArea(3, 10)]
	public string[] sentences;

	public TextMeshProUGUI dialogueText;

	public Animator anim;

	public void Next()
	{
		anim.SetTrigger("Next");
	}

	public void DisplayNextSentence()
	{
		StopAllCoroutines();
		StartCoroutine(TypeSentence(sentences[currentSentenceNumber]));
		currentSentenceNumber++;
	}

	public void Leave()
	{
		SceneManager.LoadScene("House");
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
}
