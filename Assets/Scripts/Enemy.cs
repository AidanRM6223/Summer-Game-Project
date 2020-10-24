using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {
    public int maxHealth = 100;
    public GameObject deathEffect;
    int currentHealth;

    public int attackDamage = 10;
    void Start() {
        currentHealth = maxHealth;
    }
    public void TakeDamage (int damage)
    {
        maxHealth -= damage;
        if (maxHealth <= 0)
        {
            Die();
        }
    }
    public void Die() {
        Instantiate(deathEffect, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
    private void OnCollisionEnter2D(Collision2D other) {
        if(other.gameObject.tag == "Player") {
            other.gameObject.GetComponent<MeleeCombat>().TakeDamage(attackDamage);
        }
    }
}