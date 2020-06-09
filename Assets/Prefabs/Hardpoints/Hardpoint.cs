using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hardpoint : MonoBehaviour
{
    [Header("General")]
    public int RotationSpeed;
    
    [Header("Ammunition")]
    public GameObject ammunition;

    private Camera skyCam;
    private GameController controller;

    void Start() {
        skyCam = GameObject.Find("SkyCamera").GetComponent<Camera>();
        controller = GameObject.Find("MainController").GetComponent<GameController>();
    }

    void Update()
    {
        if(!controller.IsBombingMode()) {
            DrawDirectionLine();
            RotateTowardsMouse();

            if(Input.GetMouseButton(0)) {
                FireBarrels();
            }   
        }
    }

    private void RotateTowardsMouse() {
        Vector3 mousePos = skyCam.ScreenToWorldPoint(Input.mousePosition);
        float angle = Mathf.Atan2(mousePos.y - transform.position.y, mousePos.x - transform.position.x) * Mathf.Rad2Deg;
        Quaternion quat = Quaternion.AngleAxis(angle - 90f, transform.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, quat, Time.deltaTime * RotationSpeed);
    }

    private void DrawDirectionLine() {
        Vector3 mousePos = skyCam.ScreenToWorldPoint(Input.mousePosition);
        Debug.DrawLine(transform.position, mousePos, Color.red);
    }

    private void FireBarrels() {
        foreach(Transform child in transform) {
            if(child.gameObject.name == "Barrel") {
                Instantiate(ammunition, child.position, child.rotation);
            }
        }
    }
}
