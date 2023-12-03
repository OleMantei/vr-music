using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeRoom : MonoBehaviour
{
    public Transform player;

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            if (this.tag == "TeleportArea")
            {
                if (SceneManager.GetActiveScene().name == "MainRoom")
                {
                    SceneManager.LoadScene("SecondRoom");
                    Debug.Log("Teleport to SecondRoom");
                }
                else if (SceneManager.GetActiveScene().name == "SecondRoom")
                {
                    SceneManager.LoadScene("MainRoom");
                    Debug.Log("Teleport to MainRoom");
                }
            } 
        } 
    }
}
