using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_eagle : Enemy
{
    private Rigidbody2D rb;
    private bool Flyup = true;
    private float topy, bottomy;

    public float speed;
    public Transform toppoint, bottompoint;

    
    // Start is called before the first frame update
    protected  override void Start()
    {
        base.Start();
        rb = GetComponent<Rigidbody2D>();

        topy = toppoint.position.y;
        bottomy = bottompoint.position.y;
        Destroy(toppoint.gameObject);
        Destroy(bottompoint.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
    }

    void Movement()
    {
        if (Flyup)   // 向上飞
        {
            rb.velocity = new Vector2(rb.velocity.x, speed);
            if (transform.position.y > topy)
            {
                Flyup = false;
            }
        }
        else    // 向下飞
        {
            rb.velocity = new Vector2(rb.velocity.x, -speed);
            if (transform.position.y < bottomy)
            {
                Flyup = true;
            }
        }
    }
}
