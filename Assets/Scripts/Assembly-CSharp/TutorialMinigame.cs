using TMPro;
using UnityEngine;

public class TutorialMinigame : MonoBehaviour
{
	public static TutorialMinigame instance;

	public GameObject tutorialUI;

	public GameObject[] pages;

	public TextMeshProUGUI pageNumberText;

	public int currentPage;

	public bool onTutorial;

	public void Awake()
	{
		instance = this;
	}

	public void Arrow(int toMove)
	{
		currentPage += toMove;
		if (currentPage <= 0)
		{
			currentPage = 0;
		}
		if (currentPage >= pages.Length)
		{
			currentPage = pages.Length - 1;
		}
		for (int i = 0; i < pages.Length; i++)
		{
			pages[i].SetActive(false);
		}
		pageNumberText.text = currentPage + 1 + " / " + pages.Length;
		pages[currentPage].SetActive(true);
	}

	public void TutorialUI()
	{
		if (!tutorialUI.activeInHierarchy)
		{
			tutorialUI.SetActive(true);
			Time.timeScale = 0f;
			Arrow(-100);
			onTutorial = true;
		}
		else
		{
			tutorialUI.SetActive(false);
			Time.timeScale = 1f;
			onTutorial = false;
		}
	}
}
