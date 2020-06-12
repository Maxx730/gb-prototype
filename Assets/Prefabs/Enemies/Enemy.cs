using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{   
    [Header("General")]
    public int EnemyHealth = 100;
    public int EnemySpeed;
    public float PanAmount;

    private bool isPanning = true;
    private bool movingUp = true;

    private Camera skyCam;
    private Vector2 ScreenBounds;
    private float lastDamage;

    void OnTriggerEnter2D(Collider2D other)
    {
        
        if(other.gameObject.tag == "Bullet") {
            Destroy(other.gameObject);
            EnemyHealth -= 1;
            lastDamage = Time.time;
        }
    }
    void Start()
    {
        skyCam = GameObject.Find("SkyCamera").GetComponent<Camera>();
        ScreenBounds = skyCam.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
    }
    void Update()
    {
        if(isPanning) {
            PanEnemy();
        }

        MoveEnemy();
        DrawDirectionLine();

        if(transform.position.y > ScreenBounds.y) {
            transform.rotation = Quaternion.Euler(0,0,180);
            movingUp = false;
        }

        if(transform.position.y < -ScreenBounds.y) {
            transform.rotation = Quaternion.Euler(0,0,0);
            movingUp = true;
        }

        FlashDamage();
        if(EnemyHealth < 0) {
            Destroy(transform.gameObject);
        }
    }

    void FlashDamage() {
        if(Time.time - lastDamage > 0.05) {
            GetComponent<SpriteRenderer>().color = Color.white;
        } else {
            GetComponent<SpriteRenderer>().color = Color.red;
        }
    }

    void MoveEnemy() {
        if(movingUp) {
            transform.position += transform.up * Time.deltaTime * EnemySpeed;
        } else {
            transform.position -= transform.up * Time.deltaTime * EnemySpeed;
        }
    }

    void DrawDirectionLine() {
        float upAmount = 0;

        if(movingUp) {
            upAmount = 1f;
        } else {
            upAmount = -1f;
        }
        Debug.DrawLine(transform.position, transform.forward * 20f, Color.red);
    }

    void PanEnemy() => transform.rotation = Quaternion.Euler(0,0,Mathf.Sin(Time.time) * PanAmount);
}
