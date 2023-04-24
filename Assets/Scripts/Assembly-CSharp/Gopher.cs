using UnityEngine;

public class Gopher : MonoBehaviour
{
	public Animator anim;

	[Space]
	[Header("Values")]
	public int health = 1;

	public float stayTime;

	public int spawnPos;

	private bool whacked = true;

	private int scoreToGive;

	[Space]
	[Header("Variety")]
	public bool notGopher;

	public SpriteRenderer spriteObj;

	public Sprite bunny;

	public Sprite[] gopherTypes;

	public GameObject whack;

	private int typeGopher;

	private void Start()
	{
		if (Random.Range(0, 100) <= 15)
		{
			notGopher = true;
			spriteObj.sprite = bunny;
			stayTime = 3f;
			scoreToGive = -200;
			return;
		}
		scoreToGive = 50;
		if (Random.Range(0, 100) <= 30)
		{
			switch (typeGopher = Random.Range(1, 5))
			{
			case 1:
				health = 5;
				spriteObj.sprite = gopherTypes[0];
				scoreToGive = 200;
				break;
			case 2:
				health = 2;
				stayTime = 1.2f;
				spriteObj.sprite = gopherTypes[1];
				scoreToGive = 100;
				break;
			case 3:
				health = 3;
				spriteObj.sprite = gopherTypes[2];
				scoreToGive = 150;
				break;
			case 4:
				health = 2;
				spriteObj.sprite = gopherTypes[3];
				scoreToGive = 100;
				break;
			default:
				Debug.Log("xDD");
				break;
			}
		}
	}

	private void Update()
	{
		stayTime -= Time.deltaTime;
		if (stayTime <= 0f && whacked)
		{
			if (typeGopher == 2)
			{
				Object.FindObjectOfType<GopherFlash>().Flash();
			}
			if (!notGopher)
			{
				GopherManager.instance.UpdateScore(-100);
				AudioManager.instance.PlayUIs("Deduct");
			}
			anim.SetTrigger("Whack");
			whacked = false;
		}
		if (Input.GetKeyDown(KeyCode.Q) && spawnPos == 0)
		{
			WhackKey();
		}
		else if (Input.GetKeyDown(KeyCode.W) && spawnPos == 1)
		{
			WhackKey();
		}
		else if (Input.GetKeyDown(KeyCode.E) && spawnPos == 2)
		{
			WhackKey();
		}
		else if (Input.GetKeyDown(KeyCode.A) && spawnPos == 3)
		{
			WhackKey();
		}
		else if (Input.GetKeyDown(KeyCode.S) && spawnPos == 4)
		{
			WhackKey();
		}
		else if (Input.GetKeyDown(KeyCode.D) && spawnPos == 5)
		{
			WhackKey();
		}
		else if (Input.GetKeyDown(KeyCode.Z) && spawnPos == 6)
		{
			WhackKey();
		}
		else if (Input.GetKeyDown(KeyCode.X) && spawnPos == 7)
		{
			WhackKey();
		}
		else if (Input.GetKeyDown(KeyCode.C) && spawnPos == 8)
		{
			WhackKey();
		}
	}

	private void OnMouseDown()
	{
		if (Time.timeScale == 0f || health <= 0)
		{
			return;
		}
		Vector2 vector = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		Object.Instantiate(whack, vector, Quaternion.identity);
		health--;
		if (health <= 0)
		{
			anim.SetTrigger("Whack");
			if (notGopher)
			{
				GopherManager.instance.UpdateScore(scoreToGive);
				AudioManager.instance.PlayUIs("Deduct");
			}
			else
			{
				GopherManager.instance.UpdateScore(scoreToGive);
				AudioManager.instance.PlayUIs("Points");
			}
		}
	}

	private void WhackKey()
	{
		if (Time.timeScale == 0f || health <= 0)
		{
			return;
		}
		Object.Instantiate(whack, base.transform.position + new Vector3(0f, 1.5f, 0f), Quaternion.identity);
		health--;
		if (health <= 0)
		{
			anim.SetTrigger("Whack");
			if (notGopher)
			{
				GopherManager.instance.UpdateScore(scoreToGive);
				AudioManager.instance.PlayUIs("Deduct");
			}
			else
			{
				GopherManager.instance.UpdateScore(scoreToGive);
				AudioManager.instance.PlayUIs("Points");
			}
		}
	}

	public void Destroy()
	{
		GopherManager.instance.posAvailable[spawnPos] = false;
		Object.Destroy(base.gameObject);
	}
}
