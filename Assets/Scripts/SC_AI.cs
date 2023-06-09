using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SC_AI : MonoBehaviour
{
    [HideInInspector] public GameObject holding;

    public float speed = 5;
    public float health = 100;
    public float atackSpeed = 0.5f;
    public GameObject gameOverOverlay;
    public GameObject pauseGameOverlay;

    NavMeshAgent navMesh;
    GameObject player;
    SC_PlayerController playerScript;
    float time;
    float atackDamage = 5;

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
           if (playerScript.task.transform.name.Contains("Sword") && !holding)
            {
                holding = playerScript.task;
                holding.transform.parent = transform;
                holding.transform.localPosition = new Vector3(.5f, 1, 0);
                holding.transform.GetComponent<BoxCollider>().enabled = false;
                atackDamage = 5;
                holding.transform.GetComponentInChildren<Animator>().SetBool("isAnim", false);
                playerScript.task = null;
            }
            else if (playerScript.task.transform.name.Contains("Zomble") && holding && Time.time >= time)
            {
                time = Time.time + atackSpeed;
                playerScript.task.GetComponent<SC_ZombleAI>().health -= atackDamage;
            }
           else if (playerScript.task.transform.name.Contains("PewPew") && !holding)
            {
                holding = playerScript.task;
                holding.transform.parent = transform;
                holding.transform.localPosition = new Vector3(0, 1, 0);
                holding.transform.GetComponent<BoxCollider>().enabled = false;
                holding.transform.GetComponentInChildren<Animator>().SetBool("isAnim", false);
                playerScript.task = null;
            }
           else if (playerScript.task.transform.name.Contains("Clip"))
            {
                playerScript.ammo += 7;
                Destroy(playerScript.task);
                playerScript.task = null;
            }
        }

        if (health <= 0)
        {
            Time.timeScale = 0;
            gameOverOverlay.SetActive(true);
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!pauseGameOverlay.activeSelf)
            {
                pauseGameOverlay.SetActive(true);
                Time.timeScale = 0;
            }
        }
    }

    public void ReActivateGame()
    {
        pauseGameOverlay.SetActive(false);
        Time.timeScale = 1;
    }
}
