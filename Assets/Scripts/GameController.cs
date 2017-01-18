using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Networking;

[System.Serializable]
public class Difficulty
{
    public float speed = 3;
    public float maxSpeed = 10;
    public float speedMultiplier = 1.25f;
    public float spawnRate = 2;
    public float minSpawnRate = 0.5f;
    public float spawnMultiplier = 0.75f;
    public float raiseInterval = 5;
}

public class GameController : NetworkBehaviour
{
    public GameObject enemy;
    public Vector3 spawnValues;
    public Text scoreText;

    public float startWait;

    public Difficulty difficulty;
    [SyncVar(hook = "OnUpdateScore")]
    private int score;
    private float nextDifficultyRaise;

    void Start()
    {
        if (isServer)
        {
            StartCoroutine(SpawnEnemies());
            nextDifficultyRaise = Time.time + difficulty.raiseInterval;
        }
    }

    IEnumerator SpawnEnemies()
    {
        yield return new WaitForSeconds(startWait);
        
        while (true)
        {
            if (Time.time >= nextDifficultyRaise)
            {
                nextDifficultyRaise += difficulty.raiseInterval;

                if (difficulty.spawnRate > difficulty.minSpawnRate)
                    difficulty.spawnRate *= difficulty.spawnMultiplier;
                else
                    difficulty.spawnRate = difficulty.minSpawnRate;

                if (difficulty.speed < difficulty.maxSpeed)
                    difficulty.speed *= difficulty.speedMultiplier;
                else
                    difficulty.speed = difficulty.maxSpeed;
            }

            var spawnPosition = new Vector3(Random.Range(-spawnValues.x, spawnValues.x), spawnValues.y, spawnValues.z);
            var enemyObject = (GameObject)Instantiate(enemy, spawnPosition, enemy.transform.rotation);
            var enemyMover = enemyObject.GetComponent<EnemyMover>();
            enemyMover.speed = difficulty.speed;
            NetworkServer.Spawn(enemyObject);

            yield return new WaitForSeconds(difficulty.spawnRate);
        }
    }

    public void AddScore(int newScoreValue)
    {
        if (!isServer)
        {
            return;
        }
        score += newScoreValue;
    }

    void OnUpdateScore(int score)
    {
        scoreText.text = "Score: " + score;
    }

    public override void OnStartClient()
    {
        base.OnStartClient();
        OnUpdateScore(score);
    }

    public void GameOver(bool isServerCall)
    {
        if (isServerCall)
        {
            NetworkManager.singleton.StopHost();
        }
        else
        {
            NetworkManager.singleton.StopClient();
        }
    }

    [ClientRpc]
    public void RpcPauseControl()
    {
        if (Time.timeScale == 1)
        {
            Time.timeScale = 0;
        }
        else if (Time.timeScale == 0)
        {
            Time.timeScale = 1;
        }
    }

    public void PauseControl()
    {
        if (isServer)
        {
            RpcPauseControl();
        }
    }
}
