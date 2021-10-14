using Pathfinding;
using UnityEngine;

public class AliesAI : MonoBehaviour
{
    public Transform idk;
    Transform target;

    public Transform placement;

    public float speed = 10f;
    public float nextWaypointDistance = 0.1f;

    Path path;
    int currentWaypoint = 0;
    //bool reachedEndOfPath = false;

    Seeker seeker;
    Rigidbody2D rb;

    int direct;
    bool facingRight;
    bool facingFront;

    public LayerMask whatIsBox;
    public float range;

    public LayerMask whatIsEmpty;

    public float timeBtwMove;
    private float startTimeBtwMove;

    void Start()
    {
        searchNearestBox();
        direct = 2;
        facingFront = facingRight = true;

        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();

        InvokeRepeating("UpdatePath", 0f, 1f);

        startTimeBtwMove = timeBtwMove;
    }
    
    void searchNearestBox()
    {
        Collider2D[] bringItem = Physics2D.OverlapCircleAll(transform.position, range, whatIsBox);

        Transform nearbox = null;
        float closest = 0f;
        int loc = 0;
        for (int i = 0; i < bringItem.Length; i++)
        {
            if (bringItem[i].tag == "Box" && !bringItem[i].GetComponent<BoxInfo>().isOnDetect() && bringItem[i].GetComponent<BoxInfo>().isGrounded() && !GetComponent<AliesInteraction>().isBringing())
            {
                if (nearbox == null)
                {
                    nearbox = bringItem[i].transform;
                    closest = Vector2.Distance(transform.position, bringItem[i].transform.position);
                    loc = i;
                }
                else if (closest > Vector2.Distance(transform.position, bringItem[i].transform.position))
                {
                    nearbox = bringItem[i].transform;
                    closest = Vector2.Distance(transform.position, bringItem[i].transform.position);
                    loc = i;
                }
            }
            
        }
        if (nearbox != null)
        {
            target = nearbox;
        }
        else
        {
            float x;
            float y;
            Vector2 nextDest;
            do
            {
                x = Random.Range(-20, 20);
                y = Random.Range(-20, 20);
                nextDest = new Vector2(x, y);
                Collider2D[] z = Physics2D.OverlapCircleAll(nextDest, 3f, whatIsEmpty);
                if (z.Length <= 0)
                    break;
            } while (true);
            idk.position = nextDest;
            target = idk;
        }
    }

    void Update()
    {
        if (startTimeBtwMove <= 0 && !GetComponent<AliesInteraction>().isBringing())
        {
            searchNearestBox();
            startTimeBtwMove = timeBtwMove;
        }
        else if (GetComponent<AliesInteraction>().isBringing())
        {
            Transform nextTarget = placement;
            target = nextTarget;
        }
        else
        {
            startTimeBtwMove -= Time.deltaTime;
        }

        
    }
    void FixedUpdate()
    {

        if (path == null)
            return;

        if (currentWaypoint >= path.vectorPath.Count)
            return;

        Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint]);

        rb.position = Vector2.MoveTowards(rb.position, direction, speed * Time.fixedDeltaTime);

        Vector2 move = direction - rb.position;
        float verti = move.y;
        float horiz = move.x;

        if ((horiz < 0 && facingRight) || (horiz > 0 && !facingRight))
            FlipHoriz();
        if ((verti < 0 && !facingFront) || (verti > 0 && facingFront))
            FlipVerti();

        if (verti > 0.5f)
            direct = 1;
        else if (verti < -0.5f)
            direct = 3;
        else if (horiz > 0.5f)
            direct = 2;
        else if (horiz < -0.5f)
            direct = 4;

        if (direct == 1 || direct == 3)
            if (horiz > 0.5f)
                direct = 2;
            else if (horiz < -0.5f)
                direct = 4;
        if (direct == 2 || direct == 4)
            if (verti > 0.5f)
                direct = 1;
            else if (verti < -0.5f)
                direct = 3;

        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);

        if (distance < nextWaypointDistance)
        {
            currentWaypoint++;
        }
    }

    void FlipHoriz()
    {
        transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        facingRight = !facingRight;
    }

    void FlipVerti()
    {
        transform.localScale = new Vector3(transform.localScale.x, -transform.localScale.y, transform.localScale.z);
        facingFront = !facingFront;
    }

    void OnPathComplete(Path P)
    {
        if (!P.error)
        {
            path = P;
            currentWaypoint = 0;
            if (AstarPath.active.isScanning == false)
                AstarPath.active.Scan();
        }
    }

    void UpdatePath()
    {
        if (seeker.IsDone() && target != null)
            seeker.StartPath(rb.position, target.position, OnPathComplete);
    }

    public int whereDirection()
    {
        return direct;
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
