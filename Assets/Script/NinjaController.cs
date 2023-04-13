using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NinjaController : MonoBehaviour
{
    //Component
    private Rigidbody2D rb;
    private BoxCollider2D coll;
    private SpriteRenderer sprite;
    private Animator anim;
    // Attack
    public Transform attackPoint;
    public float attackRange = 0.5f;
    public LayerMask enemyLayers;

    //========Atribute========
    [SerializeField] private int maxHealth = 5;
    private int Level;
    private int expMax = 20;
    private int expCurrent = 0;
    private int currentHealth;
    private int Damage;
    //
    public HealthBar healthBar;
    //[SerializeField] private LayerMask jumpableGround;

    //========Movement========
    private float dirX = 0f;
    [SerializeField] private float moveSpeed = 7f;
    bool grounded;
    private bool facingRight = true;

    //========Jump============
    [SerializeField]
    private int maxJumps = 2;
    private int _jumpsLeft;
    [SerializeField] private float jumpForce = 14f;

    //========State==========
    private enum MovementState { idle, running, jumping, falling , attacking}
    private MovementState state = MovementState.idle;
    
    //========Audio==========
    private AudioSource audioSource;
    public AudioClip jumpSound;
    public AudioClip attackSound;
    public AudioClip attackBulletSound;

    //========Dead==========
    private bool isDead;

    // Start is called before the first frame update
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<BoxCollider2D>();
        sprite = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();

        _jumpsLeft = maxJumps;

        Level = 1;
        maxHealth +=Level;
        expMax *=Level;
        currentHealth = maxHealth;
        Damage = 1;
        healthBar.SetMaxHealth(maxHealth);
        isDead = false;

        enemyLayers = LayerMask.GetMask("Monster");
    }

    // Update is called once per frame
    private void Update()
    {
        if (isDead == true)
        {
            if (Input.GetKeyDown(KeyCode.X))
            {
                SceneManager.LoadScene(1, LoadSceneMode.Single);
                isDead = false;
            }
            return;
        }
        if(expCurrent == expMax)
        {
            ChangeLevel(1);
        }
        //==============Jump=================
        if(grounded && rb.velocity.y<=0)
        {
            _jumpsLeft=maxJumps;
        }

        if (Input.GetButtonDown("Jump")  && _jumpsLeft > 0)
        {
            PlaySound(jumpSound);
            //audiosource.Play(jumpSoundEffect);
            //if(jumpNumber<2)
            //{ 
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                _jumpsLeft -= 1;
                grounded=false;
            
            //}
        
        }
        // ================Attack===========
        if(Input.GetButtonDown("Fire1"))
        {
            ChangeEXP(1);
            Attack();
        }
        if(Input.GetButtonDown("Fire2"))
        {
            AttackWithBullet();
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            RaycastHit2D hit = Physics2D.Raycast(rb.position, new Vector2(-1, 0), 1.5f, LayerMask.GetMask("NPC"));
            if (hit.collider != null)
            {
                NPCController character = hit.collider.GetComponent<NPCController>();
                if (character != null)
                {
                    character.DisplayDialog();
                }
            }
        }
        
        UpdateAnimationState();
        
    }

    private void FixedUpdate()
    {
        if (isDead == true)
        {
            return;
        }
        dirX = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(dirX * moveSpeed, rb.velocity.y);
        if(dirX>0 && !facingRight)
        {
            Flip();
        }
        if(dirX<0 && facingRight)
        {
            Flip();
        }
    }
    //==============CHANGE HEALTH===========
    public void ChangeHealth(int amount)
    {
        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
        healthBar.SetHealth(currentHealth);
        if (amount < 0)
        {
            anim.SetTrigger("attacked");
        }
        if (currentHealth <= 0)
        {
            isDead = true;
            anim.SetTrigger("dead");
            rb.velocity = Vector2.zero;
        }
    }
    //==============CHANGE EXP==============
    public void ChangeEXP(int amount)
    {
        expCurrent = Mathf.Clamp(expCurrent + amount, 0, expMax);
    }
    //==============CHANGE LEVEL==============
    public void ChangeLevel(int amount)
    {
        Level+= amount;
        expCurrent = 0;
        expMax = expMax + Level*Level;
        maxHealth +=Level;
        healthBar.SetMaxHealth(maxHealth);
    }
    //==============STATE ANIMATION=========
    private void UpdateAnimationState()
    {
        if (dirX != 0f)
        {
            state = MovementState.running;
        }
        else
        {
            state = MovementState.idle;
        }

        if (rb.velocity.y > .1f)
        {
            state = MovementState.jumping;
        }
        else if (rb.velocity.y < -.1f)
        {
            state = MovementState.falling;
        }

        anim.SetInteger("state", (int)state);
    }
    //================Attack===============
    private void Attack()
    {
        // Detect enemies in range of attack
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);
        
        if (hitEnemies.Length != 0)
        {
            // Play an attack animation
            anim.SetTrigger("attack");
            PlaySound(attackSound);
            //Damage them
            foreach (Collider2D enemy in hitEnemies)
            {
                Debug.Log("We hit "+enemy.name);
                monsterController mc = enemy.gameObject.GetComponent<monsterController>();
                mc.Hit(Damage);
            }
        }
    }
    //==============Attack with bullet====
    private void AttackWithBullet()
    {
        // Play an attack animation
        Collider2D hitEnemy = Physics2D.OverlapCircle(attackPoint.position, 10f, enemyLayers);
        if(hitEnemy!=null && hitEnemy.gameObject.GetComponent<SpriteRenderer>().enabled == true)
        {
            PlaySound(attackBulletSound);
            anim.SetTrigger("attack");
            GameObject bullet = ObjectPool.instance.Spawn();
            if(bullet!=null)
            {
                bullet.transform.position = gameObject.transform.position;
                bullet.SetActive(true);
                
                bullet.GetComponent<bullet>().SetDirection(hitEnemy.transform.position);
            }
        }
    }
    //
    private void OnDrawGizmosSelected() {
        if(attackPoint == null)
        {
            return;
        }
        Gizmos.DrawWireSphere(attackPoint.position,attackRange);
    }
    //==================FLIP()=============================
    private void Flip()
    {
        Vector3 currentScale = gameObject.transform.localScale;
        currentScale.x *=-1;
        gameObject.transform.localScale = currentScale;
        facingRight = !facingRight;
    }

    public void PlaySound(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }

    //
    // private bool IsGrounded()
    // {
    //     return Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.down, .1f, jumpableGround);
    // }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // if(collision.gameObject.tag == "Ground")
        // {
            grounded = true;
        // }
    }
}