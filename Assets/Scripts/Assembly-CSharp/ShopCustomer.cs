using UnityEngine;

public class ShopCustomer : MonoBehaviour
{
	[Header("Customer Sprite")]
	public Sprite[] randomSprite;

	public Sprite thiefSprite;

	public SpriteRenderer customerSprite;

	public GameObject pow;

	public Vector2 mousePositionOffset;

	[Space]
	[Header("Values")]
	public int health = 1;

	public int scoreGive;

	public bool thief;

	public float timeToSteal = 5f;

	public float timeToLeave = 7f;

	public int spawnPos;

	[Space]
	[Header("Items")]
	public string[] itemNeed;

	public string[] itemAvailable;

	public int itemCount;

	public SpriteRenderer[] itemSprite = new SpriteRenderer[3];

	public void Start()
	{
		int num = (itemCount = Random.Range(1, itemNeed.Length + 1));
		itemNeed = new string[num];
		scoreGive = num * 20;
		int num2 = Random.Range(0, randomSprite.Length);
		customerSprite.sprite = randomSprite[num2];
		for (int i = 0; i < itemSprite.Length; i++)
		{
			itemSprite[i].gameObject.SetActive(false);
		}
		if (thief)
		{
			scoreGive = 100;
			health = 3;
			customerSprite.sprite = thiefSprite;
			return;
		}
		for (int j = 0; j < num; j++)
		{
			itemSprite[j].gameObject.SetActive(true);
			int num3 = Random.Range(0, itemAvailable.Length);
			itemNeed[j] = itemAvailable[num3];
			itemSprite[j].sprite = GameManager.instance.GetItemDetails(itemNeed[j]).sprite;
		}
	}

	public void Update()
	{
		if (thief)
		{
			timeToSteal -= Time.deltaTime;
			if (timeToSteal <= 0f)
			{
				ShopMinigameManager.instance.posAvailable[spawnPos] = false;
				ShopMinigameManager.instance.UpdateScore(-scoreGive);
				AudioManager.instance.PlayUIs("Deduct");
				Object.Destroy(base.gameObject);
			}
		}
		else
		{
			timeToLeave -= Time.deltaTime;
			if (timeToLeave <= 0f)
			{
				ShopMinigameManager.instance.posAvailable[spawnPos] = false;
				ShopMinigameManager.instance.UpdateScore(-scoreGive);
				AudioManager.instance.PlayUIs("Deduct");
				Object.Destroy(base.gameObject);
			}
		}
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (!(collision.tag == "ShopItem"))
		{
			return;
		}
		for (int i = 0; i < itemNeed.Length; i++)
		{
			if (collision.GetComponent<ShopItem>().itemName == itemNeed[i])
			{
				Debug.Log("right Item");
				collision.GetComponent<ShopItem>().Destroy();
				itemNeed[i] = "";
				itemSprite[i].gameObject.SetActive(false);
				itemCount--;
				if (itemCount <= 0)
				{
					ShopMinigameManager.instance.posAvailable[spawnPos] = false;
					ShopMinigameManager.instance.UpdateScore(scoreGive);
					AudioManager.instance.PlayUIs("Points");
					Object.Destroy(base.gameObject);
				}
				break;
			}
		}
	}

	private void OnMouseDown()
	{
		Vector2 vector = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		if (!ShopMinigameManager.instance.bMode)
		{
			return;
		}
		health--;
		Object.Instantiate(pow, vector, Quaternion.identity);
		if (health <= 0)
		{
			if (thief)
			{
				ShopMinigameManager.instance.posAvailable[spawnPos] = false;
				ShopMinigameManager.instance.UpdateScore(scoreGive);
				AudioManager.instance.PlayUIs("Points");
				Object.Destroy(base.gameObject);
			}
			else
			{
				ShopMinigameManager.instance.posAvailable[spawnPos] = false;
				ShopMinigameManager.instance.UpdateScore(-scoreGive);
				AudioManager.instance.PlayUIs("Deduct");
				Object.Destroy(base.gameObject);
			}
		}
	}
}
