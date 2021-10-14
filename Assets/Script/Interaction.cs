using UnityEngine;

public class Interaction : MonoBehaviour
{
    // Detect Box
    private bool isBring;
    public Transform leftRightDetector;
    public Transform frontBackDetector;
    public float range;
    private int direction;

    public LayerMask whatIsBox;

    public float space;
    private Transform boxLocation;
    private bool nowBringing;
    private Vector3 target;


    void Start()
    {
        nowBringing = false;
        isBring = false;
    }

    void Update()
    {
        // Lifting up box
        if (Input.GetKeyDown(KeyCode.C) && !nowBringing)
        {
            isBring = true;
        }
        // Drop box
        if(Input.GetKeyDown(KeyCode.C) && nowBringing)
        {
            nowBringing = false;
        }
    }

    private void FixedUpdate()
    {
        direction = GetComponent<PlayerMovement>().whereDirection();
        if (isBring && (direction == 1 || direction == 3))
        {
            Collider2D[] bringItem = Physics2D.OverlapCircleAll(frontBackDetector.position, range, whatIsBox);
            for (int i = 0; i < bringItem.Length; i++)
            {
                if (bringItem[i].tag == "Box")
                {
                    bringItem[i].GetComponent<BoxInfo>().setIsOnDetector(false);
                    bringItem[i].GetComponent<BoxInfo>().setIsGrounded(false);
                    bringItem[i].GetComponent<Rigidbody2D>().isKinematic = false;
                    boxLocation = bringItem[i].GetComponent<Transform>();
                    nowBringing = true;
                    break;
                }
            }
            isBring = false;
        }
        else if (isBring && (direction == 2 || direction == 4))
        {
            Collider2D[] bringItem = Physics2D.OverlapCircleAll(leftRightDetector.position, range, whatIsBox);
            for (int i = 0; i < bringItem.Length; i++)
            {
                if (bringItem[i].tag == "Box")
                {
                    bringItem[i].GetComponent<BoxInfo>().setIsOnDetector(false);
                    bringItem[i].GetComponent<BoxInfo>().setIsGrounded(false);
                    bringItem[i].GetComponent<Rigidbody2D>().isKinematic = false;
                    boxLocation = bringItem[i].GetComponent<Transform>();
                    nowBringing = true;
                    break;
                }
            }
            isBring = false;
        }

        if (nowBringing)
        {
            if(direction == 1)
            {
                target = new Vector3(0 , space);
            }
            else if(direction == 2)
            {
                target = new Vector3(space, 0);
            }
            else if (direction == 3)
            {
                target = new Vector3(0, -space);
            }
            else if (direction == 4)
            {
                target = new Vector3(-space, 0);
            }
            boxLocation.position = transform.position + target;
            //boxLocation.position = Vector2.MoveTowards(boxLocation.position, transform.position + target, 20f * Time.deltaTime);
        }
        else
        {
            if(boxLocation != null)
            {
                boxLocation.GetComponent<BoxInfo>().setIsGrounded(true);
                boxLocation.position = this.boxLocation.position;
            }
            boxLocation = null;
        }
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(leftRightDetector.position, range);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(frontBackDetector.position, range);
    }
}
