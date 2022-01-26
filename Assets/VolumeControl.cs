using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VolumeControl : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        this.GetComponent<AudioSource>().volume = Options.volume;
    }

    // Update is called once per frame
    void Update()
    {
        this.GetComponent<AudioSource>().volume = Options.volume;
    }
}
