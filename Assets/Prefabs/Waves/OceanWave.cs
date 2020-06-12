using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OceanWave : MonoBehaviour
{
    [Header("General")]
    public float Speed;

    void Update()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y - Time.deltaTime * Speed, transform.position.z);    
    }

    void OnBecameInvisible()
    {
        Destroy(transform.gameObject);
    }
}
