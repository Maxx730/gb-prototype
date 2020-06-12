using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    [Header("General")]
    public float panSpeed;
    public int craterDisplay = 0;

    private float blastRad = 1;
    private int blastDamage;
    public void SetBlastRadius(float radius) => blastRad = radius;
    public void SetBlastDamage(int damage) => blastDamage = damage;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, blastRad);
    }

    private void Start()
    {
        ParticleSystem particles = transform.GetChild(0).GetComponent<ParticleSystem>();
        float dur = particles.main.duration;
        Destroy(particles, dur);
        FindTargets();
        StartCoroutine(DisplayCrater());
    }

    private void Update() => transform.position = new Vector3(transform.position.x, transform.position.y - (Time.deltaTime * panSpeed), transform.position.z);

    private void FindTargets()
    {
        Collider2D[] objs = Physics2D.OverlapCircleAll(transform.position, blastRad);

        foreach(Collider2D obj in objs)
        {
            if(obj.gameObject.tag == "Target")
            {
                if(!obj.gameObject.GetComponent<Target>().IsDead()) {
                    obj.gameObject.GetComponent<Target>().DamageTarget(blastDamage);
                } else {
                    obj.gameObject.GetComponent<Target>().AnimateDestruction();
                }
            }
        }
    }

    private IEnumerator DisplayCrater()
    {
        yield return new WaitForSeconds(craterDisplay);
        transform.GetChild(1).gameObject.SetActive(true);
    }

    private void OnBecameInvisible() => Destroy(transform.gameObject);
}
