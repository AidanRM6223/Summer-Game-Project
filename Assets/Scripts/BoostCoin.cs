using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoostCoin : MonoBehaviour
{
    public GameObject player;
    public float boostValue = 5;
    public Respawn respawn;

    PlayerController playerController;
    public float rotationSpeed = 50f;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        playerController = player.GetComponent<PlayerController>();
        respawn = Camera.main.GetComponent<Respawn>();
        
    }
    void Update() {
        transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);
        if(player == null) {
            GetComponent<Collider2D>().enabled = true;
            GetComponent<SpriteRenderer>().enabled = true;
            GetComponent<Collider2D>().isTrigger = true;
            player = GameObject.FindWithTag("Player");
        }
    }
    // Update is called once per frame
    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.tag == "Player") {
            playerController.dashTime += boostValue;
            GetComponent<Collider2D>().enabled = false;
            GetComponent<SpriteRenderer>().enabled = false;
            GetComponent<Collider2D>().isTrigger = false;
        }
    }
}
