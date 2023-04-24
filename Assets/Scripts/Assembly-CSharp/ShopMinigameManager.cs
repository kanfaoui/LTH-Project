using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ShopMinigameManager : MonoBehaviour
{
	public static ShopMinigameManager instance;

	public bool bMode;

	public ShopCustomer customer;

	public bool[] posAvailable;

	public Transform[] spawnPos;

	public float spawnTime;

	public int score;

	public bool tutorial;

	[Space]
	[Header("Canvas")]
	public Image indicatorXRay;

	public TextMeshProUGUI scoreText;

	public GameObject tutorialUI;

	public GameObject[] pages;

	public TextMeshProUGUI pageNumberText;

	public int currentPage;

	public void Start()
	{
		instance = this;
		UpdateScore(0);
	}

	public void Update()
	{
		spawnTime -= Time.deltaTime;
		if (spawnTime <= 0f)
		{
			int num = Random.Range(0, spawnPos.Length);
			if (!posAvailable[num])
			{
				int num2 = Random.Range(0, 100);
				ShopCustomer shopCustomer = Object.Instantiate(customer, spawnPos[num].position, Quaternion.identity, spawnPos[num]);
				if (num2 <= 15)
				{
					shopCustomer.thief = true;
				}
				shopCustomer.spawnPos = num;
				posAvailable[num] = true;
				spawnTime = 3f;
			}
		}
		if (spawnTime < -1f)
		{
			spawnTime = 3f;
		}
		if (Input.GetKeyDown(KeyCode.Q))
		{
			BonkMode();
		}
	}

	public void UpdateScore(int scoreAdd)
	{
		score += scoreAdd;
		if (score <= 0)
		{
			score = 0;
		}
		scoreText.text = score.ToString();
	}

	public void TutorialUI()
	{
		if (!tutorialUI.activeInHierarchy)
		{
			tutorialUI.SetActive(true);
			Time.timeScale = 0f;
			tutorial = true;
			Arrow(-100);
		}
		else
		{
			tutorialUI.SetActive(false);
			Time.timeScale = 1f;
			tutorial = false;
		}
	}

	public void BonkMode()
	{
		if (bMode)
		{
			indicatorXRay.color = Color.red;
			bMode = false;
		}
		else
		{
			indicatorXRay.color = Color.green;
			bMode = true;
		}
		AudioManager.instance.PlayUIs("Inv_Select");
	}

	public void LeaveMiniGame()
	{
		Lighting.instance.preLighting = true;
		GameManager.instance.money += score;
		PlayerHUD.instance.UpdateMoney();
		PlayerHUD.instance.energyBar.value -= 20f;
		if (PlayerHUD.instance.energyBar.value <= 0f)
		{
			PlayerHUD.instance.energyBar.value = 0f;
		}
		SceneManager.LoadScene("Gen.Store");
		AudioManager.instance.PlayUIs("Inv_Select");
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
