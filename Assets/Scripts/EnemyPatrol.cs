using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{

    public List<GameObject> waypoints;
    public float speed = 2;
    public int index = 0;
    public bool isLoop = true;
    public float timeCount;
    Vector3 destination;
    bool changingDirection = false;
    bool canSeePlayer = false;

    float DistanceRay;

    // Start is called before the first frame update
    void Start()
    {
        destination = waypoints[index].transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        float distance = Vector3.Distance(transform.position, destination);
        Debug.Log(distance);

        RaycastHit hit;

        Vector3 forward = transform.TransformDirection(Vector3.forward * 10f);


        Physics.Raycast(transform.position,forward, out hit);

        Debug.DrawRay(transform.position, forward, Color.red);

        if (hit.collider.tag == "Player")
        {
            canSeePlayer = true;
            Debug.Log("FOUND PLAYER!!!");
        }
        else
        {
            canSeePlayer = false;
        }

        Debug.Log(canSeePlayer);

        if (!changingDirection)
        {
            if (distance <= 0.7f)
            {
                Debug.Log("CloseBy");
                if (index < waypoints.Count - 1)
                {
                    ChangeWaypoint();
                }
                else
                {
                    if (isLoop)
                    {
                        index = -1;
                    }
                    Debug.Log("Finished at destination");
                }
            }
            else
            {
                Vector3 newPos = Vector3.MoveTowards(transform.position, destination, speed * Time.deltaTime);
                transform.position = newPos;
            }
        } 
        else
        {
            Vector3 targetRotation = (destination - transform.position).normalized;

            transform.forward = Vector3.RotateTowards(transform.forward, targetRotation, Time.deltaTime, Time.deltaTime);
            //transform.LookAt(waypoints[index].transform.position);
            if (Vector3.Dot(transform.forward, targetRotation) > 0.95f)
            {
                changingDirection = false;
            }
        }
    }

    void ChangeWaypoint()
    {
        index++;
        destination = waypoints[index].transform.position;
        changingDirection = true;
    }
}
