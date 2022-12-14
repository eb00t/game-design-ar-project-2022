using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.UIElements;

public class PuzzleHandler : MonoBehaviour
{
    [SerializeField] private List<Transform> pieceList; // holds the current puzzle pieces
    [SerializeField] private List<Vector3> pieceDest; // holds target positions for completed puzzles
    [SerializeField] private List<Vector3> ghostDest;
    [SerializeField] private List<Transform> ghostList;

    public Transform[] puzzleArray; // holds all the game puzzles
    public Transform[] ghostArray; // holds the destination objects

    public GameObject groundPlane, selectedObject;
    public Camera cam;

    // Start is called before the first frame update
    void Start()
    {
        GetChildren(puzzleArray[0]);
        GetChildrenObjects(ghostArray[0]);
    }

    // Update is called once per frame
    void Update()
    {
        // if mouse is clicked this casts a ray to check for
        // pieces allowed to be moved and targets that pieces will
        // move to, once a piece is placed in the right place you can no longer
        // move it
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit = CastRay();

            if (
                hit.collider == null ||
                (hit.collider != null && hit.transform.CompareTag("InPlace")) ||
                (hit.collider != null && !pieceList.Contains(hit.transform) && !ghostList.Contains(hit.transform))
                )
            {
                return;
            }
            else
            {
                if (selectedObject == null && !ghostList.Contains(hit.transform)) // select piece 
                {
                    selectedObject = hit.collider.gameObject;
                }
                else if (ghostList.Contains(hit.transform) && selectedObject != null)
                { // place piece
                    selectedObject.transform.position = hit.collider.transform.position;
                    //hit.collider.gameObject.SetActive(false);

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

    private void GetChildrenObjects(Transform transform)
    {
        foreach (Transform child in transform.gameObject.GetComponentsInChildren<Transform>())
        {
            if (child.GetComponent<MeshRenderer>())
            {
                ghostList.Add(child.transform);
            }
        }
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
