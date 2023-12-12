using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.HID;

public class ChangeAudioLevel : MonoBehaviour
{
    private float volumeChange = 0.02f;

    void Update()
    {
        if (Input.GetButton("Louder"))
        {
            Debug.Log(AudioListener.volume);
            if (AudioListener.volume < 2.0f)
            {
                AudioListener.volume = AudioListener.volume + volumeChange;
            }


        }
        if (Input.GetButton("Quieter"))
        {
            Debug.Log(AudioListener.volume);
            if (AudioListener.volume > 0.0f)
            {
                AudioListener.volume = AudioListener.volume - volumeChange;
            }
        }
    }
}
