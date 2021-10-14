using System;
using UnityEngine;

public class BulletMovement : MonoBehaviour
{
    public float speed;

    public float lifeTime;
    private float startLifeTime;

    Vector3 target;

    void Start()
    {
        startLifeTime = lifeTime;
        target = transform.position + new Vector3(Mathf.Cos(transform.eulerAngles.z * Mathf.Deg2Rad) , Mathf.Sin(transform.eulerAngles.z * Mathf.Deg2Rad)) * 10f;
        GetComponent<BoxCollider2D>().enabled = false;
    }


    void Update()
    {

        transform.position = Vector2.MoveTowards(transform.position, target, speed * Time.deltaTime);

        if(startLifeTime <= 0)
        {
            DestroyProjectile();
        }
        else if(startLifeTime <= lifeTime - 0.1f && startLifeTime >0)
        {
            GetComponent<BoxCollider2D>().enabled = true;
            startLifeTime -= Time.deltaTime;
        }
        else
        {
            startLifeTime -= Time.deltaTime;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        DestroyProjectile();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        DestroyProjectile();
    }


    void DestroyProjectile()
    {
        Destroy(gameObject);
    }
}
