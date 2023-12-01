using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowHideCanvas : MonoBehaviour
{
    private Canvas menuSystem;

    void Start()
    {
        menuSystem = GetComponent<Canvas>();
        menuSystem.enabled = !menuSystem.enabled;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Paus"))
        {
            menuSystem.enabled = !menuSystem.enabled;
        }
    }
}
