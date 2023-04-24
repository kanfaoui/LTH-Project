using Cinemachine;
using UnityEngine;
using UnityEngine.UI;

public class HeartMode : MonoBehaviour
{
	public static HeartMode instance;

	public GameObject heartModeUI;

	public bool isInstant;

	public bool isIntimate;

	public PositionsManager posManager;

	public Intimacy intimacy;

	public GameObject[] buttonPos;

	public GameObject[] textPos;

	[Header("Indicators")]
	[Space]
	public Image indicatorXRay;

	public Image indicatorXModel;

	public Image indicatorUnlockCamera;

	[Header("For Movement Anim")]
	[Space]
	public AnimationClip anim;

	[Header("Miscellaneous")]
	[Space]
	public Slider unlockCameraSlider;

	public Slider playerTransparencySlider;

	public Slider climaxMeter;

	public Slider heartMeter;

	public GameObject posToReveal;

	public GameObject optionsToReveal;

	public Button[] buttons;

	public int speedType;

	public bool justClimaxed;

	public int currentSpeed;

	public int changedSpeed;

	public float secToChangeSpeed;

	[Header("Objects")]
	public PlayerMovement player;

	public CinemachineVirtualCamera vCam;

	public SecondaryCamera sCam;

	public Camera cam;

	public PlayerHUD pHud;

	public PartnerMovement partner;

	public HeartModePositions hModePos;

	private void Awake()
	{
		instance = this;
	}

	private void Start()
	{
		heartModeUI.SetActive(true);
		player = Player.instance.playerPrefab;
		partner = Object.FindObjectOfType<PartnerMovement>();
		pHud = PlayerHUD.instance;
		heartModeUI.SetActive(false);
	}

	public void ProceedHMode()
	{
		GameManager gameManager = GameManager.instance;
		DateManager dateManager = DateManager.instance;
		if (dateManager.date)
		{
			dateManager.leaveButton.SetActive(false);
		}
		heartModeUI.SetActive(true);
		player.gameObject.SetActive(false);
		climaxMeter.gameObject.SetActive(true);
		heartMeter.gameObject.SetActive(false);
		if (partner != null)
		{
			partner.gameObject.SetActive(false);
		}
		indicatorXModel.color = Color.red;
		playerTransparencySlider.gameObject.SetActive(false);
		indicatorXRay.color = Color.red;
		if (isIntimate)
		{
			heartMeter.gameObject.SetActive(true);
			climaxMeter.gameObject.SetActive(false);
			for (int i = 0; i < buttons.Length; i++)
			{
				buttons[i].interactable = false;
			}
			return;
		}
		for (int j = 0; j < buttons.Length; j++)
		{
			buttons[j].interactable = true;
		}
		for (int k = 0; k < buttonPos.Length; k++)
		{
			buttonPos[k].SetActive(false);
			buttonPos[k].GetComponent<Button>().interactable = true;
			textPos[k].SetActive(false);
		}
		for (int l = 0; l < posManager.currentAnim.Length; l++)
		{
			buttonPos[l].SetActive(true);
		}
		if (gameManager.patreonOnly)
		{
			for (int m = 0; m < posManager.currentAnim.Length; m++)
			{
				if (posManager.positionsAllower[m])
				{
					buttonPos[m].GetComponent<Button>().interactable = false;
					textPos[m].SetActive(true);
				}
			}
		}
		posManager.gameObject.SetActive(true);
		ChoosePos(0);
		for (int n = 0; n < posManager.currentAnim.Length; n++)
		{
			posManager.currentAnim[n].gameObject.SetActive(false);
		}
		justClimaxed = false;
		posManager.currentAnimModel[0].SetActive(true);
		posManager.currentAnimModel[posManager.currentPos].GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
		posManager.currentAnim[0].gameObject.SetActive(true);
		anim = posManager.currentAnim[0].GetCurrentAnimatorClipInfo(0)[0].clip;
		AudioManager.instance.PlayUIs("Select");
	}

