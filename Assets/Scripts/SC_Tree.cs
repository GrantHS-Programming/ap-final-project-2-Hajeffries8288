using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SC_Tree : MonoBehaviour
{
    [HideInInspector] public bool despawn = false;

    public float despawnTime = 5;

    float time = 0;

    void Update()
    {
        if (despawn)
        {
            if (Time.time >= time)
            {
                Destroy(transform.gameObject);
            }
        }
        else
        {
            time = Time.time + despawnTime;
        }
    }
}
