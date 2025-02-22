using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class PlayerControl : MonoBehaviour
{
    public float speed;
    public float jumpForce;
    public float trampolineForce;
    public Text finishText;
    public Button reloadButton;
    public AudioSource soundControl;
    public AudioClip jumpSound;
    public AudioClip damageSound;
    public AudioClip finishSound;
    public GameObject GameMusic;
    public GameObject[] hearts;
    bool shouldJump = true;
    bool left = false;
    int goldCounter = 0;
    int health = 3;
    public Text coinText;
    
    public float knockbackForce = 10f; 
    public float knockbackDuration = 0.2f;
    private bool isKnockedBack = false;



    private Rigidbody2D rb;
    Animator playerAnimator;
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerAnimator = GetComponent<Animator>();
        finishText.gameObject.SetActive(false);
        reloadButton.gameObject.SetActive(false);
    }

   
    void Update()
    {

    }

    private void FixedUpdate()
    {
        if (isKnockedBack) return;

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
                
                soundControl.PlayOneShot(jumpSound);
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
        if (collision.transform.CompareTag("LowTrampoline"))
        {
            rb.linearVelocity = new Vector2(rb.linearVelocityX, trampolineForce);
        }
        if (collision.transform.CompareTag("MidTrampoline"))
        {
            rb.linearVelocity = new Vector2(rb.linearVelocityX, trampolineForce * 1.4f);
        }
        if (collision.transform.CompareTag("HighTrampoline"))
        {
            rb.linearVelocity = new Vector2(rb.linearVelocityX, trampolineForce * 1.65f);
            
        }
        /*if (collision.transform.CompareTag("Enemy"))
        {
            
            Vector2 knockbackDirection = (transform.position - collision.transform.position).normalized;
            transform.position = new Vector2((transform.position.x + knockbackDirection.x) * 2, transform.position.y + knockbackDirection.y);
            //rb.linearVelocity = new Vector2(rb.linearVelocityX, trampolineForce * 0.5f);
            //health --;
            Debug.Log(health);
            soundControl.PlayOneShot(damageSound);
        }*/
        if (collision.transform.CompareTag("Water"))
        {
            SceneManager.LoadScene("GameScene");
        }
        if(collision.transform.CompareTag("Finish"))
        {
            Destroy(GameMusic);
            soundControl.Stop();
            soundControl.PlayOneShot(finishSound);
            Time.timeScale = 0f;
            finishText.gameObject.SetActive(true);
            reloadButton.gameObject.SetActive(true);
        }
        
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.transform.CompareTag("Gold"))
        {
            goldCounter++;
            Destroy(collision.gameObject);
            coinText.text = goldCounter.ToString();
            
        }
        if(collision.transform.CompareTag("Enemy"))
        {
            float playerY = transform.position.y;
            float enemyY = collision.transform.position.y;

            if (playerY > enemyY + 0.2f)
            {
                Destroy(collision.gameObject);
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, 10f);
            }
            else
            {
                StartCoroutine(Knockback(collision.transform));
                health --;
                UpdateHeartsUI();
                soundControl.PlayOneShot(damageSound);
                if (health == 0)
                {
                    Time.timeScale = 0f;
                    finishText.gameObject.SetActive(true);
                    reloadButton.gameObject.SetActive(true);
                }
            }
        }
    }

    public void ReloadGame()
    {
        SceneManager.LoadScene("GameScene");
        Time.timeScale = 1f;
    }

    private IEnumerator Knockback(Transform enemy)
    {
        if (isKnockedBack) yield break;
        isKnockedBack = true;

        Vector2 knockbackDirection = (transform.position - enemy.position).normalized;
        rb.linearVelocity = knockbackDirection * knockbackForce;

        yield return new WaitForSeconds(knockbackDuration);

        rb.linearVelocity = Vector2.zero;
        isKnockedBack = false;
    }

    void UpdateHeartsUI()
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            if (i < health)
                hearts[i].SetActive(true);  
            else
                hearts[i].SetActive(false); 
        }
    }

}
