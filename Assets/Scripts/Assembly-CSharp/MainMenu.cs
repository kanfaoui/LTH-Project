using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
	public GameObject continueButton;

	public ParticleSystem ps;

	public GameObject confirmUI;

	public bool usePrewarm;

	public void Start()
	{
		ps = Object.FindObjectOfType<ParticleSystem>();
		ParticleSystem.MainModule main = ps.main;
		main.loop = true;
		Restart();
		if (File.Exists(Application.persistentDataPath + "/SavedData.inf"))
		{
			continueButton.SetActive(true);
		}
		else
		{
			continueButton.SetActive(false);
		}
	}

	public void ConfirmUI()
	{
		if (confirmUI.activeInHierarchy)
		{
			confirmUI.SetActive(false);
		}
		else
		{
			confirmUI.SetActive(true);
		}
	}

	public void ContinueGame()
	{
		GameManager.instance.LoadGame();
	}

	public void NewGame()
	{
		string path = Application.persistentDataPath + "/SavedData.inf";
		if (confirmUI.activeInHierarchy)
		{
			File.Delete(path);
			SceneManager.LoadScene("Introduction");
		}
		if (File.Exists(path))
		{
			confirmUI.SetActive(true);
		}
		else
		{
			SceneManager.LoadScene("Introduction");
		}
	}

	public void Exit()
	{
		Application.Quit();
	}

	private void Restart()
	{
		ps.Stop();
		ps.Clear();
		ParticleSystem.MainModule main = ps.main;
		main.prewarm = usePrewarm;
		ps.Play();
	}
}
