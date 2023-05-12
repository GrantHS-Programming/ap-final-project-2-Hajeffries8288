using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SC_AI : MonoBehaviour
{
    public float speed = 5;
    public float health = 100;

    NavMeshAgent navMesh;
    GameObject player;
    SC_PlayerController playerScript;

    void Start()
    {
        navMesh = GetComponent<NavMeshAgent>();
        player = GameObject.Find("Player");
        playerScript = player.GetComponent<SC_PlayerController>();
    }

    void Update()
    {
        if (playerScript.setAIDestination != new Vector3(999999999999, 0, 999999999999))
        {
            navMesh.SetDestination(playerScript.setAIDestination);
        }
        else
        {
            navMesh.SetDestination(transform.position);
        }

        if (Vector3.Distance(transform.position, navMesh.destination) <= 1 && playerScript.task != null)
        {
            if (playerScript.task.transform.name.Contains("P_Tree"))
            {
                playerScript.task.GetComponent<Rigidbody>().isKinematic = false;
                playerScript.task.GetComponent<Rigidbody>().AddForce(Vector3.forward);
                playerScript.wood++;
                playerScript.task.transform.name = "P_DeadTree";
                playerScript.task.GetComponent<SC_Tree>().despawn = true;
                playerScript.task = null;
                print("wood: " + playerScript.wood);
            }
        }
    }
}
