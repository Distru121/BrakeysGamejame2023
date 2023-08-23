using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeath : MonoBehaviour
{
    public GameObject deathParticles;

    public void Death() {
        Debug.Log("dead");
        GameObject death = Instantiate(deathParticles, transform.position, Quaternion.identity);
        Destroy(death, 5);
    }
}
