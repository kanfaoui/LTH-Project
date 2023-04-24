using UnityEngine;

public class ResetFade : MonoBehaviour
{
	public Animator Anim;

	public void ResetAnim()
	{
		Anim.SetTrigger("Fade");
	}
}