	public void ExitHeartMode()
	{
		DateManager dateManager = DateManager.instance;
		if (dateManager.date)
		{
			dateManager.leaveButton.SetActive(true);
		}
		if (sCam.unlockCamera)
		{
			UnlockCamera();
		}
		if (speedType != 0)
		{
			Speed(0);
		}
		climaxMeter.value = 0f;
		if (posManager.gameObject.activeInHierarchy)
		{
			posManager.currentAnim[posManager.currentPos].ResetTrigger("Climax");
			posManager.currentAnim[posManager.currentPos].ResetTrigger("Reset");
		}
		if ((bool)intimacy)
		{
			intimacy.gameObject.SetActive(false);
		}
		posManager.gameObject.SetActive(false);
		posToReveal.SetActive(false);
		optionsToReveal.SetActive(false);
		heartModeUI.SetActive(false);
		player.gameObject.SetActive(true);
		if (partner != null)
		{
			partner.gameObject.SetActive(true);
		}
		if (isInstant)
		{
			for (int i = 0; i < hModePos.removedInstants.Length; i++)
			{
				if (hModePos.removedInstants[i] != null)
				{
					hModePos.removedInstants[i].SetActive(true);
				}
			}
			hModePos = null;
			isInstant = false;
		}
		AudioManager.instance.PlayUIs("Select");
	}

	private void Update()
	{
		if (!isIntimate && heartModeUI.activeInHierarchy)
		{
			if ((climaxMeter.value < 1f && changedSpeed == currentSpeed) || posManager.fText[posManager.currentPos] == null)
			{
				climaxMeter.value += Time.deltaTime * 0.05f;
			}
			if (secToChangeSpeed > 0f && !justClimaxed)
			{
				secToChangeSpeed -= Time.deltaTime * 0.5f;
			}
			if (secToChangeSpeed <= 0f)
			{
				ChangeSpeed();
			}
		}
	}

	public void ChangeSpeed()
	{
		if (!isIntimate && !(posManager.fText[posManager.currentPos] == null))
		{
			int num = (changedSpeed = Random.Range(0, 3));
			secToChangeSpeed = Random.Range(2, 5);
			switch (num)
			{
			case 0:
				posManager.fText[posManager.currentPos].SpawnTextSignal("Normal&");
				break;
			case 1:
				posManager.fText[posManager.currentPos].SpawnTextSignal("Fast&");
				break;
			case 2:
				posManager.fText[posManager.currentPos].SpawnTextSignal("Faster&");
				break;
			}
		}
	}

	public void ChoosePos(int Position)
	{
		if (pHud.energyBar.value <= 20f)
		{
			Notify.instance.NotifyPlayer("Not enough energy");
		}
		else if (Position != posManager.currentPos)
		{
			if (speedType != 0)
			{
				Speed(0);
			}
			climaxMeter.value = 0f;
			justClimaxed = false;
			posManager.currentPos = Position;
			for (int i = 0; i < posManager.currentAnim.Length; i++)
			{
				posManager.currentAnim[i].gameObject.SetActive(false);
			}
			posManager.currentAnimModel[Position].SetActive(true);
			playerTransparencySlider.gameObject.SetActive(false);
			indicatorXModel.color = Color.red;
			posManager.currentAnimModel[posManager.currentPos].GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
			posManager.currentAnim[Position].gameObject.SetActive(true);
			anim = posManager.currentAnim[Position].GetCurrentAnimatorClipInfo(0)[0].clip;
			indicatorXRay.color = Color.red;
			if (posManager.currentAnimXRay[posManager.currentPos] != null && posManager.currentAnimXRay[posManager.currentPos].activeInHierarchy)
			{
				posManager.currentAnimXRay[posManager.currentPos].SetActive(false);
			}
			AudioManager.instance.PlayUIs("Select");
		}
	}

	public void Climax()
	{
		if (!justClimaxed && climaxMeter.value >= 1f)
		{
			posManager.currentAnim[posManager.currentPos].SetTrigger("Climax");
			posManager.currentAnim[posManager.currentPos].ResetTrigger("Reset");
			if (posManager.fText[posManager.currentPos] != null)
			{
				posManager.fText[posManager.currentPos].SpawnTextSignal("Ahhhhnnnnnnn!&");
			}
			if (speedType != 0)
			{
				Speed(0);
			}
			pHud.energyBar.value -= 10f;
			justClimaxed = true;
			AudioManager.instance.PlayUIs("Select");
			DateManager dateManager = DateManager.instance;
			if (dateManager.date)
			{
				dateManager.UpdateImpressions(3);
				pHud.energyBar.value -= 15f;
			}
		}
	}

