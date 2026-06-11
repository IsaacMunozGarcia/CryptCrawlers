using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float maxHealth = 200;
    [SerializeField] private float movementForce = 10f;
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private GameObject swordProjectile;
    [SerializeField] private Transform shootPoint;
    
    [Header("Shield")]
    [SerializeField] private BoxCollider2D shieldColliderL;
    [SerializeField] private BoxCollider2D shieldColliderR;
    [Header("UI")]
    [SerializeField] private Image healthBar;
    
    [Header ("Particles")]
    [SerializeField] private ParticleSystem jumpParticle;
    [SerializeField] private ParticleSystem attackParticle;
    [SerializeField] private ParticleSystem damageParticle;
    [SerializeField] private ParticleSystem healParticle;
    
    
    private Rigidbody2D rb;
    private Collider2D groundDetector;
    private Animator animator;
    private float hInput;
    private Vector3 originalScale;
    private float score;
    private float currentHealth;
    private bool blocking;
    private bool attacking;
    private bool canTakeDamage;
    private bool isDead;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        groundDetector = GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();
        originalScale = transform.localScale;
        currentHealth = maxHealth;
        canTakeDamage = true;
        isDead = false;
    }

    private void Update()
    {
        if (!isDead)
        {
            MovementUpdate();
        }
        UpdateAnimations();
        FaceMovement();
        HealthTimer();
    }

    #region MOVEMENT

    private void MovementUpdate()
    {
        //WALK
        if (IsGrounded() && !blocking && !attacking)
        {
            hInput = Input.GetAxisRaw("Horizontal");
        }
        
        //JUMP
        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded() && !attacking)
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            jumpParticle.Play();
            animator.SetTrigger("jump");
            SoundEffectManager.Play("Jump");
        }

        //ATTACK
        if (Input.GetMouseButtonDown(0) && !blocking && IsGrounded() && !attacking)
        {
            attacking = true;
            hInput = 0;
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
            animator.SetTrigger("attack");
        }
        //BLOCK
        if (Input.GetMouseButton(1) && IsGrounded() && rb.linearVelocity.y < 0.1)
        {
            hInput = 0;
            blocking = true;
            animator.SetBool("blocking", true);
            if (transform.localScale.x > 0)
            {
                shieldColliderR.enabled = true;
            }
            else
            {
                shieldColliderL.enabled = true;
            }
        }
        else
        {
            blocking = false;
            animator.SetBool("blocking", false);
            shieldColliderL.enabled = false;
            shieldColliderR.enabled = false;
        }
    }

    public void SwordProjectile()
    {
        attackParticle.Play();
        SoundEffectManager.Play("Attack");
        float dir = transform.localScale.x > 0 ? 1f : -1f;
        GameObject proj = Instantiate(swordProjectile, shootPoint.position, Quaternion.identity);
        proj.transform.localScale = new Vector3(Mathf.Sign(dir), 0.3f, 0.3f);
        proj.GetComponent<SwordProjectile>()?.SetDirection(new Vector2(dir, 0));
    }

    public void EndAttacking()
    {
        attacking = false;
    }
    
    

    #endregion

    #region PHYSICS

    private void FixedUpdate()
    {
            if (attacking)
            {
                rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
                return;
            }

            rb.linearVelocity = new Vector2(hInput * movementForce, rb.linearVelocity.y);
    }

    private bool IsGrounded()
    {
        return groundDetector.IsTouchingLayers(groundLayer);
    }

    #endregion

    #region ANIMATIONS

    private void UpdateAnimations()
    {
        animator.SetFloat("yVelocity", rb.linearVelocity.y);
        animator.SetFloat("speed", Mathf.Abs(rb.linearVelocity.x));
    }

    private void FaceMovement()
    {
        if (!isDead)
        {
            if (hInput < 0)
            {
                transform.localScale = new Vector3(-Mathf.Abs(originalScale.x), originalScale.y, originalScale.z);
            }
            else if (hInput > 0)
            {
                transform.localScale = new Vector3(Mathf.Abs(originalScale.x), originalScale.y, originalScale.z);
            }
        }
    }

    private IEnumerator DeathSequence()
    {
        animator.SetTrigger("isDead");
        SoundEffectManager.Play("Death");
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene("Defeat");
    }

    #endregion
    
    #region TRIGGER INTERACTIONS

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Coin"))
        {
            score += 10;
            Destroy(other.gameObject);
        }
        else if (other.CompareTag("Soul"))
        {
            healParticle.Play();
            SoundEffectManager.Play("Soul");
            currentHealth += 25;
            Destroy(other.gameObject);
        }
        else if (other.CompareTag("Enemy") && canTakeDamage)
        {
            currentHealth -= 10;
            CanTakeDamage();
            damageParticle.Play();
            StartCoroutine(CanTakeDamage());
        }
        else if (other.CompareTag("EnemyBullet") && canTakeDamage)
        {
            currentHealth -= 10;
            Destroy(other.gameObject);
            damageParticle.Play();
            StartCoroutine(CanTakeDamage());
        }
        else if (other.CompareTag("Goal"))
        {
            SceneManager.LoadScene("Win");
        }
    }

    private IEnumerator CanTakeDamage()
    {
        canTakeDamage = false;
        SoundEffectManager.Play("Damage");
        animator.SetTrigger("damage");
        yield return new WaitForSeconds(0.5f);
        canTakeDamage = true;
    }

    private void HealthTimer()
    {
        currentHealth -= Time.deltaTime * 2;
        healthBar.fillAmount = currentHealth / maxHealth;
        
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }

        if (currentHealth <= 0)
        {
            isDead = true;
            StartCoroutine(DeathSequence());
        }
    }
    #endregion
}