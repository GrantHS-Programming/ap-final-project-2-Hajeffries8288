using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SC_DayNight : MonoBehaviour
{
    [HideInInspector] public int days;

    public GameObject directionalLight;
    public float secondsInDay;
    public int sunStages;

    float secondsTellNextLightRotation;
    float angleSunWillRotate;
    float time;

    void Start()
    {
        secondsTellNextLightRotation = secondsInDay / sunStages;
        angleSunWillRotate = 360 / sunStages;
    }

    void Update()
    {
        if (Time.time >= time)
        {
            float rotationOfNextSunStage = directionalLight.transform.rotation.x + angleSunWillRotate;
            time += secondsTellNextLightRotation;
            directionalLight.transform.rotation *= Quaternion.Euler(rotationOfNextSunStage, 0, 0);
            RaycastHit hit;
            if (Physics.Raycast(directionalLight.transform.position, directionalLight.transform.forward * 100, out hit))
            {
                if (hit.transform.name == "DayObject") days++;
            }
        }
    }
}
