using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;
using UnityEngine;

public class MeleeCombat : MonoBehaviour
{
    public AudioManager audioManager;
    public int maxHealth = 100;
    public int currentHealth;
    public HealthBar healthBar;
    public Animator animator;

    public Transform attackPoint;
    public float attackRange = 0.5f;
    public LayerMask targetLayers;

    public int attackDamage = 40;
    public Rigidbody2D rb;
    public int direction;
    public GameObject canvas;
    public PlayerController playerController;
    public bool isDead = false;
    public Animator flashAnim;
    // Update is called once per frame
    private void Start() {
        flashAnim = GameObject.Find("FlashScreen").GetComponent<Animator>();
        audioManager = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>();
        audioManager.BGMSource.Play();
        canvas = GameObject.Find("Canvas");
        rb =  GetComponent<Rigidbody2D>();
        healthBar = canvas.GetComponentInChildren<HealthBar>();
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
        playerController = GetComponent<PlayerController>();

    }
    public void Die() {
        isDead = true;
        currentHealth = 0;
        rb.velocity = Vector2.zero;
        rb.constraints = RigidbodyConstraints2D.FreezePosition;
        animator.Play("Death");
        audioManager.BGMSource.Stop();
        FindObjectOfType<AudioManager>().Play("Death");
        playerController.respawn.isRespawned = false;
        playerController.enabled = false;
        Destroy(gameObject, 1f);
        
    }
    
    void Update()
    {
        /* if(playerController.isJumping && Input.GetButtonDown("Jump")) {
            Homing();
        } */
        if(transform.localScale.x == -1f)
            direction = -1;
        else
            direction = 1;
        if(playerController.dashBoost) {
            DashDamage();
        }
        healthBar.SetHealth(currentHealth);
        if((currentHealth <= 0 && !isDead)) {
            Die();
        }
        if(Input.GetKeyDown(KeyCode.Delete)) currentHealth = 0;
        animator.SetInteger("Health", currentHealth);
    }
    public void TakeDamage(int damage) {
        currentHealth -= damage;
        rb.AddForce(new Vector2(10 * -transform.localScale.x, 10), ForceMode2D.Impulse);
        audioManager.Play("Hurt");
    }
    void DashDamage() {
        Collider2D[] hitTargets = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, targetLayers);
        //Damage them
        foreach(Collider2D target in hitTargets) {     
            if(target.GetComponent<EnemyController>()) {
                target.GetComponent<EnemyController>().Die();
            }  
                
        }
    }
    
    private void OnDrawGizmosSelected() {
        if(attackPoint == null){return;}
        //Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
    private void OnTriggerEnter2D(Collider2D other) {
        
    }
}
