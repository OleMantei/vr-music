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
    public LayerMask pickable;
    private bool pickUpRC;
    private RaycastHit hit;
    private GameObject prevHit;


    [Header("Physics Parameters")]
    [SerializeField] private float pickupRange = 5.0f;
    [SerializeField] private float pickupForce = 150.0f;
    private float objY;

    private void Update()
    {
        pickUpRC = Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, pickupRange, pickable);
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
                    PickupObject(hit.transform.gameObject);
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
        if (Input.GetButtonDown("Store"))
        {
            if (pickUpRC)
            {
                Rigidbody objBody = hit.transform.gameObject.GetComponent<Rigidbody>();
                Vector3 scaleChange = new Vector3(-0.015f, -0.015f, -0.015f);
                objBody.transform.localScale += scaleChange;
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
        Vector3 position = transform.position;
        position.y = objY;
        heldObjRB.transform.position = position;
        heldObjRB.constraints = RigidbodyConstraints.FreezeAll;

        heldObj.transform.parent = null;
        heldObj = null;

    }
}
