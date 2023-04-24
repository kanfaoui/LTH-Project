using System;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MassageManager : MonoBehaviour
{
	[Serializable]
	public class MassagePartner
	{
		public string pName;

		public Sprite pBackSprite;
	}

	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass17_0
	{
		public DateManager dManager;

		internal bool _003CStart_003Eb__0(MassagePartner m)
		{
			return m.pName == dManager.datePartnerName;
		}
	}

	public static MassageManager instance;

	public Slider hSlider;

	public int mStr;

	public int mPart;

	public float hValue;

	public float roundTimer;

	public TextMeshProUGUI timeText;

	[Header("Bodypart Changer")]
	[Space]
	public float timeChange;

	public int str;

	public TextMeshProUGUI queueText;

	public TextMeshProUGUI cText;

	public string strText;

	public bool oneTime;

	public Animator anim;

	[Header("Partner")]
	[Space]
	[NonReorderable]
	public MassagePartner[] mPartners;

	public SpriteRenderer backSprite;

	public GameObject[] pages;

	public TextMeshProUGUI pageNumberText;

	public int currentPage;

	public GameObject tutorialUI;

	public bool tutorial;

	private void Awake()
	{
		instance = this;
	}

	private void Start()
	{
		_003C_003Ec__DisplayClass17_0 _003C_003Ec__DisplayClass17_ = new _003C_003Ec__DisplayClass17_0();
		hSlider.value = 0f;
		_003C_003Ec__DisplayClass17_.dManager = DateManager.instance;
		if (_003C_003Ec__DisplayClass17_.dManager.date)
		{
			_003C_003Ec__DisplayClass17_.dManager.leaveButton.SetActive(false);
			MassagePartner massagePartner = Array.Find(mPartners, _003C_003Ec__DisplayClass17_._003CStart_003Eb__0);
			backSprite.sprite = massagePartner.pBackSprite;
		}
	}

	private void Update()
	{
		roundTimer -= Time.deltaTime;
		timeChange -= Time.deltaTime;
		timeText.text = "Timer: " + roundTimer.ToString("F0");
		if (roundTimer <= 0f)
		{
			Leave();
		}
		if (timeChange <= 0f)
		{
			mPart = UnityEngine.Random.Range(0, 5);
			mStr = UnityEngine.Random.Range(0, 2);
			timeChange = 7f;
			if (mStr == 0)
			{
				strText = "Softly";
			}
			else if (mStr == 1)
			{
				strText = "Harder";
			}
			TextAppearPart();
			oneTime = false;
		}
		if (Input.GetKeyDown(KeyCode.Q))
		{
			ChangeStr();
		}
	}

	public void ChangeStr()
	{
		if (str == 0)
		{
			str = 1;
			anim.speed = 1.5f;
			cText.text = "Hard";
		}
		else if (str == 1)
		{
			str = 0;
			anim.speed = 1f;
			cText.text = "Soft";
		}
		oneTime = false;
	}

	public void TextAppearPart()
	{
		switch (mPart)
		{
		case 0:
			queueText.text = "Left arm please do it " + strText;
			break;
		case 1:
			queueText.text = "My shoulders please do it " + strText;
			break;
		case 2:
			queueText.text = "Right arm please do it " + strText;
			break;
		case 3:
			queueText.text = "My waist please do it " + strText;
			break;
		case 4:
			queueText.text = "My hips please do it " + strText;
			break;
		default:
			Debug.Log("xDD");
			break;
		}
	}

	public void Massage(float increase)
	{
		if (mStr != str)
		{
			queueText.text = "Do it " + strText;
			return;
		}
		hValue += increase * 4f;
		hSlider.value = hValue;
		if (!oneTime)
		{
			switch (UnityEngine.Random.Range(0, 4))
			{
			case 0:
				queueText.text = "That feels nice";
				break;
			case 1:
				queueText.text = "You're really good at this";
				break;
			case 2:
				queueText.text = "Nghhh... Amazing";
				break;
			case 3:
				queueText.text = "Nhhh... So good";
				break;
			default:
				Debug.Log("xDD");
				break;
			}
			oneTime = true;
		}
	}

	public void Leave()
	{
		DateManager dateManager = DateManager.instance;
		if (dateManager.date)
		{
			if (hValue >= 100f)
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
		SceneManager.LoadScene("Beach");
		AudioManager.instance.PlayUIs("Inv_Select");
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
