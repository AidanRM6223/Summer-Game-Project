using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public Transform firePoint;
    public GameObject bulletPrefab;
    public Animator animator;
    public bool canShoot = true;
    public float delaySeconds = 0.5f;
    PlayerController pc;
    private void Start() {
        pc = gameObject.GetComponent<PlayerController>();
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Space)) {
            Shoot();
        }

    }
    void Shoot() {
        animator.SetBool("Shoot", true);
        if(canShoot) {
            //Shooting Logic
            pc.MC.audioManager.Play("Shoot");
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            bullet.transform.localScale = transform.localScale;
            bullet.GetComponent<bullet>().bulletSpeed = transform.localScale.x * bullet.GetComponent<bullet>().bulletSpeed;
            canShoot = false;
            StartCoroutine(ShootDelay());
        }
        
    }
    IEnumerator ShootDelay() {
        yield return new WaitForSeconds(delaySeconds);
        canShoot = true;
        animator.SetBool("Shoot", false);
    }
}
