using UnityEngine;

public class ShopItem : MonoBehaviour
{
	public Vector2 mousePositionOffset;

	public ShopItem toSpawn;

	public Vector2 origPos;

	public string itemName;

	public bool allowSpawn = true;

	public bool allowDrag = true;

	private Rigidbody2D rb;

	public void Start()
	{
		origPos = base.gameObject.transform.position;
		GetComponent<SpriteRenderer>().sprite = GameManager.instance.GetItemDetails(itemName).sprite;
		rb = GetComponent<Rigidbody2D>();
	}

	public void Update()
	{
		if (Input.GetMouseButtonUp(0))
		{
			allowDrag = false;
		}
	}

	private void OnMouseDown()
	{
		if (!ShopMinigameManager.instance.bMode && !ShopMinigameManager.instance.tutorial && allowSpawn)
		{
			Vector2 vector = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			Vector2 vector2 = (Vector2)base.gameObject.transform.position - vector;
			base.transform.position = vector + vector2;
			Object.Instantiate(toSpawn, origPos, Quaternion.identity).itemName = itemName;
			allowSpawn = false;
			allowDrag = true;
		}
	}

	private void OnMouseDrag()
	{
		if (!ShopMinigameManager.instance.bMode && !ShopMinigameManager.instance.tutorial)
		{
			if (allowDrag)
			{
				Vector2 vector = Camera.main.ScreenToWorldPoint(Input.mousePosition);
				base.transform.position = vector + mousePositionOffset;
			}
			rb.gravityScale = 1f;
		}
	}

	public void Destroy()
	{
		Object.Destroy(base.gameObject);
	}
}
