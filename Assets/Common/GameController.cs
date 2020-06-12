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
    public List<GameObject> GrassSections;
    public List<GameObject> SandSections;
    public List<GameObject> WaterSections;
    public float SectionSpeed = 0.05f;

    [Header("UI Elements")]
    public Slider progressIndicator;
    public Button switchCameras;
    public GameObject WaveMenuCanvas;

    [Header("Wave Interval Sections")]
    public GameObject Beginning;
    public GameObject Middle;
    public GameObject End;

    private static bool WaveMenu = false;
    private GameObject cursor;
    private List<GameObject> activeSections = new List<GameObject>();
    private Vector2 screenBounds;
    private float lastShake;
    private int CurrentWave = 1;
    private float levelProgress = 0;
    private float levelDistance;
    private float lastShot;

    //Score keeping variables.
    private static int TargetsDestroyed = 0, FightersDestroyed = 0;
    private static Text TargetsWaveFinal, FightersWaveFinal;

    private void Start()
    {

        //Hide the system mouse cursor;
        Cursor.visible = false;
        cursor = Instantiate(cursors[0], new Vector3(0, 0, 0), Quaternion.identity);

        //Get Screen bounds
        screenBounds = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));

        //Spawn the first section at zero zero
        GameObject initialSection = Instantiate(End, new Vector3(0, 0, 0), Quaternion.identity);
        activeSections.Add(initialSection);

        //Spawn the second section behind.
        GameObject secondSection = Instantiate(Sections[1], new Vector3(0, (initialSection.GetComponent<Collider2D>().bounds.size.y), 0), Quaternion.identity);
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

        if(WaveMenu) {
            
        }

        //When the users moves mouse OR touch, move the cursor exactly there.
        SetCursorPosition();
        MoveSectionsDown();
        SetProgressIndicator();
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
            if(CheckSectionPosition(section)){
                Vector3 pos = section.transform.position;
                section.transform.position = new Vector3(pos.x, pos.y - (Time.deltaTime * SectionSpeed), pos.z);
            } else {
                //Destroy the section if it is not passing positoin check;
                DestroySection(section);
            }

            if(section.gameObject.name == "EndlessBeginning(Clone)" && section.gameObject.transform.position.y < 0) {
                WaveMenuCanvas.SetActive(true);
                UpdateFinalsCounters();
            }
        }

        levelProgress += Time.deltaTime * SectionSpeed;
    }

    private bool CheckSectionPosition(GameObject section) {
        if(section.transform.position.y < -(section.GetComponent<Collider2D>().bounds.size.y)) {
            return false;
        }
        return true;
    }

    private void DestroySection(GameObject section)
    {
        if(waveAmount > 0) {
            activeSections.Add(Instantiate(Sections[0], new Vector3(0, section.GetComponent<Collider2D>().bounds.size.y, 0), Quaternion.identity));
            waveAmount--;
        } else {
            //IF we have hit the end of the wave amount, then we want to start making the endless background for the 
            //menu.
            if(!WaveMenu) {
                activeSections.Add(Instantiate(Beginning, new Vector3(0, section.GetComponent<Collider2D>().bounds.size.y, 0), Quaternion.identity));  
                WaveMenu = true;     
            } else {
                activeSections.Add(Instantiate(Middle, new Vector3(0, section.GetComponent<Collider2D>().bounds.size.y, 0), Quaternion.identity));
            }
        }

        activeSections.Remove(section);
        Destroy(section);
    }

    private void AddSection(GameObject section){
        GameObject addedSection = Instantiate(section, new Vector3(0, section.GetComponent<Collider2D>().bounds.size.y, 0), Quaternion.identity);
        activeSections.Add(addedSection);
    }

    private void PanCamera() => transform.position = new Vector3(Mathf.Sin(Time.time * PanAmount), transform.position.y, transform.position.z);

    private void ShakeCamera() => transform.position = new Vector3(Mathf.Sin(Time.time * ShakeAmount) * 0.25f, transform.position.y, transform.position.z);

    private void SetProgressIndicator() => progressIndicator.value = (levelProgress / levelDistance) * 100;

    public void SetShaking(float last) => lastShake = last;

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
        if (transform.GetChild(0).gameObject.GetComponent<Camera>().enabled == false || WaveMenuCanvas.active)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    public static void AddTargetDestroyed() => TargetsDestroyed++;
    public static void AddFighterDestroyed() => FightersDestroyed++;
    public static void UpdateFinalsCounters() {
        //Get UI Elements
        TargetsWaveFinal = GameObject.Find("TargetsFinal").GetComponent<Text>();
        FightersWaveFinal = GameObject.Find("FightersFinal").GetComponent<Text>();
        TargetsWaveFinal.text = TargetsDestroyed + " Targets Destroyed";
        FightersWaveFinal.text = FightersDestroyed + " Fighters Destroyed";
    }
}
