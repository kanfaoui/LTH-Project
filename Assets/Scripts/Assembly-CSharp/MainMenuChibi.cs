using UnityEngine;

public class MainMenuChibi : MonoBehaviour
{
	public SpriteRenderer spriteRenderer;

	public Sprite normal;

	public Sprite change;

	public bool revert;

	public float stayTime;

	private void Update()
	{
		if (stayTime > 0f)
		{
			stayTime -= Time.deltaTime;
		}
		if (stayTime <= 0f && revert)
		{
			spriteRenderer.sprite = normal;
			revert = false;
		}
	}

	private void OnMouseDown()
	{
		spriteRenderer.sprite = change;
		revert = true;
		stayTime = 2f;
	}
}
