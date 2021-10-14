using UnityEngine;
using UnityEngine.UIElements;

public class BoxSpawner : MonoBehaviour
{
    public Transform middleLocation;
    public float radius;
    public LayerMask whatIsCollider;
    public LayerMask whatIsBox;

    public float timeBtwSpawnBox;
    private float startTimeBtwSpawnBox;

    public GameObject box;

    void Start()
    {
        middleLocation = GetComponent<Transform>();
        startTimeBtwSpawnBox = timeBtwSpawnBox;
    }

    void FixedUpdate()
    {
        Collider2D[] countBox = Physics2D.OverlapCircleAll(middleLocation.position, radius, whatIsBox);
        if(countBox.Length > 10)
            startTimeBtwSpawnBox = timeBtwSpawnBox;

        if (startTimeBtwSpawnBox <= 0)
        {
            //Range 0.66 buat cek apakah ada object apa nga
            float x, y;
            Vector2 spawnBoxLocation;
            for (int i = 0; i < 10; i++)
            {
                x = Random.Range(-radius ,radius);
                y = Random.Range(-radius, radius);
                spawnBoxLocation = new Vector2(x, y);
                if (Vector2.Distance(transform.position, spawnBoxLocation) >= radius)
                    continue;
                Collider2D[] info = Physics2D.OverlapCircleAll(spawnBoxLocation, 0.66f, whatIsCollider);
                if (info.Length > 0)
                    continue;
                Instantiate(box, spawnBoxLocation, Quaternion.identity);
                break;
            }
            startTimeBtwSpawnBox = timeBtwSpawnBox;
        }
        else
        {
            startTimeBtwSpawnBox -= Time.fixedDeltaTime;
        }

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(middleLocation.position, radius);
    }
}
