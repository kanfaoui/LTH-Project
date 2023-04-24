using UnityEngine;
using UnityEngine.UI;

public class AreaTrigger : MonoBehaviour
{
	public GameObject buttonList;

	public Button button;

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.E) && button.gameObject.activeInHierarchy)
		{
			button.onClick.Invoke();
			AudioManager.instance.PlayUIs("Select");
		}
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.tag == "Player")
		{
			buttonList.SetActive(true);
		}
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		if (collision.tag == "Player")
		{
			buttonList.SetActive(false);
		}
	}
}
