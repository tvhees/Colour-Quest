using UnityEngine;
using System.Collections;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class SaveSystem : MonoBehaviour {
    public GameObject player;

    public void LoadGame() {
        Debug.Log("Loading game from file");
        if (File.Exists(Application.persistentDataPath + "/saveData.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/saveData.dat", FileMode.Open);

            // Read data from file
            SaveData data = (SaveData)bf.Deserialize(file);
            file.Close();

            // Load objects from data
            player = data.player;
        }
    }

    public void SaveGame() {
        Debug.Log("Saving game to file");
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/saveData.dat");

        // Pass objects to data
        SaveData data = new SaveData();
        data.player = player;

        // Write data to file
        bf.Serialize(file, data);
        file.Close();
    }
}

[Serializable]
class SaveData{
    public GameObject player;
}