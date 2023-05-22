using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SC_ZombleAI : MonoBehaviour
{
    public float speed = 5;
    public float health = 100;
    public float viewDistance = 5;
    public float timeIdleDestination = 5;
    public Vector2 maximumIdleDestination = new Vector2(10, 10);

    bool idle = true;
    float time;

    NavMeshAgent navMesh;

    void Start()
    {
        navMesh = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        if (Time.time >= time && idle)
        {
            time = Time.time + timeIdleDestination;
            navMesh.destination = new Vector3(Random.Range(transform.position.x - maximumIdleDestination.x, transform.position.x + maximumIdleDestination.x), 0, Random.Range(transform.position.y - maximumIdleDestination.y, transform.position.y + maximumIdleDestination.y));
        }

        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, viewDistance))
        {
            if (hit.transform.name.Contains("Human"))
            {
                idle = false;
                Debug.DrawLine(transform.position, hit.point, Color.red);
                navMesh.destination = hit.transform.position;
                navMesh.transform.LookAt(hit.transform);
                if (navMesh.remainingDistance <= 2 && hit.transform.GetComponent<SC_AI>())
                {
                    hit.transform.GetComponent<SC_AI>().health--;
                }
            }
            else
            {
                idle = true;
            }
            Debug.DrawLine(transform.position, hit.point, Color.green);
        }
        else
        {
            idle = true;
            Debug.DrawLine(transform.position, transform.forward * 100, Color.green);
        }
        if (health <= 0)
        {
            DestroyImmediate(transform.gameObject);
        }
    }
}
