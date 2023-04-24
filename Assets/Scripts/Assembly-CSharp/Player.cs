using UnityEngine;

public class Player : MonoBehaviour
{
	public static Player instance;

	public PlayerMovement playerPrefab;

	private void Awake()
	{
		instance = this;
	}
}
