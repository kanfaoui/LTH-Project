using TMPro;
using UnityEngine;

public class FloatingText : MonoBehaviour
{
	public TextMeshProUGUI floatText;

	private Rigidbody2D rb;

	public string[] texts;

	public float sec;

	public float alphaColor = 1.2f;

	public void Awake()
	{
		rb = GetComponent<Rigidbody2D>();
	}

	private void Update()
	{
		alphaColor -= Time.deltaTime;
		floatText.color = new Color(1f, 1f, 1f, alphaColor);
		if (alphaColor <= 0f)
		{
			Object.Destroy(base.gameObject);
		}
	}

	public void Spawn(string Text)
	{
		floatText.text = Text;
		float num = Random.Range(-1f, 1f);
		float num2 = Random.Range(-1f, 1f);
		if (num == 0f || num2 == 0f)
		{
			num = 0.5f;
			num2 = 0.5f;
		}
		rb.velocity = new Vector2(num, num2);
		alphaColor = 1.2f;
	}

	public void SpawnBig(string Text)
	{
		floatText.text = Text;
		floatText.fontSize = 36f;
		base.gameObject.transform.position = new Vector3(base.gameObject.transform.position.x, base.gameObject.transform.position.y + 0.5f, base.gameObject.transform.position.z);
		rb.velocity = new Vector2(0f, 1f);
		alphaColor = 1.2f;
	}
}
