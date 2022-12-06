using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleHandler : MonoBehaviour
{
    public Transform[] pieceArray; // holds the current puzzle pieces
    public Transform[] puzzleArray; // holds all the game puzzles

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < puzzleArray[0].childCount; i++)
        {
            pieceArray[i] = puzzleArray[i].GetChild(i);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
