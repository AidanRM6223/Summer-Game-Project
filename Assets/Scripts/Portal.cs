using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    public GameObject telePortal;
    public GameObject Player;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
    }
    public void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.tag == "Player") {
            Teleport(); 
        }
    }
    void Teleport() {
        Player.transform.position = new Vector2(telePortal.transform.position.x + Player.transform.localScale.x,telePortal.transform.position.y);

    }
}
