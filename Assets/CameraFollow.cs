using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform playerTarget;
    public float speedCamera;
    public Vector3 offset;
    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, playerTarget.position + offset, speedCamera * Time.deltaTime);
    }
}
