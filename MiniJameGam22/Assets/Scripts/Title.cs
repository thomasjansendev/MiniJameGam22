using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Title : MonoBehaviour
{
    //THIS CLASS IS NO LONGER NEEDED
    
    private Image image;
    
    void Start()
    {
        image = GetComponent<Image>();
    }

    public void ShowTitle()
    {
        image.enabled = true;        
    }

    public void HideTitle()
    {
        image.enabled = false;
    }
}
