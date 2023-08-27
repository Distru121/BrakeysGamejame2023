using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalGameGestion : MonoBehaviour
{
    public static GlobalGameGestion globalGestion;

    public bool hasCompletedGame = false;

    public float timetocomplete = -1;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
}