	public void ResetMovement()
	{
		if (!(pHud.energyBar.value < 20f))
		{
			justClimaxed = false;
			if (speedType != 0)
			{
				Speed(0);
			}
			climaxMeter.value = 0f;
			posManager.currentAnim[posManager.currentPos].ResetTrigger("Climax");
			posManager.currentAnim[posManager.currentPos].SetTrigger("Reset");
			AudioManager.instance.PlayUIs("Select");
		}
	}

	public void RemoveMc()
	{
		if (!isIntimate)
		{
			playerTransparencySlider.value = 1f;
			posManager.currentAnimModel[posManager.currentPos].GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
			if (playerTransparencySlider.gameObject.activeInHierarchy)
			{
				playerTransparencySlider.gameObject.SetActive(false);
				indicatorXModel.color = Color.red;
			}
			else
			{
				playerTransparencySlider.gameObject.SetActive(true);
				indicatorXModel.color = Color.green;
			}
			AudioManager.instance.PlayUIs("Select");
		}
	}

	public void SetlevelTransparency(float sliderValue)
	{
		if (heartModeUI.activeInHierarchy)
		{
			posManager.currentAnimModel[posManager.currentPos].GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, sliderValue);
		}
	}

	public void XRay()
	{
		if (posManager.currentAnimXRay[posManager.currentPos] != null)
		{
			if (posManager.currentAnimXRay[posManager.currentPos].activeInHierarchy)
			{
				posManager.currentAnimXRay[posManager.currentPos].SetActive(false);
				indicatorXRay.color = Color.red;
			}
			else
			{
				posManager.currentAnimXRay[posManager.currentPos].SetActive(true);
				indicatorXRay.color = Color.green;
			}
		}
		AudioManager.instance.PlayUIs("Select");
	}

	public void Speed(int number)
	{
		currentSpeed = number;
		if (anim == posManager.currentAnim[posManager.currentPos].GetCurrentAnimatorClipInfo(0)[0].clip)
		{
			switch (number)
			{
			case 0:
				posManager.currentAnim[posManager.currentPos].speed = 1f;
				break;
			case 1:
				posManager.currentAnim[posManager.currentPos].speed = 2f;
				break;
			case 2:
				posManager.currentAnim[posManager.currentPos].speed = 2.5f;
				break;
			}
		}
		speedType = number;
		AudioManager.instance.PlayUIs("Select");
	}

	public void UnlockCamera()
	{
		sCam = Object.FindObjectOfType<SecondaryCamera>();
		if (!sCam.unlockCamera)
		{
			unlockCameraSlider.gameObject.SetActive(true);
			sCam.gameObject.transform.position = cam.gameObject.transform.position;
			vCam.Follow = sCam.gameObject.transform;
			vCam.GetCinemachineComponent<CinemachineFramingTransposer>().m_TrackedObjectOffset.y = 0f;
			unlockCameraSlider.value = 4.5f;
			sCam.unlockCamera = true;
			indicatorUnlockCamera.color = Color.green;
			Notify.instance.NotifyPlayer("Use movement controls to move the camera");
		}
		else
		{
			player.gameObject.SetActive(true);
			sCam.gameObject.transform.position = player.gameObject.transform.position;
			vCam.Follow = player.gameObject.transform;
			vCam.m_Lens.OrthographicSize = 4.5f;
			vCam.GetCinemachineComponent<CinemachineFramingTransposer>().m_TrackedObjectOffset.y = 3f;
			player.gameObject.SetActive(false);
			indicatorUnlockCamera.color = Color.red;
			sCam.unlockCamera = false;
			unlockCameraSlider.gameObject.SetActive(false);
		}
		AudioManager.instance.PlayUIs("Select");
	}

	public void RevealPos()
	{
		if (!posToReveal.activeInHierarchy)
		{
			posToReveal.SetActive(true);
		}
		else
		{
			posToReveal.SetActive(false);
		}
	}

	public void RevealOptions()
	{
		if (!optionsToReveal.activeInHierarchy)
		{
			optionsToReveal.SetActive(true);
		}
		else
		{
			optionsToReveal.SetActive(false);
		}
	}
}
