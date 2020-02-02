using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    // Public Variables
    public int totalEnemies = 100;
    public int numberOfSpawn = 5;
    public GameObject enemyPrefab;
    public Transform enemyParent;
    public float spawnInterval = 3f;
    public float currentSpawnTime = 0;
    public float bigCountdown = 120; // 120 seconds is 2 minutes
    public float currentBigTime = 0;

    public float minSpawnRadius = 80f;
    public float maxSpawnRadius = 120f;

    // Private Variables
    private PlayerController pController;
    private List<GameObject> enemyPool;

    // Start is called before the first frame update
    void Start()
    {
        enemyPool = new List<GameObject>();
        for(int i = 0; i < totalEnemies; i++)
        {
            //float angle = Random.Range(0, 2 * Mathf.PI);
            //float radius = Random.Range(minSpawnRadius, maxSpawnRadius);
            //Vector3 pos = new Vector3(Mathf.Sin(angle) * radius, 2f, Mathf.Cos(angle) * radius);
            //Vector3 pos = new Vector3(Mathf.Sin(angle) * radius, Mathf.Cos(angle) * radius, 0);
            Vector3 pos = new Vector3(0f, 100f, 0f); // Spawn offworld
            GameObject enemy = Instantiate(enemyPrefab, pos, enemyPrefab.transform.rotation);
            enemy.transform.SetParent(enemyParent);
            enemy.gameObject.SetActive(false);
            enemyPool.Add(enemy);
        }
        pController = PlayerController.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        // Move to position of player for close quarter spawning
        

        // Somewhere I need to include initial spawn

        currentSpawnTime += Time.deltaTime;
        currentBigTime += Time.deltaTime;

        if (currentSpawnTime >= spawnInterval)
        {
            RespawnEnemy(numberOfSpawn);
            currentSpawnTime = 0;
        }

        if (currentBigTime >= bigCountdown && spawnInterval > 1.5f)
        {
            spawnInterval -= .1f;
            currentBigTime = 0;
        }
        //timer += Time.deltaTime;
        //timer = 0f;
        //if (GameController.Instance.IsPlaying())
        //{
        //}
    }

    
    // Handles enemy respawning from enemyPool
    private void RespawnEnemy(int noOfEnemies)
    {
        int counter = 0;
        float angle = 0;
        float radius = 0;
        Vector3 pos;
        Vector3 final;

        foreach (GameObject enemy in enemyPool)
        {
            if (!enemy.gameObject.activeInHierarchy) 
            {
                do
                {
                     angle = Random.Range(0, 2 * Mathf.PI);
                     radius = Random.Range(minSpawnRadius, maxSpawnRadius);
                     pos = new Vector3(Mathf.Sin(angle) * radius, 2f, Mathf.Cos(angle) * radius);
                     final = pos + pController.transform.position;
                } while (!WithinBounds(final));
                
                enemy.gameObject.transform.position = final;
                enemy.gameObject.SetActive(true);
                
                // set spawn position
                counter++;
            }
            if (counter == noOfEnemies)
            {
                break;
            }
        }
        /*
        for (int i = 0; i < noOfEnemies; i++)
        {
            float angle = Random.Range(0, 2 * Mathf.PI);
            float radius = Random.Range(minSpawnRadius, maxSpawnRadius);
            Vector3 pos = new Vector3(Mathf.Sin(angle) * radius, 2f, Mathf.Cos(angle) * radius);
            //Vector3 pos = new Vector3(Mathf.Sin(angle) * radius, Mathf.Cos(angle) * radius, 0);
            Enemy enemy = Instantiate(enemyPrefab, pos, enemyPrefab.transform.rotation);
            enemy.transform.SetParent(enemyParent);
        }
        */
    }

    // Check Enemy position is within player bounds
    private bool WithinBounds(Vector3 position)
    {
        // Upper right corner 350 0 300
        Vector3 rightUpperCorner = new Vector3(350, 0, 300);
        // Lower left corner -350 0 -300
        Vector3 leftLowerCorner = new Vector3(-350, 0, -300);
        // 
        if ((position.x < rightUpperCorner.x && position.z < rightUpperCorner.z) // Upper Right
            && (position.x > leftLowerCorner.x && position.z > leftLowerCorner.z)) // Lower Left 
        {
            return true;
        }
        return false;
    }
}
