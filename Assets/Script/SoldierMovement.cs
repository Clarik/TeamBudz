using Unity.Mathematics;
using UnityEngine;

public class SoldierMovement : MonoBehaviour
{

    private bool stop = false;
    public Transform radar;
    private Transform target;
    // Set for shoot
    public Transform targetBase;
    public float speed;
    public float range;
    public GameObject projectile;
    public float timeBtwShoot;
    private float startTimeBtwShoot;
    void Start()
    {
    }

    void Update()
    {
        Collider2D[] checker = Physics2D.OverlapCircleAll(radar.position, range);
        for (int i = 0; i < checker.Length; i++)
        {
            if(checker[i].tag == "Enemy")
            {
                stop = true;
                if (startTimeBtwShoot <= 0)
                {
                    //Spawn bullet
                    shoot(checker[i].transform);
                }
                break;
            }
        }

        if(Vector2.Distance(transform.position, targetBase.position) > range && !stop)
        {
            Vector2 nextPos = new Vector2(targetBase.position.x, transform.position.y);
            
            transform.position = Vector2.MoveTowards(transform.position, nextPos, speed * Time.deltaTime);
        }
        else if(Vector2.Distance(transform.position, targetBase.position) <= range && !stop)
        {
            transform.position = this.transform.position;
            if(startTimeBtwShoot <= 0)
            {
                //Spawn bullet
                shoot(targetBase);
            }
        }

        if(startTimeBtwShoot> 0)
        {
            startTimeBtwShoot -= Time.deltaTime;
        }

        stop = false;
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
