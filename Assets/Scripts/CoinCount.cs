using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class CoinCount : MonoBehaviour
{
    public GameObject player;

    PlayerController playerController;
    public float rotationSpeed = 200f;
    public Respawn respawn;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        playerController = player.GetComponent<PlayerController>();
        respawn = Camera.main.GetComponent<Respawn>();
    }

    // Update is called once per frame
    void Update()
    {
        if(player == null) {
            GetComponent<Collider2D>().enabled = true;
            GetComponent<SpriteRenderer>().enabled = true;
            GetComponent<Collider2D>().isTrigger = true;
            player = GameObject.FindWithTag("Player");
        }
        transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);
    }
    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.tag == "Player" || other.gameObject.tag == "Projectile") {
            respawn.Coins += 1;
            GetComponent<Collider2D>().enabled = false;
            GetComponent<SpriteRenderer>().enabled = false;
            GetComponent<Collider2D>().isTrigger = false;
        }
    }
}
