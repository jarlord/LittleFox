using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class controler : MonoBehaviour
{
    public float hurtTime = 0.5f;

    public AudioSource jumpAudio;
    [Space]
    private Rigidbody2D rb;
    private Animator anim;

    public float speed, jumpForce;

    public LayerMask ground;

    public Collider2D coll;
    public Collider2D Discoll;

    public Transform CellingCheck;
    public Transform groundCheck;

    private bool checkhead = false;

    public bool isGround, isJump, jumpPressed;
    private float horizontalMove;
    public int jumpCount;
 
    public int Cherry;
    public int Gem;

    public Text GemNumber;
    public Text CherryNumber;
    private bool isHurt; //bool 默认false

    private bool isDashing;

    [Header("Dash parameter")]
    public float dashTime;
    private float dashTimeLeft;
    private float lastDash = -10f;
    public float dashCoolDown;
    public float dashSpeed;

    [Header("CD的UI组件")]
    public Image CDImage;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<Collider2D>();
        anim = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        isGround = Physics2D.OverlapCircle(groundCheck.position, 0.1f, ground);
        Jump();
        Crouch();

        Dash();
        if (isDashing)
            return;

        if (!isHurt)
        {
            GroundMovement();
        }
        else if (isHurt)
        {
            hurtTime -= Time.fixedDeltaTime;
            if (hurtTime <= 0)
            {
                isHurt = false;
            }
        }

        SwitchAnim();
    }

    private void Update()
    {
        if (Input.GetButtonDown("Jump") && jumpCount > 0)
        {
            jumpPressed = true;
        }
        CherryNumber.text = Cherry.ToString();
        GemNumber.text = Gem.ToString();

        if (Input.GetKeyDown(KeyCode.J))
        {
            if (Time.time >= (lastDash + dashCoolDown))
            {
                //可以执行dash
                ReadyToDash();
            }
        }
        CDImage.fillAmount -= 1.0f / dashCoolDown * Time.deltaTime;

    }

    // 移动
    void GroundMovement()
    {
        horizontalMove = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(horizontalMove * speed, rb.velocity.y);

        if (horizontalMove != 0)
        {
            transform.localScale = new Vector3(horizontalMove, 1, 1);
        }
    }

    // 跳跃
    void Jump()
    {
        if (isGround)
        {
            jumpCount = 1;
            isJump = false;

        }
        if (jumpPressed && isGround)
        {
            jumpAudio.Play();
            isJump = true;
;           rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            jumpCount--;
            jumpPressed = false;
        }
        else if (jumpPressed && jumpCount >0 && !isGround)
        {
            jumpAudio.Play();
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            jumpCount--;
            jumpPressed = false;
        }
    }

    // 动画
    void SwitchAnim()
    {
        anim.SetFloat("running", Mathf.Abs(rb.velocity.x));

        if (isGround)
        {
            anim.SetBool("falling", false);
            anim.SetBool("jumping", false);
        }
        else if (!isGround && rb.velocity.y > 0)
        {
            anim.SetBool("jumping", true);
        }
        else if (!isGround && rb.velocity.y <0)
        {
            anim.SetBool("jumping", false);
            anim.SetBool("falling", true);
        }
        if (isHurt)
        {
            anim.SetBool("hurt", true);
            anim.SetFloat("running", 0);
        }
        else if (!isHurt)
        {
            anim.SetBool("hurt", false);
            hurtTime = 0.5f;
        }

    }

    // 触发器
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "DeathLine")
        {
            Invoke("Restart", 2f);
        }

        // 收集
        if (collision.tag == "Cherry")
        {
            collision.GetComponent<Animator>().Play("IsGot");
        }
        if (collision.tag == "Gem")
        {
            collision.GetComponent<Animator>().Play("IsGot");
        }
            
    }

    // 消灭敌人
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            Enemy enemy = collision.gameObject.GetComponent<Enemy>();
            if (anim.GetBool("falling"))
            {
                enemy.JumpOn();
                rb.velocity = new Vector2(rb.velocity.x, 0.75f * jumpForce);
                anim.SetBool("jumping", true);
            }else if (transform.position.x < collision.gameObject.transform.position.x)
            {
                rb.velocity = new Vector2(-5f, rb.velocity.y);
                isHurt = true;
            }
            else if (transform.position.x > collision.gameObject.transform.position.x)
            {
                rb.velocity = new Vector2(5f, rb.velocity.y);
                isHurt = true;
            }
        }
    }

    // 趴下
    void Crouch()
    {
        if (Input.GetButton("Crouch"))
        {
            Discoll.enabled = false;
            anim.SetBool("crouching", true);
        }else
        {
            checkhead = true;
        }
        if (checkhead)
        {
            if(!Physics2D.OverlapCircle(CellingCheck.position, 0.2f, ground))
            {
                Discoll.enabled = true;
                anim.SetBool("crouching", false);
                checkhead = false;
            }
        }
    }

    void ReadyToDash()
    {
        isDashing = true;

        dashTimeLeft = dashTime;

        lastDash = Time.time;

        CDImage.fillAmount = 1;
    }

    void Dash()
    {
        if (isDashing)
        {
            if (dashTimeLeft > 0)
            {
                if(rb.velocity.y >0 && !isGround)
                {
                    rb.velocity = new Vector2(dashSpeed * horizontalMove, jumpForce);
                }

                rb.velocity = new Vector2(dashSpeed * horizontalMove, rb.velocity.y);

                dashTimeLeft -= Time.deltaTime;

                ShadowPool.instance.GetFromPool();
            }
            if (dashTimeLeft <= 0)
            {
                isDashing = false;
                if (!isGround)
                {
                    jumpCount += 1;
                }
            }
        }
    }

    void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void CherryCount()
    {
        Cherry += 1;
    }
    public void GemCount()
    {
        Gem += 1;
    }
}
