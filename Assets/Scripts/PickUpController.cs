using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpController : MonoBehaviour
{
    [Header("Pickup Settings")]
    [SerializeField] Transform holdArea;
    [SerializeField] GameObject Evidence;
    private GameObject heldObj;
    private Rigidbody heldObjRB;

    [Header("Physics Parameter")]
    [SerializeField] private float pickupRange = 5.0f;
    [SerializeField] private float pickupForce = 150.0f;

    [Header("Logic")]
    bool isItEvidence;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            if(heldObj == null)
            {
                RaycastHit hit;
                if(Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, pickupRange))
                {
                    if(hit.collider.tag == "Evidence")
                    {
                        //Picking Up Object 
                        isItEvidence = true;
                        PickupObject(hit.transform.gameObject);
                    }
                    else
                    {
                        isItEvidence = false;
                        PickupObject(hit.transform.gameObject);
                    }
                }
            }
            else
            {
                DropObject();
                Debug.Log("dropping");
            }
        }
        if(heldObj != null)
        {
            MoveObject();
        }
    }

    void MoveObject()
    {
        if(Vector3.Distance(heldObj.transform.position, holdArea.position) > 0.1f)
        {
            Vector3 moveDirection = (holdArea.position - heldObj.transform.position);
            heldObjRB.AddForce(moveDirection * pickupForce);
        }
    }

    void PickupObject(GameObject pickObj)
    {
        if(pickObj.GetComponent<Rigidbody>())
        {
            heldObjRB = pickObj.GetComponent<Rigidbody>();
            heldObjRB.useGravity = false;
            heldObjRB.drag = 10;
            heldObjRB.constraints = RigidbodyConstraints.FreezeRotation;

            heldObj = pickObj;

        }
    }

    void DropObject()
    {
        heldObjRB.useGravity = true;
        heldObjRB.drag = 1;
        heldObjRB.constraints = RigidbodyConstraints.None;

        if (isItEvidence == true)
        {
            heldObj.transform.parent = Evidence.transform;
            isItEvidence = false;
        }
        else
        {
            heldObjRB.transform.parent = null;
            isItEvidence = false;
        }
    }
}
