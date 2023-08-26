using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeSceneScript : MonoBehaviour
{
    public int sceneIndex1 = 1;
    public int sceneIndex2 = 2;
    // Start is called before the first frame update
    public void ChangeScene1()
    {
        SceneManager.LoadScene(sceneIndex1);
    }

    public void ChangeScene2()
    {
        SceneManager.LoadScene(sceneIndex2);
    }
}
