using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.HID;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem.XR;

public class PickUpController : MonoBehaviour
{
    [Header("Pickup Settings")]
    [SerializeField] Transform holdArea;
    private GameObject heldObj;
    private Rigidbody heldObjRB;
    public LayerMask interactible;
    public LayerMask UIinteractible;
    private bool pickUpRC;
    private bool UIRC;
    private RaycastHit hit;
    private RaycastHit UIhit;
    private GameObject prevHit;
    public Shelf shelf;
    AudioSource[] sources;


    [Header("Physics Parameters")]
    [SerializeField] private float pickupRange = 5.0f;
    [SerializeField] private float UIRange = 50.0f;
    [SerializeField] private float pickupForce = 150.0f;
    private float objY;
    public System.Int32 currentSong;
    public System.Int32 nbSongs;

    void Start()
    {
        sources = GameObject.FindObjectsOfType(typeof(AudioSource)) as AudioSource[];
        ChangeSong();
    }

    private void Update()
    {
        pickUpRC = Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, pickupRange, interactible);
        UIRC = Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out UIhit, UIRange, UIinteractible);

        if (heldObj == null)
        {
            if (pickUpRC && hit.transform.gameObject.GetComponent<Outline>() != null)
            {
                hit.transform.gameObject.GetComponent<Outline>().enabled = true;
                if (hit.transform.gameObject.layer == 3)
                {
                    hit.transform.gameObject.GetComponent<Outline>().OutlineColor = new Color(0.9547f, 0.5077f, 0.0414f);
                }
                else if (hit.transform.gameObject.layer == 6)
                {
                    hit.transform.gameObject.GetComponent<Outline>().OutlineColor = new Color(0.3f, 0.8f, 1.0f);
                }
                
                if (prevHit != null && prevHit != hit.transform.gameObject)
                {
                    prevHit.GetComponent<Outline>().enabled = false;
                }
                prevHit = hit.transform.gameObject;
            }
            if (!pickUpRC && prevHit != null)
            {
                prevHit.GetComponent<Outline>().enabled = false;
            }
        }
        

        if (Input.GetButtonDown("Interact") || Input.GetKey("b"))
        {
            if (heldObj == null)
            {
                if (UIRC)
                {
                    GameObject ui_element = UIhit.transform.gameObject;
                    if (ui_element.name == "Item")
                    {
                        string nameItem = ui_element.GetComponent<Image>().sprite.name;
                        shelf.RemoveItem(nameItem);
                        ShowObject(GameObject.Find(nameItem).transform.gameObject, true);
                    }
                }
                if (pickUpRC)
                {
                    if (hit.transform.gameObject.layer == 3)
                    {
                        PickupObject(hit.transform.gameObject);
                        hit.transform.gameObject.GetComponent<Outline>().OutlineColor = Color.green;

                    }
                    else if (hit.transform.gameObject.layer == 6)
                    {
                        if (hit.transform.gameObject.name == "Headphones")
                        {
                            HeadphonesInteract(hit.transform.gameObject);
                        }
                        else if (hit.transform.gameObject.name == "Jukebox")
                        {
                            ChangeSong();
                        }
                        else if (hit.transform.gameObject.name == "Door_Main"|| hit.transform.gameObject.name == "doorWing")
                        {
                            Application.Quit();
                        }
                    }
                }
            }
            else
            {
                DropObject();
            }
        }
        if (heldObj!= null)
        {
            MoveObject();
        }
        else if (Input.GetButtonDown("Store"))
        {
            if (pickUpRC)
            {
                shelf.GiveItem(hit.transform.gameObject.name);
                ShowObject(hit.transform.gameObject, false);
            }
        }
    }

    void ChangeSong()
    {
        currentSong++;
        if (SceneManager.GetActiveScene().name == "MainRoom")
        {
            if (currentSong > 2)
            {
                currentSong = 1;
            }
        }
        else if (SceneManager.GetActiveScene().name == "SecondRoom")
        {
            if (currentSong > 3)
            {
                currentSong = 1;
            }
        }

        foreach (AudioSource audioSource in sources)
        {
            GameObject instrument = audioSource.transform.parent.gameObject.transform.parent.gameObject;
            shelf.RemoveItem(instrument.name);
            ShowObject(instrument, false);
        }

        foreach (AudioSource audioSource in sources)
        {
            GameObject instrument = audioSource.transform.parent.gameObject.transform.parent.gameObject;

            string nameOfSource = audioSource.name;
            int songNumber = (int)char.GetNumericValue(nameOfSource[nameOfSource.Length - 1]);
            if (songNumber == currentSong)
            {
                ShowObject(instrument, true);
                audioSource.enabled = true;
            }
            else
            {
                audioSource.enabled = false;
            }
        }
    }

    void HeadphonesInteract(GameObject headphonesStand)
    {
        GameObject headphones = headphonesStand.transform.GetChild(0).gameObject;
        MeshRenderer headphonesMesh = headphones.GetComponentInChildren<MeshRenderer>();
        if (headphonesMesh.enabled)
        {
            AudioListener listenerPlayer = GameObject.Find("FirstPersonCamera").GetComponent<AudioListener>();
            AudioListener listenerMic = GameObject.Find("MicListener").GetComponent<AudioListener>();
            Canvas headphonesUI = GameObject.Find("Headphones_Canvas").GetComponent<Canvas>();
            listenerPlayer.enabled = false;
            listenerMic.enabled = true;
            headphonesUI.enabled = true;
            headphonesMesh.enabled = false;
        }
        else
        {
            AudioListener listenerPlayer = GameObject.Find("FirstPersonCamera").GetComponent<AudioListener>();
            AudioListener listenerMic = GameObject.Find("MicListener").GetComponent<AudioListener>();
            Canvas headphonesUI = GameObject.Find("Headphones_Canvas").GetComponent<Canvas>();
            listenerPlayer.enabled = true;
            listenerMic.enabled = false;
            headphonesUI.enabled = false;
            headphonesMesh.enabled = true;
        }
    }

    void MoveObject()
    {
        if (Vector3.Distance(heldObj.transform.position, holdArea.position) > 0.1f)
        {
            Vector3 moveDirection = (holdArea.position - heldObj.transform.position);
            heldObjRB.AddForce(moveDirection * pickupForce);
        }
    }

    void ShowObject(GameObject instrument, bool show)
    {
        instrument.GetComponent<MeshRenderer>().enabled = show ? true : false;
        instrument.GetComponent<Rigidbody>().isKinematic = show ? true : false;
        instrument.GetComponent<MeshCollider>().enabled = show ? true : false;
        AudioSource[] objAudio = instrument.GetComponentsInChildren<AudioSource>();
        foreach (AudioSource audioSource in objAudio)
        {
            audioSource.mute = show ? false : true;
        }
        if (instrument.transform.childCount > 1)
        {
            for (int i = 1; i < instrument.transform.childCount; i++)
            {
                GameObject stand = instrument.transform.GetChild(i).gameObject;
                stand.GetComponent<MeshRenderer>().enabled = show ? true : false;
                stand.GetComponent<MeshCollider>().enabled = show ? true : false;
            }
        }
    }

    void PickupObject(GameObject pickObj)
    {
        if (pickObj.GetComponent<Rigidbody>())
        {
            heldObjRB = pickObj.GetComponent<Rigidbody>();
            objY = heldObjRB.position.y;
            heldObjRB.useGravity = false;
            heldObjRB.drag = 10;
            heldObjRB.constraints &= ~RigidbodyConstraints.FreezePositionY;

            heldObjRB.transform.parent = holdArea;
            heldObj = pickObj;
        }
    }

    void DropObject()
    {
        heldObjRB.useGravity = true;
        heldObjRB.drag = 1;
        heldObjRB.transform.position = new Vector3(heldObjRB.transform.position.x, objY, heldObjRB.transform.position.z);
        heldObjRB.constraints = RigidbodyConstraints.FreezeAll;

        heldObj.transform.parent = null;
        heldObj = null;
    }
}
