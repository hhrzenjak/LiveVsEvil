using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeSlider : MonoBehaviour
{
    public void setOptionValue()
    {
        Options.volume = this.GetComponent<Slider>().value;
        Debug.Log(Options.volume);
    }
}
