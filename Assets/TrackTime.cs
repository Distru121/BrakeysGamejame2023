using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackTime : MonoBehaviour
{
    GlobalGameGestion timeScript; 
    // Start is called before the first frame update
    void Start()
    {
        if(GameObject.Find("GlobalGameGestion") != null)
            timeScript = GameObject.Find("GlobalGameGestion").GetComponent<GlobalGameGestion>();
    }

    // Update is called once per frame
    void Update()
    {
        if(timeScript != null)
            timeScript.timetocomplete += Time.deltaTime; //each frame, update time
    }
}
