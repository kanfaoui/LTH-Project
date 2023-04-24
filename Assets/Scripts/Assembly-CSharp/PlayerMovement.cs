using Cinemachine;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
	public static PlayerMovement instance;

	public Rigidbody2D rb;

	public Animator anim;

	public float panSpeed = 20f;

	public Joystick joyStick;

	[Space]
	[Header("Clothes")]
	public GameObject normalClothes;

	public GameObject beachClothes;

	public bool facingRight = true;

	public bool stayPos;

	public bool movePlayer = true;

	public PolygonCollider2D coll;

	public void Start()
	{
		rb = GetComponent<Rigidbody2D>();
		instance = this;
	}

	private void FixedUpdate()
	{
		if (movePlayer && !DialogueManager.instance.dialogueActive)
		{
			if (!MobileController.instance.joystickState)
			{
				float axisRaw = Input.GetAxisRaw("Horizontal");
				float axisRaw2 = Input.GetAxisRaw("Vertical");
				Move(axisRaw, axisRaw2);
			}
			else if (MobileController.instance.joystickState)
			{
				float axisRaw = joyStick.Horizontal;
				float axisRaw2 = joyStick.Vertical;
				Move(axisRaw, axisRaw2);
			}
		}
	}

	private void Move(float moveH, float moveV)
	{
		float f = Mathf.Abs(moveH) + Mathf.Abs(moveV);
		anim.SetFloat("Speed", Mathf.Abs(f));
		rb.velocity = new Vector2(moveH * panSpeed, moveV * panSpeed);
		if (moveH > 0f && !facingRight)
		{
			Flip();
		}
		else if (moveH < 0f && facingRight)
		{
			Flip();
		}
	}

	public void Flip()
	{
		facingRight = !facingRight;
		base.transform.Rotate(0f, 180f, 0f);
	}

	public void DetectCollider()
	{
		Collider2D[] array = Physics2D.OverlapAreaAll(base.transform.position, base.transform.position);
		for (int i = 0; i < array.Length; i++)
		{
			if (array[i].GetComponent<PolygonCollider2D>() != null)
			{
				Object.FindObjectOfType<CinemachineConfiner>().m_BoundingShape2D = array[i].GetComponent<PolygonCollider2D>();
				coll = array[i].GetComponent<PolygonCollider2D>();
				Object.FindObjectOfType<CinemachineVirtualCamera>().ForceCameraPosition(base.transform.localPosition, Quaternion.identity);
			}
		}
	}
}
