using TMPro;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
	public GameObject tutorialUI;

	public GameObject[] pages;

	public TextMeshProUGUI pageNumberText;

	public int currentPage;

	public void TutorialUI()
	{
		if (tutorialUI.activeInHierarchy)
		{
			tutorialUI.SetActive(false);
		}
		else
		{
			if (PlayerMovement.instance == null)
			{
				return;
			}
			PlayerHUD.instance.HideUIS();
			tutorialUI.SetActive(true);
			Arrow(-100);
		}
		AudioManager.instance.PlayUIs("Select");
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
}
