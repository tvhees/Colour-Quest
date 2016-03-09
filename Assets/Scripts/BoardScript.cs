using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BoardScript : MonoBehaviour {

	public GameObject[] baseHexes, advancedHexes;
    public GameObject startTiles;
    public List<GameObject> hiddenTiles = new List<GameObject>();
    public float sightDistance;

	private float zF = 1 / Mathf.Sqrt (3f);

    public void NewBoard()
    {
        if (transform.childCount > 0)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                Destroy(transform.GetChild(i).gameObject);
            }
        }

        hiddenTiles.Clear();

        transform.InstantiateChild(startTiles);

        PlaceHexes(0, baseHexes);
        PlaceHexes(1, baseHexes);
        PlaceHexes(2, baseHexes);
        PlaceHexes(3, advancedHexes);
        PlaceHexes(4, advancedHexes);

        FlipTiles(Vector3.zero);
    }

    void PlaceHexes(float j, GameObject[] hexType) {
        float i = 3 * j;
        hexType.Randomise();
        transform.InstantiateChild(hexType[0], new Vector3(i + 1f, 0f, (j + 5f) * zF), Quaternion.Euler(0f, Random.Range(0, 6) * 60f, 0));
        transform.InstantiateChild(hexType[1], new Vector3(i + 2f, 0f, (j - 4f) * zF), Quaternion.Euler(0f, Random.Range(0, 6) * 60f, 0));
        transform.InstantiateChild(hexType[2], new Vector3(i + 3f, 0f, (j + 1f) * zF), Quaternion.Euler(0f, Random.Range(0, 6) * 60f, 0));
    }

    public void FlipTiles(Vector3 pos) {
        for (int i = hiddenTiles.Count - 1; i >= 0; i--) {
            hiddenTiles[i].GetComponent<TileScript>().Flip(pos, sightDistance);
        }
    }
}