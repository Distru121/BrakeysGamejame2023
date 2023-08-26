using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateCheckpoint : MonoBehaviour
{

    public bool active;
    public Sprite defaultSprite;
    public Sprite activeSprite;

    public LayerMask whatIsPlayer;
    private Collider2D coll;
    private SpriteRenderer thisThingsSprite;

    void Awake(){
        coll = GetComponent<Collider2D>();
        thisThingsSprite = GetComponent<SpriteRenderer>();
    }
    // Update is called once per frame
    void Update()
    {
        //collision with player
        if(coll.IsTouchingLayers(whatIsPlayer))
        {
            if(!active){
                GameObject[] otherCheckpoints;
                otherCheckpoints = GameObject.FindGameObjectsWithTag("checkpoint");
                if(otherCheckpoints.Length != 0)
                {
                    for(int i = 0; i < otherCheckpoints.Length; i++) //iterates through all checkpoints and sets them to inactive
                    {
                        otherCheckpoints[i].GetComponent<ActivateCheckpoint>().active = false;
                    }
                }
                active = true;
            }
        }

        //updates sprite depending on active or not
        if(active)
            thisThingsSprite.sprite = activeSprite;
        else
            thisThingsSprite.sprite = defaultSprite;
    }
}
