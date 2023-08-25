using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeath : MonoBehaviour
{
    public GameObject deathParticles;
    [SerializeField] private CharacterController2D controller;
    [SerializeField] private Grappling grappleScript;

    public void Death() {
        // instantiates the death
        GameObject death = Instantiate(deathParticles, transform.position, Quaternion.identity);
        // destroys the death
        Destroy(death, 4.95f);

        // if the player is dead he cannot move and grapple
        controller.dead = true;
        grappleScript.dead = true;
        grappleScript.destroyRope();
        
    }
}
