using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndLevel : MonoBehaviour
{
    //if player hits end of level, it changes scene
    [SerializeField] private LayerMask WhatIsEnd;
    private Collider2D coll;
    void Awake(){
        coll = GetComponent<Collider2D>();
    }
    void Update()
    {
        if(coll.IsTouchingLayers(WhatIsEnd))
        {
            if(SceneManager.GetActiveScene().name == "tutorialScene") //if you are in the tutorial scene, go to menu
            {
                SceneManager.LoadScene(1);
            }
            else
            {
                if(GameObject.Find("GlobalGameGestion")!= null)
                {
                    Debug.Log(GameObject.Find("GlobalGameGestion").GetComponent<GlobalGameGestion>().hasCompletedGame);
                    GameObject.Find("GlobalGameGestion").GetComponent<GlobalGameGestion>().hasCompletedGame = true;
                }
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1); //for now, it will play credits.
            }
        }
    }
}
