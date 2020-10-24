using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouncePad : MonoBehaviour
{
    public Vector2 bounceForce;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnCollisionEnter2D(Collision2D other) {
        if(other.gameObject.CompareTag("Player") && (other.gameObject.GetComponent<PlayerController>().isJumping || other.gameObject.GetComponent<PlayerController>().isFalling)) {
            Rigidbody2D otherRB = other.gameObject.GetComponent<Rigidbody2D>();
            otherRB.AddForce(bounceForce * otherRB.velocity, ForceMode2D.Impulse);
        }
    }
    float DetermineDirection() {
        var value = 1;
        // if(transform.rotation.z > 0)
        return value;
    }
}
