using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DoorAnim : MonoBehaviour
{
    public Transform RDoor;
    public Transform LDoor;

    public TextMeshProUGUI Interact;

    private bool isInTrigger = false;
    private bool isOpen = false;

    float openTimer = 5f;

    public float temp = 20;
    public float temp1 = 20;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(isInTrigger == true)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                isOpen = !isOpen;
            }
        }


        if (isOpen == true)
        {
            if (openTimer > 0)
            {
                RDoor.localEulerAngles = new Vector3(RDoor.localEulerAngles.x, RDoor.localEulerAngles.y + temp * Time.deltaTime, RDoor.localEulerAngles.z);
                LDoor.localEulerAngles = new Vector3(LDoor.localEulerAngles.x, LDoor.localEulerAngles.y - (temp * Time.deltaTime), LDoor.localEulerAngles.z);
                openTimer -= Time.deltaTime;
                Debug.Log(openTimer);
            }
        } 
        else if(isOpen == false)
        {
            if (openTimer <5)
            {
                RDoor.localEulerAngles = new Vector3(RDoor.localEulerAngles.x, RDoor.localEulerAngles.y - temp1 * Time.deltaTime, RDoor.localEulerAngles.z);
                LDoor.localEulerAngles = new Vector3(LDoor.localEulerAngles.x, LDoor.localEulerAngles.y + (temp1 * Time.deltaTime), LDoor.localEulerAngles.z);
                openTimer += Time.deltaTime;
                Debug.Log(openTimer);
            }
        }   
    }
    private void OnTriggerStay(Collider other)
    {
        if(other.tag == "Guard")
        {
            RDoor.Rotate(0, 115f, 0);
            LDoor.Rotate(0, -115f, 0);
        }
        else if(other.tag == "Player")
        {
            Interact.gameObject.SetActive(true);
            isInTrigger = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            Interact.gameObject.SetActive(false);
            isInTrigger = false;
        }
    }
}
