using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSpriteAllocation : MonoBehaviour
{

    [SerializeField] private Sprite[] _sprites;
    private SpriteRenderer _spriteRenderer;

    private int _rngMaxRange;
    
    // Start is called before the first frame update
    void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _rngMaxRange = _sprites.Length;
        ChoseRndSprite();
    }

    private void ChoseRndSprite()
    {
        int rnd = Random.Range(0, _rngMaxRange); // [minInclusive..maxExclusive)
        _spriteRenderer.sprite = _sprites[rnd];
    }
    
}
