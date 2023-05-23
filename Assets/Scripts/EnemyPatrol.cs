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
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        Vector3 destination = waypoints[index].transform.position;
        Vector3 newPos = Vector3.MoveTowards(transform.position, destination,speed * Time.deltaTime);
        transform.position = newPos;

        float distance = Vector3.Distance(transform.position, destination);
        Debug.Log(distance);
        if(distance <= 0.7f)
        {
            Debug.Log("CloseBy");
            if(index < waypoints.Count - 1)
            {
                //transform.Rotate(waypoints[index].transform.position.x, transform.position.y, waypoints[index].transform.position.z);
                index++;
            }
            else
            {
                if(isLoop)
                {
                    index = 0;
                }
                Debug.Log("Finished at destination");
            }
        }

    }
}
