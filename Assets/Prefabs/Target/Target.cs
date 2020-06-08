using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    [Header("General")]
    public float HitPoints = 35;
    public float targetPoints = 10;

    private float damageTimeout = 0.25f;
    private float damageEventTime;
    public void DamageTarget(int value)
    {
        if(HitPoints - value > 0)
        {
            HitPoints -= value;
            damageEventTime = Time.time;
        } else
        {
            Debug.Log("TARGET DESTROYED");
            GetComponent<Animator>().SetBool("isDestroyed", true);
        }
    }
    private void Update()
    {
        if (Time.time - damageEventTime > damageTimeout) 
        {
            GetComponent<SpriteRenderer>().color = Color.white;
        }
    }
}
