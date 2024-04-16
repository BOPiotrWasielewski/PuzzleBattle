using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardController : MonoBehaviour
{
    // Number of rows in board
    public int rows;
    // Number of cols in board
    public int cols;
    
    // Prefab for grid tile element
    public GameObject tilePrefab;
    private TileController[,] _gridTiles;
    public GameObject[,] Puzzles;
    // Start is called before the first frame update
    void Start()
    {
        _gridTiles = new TileController[rows, cols];
        Puzzles = new GameObject[rows, cols];
        if (!tilePrefab) tilePrefab = new GameObject();
        Initialize();
    }

    private void Initialize()
    {
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                float xPos = (float)(-3.5 + i);
                float yPos = (float)(-3.5 + j);
                Vector2 tilePosition = new Vector2(xPos, yPos);
                
                GameObject singleTile = Instantiate(tilePrefab, tilePosition, Quaternion.identity);
                singleTile.transform.parent = this.transform;
                singleTile.name = string.Format("({0},{1})", i, j);

                _gridTiles[i,j] = singleTile.GetComponent<TileController>();
                GameObject singlePuzzle = _gridTiles[i, j].GeneratePuzzle(xPos, yPos);
                singlePuzzle.transform.parent = this.transform;
                singlePuzzle.GetComponent<PuzzleController>().UpdatePosition(i, j);
                Puzzles[i, j] = singlePuzzle;
            }
        }
    }
}
