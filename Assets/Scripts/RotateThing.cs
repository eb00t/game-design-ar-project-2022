using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateThing : MonoBehaviour
{
    public void RotateGroundPlane(float step)
    {
        float rotY = transform.eulerAngles.y;
        float newY = rotY + step;
        transform.eulerAngles = new Vector3(transform.localRotation.x, newY, transform.localRotation.z);
    }
}
