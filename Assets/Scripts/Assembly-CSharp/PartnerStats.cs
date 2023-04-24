using System;
using UnityEngine;

[Serializable]
public class PartnerStats
{
	public string partnerName;

	public GameObject partnerPrefab;

	public int numDates;

	public bool[] doneStory;

	public bool[] likedItem;

	public bool canBeCalled;

	public bool doneIntimate;
}
