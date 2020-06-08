using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Title : MonoBehaviour
{
    [Header("General")]
    public float SlideAmount = 200;
    public float TopOffset = 1;

    private void Update()
    {
        transform.position = new Vector3(transform.position.x, (SlideAmount * Mathf.Sin(Time.time)) + (Screen.height - TopOffset), transform.position.z);
    }
}
