using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // Movement
    public float speed;
    private float horiz, verti;
    private Vector2 direction;
    private bool facingRight, facingFront;
    private int direct;

    /*
     * 1 -> up
     * 2 -> right
     * 3 -> down
     * 4 -> left
     */

    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        facingRight = facingFront = true;
        direct = 2;
    }
    void Update()
    {
        horiz = Input.GetAxisRaw("Horizontal"); // left = -1, right = 1
        verti = Input.GetAxisRaw("Vertical"); // up = 1 , down = -1
        direction = new Vector2(horiz, verti) * speed;
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
        if(direct == 2 || direct == 4)
            if (verti > 0.5f)
                direct = 1;
            else if (verti < -0.5f)
                direct = 3;
    }

    void FixedUpdate()
    {
        if ((horiz < 0 && facingRight) || (horiz > 0 && !facingRight))
            FlipHoriz();
        if((verti < 0 && !facingFront) || (verti > 0 && facingFront))
            FlipVerti();

        rb.MovePosition(rb.position + direction * Time.deltaTime);
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

    public int whereDirection()
    {
        return direct;
    }

    public bool IsFacingRight()
    {
        return facingRight;
    }
    
    public bool IsFacingFront()
    {
        return facingFront;
    }
    
}
