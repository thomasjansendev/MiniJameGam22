using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Serialization;

public class ScoreUIHandler : MonoBehaviour
{
    private ChestContentManager _chestContent;
    private TextMeshProUGUI _guiText;
    private int _score;

    // Start is called before the first frame update
    void Start()
    {
        _guiText = GetComponent<TextMeshProUGUI>();
        _chestContent = GameObject.FindGameObjectWithTag("Chest").GetComponent<ChestContentManager>();
    }
    private void Update()     
    {
        _guiText.text = "Score: " + _chestContent.ItemQuantityInChest;
    }
    
}
