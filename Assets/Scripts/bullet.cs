using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullet : MonoBehaviour
{
    public float bulletSpeed = 20f;
    public Rigidbody2D rb;
    
    public int damage = 20;

    public GameObject impactEffect;
    public float delayDestroy = 5f;
    
    // Start is called before the first frame update
    void Start()
    {
        rb.velocity = transform.right * bulletSpeed;
        Destroy(gameObject, delayDestroy);
    }
    void OnTriggerEnter2D(Collider2D other) {
        if(other.tag == "Enemy" || other.CompareTag("Ground") || other.CompareTag("Destroyable")){
            Instantiate(impactEffect, transform.position, transform.rotation);
            Destroy(gameObject);
        }
        else {

        }
        
        
        
    }
}
