using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BoardScript : MonoBehaviour {

    public int startHandSize;
    public Material[] materialArray;
    public GameObject tileHolder;
    public List<GameObject> hiddenTiles = new List<GameObject>();
    public float sightDistance, dX;
    public List<GameObject> tiles = new List<GameObject>();
    public Goal goal;
    public Player player;

	private float dZ = 2 / Mathf.Sqrt (3f);
    private int[][] values =
{
    new int[] {1,0,0},
    new int[] {0,1,0},
    new int[] {0,0,1},
    new int[] {1,1,0},
    new int[] {1,0,1},
    new int[] {0,1,1}
};

    public void NewBoard() {
        // This defines the number of rows - might increase with difficulty.
        // Row zero will always have just 1 tile
        int boardLength = Preferences.Instance.difficulty > 10 ? 10 : 5;
        int[] tilesPerRow = new int[boardLength];
        tilesPerRow[0] = 1;

        int row = 0; // row
        int col = 0; // column
        int m = 0; // material index

        // Here we define how wide the board is for any given row
        // Test code
        for (row = 1; row < tilesPerRow.Length; row++)
        {
            tilesPerRow[row] = Mathf.Min(Preferences.Instance.difficulty + 1, 6);
        }

        // Creating a master colour distribution list that can be copied
        // For random selection without replacement
        int[] colourDistribution = GetColourDistribution(false);

        List<int> colourMaster = GetColourList(colourDistribution);
        List<int> colourCopy = new List<int>();
        colourCopy.AddRange(colourMaster);

        // Here we construct new lists used to place and colour tiles later
        // We don't instantiate or position any tiles here - as long as the
        // same Row/Column method is used later it's sufficient to store the
        // values to be given to the tiles in a list
        List<int> materials = new List<int>();
        List<int> objectives = new List<int>();
        List<bool> flipped = new List<bool>(), alive = new List<bool>();
        for (row = 0; row < tilesPerRow.Length; row++) // Iterate by row
        {
            if (row > 9)
            {
                colourDistribution = GetColourDistribution(true);
                colourMaster = GetColourList(colourDistribution);
                colourCopy.Clear();
                colourCopy.AddRange(colourMaster);
            }
            for (col = 0; col < tilesPerRow[row]; col++) // Iterate by column within row
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
                alive.Add(true);
            }
        }
        float offset = row % 2 * - 0.5f;
        Debug.Log("row: " + row + ", col: " + col + ", offset: " + offset);
        Vector3 goalLocation = new Vector3(row * dX, 0, (2 + offset - (col-1)/2) * dZ);
        Vector3 playerLocation = Vector3.zero;

        // Set up the direction of movement instructions for the goal sphere
        List<bool> directionList = new List<bool>();
        int leftMax = tilesPerRow.Length/3 + 1, rightMax = 2 * tilesPerRow.Length/3 + 2;
        bool[] temp = new bool[leftMax + rightMax];
        for (int i = 0; i < leftMax; i++)
            temp[i] = true;
        for (int i = leftMax; i < temp.Length; i++)
            temp[i] = false;
        temp.Randomise();
        directionList.AddRange(temp);

        // Set up the starting value of the goal
        int[] goalCost = new int[3] { 0, 0, 0 };

        // Increase goalCost based on difficulty
        for (int i = 0; i < Preferences.Instance.difficulty; i++)
        {
            int j = (int)Mathf.Repeat(i, 3);
            goalCost[j]++;
        }

        // In this section we set up the initial mana composition
        // of the player's hand and deck
        // The starting hand is always the same. 0 = blue, 1 = red, 2 = yellow
        // The deck is randomised at start. Preview and discard are empty
        List<int> hand = new List<int>(), deck = new List<int>(), 
            preview = new List<int>(), discard = new List<int>();

        hand.AddRange(new int[5] { 0, 0, 0, 1, 2 });
        int[] deckandpreview = new int[10] { 0, 0, 0, 0, 0, 0, 1, 1, 2, 2 };
        deckandpreview.Randomise();

        for (int i = 0; i < 5; i++) {
            preview.Add(deckandpreview[i]);
        }

        for (int i = 5; i < 10; i++)
        {
            deck.Add(deckandpreview[i]);
        }

        // In this section we define a new objective tracker.
        // The first entry is the starting TOTAL collected
        List<int> objectiveTracker = new List<int>();
        objectiveTracker.AddRange(new int[3] { 0, 0, 0 });


        // Save the blueprint variables to the SaveSystem
        Save.Instance.maxHandSize = startHandSize;
        Save.Instance.hand = hand;
        Save.Instance.deck = deck;
        Save.Instance.preview = preview;
        Save.Instance.discard = discard;
        Save.Instance.deck = deck;
        Save.Instance.objectiveTracker = objectiveTracker;
        Save.Instance.tilesPerRow = tilesPerRow;
        Save.Instance.goalCost = goalCost;
        Save.Instance.materials = materials;
        Save.Instance.flipped = flipped;
        Save.Instance.alive = alive;
        Save.Instance.directionList = directionList;
        Save.Instance.goalLocation = goalLocation;
        Save.Instance.playerLocation = playerLocation;
    }

    public void InstantiateBoard(bool newBoard)
    {
        float offset = 0f;
        int row = 0;
        int col = 0;
        int index = 0;

        for (row = 0; row < Save.Instance.tilesPerRow.Length; row++) // Iterate by row
        {
            offset = row % 2 * -0.5f; // Odd rows need to be shifted across

            for (col = 0; col < Save.Instance.tilesPerRow[row]; col++) // Iterate by column within row
            {
                Vector3 position = new Vector3(row * dX, 0, ((Save.Instance.tilesPerRow[row])/2 + 1 + offset - col) * dZ);

                // Create a new tile as child of the board object
                GameObject tile = transform.InstantiateChild(tileHolder, position);

                // Give the tile a colour and flip if required
                TileScript t = tile.GetComponentInChildren<TileScript>();
                t.colouredMaterial = materialArray[Save.Instance.materials[index]];
                t.tileCost = values[Save.Instance.materials[index]];
                t.index = index;
                if (Save.Instance.flipped[index])
                    t.Flip();
                if (!Save.Instance.alive[index])
                    t.KillTile(true);

                index++;
            }
        }
        goal.startLocation = Save.Instance.goalLocation;
        player.startLocation = Save.Instance.playerLocation;

        // Animate flipping of tiles if it's an entirely new board
        if (newBoard)
            FlipTiles(Vector3.zero);
    }

    public int[] GetColourDistribution(bool advanced) {
        // Creating a master colour distribution list that can be copied
        // For random selection without replacement
        int d = Preferences.Instance.difficulty;
        int[] colourDistribution = new int[6] { 20, 0, 0, 0, 0, 0 };
        if (!advanced)
        {
            switch (Preferences.Instance.difficulty)
            {
                case 0:
                case 1:
                    break;
                case 2:
                    colourDistribution = new int[6] { 14, 6, 0, 0, 0, 0 };
                    break;
                case 3:
                    colourDistribution = new int[6] { 12, 6, 2, 0, 0, 0 };
                    break;
                case 4:
                    colourDistribution = new int[6] { 12, 6, 2, 0, 0, 0 };
                    break;
                default:
                    colourDistribution[0] = Mathf.FloorToInt(11 - (d - 5) / 4);
                    colourDistribution[1] = Mathf.FloorToInt(6 - (d - 6) / 5);
                    colourDistribution[2] = Mathf.FloorToInt(2 + (d - 3) / 6);
                    colourDistribution[3] = Mathf.FloorToInt(1 + (d - 5) / 7);
                    colourDistribution[4] = Mathf.FloorToInt(1 + (d - 6) / 12);
                    colourDistribution[5] = Mathf.FloorToInt(1 + (d - 7) / 16);
                    break;
            }
        }
        else
        {
            // Advanced areas from difficulty 10 onwards
            // Consider using animation curves for this
            colourDistribution[0] = Mathf.FloorToInt(5 - (d - 11) / 3);
            colourDistribution[1] = Mathf.FloorToInt(4 - (d - 10) / 5);
            colourDistribution[2] = Mathf.FloorToInt(4);
            colourDistribution[3] = Mathf.FloorToInt(3 + (d - 7) / 7);
            colourDistribution[4] = Mathf.FloorToInt(2 + (d - 9) / 4);
            colourDistribution[5] = Mathf.FloorToInt(1 + (d - 7) / 4);
        }

        return colourDistribution;
    }

    public List<int> GetColourList(int[] colourDistribution) {
        List<int> colourMaster = new List<int>();

        for (int i = 0; i < colourDistribution.Length; i++)
        {
            for (int j = 0; j < colourDistribution[i]; j++)
            {
                colourMaster.Add(i);
            }
        }

        return colourMaster;
    }

    public void FlipTiles(Vector3 pos) {
        for (int i = hiddenTiles.Count - 1; i >= 0; i--)
        {
            if ((hiddenTiles[i].transform.parent.position - pos).sqrMagnitude < sightDistance)
                hiddenTiles[i].GetComponent<TileScript>().Flip();
        }
    }

    public void FlipTiles(Vector3 pos, float rotationSpeed) {
        for (int i = hiddenTiles.Count - 1; i >= 0; i--) {
            if((hiddenTiles[i].transform.parent.position - pos).sqrMagnitude < sightDistance)
                StartCoroutine(hiddenTiles[i].GetComponent<TileScript>().Flip(rotationSpeed));
        }
    }
}