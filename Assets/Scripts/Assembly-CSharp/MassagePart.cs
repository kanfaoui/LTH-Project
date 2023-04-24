using UnityEngine;

public class MassagePart : MonoBehaviour
{
	public int part;

	public GameObject hands;

	public bool oneTime;

	private void Update()
	{
		if (Input.GetMouseButtonUp(0))
		{
			hands.SetActive(false);
			MassageManager instance = MassageManager.instance;
			instance.oneTime = false;
			oneTime = false;
			instance.TextAppearPart();
		}
	}

	private void OnMouseDrag()
	{
		MassageManager instance = MassageManager.instance;
		if (instance.tutorial)
		{
			return;
		}
		hands.SetActive(true);
		if (part == 5 || part == 6)
		{
			if (!oneTime)
			{
				switch (Random.Range(0, 2))
				{
				case 0:
					instance.queueText.text = "Nhhg... Not there";
					break;
				case 1:
					instance.queueText.text = "Ahnnnn... Stop it";
					break;
				default:
					Debug.Log("xDD");
					break;
				}
				oneTime = true;
			}
		}
		else if (part == instance.mPart)
		{
			instance.Massage(Time.deltaTime);
		}
		else
		{
			instance.TextAppearPart();
		}
	}
}
