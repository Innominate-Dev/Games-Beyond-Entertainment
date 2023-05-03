using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cabinet : MonoBehaviour
{
    bool isCabinetOpen = false;
    float openTimer = 0.5f;
    float speed = 20f;

    // Update is called once per frame
    void Update()
    {
        if (isCabinetOpen == true)
        {
            Debug.Log("Set Open");
            if (openTimer > 0)
            {
                //transform.position = new Vector3(transform.position.x - speed * Time.deltaTime, transform.position.y, transform.position.z - speed * Time.deltaTime);

                transform.position -= transform.forward * Time.deltaTime;

                openTimer -= Time.deltaTime;
            }
        }
        else if (isCabinetOpen == false)
        {
            if (openTimer < 0.5f)
            {
                //transform.localPosition = new Vector3(transform.position.x + speed * Time.deltaTime, transform.position.y, transform.position.z + speed * Time.deltaTime);

                transform.position += transform.forward * Time.deltaTime;
                openTimer += Time.deltaTime;
            }
        }

        Debug.Log(transform.localPosition);
    }

    public bool IsCabinetOpen()
    {
        Debug.Log("TOggle Cabinet");

        return isCabinetOpen;
    }

    public void ToggleCabinet()
    {
        isCabinetOpen = !isCabinetOpen;
    }
}
