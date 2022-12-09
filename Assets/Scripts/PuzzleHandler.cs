using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.UIElements;

public class PuzzleHandler : MonoBehaviour
{
    [SerializeField] private List<Transform> pieceList; // holds the current puzzle pieces
    [SerializeField] private List<Vector3> pieceDest; // holds target positions for completed puzzles
    public Transform[] puzzleArray; // holds all the game puzzles

    public GameObject groundPlane, selectedObject;
    public Camera cam;

    private float xBounds1, xBounds2, zBounds1, zBounds2;
    private Vector3 bounds;

    // Start is called before the first frame update
    void Start()
    {
        GetChildren(puzzleArray[0]);
        LogLocations();
        RandomisePieces();

        Vector3 scale = groundPlane.transform.localScale;
        xBounds1 = groundPlane.transform.position.x - (scale.x / 2);
        xBounds2 = groundPlane.transform.position.x + (scale.x / 2);

        zBounds1 = groundPlane.transform.position.z - (scale.z / 2);
        zBounds2 = groundPlane.transform.position.z + (scale.z / 2);

        Debug.Log(xBounds1);
        Debug.Log(xBounds2);
        Debug.Log(zBounds1);
        Debug.Log(zBounds2);
    }

    // Update is called once per frame
    void Update()
    {
        SelectMovePiece();
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

    private void SelectMovePiece()
    {
        /*
        if (Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Moved)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (pieceList.Contains(hit.transform))
                {
                    Debug.Log("Touch input found");
                    
                    hit.transform.position = Input.GetTouch(0).position;
                }
            }
        }\
        */

        if (Input.GetMouseButton(0))
        {
            //Vector3 pos = cam.ScreenToWorldPoint(Input.mousePosition);
            //pos = new Vector3(Mathf.Round(pos.x), Mathf.Round(pos.y), Mathf.Round(pos.z));

            //Debug.Log("InBounds");

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
                selectedObject.transform.position = new Vector3(worldPosition.x, groundPlane.transform.position.y, worldPosition.z);

                selectedObject = null;
            }
        }

        if (selectedObject != null)
        {
            if (pieceList.Contains(selectedObject.transform))
            {
                Vector3 position = new Vector3(Input.mousePosition.x, Input.mousePosition.y, cam.WorldToScreenPoint(selectedObject.transform.position).z);
                Vector3 worldPosition = cam.ScreenToWorldPoint(position);
                selectedObject.transform.position = new Vector3(worldPosition.x, groundPlane.transform.position.y, worldPosition.z);
            }
        }

    }

    private void LogLocations()
    {
        for (int i = 0; i < pieceList.Count; i++)
        {
            pieceDest.Add(pieceList[0].position);
        }
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
