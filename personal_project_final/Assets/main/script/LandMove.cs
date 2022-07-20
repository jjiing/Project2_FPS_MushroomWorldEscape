using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class LandMove : MonoBehaviour
{
    Vector3 currentPos;
    public float range=10f;
    public float speed=1f;


    void Start()
    {
        currentPos = transform.position;
 
    }

    
    void Update()
    {

        Vector3 v = currentPos;
        v.y += range * Mathf.Sin(Time.time * speed);
        transform.position = v;

    }
}
