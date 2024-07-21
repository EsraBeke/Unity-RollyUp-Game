using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public bool hasTheGameStart = false;
    public bool hasTheGameEnd = false;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(Instance);
        }
    }
    public void GameDone(int gainedScore)
    {
        hasTheGameEnd = true;
        Debug.Log("Gained Score: " + gainedScore);
    }
    void Start()
    {
        
    }

    
    void Update()
    {
        
    }
}
