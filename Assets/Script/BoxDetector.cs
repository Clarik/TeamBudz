using UnityEngine;

public class BoxDetector : MonoBehaviour
{
    public Transform[] location;
    private bool[] empty;

    private bool somethingOnGround;
    Collider2D something;

    public LayerMask whatIsBox;

    public GameObject allies;
    public float where;

    public Transform baseLocation;
    public GameObject soldier;

    public Transform defenderSpawnMaxY;
    public Transform defenderSpawnMinY;
    public GameObject defender;
    void Start()
    {
        clear();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.U))
        {
            clear();
        }
    }

    void FixedUpdate()
    {
        check();

        if (somethingOnGround && something != null)
        {
            Transform closest = null;
            float closestRange = 0;
            int loc = 0;
            for (int i = 0; i < location.Length; i++)
            {
                if (empty[i])
                    continue;
                if(closest == null)
                {
                    closest = location[i].GetComponent<Transform>();
                    closestRange = Vector2.Distance(location[i].position, something.GetComponent<Transform>().position);
                    loc = i;
                }
                else if(closestRange > Vector2.Distance(location[i].position, something.GetComponent<Transform>().position))
                {
                    closest = location[i].GetComponent<Transform>();
                    closestRange = Vector2.Distance(location[i].position, something.GetComponent<Transform>().position);
                    loc = i;
                }
            }
            if(closest != null)
            {
                //Debug.Log("Done" + something.name);
                something.GetComponent<Transform>().transform.position = location[loc].transform.position;
                something.GetComponent<BoxInfo>().setIsOnDetector(true);
                something.GetComponent<Rigidbody2D>().velocity = new Vector3();
                something.GetComponent<Rigidbody2D>().isKinematic = true;
                empty[loc] = true;
            }
            somethingOnGround = false;
            something = null;
        }
    }

    void check()
    {
        int count = 0;
        for (int i = 0; i < location.Length; i++)
        {
            //Debug.Log(i + " " + empty[i]);
            Collider2D[] bringItem = Physics2D.OverlapCircleAll(location[i].position, 0f, whatIsBox);
            bool isThere = false;
            for (int j = 0; j < bringItem.Length; j++)
            {
                if (bringItem[j].tag == "Box")
                {
                    if (bringItem[j].GetComponent<BoxInfo>().isOnDetect() == true)
                    {
                        //Debug.Log(bringItem[j].name);
                        empty[i] = true;
                        isThere = true;
                        break;
                    }
                }
            }
            if (!isThere)
                empty[i] = false;
            else
                count++;
        }
        
        if(count == 4)
        {
            clear();
            Instantiate(allies, new Vector2(transform.position.x + where, transform.position.y), Quaternion.identity);
        }
    }

    void clear()
    {
        somethingOnGround = true;
        empty = new bool[location.Length];
        int count = 0;
        for (int i = 0; i < location.Length; i++)
        {
            Collider2D[] bringItem = Physics2D.OverlapCircleAll(location[i].position, 0f, whatIsBox);
            if (bringItem.Length > 0)
            {
                for (int j = 0; j < bringItem.Length; j++)
                {
                    if(bringItem[j].tag == "Box")
                    {
                        // explode and drop item or friend
                        Destroy(bringItem[j].gameObject);
                        count++;
                    }
                }
            }
            empty[i] = false;
        }
        if (count == 2)
        {
            clear();
            Vector2 spawn = new Vector2(transform.position.x + where, Random.Range(defenderSpawnMinY.position.y, defenderSpawnMaxY.position.y));
            Instantiate(defender, spawn, Quaternion.identity);

        }
        if (count == 3)
        {
            clear();
            Vector2 spawn = new Vector2(baseLocation.position.x + where, Random.Range(-10, 10));
            Instantiate(soldier, spawn, Quaternion.identity);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Box")
        {
            if(/*collision.GetComponent<BoxInfo>().isOnDetect() == false*/ !collision.GetComponent<Rigidbody2D>().isKinematic && collision.GetComponent<BoxInfo>().isGrounded() == true)
            {
                somethingOnGround = true;
                something = collision;
            }
        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        //Debug.Log("Bye" + collision.name);
    }
}
