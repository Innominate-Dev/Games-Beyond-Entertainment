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
    private float temp = 20;
    private float temp1 = -20;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(isOpen == true)
        {
            if (RDoor.localRotation.y <= 115f)
            {
                //RDoor.transform.rotation = Quaternion.Euler(new Vector3(0, temp, 0));
                RDoor.Rotate(new Vector3(0, temp, 0) * Time.deltaTime);
            }
            if (LDoor.localRotation.y >= -115f)
            {
                
                //LDoor.transform.rotation = Quaternion.Euler(new Vector3(0, temp1, 0));
                LDoor.Rotate(new Vector3(0, temp1, 0) * Time.deltaTime);
                Debug.Log(Time.deltaTime);
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
            Interaction();
            if(Input.GetKey(KeyCode.E))
            {

                if (isOpen == true)
                {
                    isOpen = false;
                }
                else
                {
                    isOpen = true;
                }
            }
        }
    }

    private void Interaction()
    {
        
        Interact.gameObject.SetActive(true);
    }
}
