using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SC_DayNight : MonoBehaviour
{
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
            time += secondsTellNextLightRotation;
            directionalLight.transform.rotation *= Quaternion.Euler(angleSunWillRotate, directionalLight.transform.rotation.y, directionalLight.transform.rotation.z);
        }
    }
}
