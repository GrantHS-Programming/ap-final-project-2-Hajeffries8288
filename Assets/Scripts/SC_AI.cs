using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SC_AI : MonoBehaviour
{
    [HideInInspector] public GameObject holding;

    public float speed = 5;
    public float health = 100;
    public float atackSpeed = 0;
    public GameObject gameOverOverlay;

    NavMeshAgent navMesh;
    GameObject player;
    SC_PlayerController playerScript;
    float time;
    float atackDamage = 0;

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
           if (playerScript.task.transform.name.Contains("Sword"))
            {
                holding = playerScript.task;
                holding.transform.parent = transform;
                holding.transform.localPosition = new Vector3(.5f, 1, 0);
                holding.transform.GetComponent<BoxCollider>().enabled = false;
                atackDamage = 5;
                playerScript.task = null;
            }
            else if (playerScript.task.transform.name.Contains("Zomble") && holding != null && Time.time >= time)
            {
                time = Time.time + atackSpeed;
                playerScript.task.GetComponent<SC_ZombleAI>().health -= atackDamage;
            }
        }

        if (health <= 0)
        {
            Time.timeScale = 0;
            gameOverOverlay.SetActive(true);
        }
    }
}
