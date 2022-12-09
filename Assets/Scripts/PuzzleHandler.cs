using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.UIElements;

public class PuzzleHandler : MonoBehaviour // TODO: restrict movement to one axis at a time (e.g. move back and forward only)
{
    [SerializeField] private List<Transform> pieceList; // holds the current puzzle pieces
    [SerializeField] private List<Vector3> pieceDest; // holds target positions for completed puzzles
    public Transform[] puzzleArray; // holds all the game puzzles

    public GameObject groundPlane, selectedObject;
    public Camera cam;
    private float step = 0.1f;

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
        SelectMovePiece();

        // checks piece locations and their target, if target is within dist
        // it snaps to the target and is immovable
        foreach (Transform child in pieceList)
        {
            for (int i = 0; i < pieceDest.Count; i++)
            {
                float dist = Vector3.Distance(child.position, pieceDest[i]);
                if (dist < 0.1f)
                {
                    pieceList.Remove(child);
                    pieceDest.Remove(pieceDest[i]);
                    child.transform.position = pieceDest[i];
                    selectedObject = null;
                }
            }
        }
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

            Debug.Log(x);
            Debug.Log(z);

            child.transform.position = new Vector3(x, groundPlane.transform.position.y, z);
        }
    }

    // allows pieces in piecelist to be moved when clicked on
    private void SelectMovePiece()
    {
        if (Input.GetMouseButton(0))
        {
            if (selectedObject == null)
            {
                RaycastHit hit = CastRay();

                if (!pieceList.Contains(hit.transform))
                {
                    return;
                }

                selectedObject = hit.collider.gameObject;
            }
            else
            {
                Vector3 position = new Vector3(Input.mousePosition.x, Input.mousePosition.y, cam.WorldToScreenPoint(selectedObject.transform.position).z);
                Vector3 worldPosition = cam.ScreenToWorldPoint(position);
                selectedObject.transform.position = new Vector3(worldPosition.x, worldPosition.y, worldPosition.z);

                selectedObject = null;
            }
        }

        if (selectedObject != null)
        {
            if (pieceList.Contains(selectedObject.transform))
            {
                Vector3 position = new Vector3(Input.mousePosition.x, Input.mousePosition.y, cam.WorldToScreenPoint(selectedObject.transform.position).z);
                Vector3 worldPosition = cam.ScreenToWorldPoint(position);
                selectedObject.transform.position = new Vector3(worldPosition.x, worldPosition.y, worldPosition.z);
            }
        }

    }

    // holds the initial/target locations of all the pieces in piecelist 
    private void LogLocations()
    {
        for (int i = 0; i < pieceList.Count; i++)
        {
            pieceDest.Add(pieceList[i].position);
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
