﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    private void Update()
    {
        if(transform.localScale.x > 0.25f)
        {
            ShrinkBomb();
        } else
        {
            Destroy(transform.gameObject);
        }
    }

    private void ShrinkBomb()
    {
        transform.localScale = new Vector3(transform.localScale.x - 0.005f, transform.localScale.y - 0.005f, 1f);
    }
}
