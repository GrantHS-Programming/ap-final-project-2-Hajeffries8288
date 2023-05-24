using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SC_PlayerController : MonoBehaviour
{
    [HideInInspector] public Vector3 setAIDestination;
    [HideInInspector] public string damageTakenForDamageTxt;
    [HideInInspector] public GameObject task;
    [HideInInspector] public bool atackTask = false;

    public GameObject thingyMcThing;
    public GameObject AI;
    public Texture2D textureThing;

    TextMeshProUGUI daysTxt;
    TextMeshProUGUI healthTxt;
    RaycastHit hit;
    GameObject placeingObject;
    GameObject dayNight;

    void Start()
    {
        daysTxt = GameObject.Find("DaysTxt").GetComponent<TextMeshProUGUI>();
        healthTxt = GameObject.Find("HealthTxt").GetComponent <TextMeshProUGUI>();
        dayNight = GameObject.Find("DayNightCycleObject");
        Cursor.lockState = CursorLockMode.Confined;
    }

    void Update()
    {
        if (Input.GetButton("Fire2"))
        {
            Cursor.SetCursor(textureThing, Vector2.zero, CursorMode.Auto);
        }

        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity))
        {
            if (Input.GetButtonDown("Fire1"))
            {
                if (!hit.transform.name.Contains("Zomble") && !placeingObject)
                {
                    setAIDestination = hit.point;
                    task = hit.transform.gameObject;
                    atackTask = false;
                    thingyMcThing.transform.position = hit.point;
                }
                else
                {
                    task = hit.transform.gameObject;
                    atackTask = true;
                }
            }
            if (Input.GetKeyDown(KeyCode.Q))
            {
                GameObject AIHolding = AI.transform.GetComponent<SC_AI>().holding;
                if (AIHolding)
                {
                    AIHolding.transform.parent = null;
                    AIHolding.transform.GetComponent<BoxCollider>().enabled = true;
                    AI.transform.GetComponent<SC_AI>().holding = null;
                }
            }
        }
        else if (Input.GetButtonDown("Fire1"))
        {
            print("Player clicked nothing");
            setAIDestination = new Vector3(999999999999, 0, 999999999999);
            task = null;
        }

        Interface();
    }

    public void Interface()
    {
        daysTxt.text = "Days: " + dayNight.GetComponent<SC_DayNight>().days;
        healthTxt.text = "Health: " + AI.GetComponent<SC_AI>().health;
    }
}
