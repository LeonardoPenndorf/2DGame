using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawTeleportSquare : MonoBehaviour
{
    public Vector3 rectangleSize;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red; // Set the color of the rectangle
        Gizmos.DrawWireCube(transform.position, rectangleSize); // Draw a wireframe cube at the object's position with the specified size
    }
}
