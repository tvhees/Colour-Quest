using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class SaveSystem : Singleton<SaveSystem> {
    public int[] tilesPerRow;
    public List<int> materials;
    public List<bool> flipped;
    public Vector3 goalLocation, playerLocation;

    public void LoadGame() {
        Debug.Log("Loading game from file");
        if (File.Exists(Application.persistentDataPath + "/saveData.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/saveData.dat", FileMode.Open);

            // Read data from file
            SaveData data = (SaveData)bf.Deserialize(file);
            file.Close();

            // Load data
            tilesPerRow = data.tilesPerRow;
            materials = data.materials;
            flipped = data.flipped;
            goalLocation = data.goalLocation;
            playerLocation = data.playerLocation;
            Master.Instance.StartGame(true);
        }
    }

    public void SaveGame() {
        Debug.Log("Saving game to file");
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/saveData.dat");

        // Pass objects to data
        SaveData data = new SaveData();
        data.tilesPerRow = tilesPerRow;
        data.materials = materials;
        data.flipped = flipped;
        data.goalLocation = goalLocation;
        data.playerLocation = playerLocation;

        // Write data to file
        bf.Serialize(file, data);
        file.Close();
    }
}

[Serializable]
class SaveData
{
    public int[] tilesPerRow;
    public List<int> materials;
    public List<bool> flipped;
    public ExtensionMethods.SerializableVector3 goalLocation, playerLocation;
}