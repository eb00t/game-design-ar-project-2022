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
    [SerializeField] private List<Vector3> randomisedPositions;

    public Transform[] puzzleArray; // holds all the game puzzles
    public Transform[] ghostArray; // holds the destination objects

    public GameObject groundPlane, currentPuzzle, selectedObject;
    public Camera cam;

    // Start is called before the first frame update
    void Start()
    {
        currentPuzzle = puzzleArray[0].parent.gameObject;
        GetChildren(puzzleArray[0], pieceList);
        //randomisedPositions.Add(new Vector3(0, 0, 0));
        //randomisedPositions.Add(new Vector3(0, 0, 0));

        GetChildren(ghostArray[0], ghostList);
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
                    selectedObject.transform.rotation = hit.collider.transform.rotation;
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

    public void RotateGroundPlane(float step)
    {
        float rotY = currentPuzzle.transform.eulerAngles.y;
        float newY = rotY + step;
        currentPuzzle.transform.eulerAngles = new Vector3 (currentPuzzle.transform.localRotation.x, newY, currentPuzzle.transform.localRotation.z);
    }

    // cycles through all children of specified puzzle and adds them to a list
    private void GetChildren(Transform transform, List<Transform> list)
    {
        foreach (Transform child in transform.gameObject.GetComponentsInChildren<Transform>())
        {
            if (child.GetComponent<MeshRenderer>())
            {
                list.Add(child);
            }
        }

        if (list == pieceList)
        {
            LogLocations();
        }
    }

    private float RandomFloat(float a, float b)
    {
        float newFloat = Random.Range(a, b);

        while (newFloat < .15 && newFloat > -.15)
        {
            newFloat = Random.Range(a, b);
        }

        return newFloat;
    }

    // randomises piece positions in a specified range
    private void RandomisePieces()
    {
        foreach (Transform child in pieceList)
        {
            Vector3 newPos = new Vector3(RandomFloat(-.5f, .5f), groundPlane.transform.position.y + .01f, RandomFloat(-.5f, .5f));

            while (randomisedPositions.Contains(newPos))
            {
                newPos = new Vector3(RandomFloat(-.5f, .5f), groundPlane.transform.position.y + .01f, RandomFloat(-.5f, .5f));
            }

            child.transform.position = newPos;
            randomisedPositions.Add(newPos);
        }
        //child.transform.rotation = Quaternion.Euler(rx, ry, rz);
    }

    // holds the initial/target locations of all the pieces in piecelist 
    private void LogLocations()
    {
        pieceDest.Clear();
        pieceDest.TrimExcess();
        ghostDest.Clear();
        ghostDest.TrimExcess();

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

    public void ChangePuzzle(int index)
    {
        if (puzzleArray.Length >= index)
        {
            currentPuzzle.SetActive(false);
            currentPuzzle = puzzleArray[index].parent.gameObject;
            GetChildren(puzzleArray[index], pieceList);
            GetChildren(ghostArray[index], ghostList);
        }
    }
}
