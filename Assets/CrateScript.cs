using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrateScript : MonoBehaviour
{
    [SerializeField]
    int health = 20;

    [SerializeField]
    Object destructableRef;

    public Material matWhite, matDefault;
    SpriteRenderer sr;
    // Start is called before the first frame update
    void Start()
    {
        matWhite = Resources.Load("whiteFlash", typeof(Material)) as Material;
        matDefault = new Material(Shader.Find("Sprites/Default"));
        sr = GetComponent<SpriteRenderer>();

    }

    // Update is called once per frame
    void Update()
    {
        if(GameObject.Find("Player") == null) {
            GetComponent<Collider2D>().enabled = true;
            GetComponent<SpriteRenderer>().enabled = true;
        }
    }
    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.tag == "PlayerBullet") {
            health -= other.gameObject.GetComponent<bullet>().damage;
            if(health <= 0) {
                sr.material = matWhite;
                Explode();
            }
            else {
                Invoke("ResetMaterial", .1f);
            }
        }
    }
    void ResetMaterial() {
        sr.material = matDefault;
    }
    private void Explode()
    {
        GameObject destructable = (GameObject)Instantiate(destructableRef);
        destructable.transform.position = transform.position;
        GetComponent<Collider2D>().enabled = false;
        GetComponent<SpriteRenderer>().enabled = false;
    }
}
