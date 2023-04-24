using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class SaveSystem
{
	public static void SavePlayer(GameManager player)
	{
		BinaryFormatter binaryFormatter = new BinaryFormatter();
		FileStream fileStream = new FileStream(Application.persistentDataPath + "/SavedData.inf", FileMode.Create);
		PlayerData graph = new PlayerData(player);
		binaryFormatter.Serialize(fileStream, graph);
		fileStream.Close();
	}

	public static PlayerData LoadPlayer()
	{
		string path = Application.persistentDataPath + "/SavedData.inf";
		if (File.Exists(path))
		{
			BinaryFormatter binaryFormatter = new BinaryFormatter();
			FileStream fileStream = new FileStream(path, FileMode.Open);
			PlayerData result = binaryFormatter.Deserialize(fileStream) as PlayerData;
			fileStream.Close();
			return result;
		}
		return null;
	}
}
