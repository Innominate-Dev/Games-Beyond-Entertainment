using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Runtime.CompilerServices;
using static UnityEngine.GraphicsBuffer;

public class Interactable : MonoBehaviour
{
    Camera cam;

    [Header("References")]

    public float DistanceX = 0.5f;
    public float DistanceY = 0.5f;

    public float DistanceRay = 0.1f;

    public RawImage UICrosshair;
    public TextMeshProUGUI Interaction;

    public TextMeshProUGUI EvidenceText;

    public Animator EvidenceOpening;
    public Animator CabinetOpening;

    [Header("Logic")]
    private float EvidenceFound = 0;
    private bool isInspecting;
    private bool isCabinetOpen;

    float openTimer = 5f;

    public float temp = 20;
    public float temp1 = 20;

    // Start is called before the first frame update
    void Start()
    {
        cam = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {

        Ray ray = cam.ViewportPointToRay(new Vector3(DistanceX, DistanceY, DistanceRay));
        RaycastHit hit;
        if (Physics.Linecast(ray.origin, ray.origin + ray.direction * DistanceRay, out hit))
        {
            Debug.Log(hit.collider.name);
            if(hit.collider.tag == "Evidence")
            {
                InteractionActive();
                if(Input.GetKeyDown(KeyCode.E) && isInspecting == false) 
                {
                    EvidenceOpening = hit.collider.GetComponent<Animator>();
                    EvidenceOpening.SetBool("IsInspecting", true);
                    Debug.Log("Picked Up");
                    isInspecting = true;
                }
                else if(Input.GetKeyDown(KeyCode.E) && isInspecting == true)
                {
                    EvidenceOpening = hit.collider.GetComponent<Animator>();
                    EvidenceOpening.SetBool("IsInspecting", false);
                    Debug.Log("Picked Up");
                    isInspecting = false;
                }
                if(Input.GetMouseButton(0))
                {
                    EvidenceFound += 1;
                    Destroy(hit.collider.gameObject);
                    EvidenceText.text = "Evidence Found " + EvidenceFound + "/10";
                }
            }
            else if(hit.collider.tag == "Cabinet")
            {
                InteractionActive();
                if(Input.GetKeyDown(KeyCode.E) && isCabinetOpen == false)
                {
                    //hit.collider.transform.localEulerAngles = new Vector3(hit.collider.transform.localEulerAngles.x + temp * Time.deltaTime, hit.collider.transform.localEulerAngles.y, hit.collider.transform.localEulerAngles.z + temp * Time.deltaTime);
                    hit.collider.GetComponent<Rigidbody>().velocity = transform.forward * Time.deltaTime;
                    openTimer -= Time.deltaTime;
                    Debug.Log(openTimer);
                    isCabinetOpen = true;
                }
                else if (Input.GetKeyDown(KeyCode.E) && isCabinetOpen == true)
                {
                    CabinetOpening = hit.collider.GetComponent<Animator>();
                    CabinetOpening.SetBool("isOpen", false);
                    isCabinetOpen = false;
                }
            }
            else
            {
                InteractionDeactive();
            }
        }
        else
        {
            InteractionDeactive();
        }

        Debug.DrawLine(transform.position, hit.point, Color.blue);
    }

    public void InteractionActive()
    {
        UICrosshair.gameObject.SetActive(false);
        Interaction.gameObject.SetActive(true);
    }

    public void InteractionDeactive()
    {
        UICrosshair.gameObject.SetActive(true);
        Interaction.gameObject.SetActive(false);
    }

}
