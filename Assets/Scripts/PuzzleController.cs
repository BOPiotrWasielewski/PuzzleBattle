using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PuzzleController : MonoBehaviour
{
    public int xPos;
    public int yPos;
    [Range(0.0f, 1.0f)]public float movingTime = 1.0f;
    private float _targetX;
    private float _targetY;

    public bool isMatch = false;
    
    private BoardController _board;
    private Vector2 _startPosition;
    private Vector2 _destinationPosition;
    private GameObject _targetPuzzle;
    private float _moveAngle = 0.0f;

    void Start()
    {
        _board = FindObjectOfType<BoardController>();
    }

    void Update()
    {
        isMatch = false;
        CheckForMarch();
        if (isMatch)
        {
            Debug.Log("Match: "+name);
        }
        _targetX = (float)(-3.5 + xPos);
        _targetY = (float)(-3.5 + yPos);
        if (Mathf.Abs(_targetX - transform.position.x) > .1)
        {
            StartCoroutine(Jump(_targetX, transform.position.y));
        }
        if (Mathf.Abs(_targetY - transform.position.y) > .1)
        {
            StartCoroutine(Jump(transform.position.x, _targetY));
        }
        
        _board.Puzzles[xPos, yPos] = gameObject;
    }

    private IEnumerator Jump(float targetX, float targetY)
    {
        Vector2 startPosition = transform.position;
        float currentTime = 0.0f;
        bool isJump = true;

        while (currentTime < movingTime && isJump)
        {
            float percentage = currentTime / movingTime;
            currentTime += Time.deltaTime;
            transform.position = Vector2.Lerp(startPosition, new Vector2(targetX, targetY), percentage);

            if (currentTime >= movingTime)
            {
                currentTime = movingTime;
                isJump = false;
            }
            yield return null;
        }
    }

    private void OnMouseDown()
    {
        if (Camera.main != null) _startPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    private void OnMouseUp()
    {
        if (Camera.main != null) _destinationPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        CalculateMoveAngle();
    }

    public void UpdatePosition(int x = 0, int y = 0)
    {
        xPos = x; yPos = y;
    }

    private void CalculateMoveAngle()
    {
        Debug.Log(string.Format("move x: {0}", Mathf.Abs(_destinationPosition.x - _startPosition.x)));
        Debug.Log(string.Format("move y: {0}", Mathf.Abs(_destinationPosition.y - _startPosition.y)));
        if (Mathf.Abs(_destinationPosition.x - _startPosition.x) < 0.5f && Mathf.Abs(_destinationPosition.y - _startPosition.y) < 0.5f) return;
        _moveAngle = Mathf.Atan2(_destinationPosition.y - _startPosition.y, _destinationPosition.x - _startPosition.x) * 180 / Mathf.PI;
        Move();
    }

    void Move()
    {
        if (_moveAngle is <= 45 and > -45 && xPos < _board.cols - 1) // Move Right
        {
            _targetPuzzle = _board.Puzzles[xPos + 1, yPos];
            _targetPuzzle.GetComponent<PuzzleController>().xPos -= 1;
            xPos += 1;
        } else if (_moveAngle is <= 135 and > 45 && yPos < _board.rows - 1) // Move Up
        {
            _targetPuzzle = _board.Puzzles[xPos, yPos + 1];
            _targetPuzzle.GetComponent<PuzzleController>().yPos -= 1;
            yPos += 1;
        } else if (_moveAngle is <= -135 or > 135 && xPos > 0) // Move Left
        {
            _targetPuzzle = _board.Puzzles[xPos - 1, yPos];
            _targetPuzzle.GetComponent<PuzzleController>().xPos += 1;
            xPos -= 1;
        } else if (_moveAngle is >= -135 and < -45 && yPos > 0) // Move Down
        {
            _targetPuzzle = _board.Puzzles[xPos, yPos - 1];
            _targetPuzzle.GetComponent<PuzzleController>().yPos += 1;
            yPos -= 1;
        }
    }

    void CheckForMarch()
    {
        if (xPos > 0 && xPos < _board.cols - 1)
        {
            GameObject prevPuzzle = _board.Puzzles[xPos - 1, yPos];
            GameObject nextPuzzle = _board.Puzzles[xPos + 1, yPos];
            if (prevPuzzle.CompareTag(tag) && nextPuzzle.CompareTag(tag)) isMatch = prevPuzzle.GetComponent<PuzzleController>().isMatch = nextPuzzle.GetComponent<PuzzleController>().isMatch = true;
        }
        if (yPos > 0 && yPos < _board.rows - 1)
        {
            GameObject prevPuzzle = _board.Puzzles[xPos, yPos - 1];
            GameObject nextPuzzle = _board.Puzzles[xPos, yPos + 1];
            if (prevPuzzle.CompareTag(tag) && nextPuzzle.CompareTag(tag)) isMatch = prevPuzzle.GetComponent<PuzzleController>().isMatch = nextPuzzle.GetComponent<PuzzleController>().isMatch = true;
        }
    }
}
