using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Createpoints : MonoBehaviour
{
    public Vector3[] points;

    public GameObject circle;
    // Start is called before the first frame update
    void Start()
    {
        foreach (var VARIABLE in points)
        {
            Instantiate(circle, VARIABLE, Quaternion.identity);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
