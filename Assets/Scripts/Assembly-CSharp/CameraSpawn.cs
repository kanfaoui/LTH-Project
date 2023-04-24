using UnityEngine;

public class CameraSpawn : MonoBehaviour
{
	public static CameraSpawn instance;

	private void Awake()
	{
		instance = this;
	}
}
