using UnityEngine;

public class SwingButton : MonoBehaviour
{
	public string arrowInput;

	private void OnMouseDown()
	{
		if (!TutorialMinigame.instance.onTutorial)
		{
			SwingManager.instance.ArrowInput(arrowInput);
		}
	}
}
