using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Item")]
public class Items : ScriptableObject
{
	public enum Type
	{
		consumable = 0,
		gift = 1
	}

	public enum ConsumableType
	{
		Energy = 0,
		Charisma = 1
	}

	public enum GiftType
	{
		Hearts = 0,
		Quest = 1
	}

	public Type itemType;

	public string itemName;

	public int itemPrice;

	[Header("If Consumable")]
	[Space]
	public ConsumableType consumableType;

	[Header("If Gift")]
	[Space]
	public GiftType giftType;

	[Header("Gains")]
	[Space]
	public int energyGain;

	[NonReorderable]
	public ItemDate[] dateGift;

	[Header("Sprite")]
	[Space]
	public Sprite sprite;

	[Header("Description and dialoge")]
	[Space]
	[TextArea(3, 10)]
	public string itemDesc;

	[NonReorderable]
	public GiftDialogues[] gDialogue;
}
