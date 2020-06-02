using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudSpawner : MonoBehaviour
{
    [Header("Ganeral")]
    public float spawnRate = 0.5f;

    [Header("Clouds")]
    public List<GameObject> Clouds;

    private float lastTime;
    private Vector2 screenBounds;

    private void Start()
    {
        //Grab the size of our screen
        screenBounds = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
    }
    private void Update()
    {
       if(Time.time - lastTime > spawnRate)
        {
            SpawnCloud();
            lastTime = Time.time;
        }
    }

    private void SpawnCloud()
    {
        Instantiate(Clouds[0], new Vector3(Random.Range((-screenBounds.x - 1), (screenBounds.x + 1)), transform.position.y, 0), Quaternion.identity);
    }
}
