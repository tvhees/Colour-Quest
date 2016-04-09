using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class Save : Singleton<Save> {
    public int maxHandSize;
    public int[] tilesPerRow, goalCost;
    public List<int> materials, hand, deck, preview, discard, objectives;
    public List<bool> flipped, alive, directionList;
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
            maxHandSize = data.maxHandSize;
            tilesPerRow = data.tilesPerRow;
            goalCost = data.goalCost;
            materials = data.materials;
            hand = data.hand;
            deck = data.deck;
            preview = data.preview;
            discard = data.discard;
            objectives = data.objectives;
            flipped = data.flipped;
            alive = data.alive;
            directionList = data.directionList;
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
        data.maxHandSize = maxHandSize;
        data.tilesPerRow = tilesPerRow;
        data.goalCost = goalCost;
        data.materials = materials;
        data.hand = hand;
        data.deck = deck;
        data.preview = preview;
        data.discard = discard;
        data.objectives = objectives;
        data.flipped = flipped;
        data.alive = alive;
        data.directionList = directionList;
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
    public int maxHandSize;
    public int[] tilesPerRow, goalCost;
    public List<int> materials, hand, deck, preview, discard, objectives;
    public List<bool> flipped, alive, directionList;
    public ExtensionMethods.SerializableVector3 goalLocation, playerLocation;
}