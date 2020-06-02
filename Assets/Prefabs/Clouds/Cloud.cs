using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cloud : MonoBehaviour
{
    public float ySpeed = 0, xSpeed = 0;

    private Vector2 screenBounds;

    private void Start()
    {
        screenBounds = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
    }
    void Update()
    {
        transform.position = new Vector3(transform.position.x + (Time.deltaTime * xSpeed), transform.position.y - (Time.deltaTime * ySpeed), transform.position.z);

        if(transform.position.y < (-screenBounds.y - 2))
        {
            Destroy(transform.gameObject);
        }
    }
}
