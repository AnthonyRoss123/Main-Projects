using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementQ : MonoBehaviour
{
    public float moveSpeed;
    public float jumpForce;
    public Transform ceilingCheck;
    public Transform groundCheck;
    public LayerMask groundObjects;
    public float checkRadius;
    public int maxJumpCount;

    private Rigidbody2D rb;
    private bool facingRight = true;
    private float moveDirection;
    private bool isJumping = false;
    private bool isGrounded;
    private int jumpCount;
    //New
    public float boostTimer;
    private bool boosting;

    
    // Start is called before the first frame update
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        jumpCount = maxJumpCount;
        //New
        moveSpeed = 5;
        boostTimer = 0;
        boosting = false;
    }

    // Update is called once per frame
    void Update()
    {
        ProcessInput();

        Animation();
        //New
        if(boosting)
        {
            boostTimer += Time.deltaTime;
            if(boostTimer >= 10)//<--Make sure this is higher than in-game boostTimer
            {
                moveSpeed = 5;
                boostTimer = 0;
                boosting = false;

            }
        }
    }

     void OnTriggerEnter2D(Collider2D other)
     {
        //New
        if(other.tag == "Speed+")
        {
            boosting = true;
            moveSpeed = 15;
            Destroy(other.gameObject);
        }
       
     }

    private void FixedUpdate()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, groundObjects);
        if(isGrounded)
        {
            jumpCount = maxJumpCount;
        }



        Movement();
    }

    private void FlipCharacter()
    {
        facingRight = !facingRight;
        transform.Rotate(0f, 180f, 0f);
    }

    private void ProcessInput()
    {
        moveDirection = Input.GetAxis("Horizontal");
        if (Input.GetButtonDown("Jump") && jumpCount > 0)
        {
            isJumping = true;
        }
    }

    private void Animation()
    {
        if(moveDirection > 0 && !facingRight)
        {
            FlipCharacter();
        }
        else if(moveDirection < 0 && facingRight)
        {
            FlipCharacter();
        }
    }

    private void Movement()
    {
        rb.velocity = new Vector2(moveDirection * moveSpeed, rb.velocity.y);
        if(isJumping)
        {
            rb.AddForce(new Vector2(0f, jumpForce));
            jumpCount--;
        }
        isJumping = false;
    }
        

        
    
}
