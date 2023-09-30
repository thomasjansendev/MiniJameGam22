using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Title : MonoBehaviour
{
    private Image image;
    private TextMeshProUGUI tmPro;
    
    void Start()
    {
        tmPro = GetComponentInChildren<TextMeshProUGUI>();
        image = GetComponent<Image>();
    }

    public void ShowTitle()
    {
        tmPro.enabled = true;
        image.enabled = true;        
    }

    public void HideTitle()
    {
        tmPro.enabled = false;
        image.enabled = false;
    }
}
