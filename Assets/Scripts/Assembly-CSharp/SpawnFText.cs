using UnityEngine;

public class SpawnFText : MonoBehaviour
{
	public GameObject instantiateHere;

	public FloatingText fText;

	public string[] words = new string[5];

	public void Awake()
	{
		words[0] = "ahn&";
		words[1] = "mngh&";
		words[2] = "nhgg&";
		words[3] = "hnn&";
		words[4] = "nya&";
	}

	public void SpawnText()
	{
		int num = Random.Range(0, 4);
		Object.Instantiate(fText, instantiateHere.transform).Spawn(words[num]);
	}

	public void SpawnTextSignal(string text)
	{
		Transform parent = instantiateHere.transform;
		Object.Instantiate(fText, parent).SpawnBig(text);
	}
}
