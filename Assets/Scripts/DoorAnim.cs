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

    public 

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Guard")
        {
            RDoor.Rotate(0, 115f, 0);
            LDoor.Rotate(0, -115f, 0);
        }
        else if(other.tag == "Player")
        {

        }
    }

    private void Interaction()
    {
        
        Interact.gameObject.SetActive(true);
    }
}
