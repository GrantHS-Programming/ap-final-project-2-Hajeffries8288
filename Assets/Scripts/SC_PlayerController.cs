using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class SC_PlayerController : MonoBehaviour
{
    [HideInInspector] public Vector3 setAIDestination;
    [HideInInspector] public string damageTakenForDamageTxt;
    [HideInInspector] public GameObject task;
    [HideInInspector] public bool atackTask = false;

    TextMeshProUGUI daysTxt;
    TextMeshProUGUI healthTxt;
    RaycastHit hit;
    GameObject thingyMcThing;
    GameObject aimThing;
    GameObject AI;
    GameObject placeingObject;
    GameObject dayNight;
    bool mainMenuMode = true;

    void Start()
    {
        if (SceneManager.GetActiveScene().name != "MainMenu")
        {
            daysTxt = GameObject.Find("DaysTxt").GetComponent<TextMeshProUGUI>();
            healthTxt = GameObject.Find("HealthTxt").GetComponent<TextMeshProUGUI>();
            dayNight = GameObject.Find("DayNightCycleObject");
            AI = GameObject.Find("P_HumanAI");
            Cursor.lockState = CursorLockMode.Confined;
        }
        else Light.DontDestroyOnLoad(GameObject.Find("Directional Light"));
    }

    void Update()
    {
        if (!mainMenuMode)
        {
            //Test for gun implementation
            if (Input.GetButton("Fire2"))
            {
                Cursor.visible = false;
                aimThing.SetActive(true);
                aimThing.transform.GetComponent<RectTransform>().position = Input.mousePosition;
            }
            else if (Input.GetButtonUp("Fire2"))
            {
                Cursor.visible = true;
                aimThing.SetActive(false);
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
                        thingyMcThing.transform.GetComponent<MeshRenderer>().enabled = true;
                        thingyMcThing.transform.position = hit.point;
                    }
                    else
                    {
                        task = hit.transform.gameObject;
                        thingyMcThing.transform.GetComponent<MeshRenderer>().enabled = false;
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
                thingyMcThing.GetComponent<MeshRenderer>().enabled = false;
                task = null;
            }

            Interface();
        }
    }

    public void Interface()
    {
        daysTxt.text = "Days: " + dayNight.GetComponent<SC_DayNight>().days;
        healthTxt.text = "Health: " + AI.GetComponent<SC_AI>().health;
    }

    // Button methods
    public void QuitGame()
    {
        if (Debug.isDebugBuild)
        {
            UnityEditor.EditorApplication.isPlaying = false;
        }
        else
        {
            Application.Quit();
        }
    }

    public void PlayGame()
    {
        mainMenuMode = false;
        SceneManager.LoadScene("SampleScene", LoadSceneMode.Single);
    }
}
