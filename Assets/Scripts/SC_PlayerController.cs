using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SC_PlayerController : MonoBehaviour
{
    [HideInInspector] public Vector3 setAIDestination;
    [HideInInspector] public GameObject task;
    [HideInInspector] public int wood = 0;
    [HideInInspector] public bool atackTask = false;

    public float speed = 5;
    public float shiftSpeed = 10;
    [Header("Place objects")]
    public Material placeingMat;
    //public GameObject superCoolWood;
    //public GameObject wall;

    Material[] materials;
    TextMeshProUGUI woodTxt;
    RaycastHit hit;
    GameObject placeingObject;

    void Start()
    {
        woodTxt = GameObject.Find("WoodTxt").GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity))
        {
            if (Input.GetButtonDown("Fire1"))
            {
                print("Player clicked object: " + hit.transform.name);
                if (!hit.transform.name.Contains("Zomble") && !placeingObject)
                {
                    setAIDestination = new Vector3(hit.point.x, 0, hit.point.z);
                    task = hit.transform.gameObject;
                    atackTask = false;
                }
                else
                {
                    task = hit.transform.gameObject;
                    atackTask = true;
                }
            }

        }
        else if (Input.GetButtonDown("Fire1"))
        {
            print("Player clicked nothing");
            setAIDestination = new Vector3(999999999999, 0, 999999999999);
            task = null;
        }

        Movement();

        PlaceingPrefabs();

        Misc();

        Interface();
    }

    public void Movement()
    {
        // Shift Check
        bool isHoldingShift = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);

        // WASD
        Vector3 input = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        Vector3 direction = input.normalized;
        Vector3 velocity = isHoldingShift ? direction * shiftSpeed : direction * speed;

        // Actual Cam Movement
        transform.Translate(velocity * Time.deltaTime);
    }

    public void PlaceingPrefabs()
    {
        if (placeingObject != null)
        {
            placeingObject.transform.position = hit.point;
            if (Input.GetButton("Fire1"))
            {
                placeingObject.layer = 0;
                wood -= placeingObject.GetComponent<SC_PlacableObject>().woodPrice;
                for (int i = 0; i < placeingObject.transform.childCount; i++)
                {
                    placeingObject.transform.GetChild(i).GetComponent<MeshRenderer>().material = materials[i];
                }
                placeingObject = null;
            }
        }
    }

    public void Misc()
    {
        if (Input.GetKeyDown(KeyCode.PageUp)) wood++;
    }

    public void Interface()
    {
        woodTxt.text = "Wood: " + wood;
    }

    // Buttons
    public void PlaceObject(GameObject objectToPlace)
    {
        placeingObject = Instantiate(objectToPlace);
        if (wood >= placeingObject.GetComponent<SC_PlacableObject>().woodPrice)
        {
            placeingObject.layer = 2;
            materials = new Material[placeingObject.transform.childCount];
            for (int i = 0; i < placeingObject.transform.childCount; i++)
            {
                materials[i] = placeingObject.transform.GetChild(i).GetComponent<MeshRenderer>().material;
                placeingObject.transform.GetChild(i).GetComponent<MeshRenderer>().material = placeingMat;
            }
        }
    }
}
