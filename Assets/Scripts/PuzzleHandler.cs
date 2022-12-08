using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.UIElements;

public class PuzzleHandler : MonoBehaviour
{
    [SerializeField] private List<Transform> pieceList; // holds the current puzzle pieces
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
        
        if (Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Moved)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (pieceList.Contains(hit.transform))
                {
                    hit.transform.position = Input.GetTouch(0).position;
                }
            }
        }
        

        /*
        if (Input.GetMouseButton(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (pieceList.Contains(hit.transform))
                {
                    Debug.Log("Done");
                    hit.transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                }
            }
        }
        */
    }
}
