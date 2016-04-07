using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BoardScript : MonoBehaviour {

    public Material[] materialArray;
    public GameObject tileHolder;
    public List<GameObject> hiddenTiles = new List<GameObject>();
    public float sightDistance, dX;
    public List<GameObject> tiles = new List<GameObject>();
    public List<int> materials = new List<int>(); 

	private float dZ = 2 / Mathf.Sqrt (3f);

    public void NewBoard() {
        int[] tilesPerRow = new int[Preferences.Instance.difficulty + 1];
        int[] colourDistribution = new int[6] { 0, 0, 0, 0, 0, 0 };
        List<int> colourMaster = new List<int>();
        List<int> colourCopy = new List<int>();

        int i; // row
        int j; // column
        int m; // material index
        float offset; //offset

        // Test code
        tilesPerRow[0] = 1;

        for (i = 1; i < tilesPerRow.Length; i++)
        {
            tilesPerRow[i] = 2;
        }
        colourDistribution[0] = 1;
        for (i = 0; i < colourDistribution.Length; i++)
        {
            for (j = 0; j < colourDistribution[i]; j++)
            {
                colourMaster.Add(i);
            }
        }
        colourCopy.AddRange(colourMaster);

        for (i = 0; i < tilesPerRow.Length; i++) // Iterate by row
        {
            if (i % 2 == 0)
                offset = 0f;
            else
                offset = -0.5f; // Odd rows need to be shifted across

            for (j = 0; j < tilesPerRow[i]; j++) // Iterate by column within row
            {
                Vector3 position = new Vector3(i * dX, 0, (1 + offset - j) * dZ);

                // Create a new tile as child of the board object
                GameObject tile = transform.InstantiateChild(tileHolder, position);
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
                    
                tile.GetComponentInChildren<MeshRenderer>().sharedMaterial = materialArray[m]; 
                tiles.Add(tile);
                materials.Add(m);
 
            }
        }
    }

    public void FlipTiles(Vector3 pos) {
        for (int i = hiddenTiles.Count - 1; i >= 0; i--) {
            StartCoroutine(hiddenTiles[i].GetComponent<TileScript>().Flip(pos, sightDistance));
        }
    }
}