using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SC_AI : MonoBehaviour
{
    public float speed = 5;
    public float health = 100;
    public GameObject deadAI;

    NavMeshAgent navMesh;
    GameObject player;
    GameObject holding;
    SC_PlayerController playerScript;

    void Start()
    {
        navMesh = GetComponent<NavMeshAgent>();
        player = GameObject.Find("Player");
        playerScript = player.GetComponent<SC_PlayerController>();
    }

    void Update()
    {
        if (playerScript.setAIDestination != new Vector3(999999999999, 0, 999999999999) && !playerScript.atackTask)
        {
            navMesh.SetDestination(playerScript.setAIDestination);
        }
        else if (!playerScript.atackTask)
        {
            navMesh.SetDestination(transform.position);
        }
        else if (playerScript.task)
        {
            navMesh.SetDestination(playerScript.task.transform.position);
        }

        if (Vector3.Distance(transform.position, navMesh.destination) <= 1.5f && playerScript.task != null)
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
            else if (playerScript.task.transform.name.Contains("Sword"))
            {
                holding = playerScript.task;
                holding.transform.parent = transform;
                holding.transform.localPosition = new Vector3(.5f, 1, 0);
                holding.transform.GetComponent<BoxCollider>().enabled = false;
                playerScript.task = null;
            }
            else if (playerScript.task.transform.name.Contains("Zomble") && holding != null)
            {
                playerScript.task.GetComponent<SC_ZombleAI>().health--;
            }
        }

        if (health <= 0)
        {
            GameObject instDeadAI = Instantiate(deadAI);
            instDeadAI.transform.position = new Vector3(transform.position.x, 0, transform.position.z);
            instDeadAI.transform.rotation = transform.rotation;
            DestroyImmediate(transform.gameObject);
        }
    }
}
