using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] private int numberOfStartItems;
    [SerializeField] private GameObject itemPrefab;

    private WorldPoints points;

    void Start()
    {
        points = GetComponent<WorldPoints>();
        SpawnStartItems();
        InvokeRepeating(nameof(SpawnItem), 0, 0.5f);
    }

    void SpawnStartItems()
    {
        for (int i = 0; i < numberOfStartItems; i++)
        {
            SpawnItem();
        }
    }

    private void SpawnItem()
    {
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