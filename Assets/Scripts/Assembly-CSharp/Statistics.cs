using System;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Statistics : MonoBehaviour
{
	[Serializable]
	public class StatName
	{
		public string sName;

		public Image sHeadSprite;

		public TextMeshProUGUI sTextName;

		public Button sDetailButton;

		public DialogueTrigger dTrigger;
	}

	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass25_0
	{
		public string pName;

		internal bool _003CPartnerDetails_003Eb__0(PartnerStats p)
		{
			return p.partnerName == pName;
		}

		internal bool _003CPartnerDetails_003Eb__1(StatName s)
		{
			return s.sName == pName;
		}
	}

	[Serializable]
	[CompilerGenerated]
	private sealed class _003C_003Ec
	{
		public static readonly _003C_003Ec _003C_003E9 = new _003C_003Ec();

		public static Predicate<PartnerStats> _003C_003E9__27_0;

		internal bool _003CCallPartner_003Eb__27_0(PartnerStats p)
		{
			return p.partnerName == DateManager.instance.datePartnerName;
		}
	}

	public static Statistics instance;

	public GameObject statUI;

	public string partnerName;

	[Header("New Background")]
	[Space]
	public TextMeshProUGUI sAttitude;

	public TextMeshProUGUI sAge;

	public TextMeshProUGUI sLocation;

	public TextMeshProUGUI[] sLikes;

	public TextMeshProUGUI sDescription;

	public TextMeshProUGUI sHearts;

	public TextMeshProUGUI sNumDates;

	public Image sChibi;

	public GameObject callButton;

	public GameObject[] storyButton;

	public Image[] storyMark;

	public Sprite checkMark;

	public Sprite wrongMark;

	[Header("Stat")]
	[Space]
	[NonReorderable]
	public StatName[] stat;

	[Header("Mary")]
	[Space]
	public DialogueTrigger dTriggerMary;

	[Header("Eul")]
	[Space]
	public DialogueTrigger dTriggerEul;

	[Header("Caroline")]
	[Space]
	public DialogueTrigger dTriggerCarol;

	[Header("Caitlyn")]
	[Space]
	public DialogueTrigger dTriggerCait;

	[Header("Sherryl")]
	[Space]
	public DialogueTrigger dTriggerSherryl;

	private void Awake()
	{
		instance = this;
	}

	public void Start()
	{
		PartnerDetails("Mary");
	}

	public void StatUI()
	{
		if (statUI.activeInHierarchy)
		{
			statUI.SetActive(false);
		}
		else
		{
			if (PlayerMovement.instance == null)
			{
				return;
			}
			PlayerHUD.instance.HideUIS();
			statUI.SetActive(true);
			PartnerDetails("Mary");
		}
		AudioManager.instance.PlayUIs("Select");
	}

	public void PartnerDetails(string pName)
	{
		_003C_003Ec__DisplayClass25_0 _003C_003Ec__DisplayClass25_ = new _003C_003Ec__DisplayClass25_0();
		_003C_003Ec__DisplayClass25_.pName = pName;
		Partner partnerDetails = GameManager.instance.GetPartnerDetails(_003C_003Ec__DisplayClass25_.pName);
		sAge.text = "Age: " + partnerDetails.pAge;
		sAttitude.text = "Attitude: " + partnerDetails.pAttitude;
		sLocation.text = "Location: " + partnerDetails.pLocation;
		sDescription.text = partnerDetails.pDescription;
		sChibi.sprite = partnerDetails.pChibi;
		GiftManager instance2 = GiftManager.instance;
		GameManager gameManager = GameManager.instance;
		PartnerStats partnerStats = Array.Find(gameManager.partners, _003C_003Ec__DisplayClass25_._003CPartnerDetails_003Eb__0);
		for (int i = 0; i < partnerStats.doneStory.Length; i++)
		{
			if (!partnerStats.doneStory[i])
			{
				storyMark[i].sprite = wrongMark;
			}
			else
			{
				storyMark[i].sprite = checkMark;
			}
		}
		sNumDates.text = "Successful Dates: " + partnerStats.numDates;
		Array.Find(stat, _003C_003Ec__DisplayClass25_._003CPartnerDetails_003Eb__1);
		for (int j = 0; j < partnerStats.likedItem.Length; j++)
		{
			if (partnerStats.likedItem[j])
			{
				sLikes[j].text = partnerDetails.pLikes[j];
			}
			else
			{
				sLikes[j].text = "???";
			}
		}
		for (int k = 0; k < gameManager.partners.Length; k++)
		{
			if (gameManager.partners[k].canBeCalled)
			{
				stat[k].sHeadSprite.color = new Color(1f, 1f, 1f, 1f);
				stat[k].sTextName.text = stat[k].sName;
				stat[k].sDetailButton.interactable = true;
			}
			else
			{
				stat[k].sHeadSprite.color = Color.black;
				stat[k].sTextName.text = "???";
				stat[k].sDetailButton.interactable = false;
			}
		}
		partnerName = _003C_003Ec__DisplayClass25_.pName;
	}

	public void CallName()
	{
		if (DateManager.instance.date)
		{
			Notify.instance.NotifyPlayer("Date in progress");
			return;
		}
		GameManager instance2 = GameManager.instance;
		SceneObjectSpawner sceneObjectSpawner = SceneObjectSpawner.instance;
		statUI.SetActive(false);
		AudioManager.instance.PlayUIs("Inv_Select");
		if (UnityEngine.Object.FindObjectOfType<PartnerMovement>() != null)
		{
			Notify.instance.NotifyPlayer("Can't do that right now");
		}
		else if (UnityEngine.Object.FindObjectOfType<PositionsManager>() != null || UnityEngine.Object.FindObjectOfType<Intimacy>() != null)
		{
			Notify.instance.NotifyPlayer("Can't do that right now");
		}
		else if (partnerName == "Mary" && sceneObjectSpawner.allowMary)
		{
			dTriggerMary.TriggerDialogue();
		}
		else if (partnerName == "Eul" && sceneObjectSpawner.allowEul)
		{
			dTriggerEul.TriggerDialogue();
		}
		else if (partnerName == "Caroline" && sceneObjectSpawner.allowCarol)
		{
			dTriggerCarol.TriggerDialogue();
		}
		else if (partnerName == "Caitlyn" && sceneObjectSpawner.allowCait)
		{
			dTriggerCait.TriggerDialogue();
		}
		else if (partnerName == "Sherryl" && sceneObjectSpawner.allowSherryl)
		{
			dTriggerSherryl.TriggerDialogue();
		}
		else
		{
			Notify.instance.NotifyPlayer("Can't do that right now");
		}
	}

	public void CallPartner()
	{
		GameManager gameManager = GameManager.instance;
		SceneObjectSpawner sceneObjectSpawner = SceneObjectSpawner.instance;
		if (DateManager.instance.date)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate(Array.Find(gameManager.partners, _003C_003Ec._003C_003E9__27_0 ?? (_003C_003Ec._003C_003E9__27_0 = _003C_003Ec._003C_003E9._003CCallPartner_003Eb__27_0)).partnerPrefab, sceneObjectSpawner.coordinatesToSpawn, base.transform.rotation, gameManager.transform.parent);
			gameManager.currentCalledPartner = gameObject.GetComponent<PartnerMovement>();
		}
		else
		{
			GameObject gameObject2 = UnityEngine.Object.Instantiate(Array.Find(gameManager.partners, _003CCallPartner_003Eb__27_1).partnerPrefab, sceneObjectSpawner.coordinatesToSpawn, base.transform.rotation, gameManager.transform.parent);
			gameManager.currentCalledPartner = gameObject2.GetComponent<PartnerMovement>();
		}
	}

	public void Story(int storyNum)
	{
		PlayerMovement.instance.stayPos = false;
		if (DateManager.instance.date)
		{
			Notify.instance.NotifyPlayer("Date in progress");
			return;
		}
		if (UnityEngine.Object.FindObjectOfType<PositionsManager>() != null || UnityEngine.Object.FindObjectOfType<Intimacy>() != null)
		{
			Notify.instance.NotifyPlayer("Can't do that right now");
			return;
		}
		if (SceneManager.GetActiveScene().name != "House")
		{
			Notify.instance.NotifyPlayer("Must be in the House");
			return;
		}
		GameManager gameManager = GameManager.instance;
		Partner partnerDetails = gameManager.GetPartnerDetails(partnerName);
		PartnerStats partnerStats = Array.Find(gameManager.partners, _003CStory_003Eb__28_0);
		if (partnerStats.numDates >= partnerDetails.pReqStory[storyNum])
		{
			if (storyNum != 0 && !partnerStats.doneStory[storyNum - 1])
			{
				Notify.instance.NotifyPlayer("Finish the previous story");
				return;
			}
			SceneManager.LoadScene(partnerDetails.pStory[storyNum]);
			statUI.SetActive(false);
		}
		else
		{
			Notify.instance.NotifyPlayer("Need " + partnerDetails.pReqStory[storyNum] + " successful dates");
		}
	}

	[CompilerGenerated]
	private bool _003CCallPartner_003Eb__27_1(PartnerStats p)
	{
		return p.partnerName == partnerName;
	}

	[CompilerGenerated]
	private bool _003CStory_003Eb__28_0(PartnerStats p)
	{
		return p.partnerName == partnerName;
	}
}
