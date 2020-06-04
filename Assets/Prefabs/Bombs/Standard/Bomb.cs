using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    [Header("General")]
    public float fallTransition = 0.5f;
    public float shrinkFactor = 0.0015f;
    public float impactRadius = 1;
    public int Damage = 15;
    public GameObject explosionPrefab;

    private GameController gameController;

    private void Start()
    {
        gameController = GameObject.Find("GameController").GetComponent<GameController>();
    }

    private void Update()
    {
        if(transform.localScale.x > 0.25f)
        {
            ShrinkBomb();
            MoveBomb();
        } else
        {
            GameObject expPref = Instantiate(explosionPrefab, new Vector3(transform.position.x, transform.position.y, -0.01f), Quaternion.identity);
            expPref.GetComponent<Explosion>().SetBlastRadius(impactRadius);
            expPref.GetComponent<Explosion>().SetBlastDamage(Damage);
            gameController.SetShaking(Time.time);
            Destroy(transform.gameObject);
        }
    }

    private void ShrinkBomb()
    {
        transform.localScale = new Vector3(transform.localScale.x - shrinkFactor, transform.localScale.y - shrinkFactor, 1f);
    }

    private void MoveBomb()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y - (Time.deltaTime * fallTransition), transform.position.z);
    }
}
