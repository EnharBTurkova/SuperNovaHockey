using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{

    public static CameraFollow instance;
    public float shakeDuration = 0.2f;
    public float shakeMagnitude;

    private Vector3 initialPosition;

    public Transform target;
    public Vector3 offset;
    public float smoothSpeed = 0.125f;

    private bool canShake = false;
    private void Start()
    {
        instance = this;
        GetComponent<Camera>().orthographicSize = 60f;
    }
    void LateUpdate()
    {
        if (canShake)
        {
            initialPosition = transform.localPosition;
            if (shakeDuration > 0)
            {
                // Generate a random offset within a range for the camera position
                Vector3 randomOffset = Random.insideUnitSphere * shakeMagnitude;

                // Apply the offset to the camera position
                transform.localPosition = initialPosition + randomOffset;

                // Reduce the shake duration over time based on the damping speed
                shakeDuration -= Time.deltaTime;
            }
            else
            {
                shakeDuration = .2f;
                canShake = false;
            }

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
        canShake = true;
    }
    public void CameraShakeFalse()
    {
        canShake = false;
    }
}
