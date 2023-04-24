using TMPro;
using UnityEngine;

public class Notify : MonoBehaviour
{
	public static Notify instance;

	public GameObject notifyUI;

	public TextMeshProUGUI notifyText;

	public float timerReveal;

	public bool revealOnce;

	public void Awake()
	{
		instance = this;
	}

	public void Update()
	{
		if (revealOnce)
		{
			if (timerReveal > 0f)
			{
				timerReveal -= Time.deltaTime;
				return;
			}
			notifyUI.SetActive(false);
			revealOnce = false;
		}
	}

	public void NotifyPlayer(string note)
	{
		AudioManager.instance.PlayUIs("Notify");
		notifyUI.SetActive(true);
		notifyText.text = note;
		timerReveal = 2f;
		revealOnce = true;
	}
}
