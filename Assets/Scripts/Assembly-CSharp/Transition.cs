using Cinemachine;
using UnityEngine;

public class Transition : MonoBehaviour
{
	public Vector3 coordinatesToSpawn;

	public PolygonCollider2D toRoom;

	public bool allowDay = true;

	public bool allowNoon = true;

	public bool allowNight = true;

	public CinemachineVirtualCamera vCam;

	public CinemachineConfiner vCamConfiner;

	public PlayerMovement player;

	public PartnerMovement partner;

	public Camera cam;

	public void MoveToRoom()
	{
		vCam = Object.FindObjectOfType<CinemachineVirtualCamera>();
		vCamConfiner = Object.FindObjectOfType<CinemachineConfiner>();
		player = Object.FindObjectOfType<PlayerMovement>();
		partner = Object.FindObjectOfType<PartnerMovement>();
		cam = Object.FindObjectOfType<Camera>();
		Lighting instance = Lighting.instance;
		if (!allowDay && instance.currentState == 0)
		{
			Notify.instance.NotifyPlayer("It's close");
			return;
		}
		if (!allowNoon && instance.currentState == 1)
		{
			Notify.instance.NotifyPlayer("It's close");
			return;
		}
		if (!allowNight && instance.currentState == 2)
		{
			Notify.instance.NotifyPlayer("It's close");
			return;
		}
		partner = Object.FindObjectOfType<PartnerMovement>();
		vCamConfiner.m_BoundingShape2D = toRoom;
		player.transform.position = coordinatesToSpawn;
		if (!(partner == null) && partner.followPlayer)
		{
			partner.transform.position = coordinatesToSpawn;
		}

		//ApplovinManager.Instance.FunctionToCall();

    }
}
