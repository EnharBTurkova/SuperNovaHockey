using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{

    public static CameraFollow instance;

    public Transform target;
    public Vector3 offset;
    public float smoothSpeed = 0.125f;

    private bool CameraShake = false;
    private void Start()
    {
        instance = this;
        GetComponent<Camera>().orthographicSize = 60f;
    }
    void LateUpdate()
    {
        if (CameraShake) {

          

        }
        else
        {


            Vector3 desiredPosition = target.position + offset;
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
            transform.position = new Vector3(Mathf.Clamp(smoothedPosition.x, 60f, 300f),
                smoothedPosition.y, Mathf.Clamp(smoothedPosition.z, -120f, 120f));
        }

     
    }

    public void CameraShaketrue()
    {
        CameraShake = true;
    }
    public void CameraShakeFalse()
    {
        CameraShake = false;
    }
}
