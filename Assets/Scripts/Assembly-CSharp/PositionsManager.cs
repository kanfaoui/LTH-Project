using UnityEngine;

public class PositionsManager : MonoBehaviour
{
	public int currentPos;

	[Header("Patreon Only Pos")]
	[Space]
	public bool[] positionsAllower = new bool[6];

	[Header("Anim Arrays")]
	[Space]
	public Animator[] currentAnim;

	public GameObject[] currentAnimModel;

	public GameObject[] currentAnimXRay;

	public SpawnFText[] fText;
}
