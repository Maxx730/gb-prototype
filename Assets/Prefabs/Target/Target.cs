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
    private bool HasDied = false;
    private SpriteRenderer renderer;

    void Start()
    {
        renderer = GetComponent<SpriteRenderer>();
    }
    public void DamageTarget(int value)
    {
        renderer.color = Color.red;
        HitPoints -= value;
        damageEventTime = Time.time;

        if(HitPoints == 0) {
            AnimateDestruction();
        }
    }

    public bool IsDead() {
        if(HitPoints >= 0) {
            return false;
        } else {
            return true;
        }
    }

    public void AnimateDestruction() {
        if(!HasDied) {
            HasDied = true;
            GameController.AddTargetDestroyed();
            GetComponent<Animator>().SetBool("isDestroyed", true);
        }
    }
    private void Update()
    {
        if (Time.time - damageEventTime > damageTimeout) 
        {
            renderer.color = Color.white;
        }
    }

    private void OnBecameInvisible() => Destroy(transform.gameObject);
}
