using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    public Vector3 nextPosition;

    private float startTime;

    private Vector3 startPosition;

    public float journeyTime = 2.0f;
    public float rotationSpeed;
    // Start is called before the first frame update
    void Start()
    {
        startTime = Time.deltaTime;
        startPosition = this.transform.position;


        transform.position = new Vector3(transform.position.x, transform.position.y + 2*1, 0);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 first = new Vector3(-4.12f, -2.24f, 0);
        Vector3 second = new Vector3(2, -2.24f, 0);
        Vector3 center = (first+ second) * 0.5F;

        center += Vector3.down;
        
        Vector3 firstC = first - center;
        Vector3 secC = second - center;

        float fracComplete = (Time.time - startTime) / journeyTime;
        
        transform.position = Vector3.Slerp(firstC, secC, fracComplete);
        transform.position += center;
        
        //Vector3 center = (startPosition + nextPosition) * 0.5F;

        //ransform.RotateAround(center, new Vector3(0, 0, 1), -rotationSpeed);
    }
}
