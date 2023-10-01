using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;
using System.Collections;
using UnityEngine.SceneManagement;

public enum GameState
{
    Play,
    NotStarted,
    GameOver
}

public class GameController : MonoBehaviour
{
    [SerializeField] private int numberOfStartItems;
    [SerializeField] private GameObject itemPrefab;
    [SerializeField] private int maxNumberOfItems;
    [SerializeField] private float spawnRate;

    private WorldPoints points;
    [NonSerialized] public GameState gameState;
    private GameObject player;
    private Title title;
    private bool _gameStartPressed;
    private PlayerController playerController;
    [SerializeField] private Timer timer;

    [SerializeField] private GameObject scoreGUI;
    [SerializeField] private GameObject endGameGUI;

    private Scene scene;
    
    void Start()
    {
        gameState = GameState.NotStarted;
        points = GetComponent<WorldPoints>();
        SpawnStartItems();
        player = GameObject.FindGameObjectWithTag("Player");
        playerController = player.GetComponent<PlayerController>();
        title = GameObject.FindGameObjectWithTag("Title").GetComponent<Title>();
        title.ShowTitle();
        scene = SceneManager.GetActiveScene();
        scoreGUI.SetActive(false);
        endGameGUI.SetActive(false);
    }
    
    private void Update()
    {
        //check if game is not started and whether the player has pressed space bar to start the game
        if (gameState == GameState.NotStarted && playerController.GameStartPressed)
        {
            GameStart();
            gameState = GameState.Play;
        }

        // check the remaining time 
        if (gameState == GameState.Play && timer.IsGameOver)
        {
            GameEnd();
            gameState = GameState.GameOver;
        }

        if (gameState == GameState.GameOver && playerController.GameStartPressed)
        {
            RestartGame();
        }

    }

    private void RestartGame()
    {
        SceneManager.LoadScene(scene.name);
    }

    private void GameStart()
    {
        title.HideTitle();
        InvokeRepeating(nameof(SpawnItem), 0, spawnRate);
        playerController.enabled = true;
        scoreGUI.SetActive(true);
        timer.SetStartTime();

        foreach (var obj in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            obj.GetComponent<EnemyPathfinding>().target = PathfindingTarget.Waypointing;
        }
        print("Game Started");
    }
    
    private void GameEnd()
    {
        endGameGUI.SetActive(true);
        scoreGUI.SetActive(false);
        playerController.GameStartPressed = false; //reset the button to start the game
    }

    private void SpawnStartItems()
    {
        for (int i = 0; i < numberOfStartItems; i++)
        {
            SpawnItem();
        }
    }

    private void SpawnItem()
    {
        if (GameObject.FindGameObjectsWithTag("Item").Length > maxNumberOfItems)
        {
            return;
        }

        for (int i = 0; i < 100; i++)
        {
            Vector2 tryItemPos = points.GenerateRandomPath()[0]; // just want one point, not a path
            if (points.CollisionAtPoint(tryItemPos))
            {
                Instantiate(itemPrefab, tryItemPos, Quaternion.identity);
                return;
            }
        }

        throw new("Not able to generate items");
    }

    
    
    
}