using UnityEngine;
using UnityEngine.UI;

public class SwingArrow : MonoBehaviour
{
	public Image arwImage;

	public void Destroy()
	{
		Object.Destroy(base.gameObject);
	}
}
