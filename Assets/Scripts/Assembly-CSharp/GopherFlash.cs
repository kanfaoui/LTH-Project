using UnityEngine;

public class GopherFlash : MonoBehaviour
{
	public Animator Anim;

	public void Flash()
	{
		Anim.SetTrigger("Flash");
	}
}
