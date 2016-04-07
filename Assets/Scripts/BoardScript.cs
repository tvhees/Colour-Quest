using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BoardScript : MonoBehaviour {

    public Material[] materialArray;
    public GameObject tileHolder;
    public List<GameObject> hiddenTiles = new List<GameObject>();
    public float sightDistance, dX;
    public List<GameObject> tiles = new List<GameObject>();
    public List<bool> flipped = new List<bool>();
    public Goal goal;
    public Player player;

	private float dZ = 2 / Mathf.Sqrt (3f);

    public void NewBoard() {
        // This defines the number of rows - might increase with difficulty.
        // Row zero will always have just 1 tile
        int[] tilesPerRow = new int[Preferences.Instance.difficulty + 1];
        tilesPerRow[0] = 1;

        int i = 0; // row
        int j = 0; // column
        int m = 0; // material index

        // Here we define how wide the board is for any given row
        // Test code
        for (i = 1; i < tilesPerRow.Length; i++)
        {
            tilesPerRow[i] = 2;
        }

        // Creating a master colour distribution list that can be copied
        // For random selection without replacement
        int[] colourDistribution = new int[6] { 3, 1, 1, 0, 0, 0 };
        List<int> colourMaster = new List<int>();
        List<int> colourCopy = new List<int>();
        for (i = 0; i < colourDistribution.Length; i++)
        {
            for (j = 0; j < colourDistribution[i]; j++)
            {
                colourMaster.Add(i);
            }
        }
        colourCopy.AddRange(colourMaster);

        // This list stores which material each tile uses
        List<int> materials = new List<int>();

        // Here we construct new lists used to place and colour tiles later
        // We don't instantiate or position any tiles here - as long as the
        // same Row/Column method is used later it's sufficient to store the
        // values to be given to the tiles in a list
        for (i = 0; i < tilesPerRow.Length; i++) // Iterate by row
        {
            for (j = 0; j < tilesPerRow[i]; j++) // Iterate by column within row
            {
                // Give the tile an appropriate material at random
                // First repopulate the colour list if all have been assigned
                if (colourCopy.Count < 1)
                {
                    colourCopy.AddRange(colourMaster);
                }
                // Choose a random colour index then remove it from the list
                int n = Random.Range(0, colourCopy.Count);
                m = colourCopy[n];
                colourCopy.RemoveAt(n);

                materials.Add(m);
                flipped.Add(false);
            }
        }
        Vector3 goalLocation = new Vector3(i * dX, 0, i%2 * 0.5f);
        Vector3 playerLocation = Vector3.zero;

        // Save the blueprint variables to the SaveSystem
        SaveSystem.Instance.tilesPerRow = tilesPerRow;
        SaveSystem.Instance.materials = materials;
        SaveSystem.Instance.flipped = flipped;
        SaveSystem.Instance.goalLocation = goalLocation;
        SaveSystem.Instance.playerLocation = playerLocation;

        InstantiateBoard(true, tilesPerRow, materials, flipped, goalLocation, playerLocation);
    }

    public void InstantiateBoard(bool newBoard, int[] tilesPerRow, List<int> materials,
        List<bool> flipped, Vector3 goalLocation, Vector3 playerLocation)
    {
        float offset = 0f;
        int i = 0;
        int j = 0;
        int index = 0;

        for (i = 0; i < tilesPerRow.Length; i++) // Iterate by row
        {
            offset = i % 2 * -0.5f; // Odd rows need to be shifted across

            for (j = 0; j < tilesPerRow[i]; j++) // Iterate by column within row
            {
                Vector3 position = new Vector3(i * dX, 0, (1 + offset - j) * dZ);

                // Create a new tile as child of the board object
                GameObject tile = transform.InstantiateChild(tileHolder, position);

                // Give the tile a colour and flip if required
                TileScript t = tile.GetComponentInChildren<TileScript>();
                t.colouredMaterial = materialArray[materials[index]];
                t.index = index;
                if (flipped[index])
                    t.Flip();

                index++;
            }
        }
        goal.startLocation = goalLocation;
        player.startLocation = playerLocation;

        // Animate flipping of tiles if it's an entirely new board
        if (newBoard)
            FlipTiles(Vector3.zero);
    }

    public void FlipTiles(Vector3 pos) {
        for (int i = hiddenTiles.Count - 1; i >= 0; i--) {
            StartCoroutine(hiddenTiles[i].GetComponent<TileScript>().Flip(pos, sightDistance));
        }
    }
}