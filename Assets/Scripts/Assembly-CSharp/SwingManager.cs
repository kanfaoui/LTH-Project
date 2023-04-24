using System;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SwingManager : MonoBehaviour
{
	[Serializable]
	public class SwingPartner
	{
		public string pName;

		public GameObject pSwingObj;
	}

	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass21_0
	{
		public DateManager dManager;

		internal bool _003CStart_003Eb__0(SwingPartner m)
		{
			return m.pName == dManager.datePartnerName;
		}
	}

	public static SwingManager instance;

	public string[] arrowQ;

	public int queue;

	public int queueDiff;

	public float arrowTimer;

	public float roundTimer;

	public TextMeshProUGUI roundTimeText;

	public int score;

	public int scoreToBeat = 2500;

	public TextMeshProUGUI scoreText;

	public Slider timeSlider;

	[Header("Swing Arrow")]
	[Space]
	public Sprite[] arrowSprite;

	public SwingArrow arrowObj;

	public SwingArrow[] arrowDisplay;

	public GameObject arrowSpawn;

	public HorizontalLayoutGroup group;

	[Header("Swing Animation")]
	[Space]
	public Animator swingAnim;

	public bool isPushing;

	public bool isKO;

	[Header("Partner")]
	[Space]
	[NonReorderable]
	public SwingPartner[] sPartners;

	public void Awake()
	{
		instance = this;
	}

	public void Start()
	{
		_003C_003Ec__DisplayClass21_0 _003C_003Ec__DisplayClass21_ = new _003C_003Ec__DisplayClass21_0();
		_003C_003Ec__DisplayClass21_.dManager = DateManager.instance;
		if (_003C_003Ec__DisplayClass21_.dManager.date)
		{
			_003C_003Ec__DisplayClass21_.dManager.leaveButton.SetActive(false);
			Array.Find(sPartners, _003C_003Ec__DisplayClass21_._003CStart_003Eb__0).pSwingObj.SetActive(true);
		}
		ChangeArrowQ(4);
		score = 0;
	}

	public void Update()
	{
		roundTimer -= Time.deltaTime;
		roundTimeText.text = "Timer: " + roundTimer.ToString("F0");
		if (roundTimer <= 0f)
		{
			Leave();
		}
		if (isPushing || isKO)
		{
			return;
		}
		arrowTimer -= Time.deltaTime;
		timeSlider.value = arrowTimer;
		if (arrowTimer <= 0f)
		{
			queueDiff = 0;
			ChangeArrowQ(4);
		}
		if (!TutorialMinigame.instance.onTutorial)
		{
			if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
			{
				ArrowInput("w");
			}
			if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
			{
				ArrowInput("s");
			}
			if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
			{
				ArrowInput("a");
			}
			if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
			{
				ArrowInput("d");
			}
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

	public void ArrowInput(string input)
	{
		group.enabled = false;
		if (arrowQ[queue] == input)
		{
			arrowDisplay[queue].Destroy();
			queue++;
			if (queue >= arrowQ.Length)
			{
				AudioManager.instance.PlayUIs("Points");
				isPushing = true;
				swingAnim.SetTrigger("Push");
				UpdateScore(50 + 50 * queueDiff);
				queueDiff++;
			}
		}
		else
		{
			AudioManager.instance.PlayUIs("Deduct");
			isKO = true;
			swingAnim.SetTrigger("Push");
			swingAnim.SetTrigger("KO");
		}
	}

	public void ChangeArrowQ(int arrowNum)
	{
		for (int i = 0; i < arrowDisplay.Length; i++)
		{
			if (arrowDisplay[i] != null)
			{
				arrowDisplay[i].Destroy();
			}
		}
		arrowQ = new string[0];
		queue = 0;
		arrowDisplay = new SwingArrow[0];
		arrowTimer = 4f;
		timeSlider.maxValue = arrowTimer;
		timeSlider.value = arrowTimer;
		if (arrowNum + queueDiff >= 7)
		{
			arrowQ = new string[7];
			arrowDisplay = new SwingArrow[7];
		}
		else
		{
			arrowQ = new string[arrowNum + queueDiff];
			arrowDisplay = new SwingArrow[arrowNum + queueDiff];
		}
		group.enabled = true;
		for (int j = 0; j < arrowQ.Length; j++)
		{
			int num = UnityEngine.Random.Range(0, 4);
			SwingArrow swingArrow = UnityEngine.Object.Instantiate(arrowObj, arrowSpawn.transform.position, Quaternion.identity, arrowSpawn.transform);
			swingArrow.arwImage.sprite = arrowSprite[num];
			arrowDisplay[j] = swingArrow;
			switch (num)
			{
			case 0:
				arrowQ[j] = "w";
				break;
			case 1:
				arrowQ[j] = "s";
				break;
			case 2:
				arrowQ[j] = "a";
				break;
			case 3:
				arrowQ[j] = "d";
				break;
			default:
				Debug.Log("xDD");
				break;
			}
		}
	}

	public void Finish()
	{
		isPushing = false;
		if (!isKO)
		{
			ChangeArrowQ(4);
		}
	}

	public void FinishKO()
	{
		isPushing = false;
		isKO = false;
		queueDiff = 0;
		ChangeArrowQ(4);
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
		SceneManager.LoadScene("Park");
		AudioManager.instance.PlayUIs("Inv_Select");
	}
}
