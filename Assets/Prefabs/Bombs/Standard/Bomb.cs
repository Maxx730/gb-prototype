using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    public float fallTransition = 0.5f;

    private void Update()
    {
        if(transform.localScale.x > 0.25f)
        {
            ShrinkBomb();
            MoveBomb();
        } else
        {
            Destroy(transform.gameObject);
        }
    }

    private void ShrinkBomb()
    {
        transform.localScale = new Vector3(transform.localScale.x - 0.005f, transform.localScale.y - 0.005f, 1f);
    }

    private void MoveBomb()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y - (Time.deltaTime * fallTransition), transform.position.z);
    }
}
