using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.UIElements;

public class PuzzleHandler : MonoBehaviour
{
    private List<Transform> pieceList; // holds the current puzzle pieces
    public Transform[] puzzleArray; // holds all the game puzzles

    private Vector3 position;
    private float width;
    private float height;

    // Start is called before the first frame update
    void Start()
    {
        GetChildren(puzzleArray[0]);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // cycles through all children of specified puzzle and adds them to a list
    private void GetChildren(Transform transform) 
    {
        foreach (Transform child in transform.gameObject.GetComponentsInChildren<Transform>())
        {
            if (child.GetComponent<MeshRenderer>())
            {
                pieceList.Add(child);
            }  
        }
    }

    private void SelectMovePiece()
    {
        Touch touch = Input.GetTouch(0);
        width = (float)Screen.width / 2.0f;
        height = (float)Screen.height / 2.0f;

        if (touch.phase == TouchPhase.Began)
        {
            Vector2 pos = touch.position;
            pos.x = (pos.x - width) / width;
            pos.y = (pos.y - height) / height;
            position = new Vector3(-pos.x, pos.y, 0.0f);


        }
    }
}
