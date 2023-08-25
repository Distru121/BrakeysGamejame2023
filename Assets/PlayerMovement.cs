using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public CharacterController2D controller;
    public Rigidbody2D player_rigidbody;
    public Animator animator;

    public float runSpeed = 100f; 
    float HorizontalMove = 0f;
    bool jump = false;
    bool dash = false;
    float dashedTime = 0;

    // Update is called once per frame
    void Update()
    {

        HorizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed; // per prendere un input con i tasti orizzontali ("a" e "d" per default) 

        animator.SetFloat("XSpeed", Mathf.Abs(HorizontalMove));  // Mathf.Abs fa in modo che diventi positivo 
        animator.SetBool("Grounded", controller.m_Grounded);
        animator.SetFloat("YSpeed", player_rigidbody.velocity.y);

        if(Input.GetButtonDown("Jump")) // se è premuto il tasto assegnato a "Jump", la variabile verrà messa in uno stato = true
        {
            jump = true;
            animator.SetBool("IsJumping",true);
        }

        // if the player has not dashed he dashes
        if(Input.GetKeyDown(KeyCode.LeftShift) && !controller.dashed) {
            dash = true;
            dashedTime = Time.time;
        }
    }


    public void OnLanding() // On Landing = atterraggio 
    {
        // è pubblic per fare in modo di vederlo negli eventi del player
        animator.SetBool("IsJumping", false);
    }

    void FixedUpdate()
    {
        // move our character 
        controller.Move(HorizontalMove * Time.fixedDeltaTime, jump, dash);
        jump = false; 
        // the dash lasts 5 frames
        if(Time.time - dashedTime > Time.fixedDeltaTime * 5)
            dash = false;
    }
}