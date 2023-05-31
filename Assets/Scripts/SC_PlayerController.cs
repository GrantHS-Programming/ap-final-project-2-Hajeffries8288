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
    GameObject pewFlash;
    GameObject placeingObject;
    GameObject dayNight;
    GameObject controles;
    GameObject pauseMenu;
    int num;

    void Start()
    {
        if (GameObject.Find("P_HumanAI"))
        {
            daysTxt = GameObject.Find("DaysTxt").GetComponent<TextMeshProUGUI>();
            healthTxt = GameObject.Find("HealthTxt").GetComponent<TextMeshProUGUI>();
            thingyMcThing = GameObject.Find("LastClickedSpotThingy");
            dayNight = GameObject.Find("DayNightCycleObject");
            AI = GameObject.Find("P_HumanAI");
            aimThing = GameObject.Find("AimThingy");
            controles = GameObject.Find("GUIcontrolls/warning");
            pewFlash = GameObject.Find("PewFlash");
            pauseMenu = GameObject.Find("PauseMenu");
            Cursor.lockState = CursorLockMode.Confined;
        }
        else Light.DontDestroyOnLoad(GameObject.Find("Directional Light"));
    }

    void Update()
    {
        if (daysTxt)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Cursor.lockState = CursorLockMode.None;
                Time.timeScale = 0;

            }

            if (Input.GetKeyDown(KeyCode.Tab))
            {
                for (int i = 0; i < controles.transform.childCount; i++)
                {
                    controles.transform.GetChild(i).GetComponent<TextMeshProUGUI>().enabled = !controles.transform.GetChild(i).GetComponent<TextMeshProUGUI>().enabled;
                }
            }

            // This means flash speed is dependent on fraim rate
            num++;
            if (num == 15)
            {
                pewFlash.transform.GetComponentInChildren<Image>().enabled = false; // This Causes Flashing light
                num = 0;
            }

            GameObject AIHolding = AI.transform.GetComponent<SC_AI>().holding;
            if (AIHolding)
            {
                if (Input.GetButton("Fire2"))
                {
                    Cursor.visible = false;
                    for (int i = 0; i < aimThing.transform.childCount; i++)
                    {
                        aimThing.transform.GetChild(i).GetComponent<Image>().enabled = true;
                    }
                    aimThing.transform.GetComponent<RectTransform>().position = Input.mousePosition;
                }
                else if (Input.GetButtonUp("Fire2"))
                {
                    Cursor.visible = true;
                    for (int i = 0; i < aimThing.transform.childCount; i++)
                    {
                        aimThing.transform.GetChild(i).GetComponent<Image>().enabled = false;
                    }
                }
            }

            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity))
            {
                if (Input.GetButtonDown("Fire1"))
                {
                    if (!hit.transform.name.Contains("Zomble") && !placeingObject && !Input.GetButton("Fire2"))
                    {
                        setAIDestination = hit.point;
                        task = hit.transform.gameObject;
                        atackTask = false;
                        thingyMcThing.transform.GetComponent<MeshRenderer>().enabled = true;
                        thingyMcThing.transform.position = hit.point;
                    }
                    else if (!Input.GetButton("Fire2"))
                    {
                        thingyMcThing.transform.GetComponent<MeshRenderer>().enabled = false;
                        atackTask = true;
                        task = hit.transform.gameObject;
                    }
                    else
                    {
                        task = hit.transform.gameObject;
                        atackTask = false;
                        if (hit.transform.name.Contains("Zomble")) hit.transform.GetComponent<SC_ZombleAI>().health -= 100;
                        pewFlash.transform.GetComponentInChildren<Image>().enabled = true; // This causes flashing light
                    }
                }
                if (Input.GetKeyDown(KeyCode.Q))
                {
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
        if (dayNight)
        {
            daysTxt.text = "Days: " + dayNight.GetComponent<SC_DayNight>().days;
            healthTxt.text = "Health: " + AI.GetComponent<SC_AI>().health;
        }
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
        SceneManager.LoadScene("SampleScene", LoadSceneMode.Single);
    }
}
