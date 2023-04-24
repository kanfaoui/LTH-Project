using UnityEngine;

[CreateAssetMenu(fileName = "Partner", menuName = "Partner")]
public class Partner : ScriptableObject
{
	public string pName;

	public string pAttitude;

	public string pAge;

	public string pLocation;

	public Sprite pChibi;

	public string[] pLikes;

	public string[] pStory;

	public int[] pReqStory;

	[NonReorderable]
	public Face[] faceSprite;

	public string pDescription;
}
