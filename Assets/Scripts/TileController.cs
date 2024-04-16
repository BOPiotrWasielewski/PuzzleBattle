using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileController : MonoBehaviour
{
    public GameObject[] puzzles;

    public GameObject GeneratePuzzle(float xPos, float yPos)
    {
        int randPuzzleIndex = Random.Range(0, puzzles.Length);
        GameObject puzzle = Instantiate(puzzles[randPuzzleIndex], new Vector2(xPos, yPos), Quaternion.identity);
        puzzle.name = string.Format("Rune {0}", this.name);

        return puzzle;
    }
}
