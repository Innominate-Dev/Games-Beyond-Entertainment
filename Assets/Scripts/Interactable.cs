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
    public RawImage HandInteract;
    public TextMeshProUGUI Interaction;

    public TextMeshProUGUI EvidenceText;

    public Animator EvidenceOpening;
    public Animator CabinetOpening;

    public AudioSource VoiceManager;
    public AudioClip WinningClip;

    [Header("Logic")]
    private float EvidenceFound = 0;
    private bool isInspecting;

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
            //Debug.Log(hit.collider.name);
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
                if(Input.GetKeyDown(KeyCode.F))
                {
                    if (EvidenceFound == 5f)
                    {
                        Debug.Log("YOU WIN!!!");
                        
                    }
                    else
                    {
                        EvidenceFound += 1;
                        if(EvidenceFound == 5f)
                        {
                            VoiceManager.PlayOneShot(WinningClip);
                            StartCoroutine(WinningScene());
                        }
                        Destroy(hit.collider.gameObject);
                        EvidenceText.text = "Evidence Found " + EvidenceFound + "/5";
                    }
                }
            }
            else if(hit.collider.tag == "Cabinet")
            {
                HandInteract.gameObject.SetActive(true);

                Cabinet cabinet = hit.collider.transform.GetComponent<Cabinet>();

                if(Input.GetMouseButtonDown(0) && cabinet.IsCabinetOpen() == false)
                {
                    //hit.collider.transform.localPosition = new Vector3(hit.collider.transform.localPosition.x + temp * Time.deltaTime, hit.collider.transform.localPosition.y, hit.collider.transform.localPosition.z + temp * Time.deltaTime);
                    //hit.collider.GetComponent<Rigidbody>().velocity = transform.forward * Time.deltaTime;
                    Debug.Log("Set cabinet open");
                    cabinet.ToggleCabinet();
                }
                else if (Input.GetMouseButtonDown(0) && cabinet.IsCabinetOpen() == true)
                {
                    //CabinetOpening = hit.collider.GetComponent<Animator>();
                    //CabinetOpening.SetBool("isOpen", false);
                    Debug.Log("Set cabinet closed");
                    cabinet.ToggleCabinet();
                }
            }
            else
            {
                 InteractionDeactive();
                HandInteract.gameObject.SetActive(false);
            }
        }
        else
        {
            InteractionDeactive();
            HandInteract.gameObject.SetActive(false);
        }

        Debug.DrawLine(transform.position, hit.point, Color.blue);

    }

    public void InteractionActive()
    {
        HandInteract.gameObject.SetActive(true);
        UICrosshair.gameObject.SetActive(false);
        Interaction.gameObject.SetActive(true);
    }

    public void InteractionDeactive()
    {
        HandInteract.gameObject.SetActive(false);
        UICrosshair.gameObject.SetActive(true);
        Interaction.gameObject.SetActive(false);
    }

    IEnumerator WinningScene()
    {
        yield return new WaitForSeconds(5f);
        UnityEngine.SceneManagement.SceneManager.LoadScene("WinScene");
    }

}
