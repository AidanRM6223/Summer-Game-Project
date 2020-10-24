using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
public class EnemyControllerSky : MonoBehaviour {
 
    public GameObject player;
    public float speed = 5f;
    private Rigidbody2D rb;
    [SerializeField]
    float agroRange;
    public int Health = 50;
    public int Damage = 10;
    public bool MoveRight;
    Vector3 startPos;
    public int patrolDist = 5;
    public Animator anim;
    public bool Attack = false;
    public GameObject explosionRef;
    public Material matWhite, matDefault;
    SpriteRenderer sr;
    bool isFacingLeft;
    private bool isAgro = false;
    private bool isSearching = false;
    public bool isPatrolling = true;
    public bool isMovingBack = false;
    public Vector2 attackEndPos;
    void Start ()
    {
        sr = GetComponent<SpriteRenderer>();
        rb = gameObject.GetComponent<Rigidbody2D>(); 
        startPos = transform.position;
        anim = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player");
        matWhite = Resources.Load("whiteFlash", typeof(Material)) as Material;
        matDefault = new Material(Shader.Find("Sprites/Default"));
        
    }
 
    void Update() 
    {   
        attackEndPos = new Vector2(transform.position.x - (agroRange * -transform.localScale.x), transform.position.y - agroRange);
        if(CanSeePlayer(agroRange)) {
            Invoke("AttackPlayer", 0.5f);
        }
        else {
            Invoke("Patrol", 2f);
        }
        anim.SetBool("Attack", Attack);
        if(player == null) {
            GetComponent<Collider2D>().enabled = true;
            GetComponent<SpriteRenderer>().enabled = true;
            transform.position = startPos;
            player = GameObject.FindGameObjectWithTag("Player");
        }
    }
    void Patrol() {
        Attack = false;
        if(!isPatrolling && !Attack)
        {
            Invoke("MoveBackToStartPos", 1f);
        }
        else if(isPatrolling){
            RaycastHit2D hit = Physics2D.Linecast(transform.position, new Vector2(transform.position.x + 1 * transform.localScale.x, transform.position.y), 1 << LayerMask.NameToLayer("Ground"));
            Vector2 leftPatrolPos = startPos;
            Vector2 rightPatrolPos = startPos;
            leftPatrolPos.x = startPos.x - patrolDist;
            rightPatrolPos.x = startPos.x + patrolDist;          
            if (!isFacingLeft)
                transform.Translate (Vector2.right * speed * Time.deltaTime);
            else
                transform.Translate (-Vector2.right * speed * Time.deltaTime);
            
            if(transform.position.x >= rightPatrolPos.x || hit.collider != null) {
                transform.localScale = new Vector2(-1, 1);
                isFacingLeft = true;
            }
            
            if(transform.position.x <= leftPatrolPos.x || hit.collider != null) {
                transform.localScale = new Vector2(1, 1);
                isFacingLeft = false;
            }  
        }
        
    }
    void AttackPlayer() {
        Attack = true;
        isPatrolling = false; 
        isMovingBack = false;
        transform.position = Vector2.MoveTowards(transform.position, attackEndPos, speed * 3 * Time.deltaTime);
    }
    void MoveBackToStartPos () {
        isMovingBack = true;
        transform.position = Vector2.MoveTowards(transform.position, startPos, (speed / 2) * Time.deltaTime);
        if(Vector2.Distance(transform.position, startPos) < 0.001f) {
            isMovingBack = false;
            rb.velocity = Vector2.zero;
            isPatrolling = true;
            
        }
    }
    bool CanSeePlayer(float distance) {
        bool val = false;
        float castDist = distance;


        if(isFacingLeft) {
            castDist = -distance;
        }
        Vector2 endPos = attackEndPos;
        RaycastHit2D hit1 = Physics2D.Linecast(transform.position, endPos, 1 << LayerMask.NameToLayer("Player"));
        if(hit1.collider != null) {
            if(hit1.collider.gameObject.CompareTag("Player")) {
                val = true;
            }
            else {
                val = false;
            }
            Debug.DrawLine(transform.position, hit1.point, Color.red);
        } else {
            Debug.DrawLine(transform.position, endPos, Color.green);
        }

        return val;
    }
    public void TakeDamage(int damage) {
        Health-= damage;
        if(Health <= 0) {
            Die();
        }
        else {
            Invoke("ResetMaterial", .1f);
        }
    }
    void ResetMaterial() {
        sr.material = matDefault;
    }
    public void Die() {
        GameObject explosion = Instantiate(explosionRef);
        explosion.transform.position = transform.position;
        GetComponent<Collider2D>().enabled = false;
        GetComponent<SpriteRenderer>().enabled = false;
        player.GetComponent<MeleeCombat>().currentHealth += 4;
    }
    private void OnTriggerEnter2D(Collider2D other) {
        if(other.CompareTag("PlayerBullet")) {
            sr.material = matWhite;
            TakeDamage(other.GetComponent<bullet>().damage);
            GameObject.Find("AudioManager").GetComponent<AudioManager>().Play("EnemyExplode");
        }
    }
}
