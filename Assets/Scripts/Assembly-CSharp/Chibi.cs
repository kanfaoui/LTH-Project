using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Chibi : MonoBehaviour
{
	public Image chibiSprite;

	public Sprite[] chibiExpression;

	public TextMeshProUGUI cheertext;

	public string[] cheers;

	public float timeCheer;

	public bool idling = true;

	public float timeIdle = 10f;

	public void Update()
	{
		if (!idling && timeCheer > 0f)
		{
			timeCheer -= Time.deltaTime;
		}
		if (!idling && timeCheer <= 0f)
		{
			idling = true;
			timeIdle = 10f;
			chibiSprite.sprite = chibiExpression[0];
			cheertext.gameObject.SetActive(false);
		}
		if (idling && timeIdle > 0f)
		{
			timeIdle -= Time.deltaTime;
		}
		if (idling && timeIdle <= 0f)
		{
			idling = false;
			timeCheer = 5f;
			RandomCheer();
			chibiSprite.sprite = chibiExpression[1];
			cheertext.gameObject.SetActive(true);
		}
	}

	public void RandomCheer()
	{
		int num = Random.Range(0, cheers.Length);
		cheertext.text = cheers[num];
	}
}
