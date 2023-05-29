using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchManager : MonoBehaviour
{
    public ControlFreak2.DynamicTouchControl t;
    public GameObject rig;

    private void FixedUpdate()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {

                // Get the position of the mouse click in screen space
                Vector3 touchPosition = touch.position;
                Vector3 oye = touchPosition - rig.transform.position;
                t.indirectInitialVector = oye.normalized;
            }
        }

        


    }


}
