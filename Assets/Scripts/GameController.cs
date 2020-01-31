using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    /* Singleton*/
    private static GameController instance;
    public static GameController Instance { get { return instance; } }

    // Class References

    // Public Variables

    // Private Variables
    bool gameRunning;
    bool gameStarted; // when player gets control

    void Awake()
    {
        /* Singleton */
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            DestroyImmediate(gameObject);
        }
        /* End of Singleton */
    }
    // Start is called before the first frame update
    void Start()
    {
        gameRunning = true;
        gameStarted = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (gameRunning)
        {
            // show menu
            ShowMenu(gameRunning);
        }
        if (gameStarted)
        {
            
        }
    }

    // Calls Main Menu
    private void ShowMenu()
    {

    }
}
