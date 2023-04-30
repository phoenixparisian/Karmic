using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform target; // The target to follow (i.e., the player)
    [SerializeField] private Vector3 offset; // The offset between the camera and the target

    private void LateUpdate()
    {
        // Update the position of the camera to match the position of the target plus the offset
        transform.position = target.position + offset;
    }
}