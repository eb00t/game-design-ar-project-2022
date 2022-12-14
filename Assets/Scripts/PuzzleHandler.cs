using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.UIElements;

public class PuzzleHandler : MonoBehaviour // TODO: click to select object and click position to place
{
    [SerializeField] private List<Transform> pieceList; // holds the current puzzle pieces
    [SerializeField] private List<Vector3> pieceDest; // holds target positions for completed puzzles
    [SerializeField] private List<Vector3> ghostDest;

    public Transform[] puzzleArray; // holds all the game puzzles
    public Transform[] ghostArray; // holds the destination objects

    public GameObject groundPlane, selectedObject;
    public Camera cam;

    private float xBounds1, xBounds2, zBounds1, zBounds2;
    private Vector3 bounds;

    // Start is called before the first frame update
    void Start()
    {
        GetChildren(puzzleArray[0]);

        Vector3 scale = groundPlane.transform.localScale;
        xBounds1 = groundPlane.transform.position.x - (scale.x / 2);
        xBounds2 = groundPlane.transform.position.x + (scale.x / 2);

        zBounds1 = groundPlane.transform.position.z - (scale.z / 2);
        zBounds2 = groundPlane.transform.position.z + (scale.z / 2);
    }

    // Update is called once per frame
    void Update()
    {
        //SelectMovePiece();

        PieceMovement();

        // checks piece locations and their target, if target is within dist
        // it snaps to the target and is immovable

        /*
        for (int i = 0; i < pieceList.Count; i++)
        {
            if (pieceList[i].gameObject.CompareTag("InPlace"))
            {
                float dist = Vector3.Distance(pieceList[i].position, pieceDest[i]);

                if (dist < 5f)
                {
                    pieceList[i].gameObject.tag = "InPlace";
                    pieceList[i].transform.position = pieceDest[i];
                    //pieceList[i].transform.rotation = pieceRot[i];
                    selectedObject = null;
                }
            }
        }
        */
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

        LogLocations();
    }

    // randomises piece positions in a specified range
    private void RandomisePieces()
    {
        foreach (Transform child in pieceList)
        {
            float x = Random.Range(-.5f, .5f);
            float z = Random.Range(-.5f, .5f);

            float rx = Random.Range(0, 360);
            float ry = Random.Range(0, 360);
            float rz = Random.Range(0, 360);

            child.transform.position = new Vector3(x, groundPlane.transform.position.y, z);
            //child.transform.rotation = Quaternion.Euler(rx, ry, rz);
        }
    }

    /*
    // allows pieces in piecelist to be moved when clicked on
    private void SelectMovePiece()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (selectedObject == null)
            {
                RaycastHit hit = CastRay();

                if (hit.collider == null || hit.transform.CompareTag("InPlace"))
                {
                    return;
                }

                selectedObject = hit.collider.gameObject;
            }
            else if (pieceList.Contains(selectedObject.transform) && !selectedObject.CompareTag("InPlace"))
            {
                Vector3 position = new Vector3(Input.mousePosition.x, Input.mousePosition.y, cam.WorldToScreenPoint(selectedObject.transform.position).z);
                Vector3 worldPosition = cam.ScreenToWorldPoint(position);
                selectedObject.transform.position = new Vector3(worldPosition.x, 0, worldPosition.z);

                selectedObject = null;
            }
        }

        if (selectedObject != null)
        {
            if (pieceList.Contains(selectedObject.transform) && !selectedObject.CompareTag("InPlace"))
            {
                Vector3 position = new Vector3(Input.mousePosition.x, Input.mousePosition.y, cam.WorldToScreenPoint(selectedObject.transform.position).z);
                Vector3 worldPosition = cam.ScreenToWorldPoint(position);
                selectedObject.transform.position = new Vector3(worldPosition.x, 0, worldPosition.z);
            }
        }
    }
    */

    private void PieceMovement()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit = CastRay();

            if (
                hit.collider == null ||
                (hit.collider != null && hit.transform.CompareTag("InPlace")) ||
                (hit.collider != null && !pieceList.Contains(hit.transform) && !ghostDest.Contains(hit.transform.position))
                )
            {
                Debug.Log("failed");
                return;
            }
            else
            {
                Debug.Log("success");
                if (selectedObject == null && !ghostDest.Contains(hit.transform.position)) // select piece
                { 
                    selectedObject = hit.collider.gameObject;
                }
                else if (ghostDest.Contains(hit.collider.gameObject.transform.position) && selectedObject != null)
                { // place piece
                    Debug.Log("piece placed");
                    selectedObject.transform.position = hit.collider.transform.position;

                    for (int i = 0; i < pieceList.Count; i++)
                    {
                        if (pieceList[i].gameObject == selectedObject)
                        {
                            if (pieceList[i].position == ghostDest[i])
                            {
                                selectedObject.tag = "InPlace";
                            }
                        }
                    }

                    selectedObject = null;
                }

                /*
                if (!ghostDest.Contains(hit.collider.gameObject.transform.position) && selectedObject != null)
                {
                    selectedObject = null;
                }
                */
            }
        }
    }

    // holds the initial/target locations of all the pieces in piecelist 
    private void LogLocations()
    {
        for (int i = 0; i < pieceList.Count; i++)
        {
            pieceDest.Insert(i, pieceList[i].position);
            ghostDest.Insert(i, pieceList[i].position);

            //pieceRot.Insert(i, pieceList[i].rotation);
        }

        RandomisePieces();
    }

    private RaycastHit CastRay()
    {
        {
            Vector3 screenMousePosFar = new Vector3(
                Input.mousePosition.x,
                Input.mousePosition.y,
                cam.farClipPlane);

            Vector3 screenMousePosNear = new Vector3(
                Input.mousePosition.x,
                Input.mousePosition.y,
                cam.nearClipPlane);
            Vector3 worldMousePosFar = cam.ScreenToWorldPoint(screenMousePosFar);
            Vector3 worldMousePosNear = cam.ScreenToWorldPoint(screenMousePosNear);

            RaycastHit hit;

            Physics.Raycast(worldMousePosNear, worldMousePosFar - worldMousePosNear, out hit);

            return hit;
        }
    }
}
