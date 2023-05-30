using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotationManager : MonoBehaviour
{ 

    public Transform[] TargetTransform;
    public Transform []targetrotation;
    private const float maxTransform = 100f;
    public const float maxRotationAngle = 45f; // The maximum rotation angle at the top of the camera
  

    private void Update()
    {

        for (int i = 0; i < TargetTransform.Length; i++)
        {
            targetrotation[i].localRotation = Quaternion.Euler((maxRotationAngle - ((maxTransform - TargetTransform[i].localPosition.x) * 24 / 97)), 0, 0);

        }
    
     

    }

}
