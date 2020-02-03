using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    /* Singleton*/
    private static GameController instance;
    public static GameController Instance { get { return instance; } }

    // Class References

    // Public Variables
    public TextMeshProUGUI gameTimeText;
    public GameObject pickupBag;
    public int pickupRespawnCount;
    public GameObject gameOverUI;
    public GameObject menuUI;
    public TextMeshProUGUI scoreText;

    // Private Variables
    private float gameTime;
    private int bagsCounter;
    private float respawnTimer;

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
        Time.timeScale = 1;
        gameTimeText.text = "0";
        gameTime = 0.0f;
        respawnTimer = 10.0f;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateText();

        //Respawn Loot
        if (bagsCounter < 50)
            respawnTimer -= Time.deltaTime;
        if (respawnTimer <= 0)
        {
            CreateLoot(pickupRespawnCount);
            respawnTimer = 10.0f;
        }
    }

    public void GameOver()
    {
        Time.timeScale = 0;
        scoreText.text = "Your Time: " + gameTimeText.text;
        gameOverUI.gameObject.SetActive(true);
    }

    public void PlayAgain()
    {
        SceneManager.LoadScene("Game");
    }

    private void UpdateText()
    {
        float hrs, min, sec;
        gameTime = Time.timeSinceLevelLoad;

        hrs = Mathf.Floor(gameTime / 3600);
        min = Mathf.Floor((gameTime % 3600) / 60);
        sec = Mathf.Floor(gameTime) % 60;
        gameTimeText.text = System.String.Format("{0:00}:{1:00}:{2:00}", hrs, min, sec);
    }

    private void CreateLoot(int amount)
    {
        if (bagsCounter >= 50)
            return;

        for (int i = 0; i < amount; i++)
        {
            Vector3 newPosition = new Vector3(Random.Range(-340.0f, 340.0f), 1.52f, Random.Range(-290.0f, 290.0f));
            Vector3 newRotation = Vector3.zero;
            if (bagsCounter < 50)
            {
                GameObject.Instantiate(pickupBag, newPosition, Quaternion.Euler(newRotation));
                bagsCounter++;
            }       
        }
    }

    public void DecreaseBagCount()
    {
        bagsCounter--;
    }

    public void OpenMenu()
    {
        Time.timeScale = 0;
        menuUI.SetActive(true);
        Cursor.visible = true;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
        menuUI.SetActive(false);
        Cursor.visible = false;
    }

    public void BackToMenu()
    {
        menuUI.SetActive(false);
        SceneManager.LoadScene("Main Menu");
    }
}
