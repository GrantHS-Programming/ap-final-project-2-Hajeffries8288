using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SC_ZombleAI : MonoBehaviour
{
    public GameObject objToShoot;
    public string zombleType = "zom";
    public float speed = 5;
    public float health = 100;
    public float atackSpeed = 1;
    public float viewDistance = 5;
    public float timeIdleDestination = 5;
    public float attackRange = 20;
    public float attackRangeSpeed;
    public int atackDamage = 5;
    public Vector2 maximumIdleDestination = new Vector2(10, 10);

    bool idle = true;
    float time;
    float atackTime;
    float attackTimeRange;

    NavMeshAgent navMesh;
    GameObject player;
    SC_PlayerController playerController;

    void Start()
    {
        navMesh = GetComponent<NavMeshAgent>();
        player = GameObject.Find("Player");
        playerController = player.GetComponent<SC_PlayerController>();
    }

    void Update()
    {
        if (Time.time >= time && idle)
        {
            time = Time.time + timeIdleDestination;
            navMesh.destination = new Vector3(Random.Range(transform.position.x - maximumIdleDestination.x, transform.position.x + maximumIdleDestination.x), 0, Random.Range(transform.position.y - maximumIdleDestination.y, transform.position.y + maximumIdleDestination.y));
        }

        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, viewDistance, 1>>0))
        {
            if (zombleType == "zom")
            {
                if (hit.transform.name.Contains("Human"))
                {
                    idle = false;
                    Debug.DrawLine(transform.position, hit.point, Color.red);
                    navMesh.destination = hit.transform.position;
                    navMesh.transform.LookAt(hit.transform);
                    if (navMesh.remainingDistance <= 2 && hit.transform.GetComponent<SC_AI>() && Time.time >= atackTime)
                    {
                        atackTime = Time.time + atackSpeed;
                        hit.transform.GetComponent<SC_AI>().health -= atackDamage;
                        playerController.damageTakenForDamageTxt = "-" + atackDamage;
                    }
                }
                else idle = true;
            }
            else if (zombleType == "range")
            {
                if (hit.transform.name.Contains("Human"))
                {
                    Debug.DrawLine(transform.position, hit.point, Color.red);
                    navMesh.transform.LookAt(hit.transform);
                    if ((transform.position - hit.transform.position).magnitude <= attackRange)
                    {
                        if (Time.time >= attackTimeRange)
                        {
                            navMesh.destination = transform.position;
                            GameObject instObjToShoot = Instantiate(objToShoot, transform, false);
                            instObjToShoot.transform.position = transform.position;
                            instObjToShoot.transform.localPosition = Vector3.forward;
                            instObjToShoot.transform.parent = null;
                            instObjToShoot.GetComponent<Rigidbody>().AddForce(transform.forward * 10, ForceMode.Impulse);

                            attackTimeRange = Time.time + attackRangeSpeed;
                        }
                    }
                    else navMesh.destination = hit.transform.position;
                    idle = false;
                }
            }
            else Debug.DrawLine(transform.position, hit.point, Color.green);
        }
        else
        {
            idle = true;
            Debug.DrawLine(transform.position, transform.forward * 100, Color.green);
        }
        if (health <= 0)
        {
            transform.parent.GetComponent<SC_ZombleSpawn>().numOfZombles--;
            DestroyImmediate(transform.gameObject);
        }
    }
}
