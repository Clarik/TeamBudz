using UnityEngine;

public class BoxInfo : MonoBehaviour
{
    bool isGround;
    bool isOnDetector;
    void Start()
    {
        isGround = true;
        isOnDetector = false;
    }
    public void setIsOnDetector(bool detect)
    {
        this.isOnDetector = detect;
    }

    public bool isOnDetect()
    {
        return isOnDetector;
    }

    public void setIsGrounded(bool grounded)
    {
        this.isGround = grounded;
    }

    public bool isGrounded()
    {
        return isGround;
    }
}
