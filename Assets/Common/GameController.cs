using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    //List of possible cursor choices
    [Header("Game Variables")]
    public int waveAmount = 5;

    [Header("Input Values")]
    public List<GameObject> cursors;
    public float CursorScaleSpeed = 1;
    public float ShakeAmount = 30;
    public float ShakeTimeout = 0.5f;
    public float PanAmount = 1;

    [Header("Weapon Values")]
    public GameObject Weapon;
    public float ReloadTimer;

    [Header("Tile Sections")]
    public List<GameObject> Sections;
    public float SectionSpeed = 0.05f;

    [Header("UI Elements")]
    public Slider progressIndicator;
    public Button switchCameras;

    private GameObject cursor;
    private List<GameObject> activeSections = new List<GameObject>();
    private Vector2 screenBounds;
    private float lastShake;
    private int CurrentWave = 1;
    private float levelProgress = 0;
    private float levelDistance;
    private float lastShot;

    private void Start()
    {
        //Hide the system mouse cursor;
        Cursor.visible = false;
        cursor = Instantiate(cursors[0], new Vector3(0, 0, 0), Quaternion.identity);

        //Get Screen bounds
        screenBounds = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));

        //Spawn the first section at zero zero
        GameObject initialSection = Instantiate(Sections[0], new Vector3(0, 0, 0), Quaternion.identity);
        activeSections.Add(initialSection);

        //Spawn the second section behind.
        GameObject secondSection = Instantiate(Sections[0], new Vector3(0, (initialSection.GetComponent<Collider2D>().bounds.size.y), 0), Quaternion.identity);
        activeSections.Add(secondSection);

        levelDistance = initialSection.GetComponent<Collider2D>().bounds.size.y * (CurrentWave * waveAmount);

        lastShot = Time.time;
    }

    private void Update()
    {

        //When the user clicks
        if(Input.GetMouseButtonDown(0)) {
            StartCoroutine("ScaleCursorDown");

            if(IsBombingMode())
            {
                if(Time.time - lastShot > ReloadTimer)
                {
                    SpawnWeapon();
                    lastShot = Time.time;
                }
            }
        }

        //Check if the cursor has been scaled down or not.
        if (cursor.transform.localScale.x < 2 && !Input.GetMouseButton(0))
        {
            ScaleCursorUp();
        }

        //When the users moves mouse OR touch, move the cursor exactly there.
        if(waveAmount > 0)
        {
            SetCursorPosition();
            MoveSectionsDown();
            DestroySections();
            SetProgressIndicator();
        }

/*        if(Time.time - lastShake < ShakeTimeout && lastShake != 0)
        {
            ShakeCamera();
        } else
        {
            PanCamera();
        }*/
    }

    private void SetCursorPosition()
    {
        GetCameraContext();
        Vector3 pos = transform.GetChild(0).gameObject.GetComponent<Camera>().ScreenToWorldPoint(Input.mousePosition);
        cursor.transform.position = new Vector3(pos.x,pos.y,0);
    }

    //Scale the Redicle size down and back up to communicate to the user that something is happening
    private void ScaleCursorDown()
    {
        cursor.transform.localScale = new Vector3(1f, 1f, 1f);
    }

    private void ScaleCursorUp()
    {
        cursor.transform.localScale = new Vector3(cursor.transform.localScale.x + 0.05f, cursor.transform.localScale.y + 0.05f, 1f);
    }

    private void SpawnWeapon()
    {
        Vector3 pos = GetCameraContext().ScreenToWorldPoint(Input.mousePosition);
        Instantiate(Weapon, new Vector3(pos.x, pos.y, 0), Quaternion.identity);
    }

    private void MoveSectionsDown()
    {
        foreach(GameObject section in activeSections)
        {
            Vector3 pos = section.transform.position;
            section.transform.position = new Vector3(pos.x, pos.y - (Time.deltaTime * SectionSpeed), pos.z);
        }

        levelProgress += Time.deltaTime * SectionSpeed;
    }

    private void DestroySections()
    {
        foreach(GameObject section in activeSections)
        {
            if (section.transform.position.y < -section.GetComponent<Collider2D>().bounds.size.y)
            {
                GameObject addedSection = Instantiate(Sections[0], new Vector3(0, section.GetComponent<Collider2D>().bounds.size.y, 0), Quaternion.identity);
                activeSections.Add(addedSection);
                activeSections.Remove(section);
                waveAmount--;
                Destroy(section);
            }
        }
    }

    private void PanCamera()
    {
        transform.position = new Vector3(Mathf.Sin(Time.time * PanAmount), transform.position.y, transform.position.z);
    }

    private void ShakeCamera()
    {
        transform.position = new Vector3(Mathf.Sin(Time.time * ShakeAmount) * 0.25f, transform.position.y, transform.position.z);
    }

    private void SetProgressIndicator()
    {
        progressIndicator.value = (levelProgress / levelDistance) * 100;
    }

    public void SetShaking(float last)
    {
        lastShake = last;
    }

    public void SwitchCameras()
    {
        bool swapValue = false;
        if(transform.GetChild(0).gameObject.GetComponent<Camera>().enabled == false)
        {
            swapValue = true;
        }
        transform.GetChild(0).gameObject.GetComponent<Camera>().enabled = swapValue;
        transform.GetChild(1).gameObject.GetComponent<Camera>().enabled = !swapValue;
    }

    private Camera GetCameraContext()
    {
        if (transform.GetChild(0).gameObject.GetComponent<Camera>().enabled == false)
        {
            return transform.GetChild(1).gameObject.GetComponent<Camera>();
        } else
        {
            return transform.GetChild(0).gameObject.GetComponent<Camera>();
        }
    }

    public bool IsBombingMode()
    {
        if (transform.GetChild(0).gameObject.GetComponent<Camera>().enabled == false)
        {
            return false;
        }
        else
        {
            return true;
        }
    }
}
