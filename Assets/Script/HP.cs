using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HP : MonoBehaviour
{
    public int hp;

    private void Update()
    {
        if (hp <= 0)
        {
            //Debug.Log(gameObject.name);
            Destroy(gameObject);
        }
    }

    public int getHp()
    {
        return hp;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Bullet")
        {
            hp--;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Bullet")
        {
            hp--;
        }
    }
}
