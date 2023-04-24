using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;

public class Lighting : MonoBehaviour
{
	public static Lighting instance;

	public Light2D globalLight2d;

	public bool preLighting;

	public int currentState;

	public void Awake()
	{
		instance = this;
	}

	public void Update()
	{
	}

	public void SwitchTime()
	{
		if (DateManager.instance.date)
		{
			Notify.instance.NotifyPlayer("Date in progress");
			return;
		}
		GameManager gameManager = GameManager.instance;
		SceneObjectSpawner sceneObjectSpawner = SceneObjectSpawner.instance;
		if (!sceneObjectSpawner.allowDay && currentState == 0)
		{
			Notify.instance.NotifyPlayer(SceneManager.GetActiveScene().name + " is close at noon");
			return;
		}
		if (!sceneObjectSpawner.allowNoon && currentState == 1)
		{
			Notify.instance.NotifyPlayer(SceneManager.GetActiveScene().name + " is close at night");
			return;
		}
		if (!sceneObjectSpawner.allowNight && currentState == 2)
		{
			Notify.instance.NotifyPlayer("You need sleep");
			return;
		}
		if (Object.FindObjectOfType<PositionsManager>() != null || Object.FindObjectOfType<Intimacy>() != null)
		{
			Notify.instance.NotifyPlayer("Can't do that right now");
			return;
		}
		if (sceneObjectSpawner.resetSpawn)
		{
			sceneObjectSpawner.StartSpawn();
		}
		if ((bool)gameManager.currentCalledPartner)
		{
			gameManager.currentCalledPartner.DestroyThis();
		}
		Object.FindObjectOfType<ResetFade>().ResetAnim();
		currentState++;
		if (currentState == 3)
		{
			currentState = 0;
			PlayerHUD.instance.ChangeDay();
			PlayerHUD.instance.energyBar.value = PlayerHUD.instance.energyBar.maxValue;
			GameManager.instance.SaveGame();
		}
		if (currentState == 0)
		{
			globalLight2d.color = new Color(1f, 1f, 1f);
		}
		else if (currentState == 1)
		{
			globalLight2d.color = new Color(1f, 0.9726278f, 0.8160377f);
		}
		else if (currentState == 2)
		{
			globalLight2d.color = new Color(0.7137255f, 0.8313726f, 1f);
		}
		SceneObjectSpawner.instance.ObjectStateSpawn();
	}

	public void Sleep()
	{
		GameManager gameManager = GameManager.instance;
		Object.FindObjectOfType<ResetFade>().ResetAnim();
		if ((bool)gameManager.currentCalledPartner)
		{
			gameManager.currentCalledPartner.DestroyThis();
		}
		currentState++;
		if (currentState == 3)
		{
			currentState = 0;
			PlayerHUD.instance.ChangeDay();
			PlayerHUD.instance.energyBar.value = PlayerHUD.instance.energyBar.maxValue;
			GameManager.instance.SaveGame();
		}
		if (currentState == 0)
		{
			globalLight2d.color = new Color(1f, 1f, 1f);
		}
		else if (currentState == 1)
		{
			globalLight2d.color = new Color(1f, 0.9726278f, 0.8160377f);
		}
		else if (currentState == 2)
		{
			globalLight2d.color = new Color(0.7137255f, 0.8313726f, 1f);
		}
	}
}
