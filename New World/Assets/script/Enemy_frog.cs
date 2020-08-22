using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_frog : Enemy
{
    private Rigidbody2D rb;
    private Collider2D coll;
    public LayerMask ground;
    private bool Faceleft = true;
    private float leftx, rightx;

    public float speed, jumpforce;
    public Transform leftpoint, rightpoint;

    protected override void Start()
    {
        base.Start();

        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<Collider2D>();

        transform.DetachChildren();
        leftx = leftpoint.position.x;
        rightx = rightpoint.position.x;
        Destroy(leftpoint.gameObject);
        Destroy(rightpoint.gameObject);

    }


    void Update()
    {
        SwitchAnim();
    }

    void Movement()
    {
        if (Faceleft)  // 面向左侧
        {
            if (coll.IsTouchingLayers(ground))
            {
                anim.SetBool("jumping", true);
                rb.velocity = new Vector2(-speed, jumpforce);
            }
            if (transform.position.x < leftx)
            {
                transform.localScale = new Vector3(-1, 1, 1);
                Faceleft = false;
            }
        } 
        else    // 面向右侧
        {
            if (coll.IsTouchingLayers(ground))
            {
                anim.SetBool("jumping", true);
                rb.velocity = new Vector2(speed, jumpforce);
            }
            if (transform.position.x > rightx)
            {
                transform.localScale = new Vector3(1, 1, 1);
                Faceleft = true;
            }
        }
    }

    void SwitchAnim()
    {
        if (anim.GetBool("jumping"))
        {
            if (rb.velocity.y < 0.1f)
            {
                anim.SetBool("jumping", false);
                anim.SetBool("falling", true);
            }
        }
        if (coll.IsTouchingLayers(ground) && anim.GetBool("falling")){
            anim.SetBool("falling", false);
        }
    }
}

