using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("General")]
    public int bulletSpeed;

    void Start()
    {
        
    }
    void Update()
    {
        transform.position += transform.up * Time.deltaTime * bulletSpeed;
    }

    void OnBecameInvisible()
    {
        Destroy(transform.gameObject);
    }
}
