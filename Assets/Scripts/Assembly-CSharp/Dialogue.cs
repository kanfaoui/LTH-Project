using System;
using UnityEngine;

[Serializable]
public class Dialogue
{
	public string[] name;

	[NonReorderable]
	public Sprite[] sprite;

	[NonReorderable]
	[TextArea(3, 10)]
	public string[] sentences;
}
