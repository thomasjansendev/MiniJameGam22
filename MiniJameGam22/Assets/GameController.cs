using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

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

    void Start()
    {
        gameState = GameState.NotStarted;
        points = GetComponent<WorldPoints>();
        SpawnStartItems();
        player = GameObject.FindGameObjectWithTag("Player");
        playerController = player.GetComponent<PlayerController>();
        title = GameObject.FindGameObjectWithTag("Title").GetComponent<Title>();
        title.ShowTitle();
    }
    
    private void Update()
    {
        if(gameState != GameState.NotStarted)
            return;
        
        if (playerController.GameStartPressed)
        {
            GameStart();
            gameState = GameState.Play;
        }

    }

    private void GameStart()
    {
        title.HideTitle();
        InvokeRepeating(nameof(SpawnItem), 0, spawnRate);
        player.GetComponent<PlayerController>().enabled = true;

        foreach (var obj in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            obj.GetComponent<EnemyPathfinding>().target = PathfindingTarget.Waypointing;
        }
        print("Game Started");
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