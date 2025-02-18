using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    public float speed;
    public float jumpForce;
    public float jumpTime = 0.5f;
    bool shouldJump = true;
    bool left = false;

    private Rigidbody2D rb;
    Animator playerAnimator;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerAnimator = GetComponent<Animator>();
    }

   
    void Update()
    {
        
    }

    private void FixedUpdate()
    {

        float moveHorizontal = Input.GetAxis("Horizontal");
        playerAnimator.SetFloat("Speed", Mathf.Abs(moveHorizontal));

        // Move
        rb.linearVelocity = new Vector2(moveHorizontal * speed, rb.linearVelocityY);
        

        // Rotate Char
        if (moveHorizontal < 0 && !left)
        {
            RotateChar();
        }
        else if (moveHorizontal > 0 && left)
        {
            RotateChar();
        }


        // Jump
        if (Input.GetKey(KeyCode.Space))
        {
            if (shouldJump)
            {
                rb.linearVelocity = new Vector3(rb.linearVelocityX, jumpForce, 0);
                shouldJump = false;
                playerAnimator.SetBool("Jump", true);
            }
        }
        if (Mathf.Approximately(rb.linearVelocityY,0) && playerAnimator.GetBool("Jump"))
        {
            playerAnimator.SetBool("Jump", false);
        }
    }

    void RotateChar()
    {
        left = !left;
        Vector3 newScale = transform.localScale;
        newScale.x *= -1;
        transform.localScale = newScale;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.transform.CompareTag("Floor"))
        {
            shouldJump = true;
        }
    }
    
}
