using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.HID;
using UnityEngine.UI;

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


    [Header("Physics Parameters")]
    [SerializeField] private float pickupRange = 5.0f;
    [SerializeField] private float UIRange = 50.0f;
    [SerializeField] private float pickupForce = 150.0f;
    private float objY;

    private void Update()
    {
        pickUpRC = Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, pickupRange, interactible);
        UIRC = Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out UIhit, UIRange, UIinteractible);

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
                if (UIRC)
                {
                    GameObject ui_element = UIhit.transform.gameObject;
                    if (ui_element.name == "Item")
                    {
                        ui_element.GetComponent<Image>();
                        string nameItem = ui_element.GetComponent<Image>().sprite.name;
                        shelf.RemoveItem(nameItem);
                        GameObject instrument = GameObject.Find(nameItem).transform.gameObject;
                        instrument.GetComponent<MeshRenderer>().enabled = true;
                        instrument.GetComponent<Rigidbody>().isKinematic = true;
                        instrument.GetComponent<MeshCollider>().enabled = true;
                        AudioSource objAudio = instrument.GetComponentInChildren<AudioSource>();
                        objAudio.mute = false;
                        if (instrument.transform.childCount > 1)
                        {
                            for (int i = 1; i < instrument.transform.childCount; i++)
                            {
                                GameObject stand = instrument.transform.GetChild(i).gameObject;
                                stand.GetComponent<MeshRenderer>().enabled = true;
                                stand.GetComponent<MeshCollider>().enabled = true;
                            }
                        }
                    }
                }
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
        objToStore.GetComponent<MeshRenderer>().enabled = false;
        objToStore.GetComponent<Rigidbody>().isKinematic = false;
        objToStore.GetComponent<MeshCollider>().enabled = false;
        AudioSource objAudio = objToStore.GetComponentInChildren<AudioSource>();
        objAudio.mute = true;
        if (objToStore.transform.childCount > 1)
        {
            for (int i = 1; i < objToStore.transform.childCount; i++)
            {
                GameObject stand = objToStore.transform.GetChild(i).gameObject;
                stand.GetComponent<MeshRenderer>().enabled = false;
                stand.GetComponent<MeshCollider>().enabled = false;
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
