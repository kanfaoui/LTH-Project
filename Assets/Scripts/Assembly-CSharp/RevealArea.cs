using UnityEngine;

public class RevealArea : MonoBehaviour
{
	public GameObject eyeButton;

	public GameObject bushesLayer;

	public void EyeButton()
	{
		if (!eyeButton.activeInHierarchy)
		{
			eyeButton.SetActive(true);
		}
		else
		{
			eyeButton.SetActive(false);
		}
		AudioManager.instance.PlayUIs("Select");
	}

	public void RevealHidden()
	{
		if (!bushesLayer.activeInHierarchy)
		{
			bushesLayer.SetActive(true);
		}
		else
		{
			bushesLayer.SetActive(false);
		}
		AudioManager.instance.PlayUIs("Select");
	}

	public void ExitHeart()
	{
		eyeButton.SetActive(false);
		bushesLayer.SetActive(true);
	}
}
