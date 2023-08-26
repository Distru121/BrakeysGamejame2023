using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeSpawnerScript : MonoBehaviour
{
    public GameObject[] spikes;

    public float ChanceToSpawn = 0.1f;

    // Start is called before the first frame update
    void Start()
    {
        if(Random.value < ChanceToSpawn){
            GameObject spike = Instantiate(spikes[Random.Range(0, spikes.Length)], transform.position, transform.rotation);
            spike.transform.localScale = transform.localScale;
        }

        Destroy(gameObject); //self destructs
    }
}
