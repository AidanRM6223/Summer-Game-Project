using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public Rigidbody2D rb;

    public float runSpeed, jumpForce, initialRunSpeed, dashSpeed, dashTime, startDashTime, gravityStore, wallJumpTime=0.1f, wallJumpCounter=0.4f, wallJumpForce=50f;

    public GameObject dashEffect;
    public GameObject[] boostEffect;
    public DashBar dashBar;
    public GameObject lightningEffectUI;
    public SpriteRenderer boostAura;
    //public AfterImage afterImage;
    public bool dashBoost;
    public Transform wallGrabPoint;
    private bool canGrab, isGrabbing;
    public KeyMapping KeyMapping;
    public Text coinText;
    public Respawn respawn;
    public Text livesText;

    public Transform groundCheckPoint;
    public LayerMask whatIsGround;

    public bool isGrounded;
    public bool isJumping;
    public bool isFalling = false;
    public Animator animator;
    private CapsuleCollider2D cc;
    public MeleeCombat MC;
    private Vector2 colliderSize;
    public Transform m_currMovingPlatform;

    [SerializeField]
    private PhysicsMaterial2D noFriction;
    [SerializeField]
    private PhysicsMaterial2D fullFriction;
    public int BoostJumps = 2;
    bool groundGravity, isOnGround;
    public float groundCheckPointEnd = 1.5f;
    public float maxSlopeAngle = 80;
    public float maxDecsendAngle = 75;
    public CollisionInfo collisions;
    // Start is called before the first frame update
    void Start()
    {
        
        respawn = Camera.main.GetComponent<Respawn>(); 
        var Canvas = GameObject.Find("Canvas");
        dashBar = Canvas.GetComponentInChildren<DashBar>();
        runSpeed = initialRunSpeed;
        rb = GetComponent<Rigidbody2D>();
        dashBar.SetMaxDash(startDashTime);
        dashTime = startDashTime;
        gravityStore = rb.gravityScale;
        dashBoost = false;
        cc = GetComponent<CapsuleCollider2D>();
        colliderSize = cc.size;
        MC = GetComponent<MeleeCombat>();
        lightningEffectUI = GameObject.Find("LightningEffect");
        coinText = GameObject.Find("CoinCount").GetComponent<Text>();
        livesText = GameObject.Find("LifeCount").GetComponent<Text>();
        transform.position = respawn.lastCheckpoint.position;
    }


    // Update is called once per frame
    void Update()
    {
        if(wallJumpCounter <= 0)
        {
            //Dashing
            if(dashBoost)
            {
                boostEffect[0].SetActive(true);
                boostEffect[1].SetActive(true);                
                animator.SetBool("DashAttack", true);
                // afterImage.makeAfterImage = true;
                //lightningEffectUI.SetActive(true);
                boostAura.enabled = true;
                if(dashTime <= 0){
                        //rb.velocity = Vector2.zero;
                        runSpeed = initialRunSpeed;
                        dashBoost = false;
                        
                }
                else {
                    dashTime -= Time.deltaTime;
                    runSpeed = dashSpeed;
                    if(dashTime > startDashTime) {
                        dashTime = startDashTime;
                    }
                }
            }
            else {
                boostEffect[0].SetActive(false);
                boostEffect[1].SetActive(false);                 
                runSpeed = initialRunSpeed;
                animator.SetBool("DashAttack", false);
                // afterImage.makeAfterImage = false;
                boostAura.enabled = false;
                //lightningEffectUI.SetActive(false);
            }
            dashBar.SetDashValue(dashTime);

            if (Input.GetKeyDown(KeyCode.X)) {
                if(Input.GetKey(KeyCode.LeftArrow)||Input.GetKey(KeyCode.RightArrow)){
                    if(dashTime > 0) {
                        GameObject dashEffectObj = Instantiate(dashEffect, transform.position, transform.rotation);
                        Destroy(dashEffectObj, 1f);
                        dashBoost = true;
                        MC.audioManager.Play("Boost");
                    }
                    
                }
                else if(Input.GetKey(KeyCode.UpArrow) && BoostJumps > 0)
                {
                    var tempVelo = rb.velocity.y;
                    rb.AddForce(new Vector2(rb.velocity.x, dashSpeed -tempVelo), ForceMode2D.Impulse);
                    GameObject dashEffectObj = Instantiate(dashEffect, transform.position, transform.rotation);
                        Destroy(dashEffectObj, 1f);
                    BoostJumps--;
                    MC.audioManager.Play("Boost");
                    animator.Play("BoostJump");
                    transform.parent = null;
                }
                else if(Input.GetKeyUp(KeyCode.LeftArrow) && Input.GetKeyUp(KeyCode.RightArrow) && Input.GetKey(KeyCode.X))
                {
                    dashBoost = false;
                }
            }
            else if(Input.GetKeyUp(KeyCode.X)){
                dashBoost = false;
            }
            
            rb.velocity = new Vector2(Input.GetAxisRaw("Horizontal") * runSpeed, rb.velocity.y);

            isGrounded = Physics2D.Linecast(groundCheckPoint.position, new Vector2(groundCheckPoint.position.x, groundCheckPoint.position.y - groundCheckPointEnd), whatIsGround);
            Debug.DrawLine(groundCheckPoint.position, new Vector2(groundCheckPoint.position.x, groundCheckPoint.position.y - groundCheckPointEnd), Color.blue);
            if (Input.GetKeyDown(KeyCode.UpArrow) && isOnGround && isGrounded)
            {
                rb.constraints = RigidbodyConstraints2D.FreezeRotation;
                BoostJumps = 2;
                groundGravity = false;
                Jump();
                
            }
            else if(!Input.GetKey(KeyCode.LeftArrow) && !Input.GetKey(KeyCode.RightArrow) && isOnGround && isGrounded && !isJumping && !isFalling) {
                rb.constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
            } else if((isOnGround && isGrounded) || (isJumping || isFalling)){
                rb.constraints = RigidbodyConstraints2D.FreezeRotation;
            }
            //flip direction
            if (rb.velocity.x > 0)
            {
                transform.localScale = Vector3.one;
            }
            else if (rb.velocity.x < 0)
            {
                transform.localScale = new Vector3(-1f, 1, 1f);
            }

            //handle wall jumping
            canGrab = Physics2D.OverlapCircle(wallGrabPoint.position, .2f, whatIsGround);
            isGrabbing = false;
            if(canGrab && !isGrounded) {
                if((transform.localScale.x == 1f && Input.GetKey(KeyCode.RightArrow)) || (transform.localScale.x == -1f && Input.GetKey(KeyCode.LeftArrow))) {
                    isGrabbing = true;
                }
            }
            if(isGrabbing) {
                dashBoost = false;
                rb.gravityScale = 0;
                rb.velocity = Vector2.zero;
                if(Input.GetButtonDown("Jump")) {
                    wallJumpCounter = wallJumpTime;
                    rb.velocity = new Vector2(-Input.GetAxisRaw("Horizontal") * runSpeed, jumpForce);
                    rb.gravityScale = gravityStore;
                    animator.Play("Jump");
                    MC.audioManager.Play("Jump");
                    isGrabbing = false;
                }
            } else {
                rb.gravityScale = gravityStore;
            }

            if(rb.velocity.y <= 0.0f) {
                isJumping = false;
                isFalling = true;
                if(isGrounded && isOnGround)
                    DescendSlope();
            }
            if(rb.velocity.y > 0.0f) {
                isFalling = false;
                rb.constraints = RigidbodyConstraints2D.FreezeRotation;
            }
            if(isGrounded) {
                isFalling = false;
                animator.SetBool("isJumping", false);
                groundGravity = true;
            }
            if(groundGravity) {
                var targetCast = Physics2D.Linecast(groundCheckPoint.position, new Vector2(groundCheckPoint.position.x - groundCheckPointEnd, groundCheckPoint.position.y - groundCheckPointEnd), 1<<LayerMask.NameToLayer("Ground"));

                Debug.DrawLine(groundCheckPoint.position, new Vector2(groundCheckPoint.position.x + groundCheckPointEnd * transform.localScale.x, groundCheckPoint.position.y - groundCheckPointEnd), Color.black);

                var angle = Vector3.SignedAngle(transform.position, targetCast.normal, Vector2.up);
                Vector2 slopeCheckPos = new Vector2(groundCheckPoint.position.x, groundCheckPoint.position.y + 0.5f);
                var hit = Physics2D.Raycast(slopeCheckPos, Vector2.right * transform.localScale.x, 2f, whatIsGround);

                

                Debug.DrawRay(slopeCheckPos, Vector2.right * transform.localScale.x, Color.red);
                float slopeAngleVal = Vector2.Angle(hit.normal, Vector2.up);
                if(isGrounded)
                {
                    if(hit)
                    {
                        Debug.Log(hit.collider.name);
                        Debug.DrawRay(slopeCheckPos, Vector2.right * transform.localScale.x, Color.green);
                        
                        if(slopeAngleVal <= maxSlopeAngle && slopeAngleVal != 0 && !Input.GetKey(KeyCode.UpArrow) && !collisions.DescendingSlope) {
                            ClimbSlope(slopeAngleVal);
                        }
                        else if(slopeAngleVal <= maxSlopeAngle && ((!Input.GetKey(KeyCode.RightArrow) && !Input.GetKey(KeyCode.LeftArrow)) && !isJumping)) { 
                                print("Not moving on slope");
                                rb.constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;}
                        print(slopeAngleVal);
                    }
                    if((Input.GetKey(KeyCode.RightArrow) ||Input.GetKey(KeyCode.LeftArrow)) && !isJumping)
                    {
                        
                        if(isOnGround && isGrounded) {
                            
                            // rb.constraints = RigidbodyConstraints2D.FreezeRotation;
                        }
                        else if(isOnGround){
                            if(slopeAngleVal <= maxSlopeAngle && collisions.climbingSlope && ((!Input.GetKey(KeyCode.RightArrow) && !Input.GetKey(KeyCode.LeftArrow)) && !isJumping)) { 
                                //rb.constraints = RigidbodyConstraints2D.FreezeAll;
                            }
                            else {
                                rb.constraints = RigidbodyConstraints2D.FreezeRotation;
                            }
                        }
                        
                        

                        
                    } /* else if (!isJumping && isOnGround){
                        rb.constraints = RigidbodyConstraints2D.FreezePosition | RigidbodyConstraints2D.FreezeRotation;
                    } */
                    
                }
            }
            coinText.text = "x " + respawn.Coins;
            livesText.text = "x " + respawn.Lives;
            animator.SetFloat("Speed", Mathf.Abs(rb.velocity.x));
            animator.SetBool("isGrounded", isGrounded);
            animator.SetFloat("DashTime", Mathf.Abs(dashTime));
            animator.SetBool("isGrabbing", isGrabbing);
            animator.SetBool("isJumping", isJumping);
            animator.SetBool("isFalling", isFalling);
        }
        else {
            wallJumpCounter -= Time.deltaTime;
        }
    }

    private void ClimbSlope(float slopeAngle)
    {
        Vector2 velocity = rb.velocity;
        float moveDistance = Mathf.Abs(rb.velocity.x);

        float climbVelocityY = Mathf.Sin(slopeAngle * Mathf.Deg2Rad) * moveDistance;
        collisions.below = true;
        if(velocity.y <= climbVelocityY) {
            collisions.climbingSlope = true;
            velocity.y = climbVelocityY;
            velocity.x = Mathf.Cos(slopeAngle * Mathf.Deg2Rad) * moveDistance * Mathf.Sign(velocity.x);
            collisions.slopeAngle = slopeAngle;
            rb.velocity = velocity;
        }
        else {
            collisions.climbingSlope = false;
        }
        
        
    }
    private void DescendSlope() {
        Vector2 velocity = rb.velocity;
        float directionX = Mathf.Sign(velocity.x);
        Vector2 rayOrigin = (directionX == -1)? new Vector2(cc.bounds.min.x - 1, cc.bounds.min.y) : new Vector2 (cc.bounds.max.x + 1, cc.bounds.min.y);
        RaycastHit2D hit = Physics2D.Raycast(rayOrigin, -Vector2.up, Mathf.Infinity, whatIsGround);
        Debug.DrawRay(rayOrigin, Vector2.down, Color.magenta);
        if(hit)
        {
            float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);
            print(slopeAngle);
            if(slopeAngle!=0 && slopeAngle <= maxDecsendAngle) {
                if(Mathf.Sign(hit.normal.x) == directionX) {
                    if(hit.distance - 0.015f <= Mathf.Tan(slopeAngle * Mathf.Deg2Rad) * Mathf.Abs(velocity.x)) {
                        float moveDistance = Mathf.Abs(velocity.x);
                        float descendVelocityY = Mathf.Sin(slopeAngle * Mathf.Deg2Rad) * moveDistance;
                        velocity.x = Mathf.Cos(slopeAngle * Mathf.Deg2Rad) * moveDistance * Mathf.Sign(velocity.x);
                        velocity.y -= velocity.x;
                        collisions.slopeAngle = slopeAngle;
                        collisions.DescendingSlope = true;
                        rb.velocity = velocity;
                    }
                }
            }
            else {
                collisions.DescendingSlope = false;
            }
        }
    }
    private void FixedUpdate() {
    }
    public void Jump() {
        MC.audioManager.Play("Jump");
        isJumping = true;
        animator.SetBool("isJumping", true);
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        if(collisions.climbingSlope)
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y + jumpForce);
        else {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
        
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if(other.gameObject.layer == LayerMask.NameToLayer("Hazard")) {
            MC.TakeDamage(10);
            MC.flashAnim.Play("HurtFlash", -1, 0f);
            MC.flashAnim.GetComponent<CanvasGroup>().alpha = 1;
            print("Hurt flash");
        }
        if(other.gameObject.layer == LayerMask.NameToLayer("Enemy") && !dashBoost) {
            MC.TakeDamage(10);
            MC.flashAnim.Play("HurtFlash", -1, 0f);
            MC.flashAnim.GetComponent<CanvasGroup>().alpha = 1;
            print("Hurt flash");
        }
        else if(other.gameObject.layer == LayerMask.NameToLayer("Enemy") && dashBoost) {
            MC.audioManager.Play("EnemyExplode");
        }
        if(other.gameObject.tag == "MovingPlatform") {
            m_currMovingPlatform = other.gameObject.transform;
            transform.SetParent(m_currMovingPlatform);
            isOnGround = true;
        }
        if(other.gameObject.tag == "Ground") {
            isOnGround = true;
        }
    }
    private void OnCollisionExit2D(Collision2D other) {
        if(other.gameObject.tag == "MovingPlatform") {
            m_currMovingPlatform = null;
            transform.parent = null;
            isOnGround = false;
        }
        if(other.gameObject.tag == "Ground") {
            isOnGround = false;
        }
    }
    private void OnTriggerEnter2D(Collider2D other) {
        if(other.tag == "DeathZone") {
            MC.Die();
        }
        if(other.tag == "Checkpoint") {
            GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Respawn>().lastCheckpoint = other.transform;
            MC.currentHealth = MC.maxHealth;
            MC.audioManager.Play("Checkpoint");
            dashTime = startDashTime;
            other.enabled = false;
        }
        if(other.tag == "Portal") {
            MC.flashAnim.GetComponent<CanvasGroup>().alpha = 1;
            MC.audioManager.Play("Teleport");
            MC.flashAnim.Play("PortalFlash", -1, 0f);
            print("Portal flash");
        }
        if(other.gameObject.GetComponent<BoostCoin>() != null || other.gameObject.GetComponent<CoinCount>() != null) {
            MC.audioManager.Play("Coin");
        }
        if(other.gameObject.name == "Goal") {
            respawn.LevelComplete = true;
        }
    }
    public struct CollisionInfo {
		public bool above, below;
		public bool left, right;

		public bool climbingSlope, DescendingSlope;
		public float slopeAngle, slopeAngleOld;

		public void Reset() {
			above = below = false;
			left = right = false;
			climbingSlope = false;
            DescendingSlope = false;
			slopeAngleOld = slopeAngle;
			slopeAngle = 0;
		}
	}
}
