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

    private bool isActive = false;
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
        if(isOpen == true)
        {
            /*if (RDoor.localRotation.eulerAngles.y <= 115f)
            {
                //RDoor.transform.rotation = Quaternion.Euler(new Vector3(0, temp, 0));
                //RDoor.Rotate(new Vector3(0, temp, 0) * Time.deltaTime);
                RDoor.localEulerAngles = new Vector3(RDoor.localEulerAngles.x, RDoor.localEulerAngles.y + temp * Time.deltaTime, RDoor.localEulerAngles.z);
            }
            if (LDoor.localRotation.eulerAngles.y >= -115f)
            {
                Debug.Log(LDoor.localRotation.eulerAngles.y);
                Debug.Log(temp1);
                //LDoor.transform.rotation = Quaternion.Euler(new Vector3(0, temp1, 0));
                //LDoor.Rotate(new Vector3(0, temp1, 0) * Time.deltaTime);
                LDoor.localEulerAngles = new Vector3(LDoor.localEulerAngles.x, LDoor.localEulerAngles.y + (temp1 * Time.deltaTime), LDoor.localEulerAngles.z);
            }*/

            if (openTimer > 0)
            {
                RDoor.localEulerAngles = new Vector3(RDoor.localEulerAngles.x, RDoor.localEulerAngles.y + temp * Time.deltaTime, RDoor.localEulerAngles.z);
                LDoor.localEulerAngles = new Vector3(LDoor.localEulerAngles.x, LDoor.localEulerAngles.y + (temp1 * Time.deltaTime), LDoor.localEulerAngles.z);
                openTimer -= Time.deltaTime;
                Debug.Log(openTimer);
            }
        } 
        else
        {
            if (openTimer < 5)
            {
                RDoor.localEulerAngles = new Vector3(RDoor.localEulerAngles.x, RDoor.localEulerAngles.y - temp * Time.deltaTime, RDoor.localEulerAngles.z);
                LDoor.localEulerAngles = new Vector3(LDoor.localEulerAngles.x, LDoor.localEulerAngles.y - (temp1 * Time.deltaTime), LDoor.localEulerAngles.z);
                if (openTimer != 5)
                {
                    openTimer += Time.deltaTime;
                }
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
            if (Input.GetKey(KeyCode.E))
            {

                if (isOpen == true)
                {
                    isOpen = false;
                }
                if (isOpen == false)
                {
                    isOpen = true;
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        
    }

    private void Interaction()
    {
        
        
    }
}
