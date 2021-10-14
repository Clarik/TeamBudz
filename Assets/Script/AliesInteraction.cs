using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class AliesInteraction : MonoBehaviour
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


    public LayerMask whatIsPlacement;

    void Start()
    {
        nowBringing = false;
        isBring = false;
    }

    void Update()
    {
        // Lifting up box
        Collider2D[] bringItem = Physics2D.OverlapCircleAll(frontBackDetector.position, range, whatIsBox);
        if (bringItem.Length > 0)
            isBring = true;
        bringItem = Physics2D.OverlapCircleAll(leftRightDetector.position, range, whatIsBox);
        if (bringItem.Length > 0)
            isBring = true;
    }

    private void FixedUpdate()
    {
        direction = GetComponent<AliesAI>().whereDirection();
        if (isBring && (direction == 1 || direction == 3))
        {
            Collider2D[] bringItem = Physics2D.OverlapCircleAll(frontBackDetector.position, range, whatIsBox);
            for (int i = 0; i < bringItem.Length; i++)
            {
                if (bringItem[i].tag == "Box" && bringItem[i].GetComponent<BoxInfo>().isGrounded() && !bringItem[i].GetComponent<BoxInfo>().isOnDetect())
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
                if (bringItem[i].tag == "Box" && bringItem[i].GetComponent<BoxInfo>().isGrounded() && !bringItem[i].GetComponent<BoxInfo>().isOnDetect())
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
            if (direction == 1)
            {
                target = new Vector3(0, space);
            }
            else if (direction == 2)
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

        if (nowBringing)
        {
            Collider2D[] bringItem = Physics2D.OverlapCircleAll(boxLocation.position, 1f, whatIsPlacement);
            for (int i = 0; i < bringItem.Length; i++)
            { 
                if (bringItem[i].tag == "Placement")
                {
                    if (boxLocation != null)
                    {
                        boxLocation.GetComponent<BoxInfo>().setIsGrounded(true);
                        boxLocation.position = this.boxLocation.position;
                    }
                    boxLocation = null;
                    nowBringing = false;
                    break;
                }
            }
        }

        
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(leftRightDetector.position, range);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(frontBackDetector.position, range);
    }

    public bool isBringing()
    {
        return nowBringing;
    }
}
