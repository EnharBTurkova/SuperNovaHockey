using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public Vector3 offset;
    public float smoothSpeed = 0.125f;

    private void Start()
    {
        GetComponent<Camera>().orthographicSize = 60f;
    }
    void LateUpdate()
    {
        Vector3 desiredPosition = target.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = new Vector3(Mathf.Clamp(smoothedPosition.x, -16.5f, 16.5f),
            smoothedPosition.y, Mathf.Clamp(smoothedPosition.z, -13.5f, 13.5f));
        

     
    }
}
