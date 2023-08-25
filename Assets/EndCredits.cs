using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndCredits : MonoBehaviour
{
    public int menuSceneIndex;

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Return)) //can skip the credits if you press enter
        {
            BackToMenu();
        }

        if(transform.position.y <= -300) //if credits ended, return to menu
        {
            BackToMenu();
        }
    }

    void BackToMenu()
    {
        SceneManager.LoadScene(menuSceneIndex);
    }
}
