using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GopherManager : MonoBehaviour
{
	public static GopherManager instance;

	public float roundTimer;

	public Gopher obj;

	public bool[] posAvailable;

	public Transform[] spawnPos;

	public int score;

	public int scoreToBeat = 2000;

	public TextMeshProUGUI scoreText;

	public TextMeshProUGUI timeText;

	public GameObject tutorialUI;

	public float spawnTime;

	private float defaultSpawnTime = 1.5f;

	private bool fastSpawn;

	private bool superFastSpawn;

	public GameObject[] pages;

	public TextMeshProUGUI pageNumberText;

	public int currentPage;

	public void Awake()
	{
		instance = this;
		spawnTime = defaultSpawnTime;
		score = 0;
	}

	public void Start()
	{
		DateManager dateManager = DateManager.instance;
		if (dateManager.date)
		{
			dateManager.leaveButton.SetActive(false);
			dateManager.EnableChibi();
		}
	}

	private void Update()
	{
		roundTimer -= Time.deltaTime;
		spawnTime -= Time.deltaTime;
		timeText.text = "Timer: " + roundTimer.ToString("F0");
		if (roundTimer <= 30f && !fastSpawn)
		{
			defaultSpawnTime = 0.8f;
			fastSpawn = true;
		}
		if (roundTimer <= 10f && !superFastSpawn)
		{
			defaultSpawnTime = 0.5f;
			superFastSpawn = true;
		}
		if (roundTimer <= 0f)
		{
			Leave();
		}
		if (spawnTime <= 0f)
		{
			int num = Random.Range(0, spawnPos.Length);
			if (!posAvailable[num])
			{
				Random.Range(0, 100);
				Object.Instantiate(obj, spawnPos[num].position, Quaternion.identity, spawnPos[num]).spawnPos = num;
				posAvailable[num] = true;
				spawnTime = defaultSpawnTime;
			}
		}
		if (spawnTime < -1f)
		{
			spawnTime = defaultSpawnTime;
		}
	}

	public void TutorialUI()
	{
		if (!tutorialUI.activeInHierarchy)
		{
			tutorialUI.SetActive(true);
			Time.timeScale = 0f;
			Arrow(-100);
		}
		else
		{
			tutorialUI.SetActive(false);
			Time.timeScale = 1f;
		}
	}

	public void UpdateScore(int scoreAdd)
	{
		score += scoreAdd;
		if (score <= 0)
		{
			score = 0;
		}
		scoreText.text = "Score: " + score;
	}

	public void Leave()
	{
		DateManager dateManager = DateManager.instance;
		if (dateManager.date)
		{
			if (score >= scoreToBeat)
			{
				dateManager.UpdateImpressions(12);
			}
			else
			{
				dateManager.UpdateImpressions(9);
			}
		}
		PlayerHUD.instance.energyBar.value -= 20f;
		if (PlayerHUD.instance.energyBar.value <= 0f)
		{
			PlayerHUD.instance.energyBar.value = 0f;
		}
		dateManager.leaveButton.SetActive(true);
		dateManager.DisableChibi();
		SceneManager.LoadScene("Arcade");
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
