using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.HID;

public class PickUpController : MonoBehaviour
{
    [Header("Pickup Settings")]
    [SerializeField] Transform holdArea;
    private GameObject heldObj;
    private Rigidbody heldObjRB;
    public LayerMask interactible;
    private bool pickUpRC;
    private RaycastHit hit;
    private GameObject prevHit;
    public Shelf shelf;


    [Header("Physics Parameters")]
    [SerializeField] private float pickupRange = 5.0f;
    [SerializeField] private float pickupForce = 150.0f;
    private float objY;

    private void Update()
    {
        pickUpRC = Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, pickupRange, interactible);
        if (heldObj == null)
        {
            if (pickUpRC && hit.transform.gameObject.GetComponent<Outline>() != null)
            {
                hit.transform.gameObject.GetComponent<Outline>().enabled = true;
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
                if (pickUpRC)
                {
                    if (hit.transform.gameObject.layer == 3)
                    {
                        PickupObject(hit.transform.gameObject);
                    }
                    else if (hit.transform.gameObject.layer == 6)
                    {
                        GameObject headphonesStand = hit.transform.gameObject;
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
                StoreObject(hit.transform.gameObject);
            }
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

    void StoreObject(GameObject objToStore)
    {
        shelf.GiveItem(objToStore.name);
        Rigidbody objBody = objToStore.GetComponent<Rigidbody>();
        AudioSource objAudio = objToStore.GetComponentInChildren<AudioSource>();
        objBody.transform.position = new Vector3(objBody.transform.position.x, objBody.transform.position.y, objBody.transform.position.z - 300);
        objAudio.mute = true;
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
