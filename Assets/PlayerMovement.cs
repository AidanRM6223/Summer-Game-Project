using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController2D controller;
    public Animator animator;
    float horizontalMove = 0f;
    public float runSpeed = 40f;
    bool jump = false;
    bool crouch = false;
    // Update is called once per frame
    void Update()
    {
        //Debug.Log(Input.GetAxisRaw("Horizontal"));
       horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;

        animator.SetFloat("Speed",Mathf.Abs(horizontalMove));
       if(Input.GetButtonDown("Jump"))
       {
           jump = true;
           animator.SetBool("IsJumping", true);
       }
       if(Input.GetButtonDown("Crouch")) {
           crouch = true;
       }
       else if (Input.GetButtonUp("Crouch")) {
           crouch = false;
       }
    }
    public void OnCrouching (bool isCrouching) {
        animator.SetBool("IsCrouching", isCrouching);
    }
    public void OnLanding () {
        animator.SetBool("IsJumping", false);
    }
    void FixedUpdate() {
        // Move our character
        controller.Move(horizontalMove * Time.fixedDeltaTime, false, jump);
        jump = false;
    }
}
