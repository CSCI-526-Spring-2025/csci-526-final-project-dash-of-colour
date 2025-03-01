using UnityEngine;
using UnityEngine.UI;

public class ColorToggle : MonoBehaviour
{
    private Color[] colors = {
        new Color32(255, 152, 222, 255),   // pink
        new Color32(142, 185, 255, 255),   // blue
        new Color32(128, 128, 128, 255)    // gray
    };

    private int colorIndex = 0;

    public Image colorDisplayImage; 

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ColorData.currColor = colors[colorIndex];
        UpdateColorDisplay();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            colorIndex = (colorIndex + 1) % colors.Length; // Cycle through colors
            ColorData.currColor = colors[colorIndex]; // Set the new color
            Debug.Log("Current Color: " + ColorData.currColor);
            UpdateColorDisplay();
        }
    }

    void UpdateColorDisplay()
    {
        if (colorDisplayImage != null)
        {
            colorDisplayImage.color = ColorData.currColor; // Update the image color
        }
    }
}
