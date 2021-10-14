using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class AliesDefender : MonoBehaviour
{
    private bool stop = false;
    public Transform radar;
    private Transform target;
    // Set for shoot
    public float speed;
    public float range;
    public GameObject projectile;
    public float timeBtwShoot;
    private float startTimeBtwShoot;

    private float xPos;
    private float yPos;
    void Start()
    {
        xPos = transform.position.x;
        yPos = transform.position.y;
    }

    private void Update()
    {
        Collider2D[] checker = Physics2D.OverlapCircleAll(radar.position, range + 2f);
        for (int i = 0; i < checker.Length; i++)
        {
            if (checker[i].tag == "Enemy")
            {
                stop = true;
                target = checker[i].GetComponent<Transform>();
                break;
            }
        }

        if (stop == true)
        {
            if (Vector2.Distance(transform.position, target.position) > range + 2f)
            {
                stop = false;
            }
            if (Vector2.Distance(transform.position, target.position) > range)
            {
                Vector2 nextPos = new Vector2(target.position.x, transform.position.y);
                transform.position = Vector2.MoveTowards(transform.position, nextPos, speed * Time.deltaTime);
            }
            else
            {
                if (startTimeBtwShoot <= 0)
                {
                    //Spawn bullet
                    shoot(target);
                }
            }
        }
        else
        {
            transform.position = Vector2.MoveTowards(transform.position, new Vector2(xPos, yPos), speed * Time.deltaTime);
        }

        if (startTimeBtwShoot > 0)
        {
            startTimeBtwShoot -= Time.deltaTime;
        }
        stop = false;
    }

    private void FixedUpdate()
    {
        
    }

    void shoot(Transform target)
    {
        Vector3 locShoot = new Vector3(transform.position.x + 0.5f, transform.position.y);

        Vector2 direction = target.position - locShoot;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        Instantiate(projectile, locShoot, rotation);
        startTimeBtwShoot = timeBtwShoot;
    }
}
