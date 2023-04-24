using System.Collections;
using Cinemachine;
using UnityEngine;

public class SceneObjectSpawner : MonoBehaviour
{
	public static SceneObjectSpawner instance;

	public bool isMiniGame;

	[Header("Spawner")]
	public Vector3 coordinatesToSpawn;

	public PolygonCollider2D toRoom;

	public bool resetSpawn;

	[Space]
	[Header("Clothes to Wear")]
	public bool normalAttire = true;

	public bool beachAttire;

	[Space]
	[Header("Partner Allower")]
	public bool allowMary;

	public bool allowEul;

	public bool allowCarol;

	public bool allowCait;

	public bool allowSherryl;

	[Space]
	[Header("Time Skip Allower")]
	public bool allowDay = true;

	public bool allowNoon = true;

	public bool allowNight = true;

	[Space]
	[Header("Day Objects")]
	public GameObject[] dayObjects;

	[Space]
	[Header("Evening Objects")]
	public GameObject[] eveObjects;

	[Space]
	[Header("Night Objects")]
	public GameObject[] nightObjects;

	[Space]
	[Header("Date Objects")]
	public GameObject[] dateObjects;

	[Space]
	[Header("Managers")]
	public GameObject gameManager;

	private void Awake()
	{
		instance = this;
		if (GameManager.instance == null)
		{
			Object.Instantiate(gameManager);
		}
	}

	private void Start()
	{
		DateManager dateManager = DateManager.instance;
		if (Lighting.instance.preLighting)
		{
			Lighting.instance.SwitchTime();
			Lighting.instance.preLighting = false;
		}
		if (isMiniGame)
		{
			Player.instance.playerPrefab.gameObject.SetActive(false);
			PlayerHUD.instance.gameObject.SetActive(false);
			return;
		}
		PlayerHUD.instance.gameObject.SetActive(true);
		Player.instance.playerPrefab.gameObject.SetActive(true);
		if (dateManager.date)
		{
			dateManager.dateUI.SetActive(true);
			Statistics.instance.CallPartner();
			if (dateManager.onMinigame)
			{
				Object.FindObjectOfType<PartnerMovement>().transform.position = dateManager.partnerSpawn;
			}
		}
		StartSpawn();
	}

	public void StartSpawn()
	{
		DateManager dateManager = DateManager.instance;
		HeartMode heartMode = HeartMode.instance;
		Player player = Player.instance;
		CinemachineVirtualCamera cinemachineVirtualCamera = Object.FindObjectOfType<CinemachineVirtualCamera>();
		CinemachineConfiner cinemachineConfiner = Object.FindObjectOfType<CinemachineConfiner>();
		Camera camera = Object.FindObjectOfType<Camera>();
		heartMode.vCam = Object.FindObjectOfType<CinemachineVirtualCamera>();
		heartMode.cam = Object.FindObjectOfType<Camera>();
		if (!player.playerPrefab.stayPos)
		{
			player.playerPrefab.transform.position = coordinatesToSpawn;
			camera.gameObject.transform.position = coordinatesToSpawn;
			cinemachineConfiner.m_BoundingShape2D = toRoom;
			if (dateManager.onMinigame)
			{
				player.playerPrefab.transform.position = dateManager.playerSpawn;
				StartCoroutine(StartDetect());
			}
		}
		player.playerPrefab.normalClothes.SetActive(false);
		player.playerPrefab.beachClothes.SetActive(false);
		if (normalAttire)
		{
			player.playerPrefab.normalClothes.SetActive(true);
		}
		else if (beachAttire)
		{
			player.playerPrefab.beachClothes.SetActive(true);
		}
		cinemachineVirtualCamera.Follow = player.playerPrefab.transform;
		Object.FindObjectOfType<SecondaryCamera>().vCam = cinemachineVirtualCamera;
		ObjectStateSpawn();
	}

	public IEnumerator StartDetect()
	{
		yield return new WaitForSeconds(0.1f);
		PlayerMovement.instance.DetectCollider();
	}

	public void ObjectStateSpawn()
	{
		for (int i = 0; i < dayObjects.Length; i++)
		{
			if (dayObjects[i] != null)
			{
				dayObjects[i].SetActive(false);
			}
		}
		for (int j = 0; j < eveObjects.Length; j++)
		{
			if (eveObjects[j] != null)
			{
				eveObjects[j].SetActive(false);
			}
		}
		for (int k = 0; k < nightObjects.Length; k++)
		{
			if (nightObjects[k] != null)
			{
				nightObjects[k].SetActive(false);
			}
		}
		for (int l = 0; l < dateObjects.Length; l++)
		{
			if (DateManager.instance.date)
			{
				if (dateObjects[l] != null)
				{
					dateObjects[l].SetActive(false);
				}
			}
			else if (dateObjects[l] != null)
			{
				dateObjects[l].SetActive(true);
			}
		}
		int currentState = Lighting.instance.currentState;
		for (int m = 0; m < dayObjects.Length; m++)
		{
			if (currentState == 0 && dayObjects[m] != null)
			{
				dayObjects[m].SetActive(true);
			}
		}
		for (int n = 0; n < eveObjects.Length; n++)
		{
			if (currentState == 1 && eveObjects[n] != null)
			{
				eveObjects[n].SetActive(true);
			}
		}
		for (int num = 0; num < nightObjects.Length; num++)
		{
			if (currentState == 2 && nightObjects[num] != null)
			{
				nightObjects[num].SetActive(true);
			}
		}
	}
}
