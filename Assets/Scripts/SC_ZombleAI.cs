using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SC_ZombleAI : MonoBehaviour
{
    public float speed = 5;
    public float health = 100;
    public float viewDistance = 5;

    NavMeshAgent navMesh;

    void Start()
    {
        navMesh = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, viewDistance))
        {
            if (hit.transform.name.Contains("Human"))
            {
                Debug.DrawLine(transform.position, hit.point, Color.red);
                navMesh.destination = hit.transform.position;
                if (navMesh.remainingDistance <= 2 && hit.transform.GetComponent<SC_AI>())
                {
                    hit.transform.GetComponent<SC_AI>().health--;
                }
            }
            Debug.DrawLine(transform.position, hit.point, Color.green);
        }
        else
        {
            Debug.DrawLine(transform.position, transform.forward * 100, Color.green);
        }
        if (health <= 0)
        {
            DestroyImmediate(transform.gameObject);
        }
    }
}
