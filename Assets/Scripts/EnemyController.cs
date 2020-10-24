using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
public class EnemyController : MonoBehaviour {
 
    public GameObject player;
    public float speed = 5f;
    private Rigidbody2D rb;
    [SerializeField]
    float agroRange;
    [SerializeField]
    Transform castPoint;
    public int Health = 50;
    public int Damage = 10;
    public bool MoveRight;
    Vector2 startPos;
    public int patrolDist = 5;
    public Animator anim;
    public bool isChasing = false;
    public GameObject explosionRef;
    public Material matWhite, matDefault;
    SpriteRenderer sr;
    bool isFacingLeft;
    private bool isAgro = false;
    private bool isSearching = false;
    public LayerMask targetLayers;
    private Transform originalPos;
    void Start ()
    {
        rb = gameObject.GetComponent<Rigidbody2D>(); 
        rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
        sr = GetComponent<SpriteRenderer>();
        
        startPos = transform.position;

        anim = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player");
        matWhite = Resources.Load("whiteFlash", typeof(Material)) as Material;
        matDefault = new Material(Shader.Find("Sprites/Default"));
    }
 
    void Update() 
    {   
        if(CanSeePlayer(agroRange)) {
            isAgro = true;
            
        }
        else {
            if(isAgro) {
                if(!isSearching) {
                    isSearching = true;
                    Invoke("StopChasingPlayer", 3);
                }
                
            }
        }
        if(isAgro) {
            if(!isChasing)
            {
                anim.Play("GetUp");
                Invoke("ChasePlayer", 0.5f);
            }
            else {
                ChasePlayer();
            }
            
        }
        anim.SetBool("isChasing", isChasing);
        if(player == null) {
            GetComponent<Collider2D>().enabled = true;
            GetComponent<SpriteRenderer>().enabled = true;
            GetComponent<Rigidbody2D>().isKinematic = false;   
            transform.position = startPos;         
            player = GameObject.FindGameObjectWithTag("Player");
        }
    }
    void ChasePlayer() {
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        isChasing = true;
        if(transform.position.x < player.transform.position.x) {
            rb.velocity = new Vector2(speed, rb.velocity.y);      
            transform.localScale= new Vector2 (2,2);
            isFacingLeft = false;
        }
        else {
            rb.velocity = new Vector2(-speed, rb.velocity.y);
            transform.localScale= new Vector2 (-2,2);
            isFacingLeft = true;
        }
    }
    void StopChasingPlayer() { 
        isChasing = false;
        isSearching = false;
        isAgro = false;
        rb.constraints = RigidbodyConstraints2D.FreezePositionX;
    }
    bool CanSeePlayer(float distance) {
        bool val = false;
        float castDist = distance;


        if(isFacingLeft || transform.localScale.x < 0) {
            castDist = -distance;
        }
        Vector2 endPos = castPoint.position + Vector3.right * castDist;
        // Vector2 endPos2 = castPoint.position + Vector3.right * -castDist;
        RaycastHit2D hit1 = Physics2D.Linecast(castPoint.position, endPos, targetLayers);
        // RaycastHit2D hit2 = Physics2D.Linecast(castPoint.position, endPos2, targetLayers);
        if(hit1.collider != null /* || hit2.collider != null */) {
             if(hit1.collider.gameObject.CompareTag("Player") /* || hit2.collider.gameObject.CompareTag("Player") */) {
                val = true;
            }
            else {
                val = false;
            }
            Debug.DrawLine(castPoint.position, hit1.point, Color.red);
            // Debug.DrawLine(castPoint.position, hit2.point, Color.red);

        } else {
            Debug.DrawLine(castPoint.position, endPos, Color.green);
            // Debug.DrawLine(castPoint.position, endPos2, Color.green);

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
        GetComponent<Rigidbody2D>().isKinematic = false;
        player.GetComponent<MeleeCombat>().currentHealth += 3;

    }
    private void OnTriggerEnter2D(Collider2D other) {
        if(other.CompareTag("PlayerBullet")) {
            sr.material = matWhite;
            TakeDamage(other.GetComponent<bullet>().damage);
        }
    }
}
