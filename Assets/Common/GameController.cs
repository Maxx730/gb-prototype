using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    //List of possible cursor choices
    [Header("Input Values")]
    public List<GameObject> cursors;
    public float CursorScaleSpeed = 1;

    [Header("Weapon Values")]
    public GameObject Weapon;

    private GameObject cursor;

    private void Start()
    {
        //Hide the system mouse cursor;
        Cursor.visible = false;
        cursor = Instantiate(cursors[0], new Vector3(0, 0, 0), Quaternion.identity);
    }

    private void Update()
    {

        //When the user clicks
        if(Input.GetMouseButtonDown(0)) {
            StartCoroutine("ScaleCursorDown");
            SpawnWeapon();
        }

        //Check if the cursor has been scaled down or not.
        if (cursor.transform.localScale.x < 1)
        {
            ScaleCursorUp();
        }

        //When the users moves mouse OR touch, move the cursor exactly there.
        SetCursorPosition();

    }

    private void SetCursorPosition()
    {
        Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        cursor.transform.position = new Vector3(pos.x,pos.y,0);
    }

    //Scale the Redicle size down and back up to communicate to the user that something is happening
    private void ScaleCursorDown()
    {
        cursor.transform.localScale = new Vector3(0.5f, 0.5f, 1f);
    }

    private void ScaleCursorUp()
    {
        cursor.transform.localScale = new Vector3(cursor.transform.localScale.x + 0.05f, cursor.transform.localScale.y + 0.05f, 1f);
    }

    private void SpawnWeapon()
    {
        Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Instantiate(Weapon, new Vector3(pos.x, pos.y, 0), Quaternion.identity);
    }
}
