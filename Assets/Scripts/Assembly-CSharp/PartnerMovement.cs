using UnityEngine;
using UnityEngine.UI;

public class PartnerMovement : MonoBehaviour
{
	public Rigidbody2D rb;

	public Animator anim;

	public string partnerName;

	public SpriteRenderer[] spriteRenderer;

	public GameObject buttonList;

	public GameObject giftButton;

	public GameObject leaveButton;

	[Header("Clothes")]
	[Space]
	public GameObject normalClothes;

	public GameObject beachClothes;

	[Header("Distance and Speed")]
	[Space]
	public float minDist = 2f;

	public float reqDist = 4.5f;

	public float panSpeed = 6f;

	[Header("Bools")]
	[Space]
	public bool facingRight = true;

	public bool followPlayer;

	public bool distReached = true;

	[Header("Miscellaneous")]
	[Space]
	public Image indicator;

	public DialogueTrigger dTriggerLeave;

	public bool once;

	private PlayerMovement player;

	private void Start()
	{
		rb = GetComponent<Rigidbody2D>();
		player = Object.FindObjectOfType<PlayerMovement>();
		SceneObjectSpawner sceneObjectSpawner = Object.FindObjectOfType<SceneObjectSpawner>();
		normalClothes.SetActive(false);
		beachClothes.SetActive(false);
		if (sceneObjectSpawner.normalAttire)
		{
			normalClothes.SetActive(true);
		}
		else if (sceneObjectSpawner.beachAttire)
		{
			beachClothes.SetActive(true);
		}
	}

	private void FixedUpdate()
	{
		Transform transform = player.transform;
		if (followPlayer && Vector2.Distance(base.transform.position, transform.position) > minDist && distReached)
		{
			distReached = false;
		}
		if (Vector2.Distance(base.transform.position, transform.position) > reqDist && !distReached)
		{
			base.transform.position = Vector2.MoveTowards(base.transform.position, transform.position, panSpeed * Time.deltaTime);
			anim.SetFloat("Speed", 1f);
			float num = base.transform.position.x - transform.position.x;
			if (num < 0f && !facingRight)
			{
				Flip();
			}
			else if (num > 0f && facingRight)
			{
				Flip();
			}
		}
		else
		{
			anim.SetFloat("Speed", 0f);
			distReached = true;
		}
	}

	public void Follow()
	{
		if (followPlayer)
		{
			followPlayer = false;
			indicator.color = Color.red;
		}
		else
		{
			followPlayer = true;
			indicator.color = Color.green;
		}
	}

	public void Flip()
	{
		facingRight = !facingRight;
		SpriteRenderer[] array = spriteRenderer;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].transform.Rotate(0f, 180f, 0f);
		}
	}

	public void OpenGift()
	{
		Inventory.instance.partner = this;
		GiftManager.instance.GiftUI();
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.tag == "Player")
		{
			buttonList.SetActive(true);
			if (DateManager.instance.date)
			{
				giftButton.SetActive(true);
				leaveButton.SetActive(false);
			}
			else
			{
				giftButton.SetActive(false);
				leaveButton.SetActive(true);
			}
		}
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		if (collision.tag == "Player")
		{
			buttonList.SetActive(false);
		}
	}

	public void Deny()
	{
		anim.SetTrigger("Deny");
	}

	public void LeaveArea()
	{
		dTriggerLeave.TriggerDialogue();
		Object.Destroy(base.gameObject);
	}

	public void DestroyThis()
	{
		Object.Destroy(base.gameObject);
	}
}
