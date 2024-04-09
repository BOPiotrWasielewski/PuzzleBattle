using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardController : MonoBehaviour
{
    // Number of rows in board
    public int rows;

    // Number of cols in board
    public int cols;

    public GameObject tilePrefab;

    private GridTile[,] _gridTiles;
    // Start is called before the first frame update
    void Start()
    {
        _gridTiles = new GridTile[rows, cols];
        if (!tilePrefab) tilePrefab = new GameObject();
        Initialize();
    }

    private void Initialize()
    {
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                Vector2 temp = new Vector2((float)(-2.4 + (tilePrefab.transform.localScale.x * i)), (float)(-2.4 + (tilePrefab.transform.localScale.y * j)));
                GameObject singleTile = Instantiate(tilePrefab, temp, Quaternion.identity);
                singleTile.transform.parent = this.transform;
                singleTile.name = string.Format("({0},{1})", i, j);
            }
        }
    }
}
