using UnityEngine;

public class ShopDestroy : MonoBehaviour
{
	private void OnTriggerEnter2D(Collider2D collision)
	{
		collision.GetComponent<ShopItem>().Destroy();
		ShopMinigameManager.instance.UpdateScore(-10);
		AudioManager.instance.PlayUIs("Deduct");
	}
}
