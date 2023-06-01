using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Transform BallLocation;
    public Transform Spawnpoint;

    public void Restart()
    {
        this.transform.position= new Vector3(Spawnpoint.position.x,this.transform.position.y,Spawnpoint.position.z);
    }
}
