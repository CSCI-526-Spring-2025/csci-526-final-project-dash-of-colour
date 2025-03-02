using UnityEngine;
using UnityEngine.UI;

public class ColorToggle : MonoBehaviour
{
    private CustomColor[] colors = {
        CustomColor.PinkShade,   // pink
        CustomColor.BlueShade,   // blue
        CustomColor.GreyShade   // gray
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
            colorDisplayImage.color = ColorData.currColor.ToColor(); // Update the image color
        }
    }
}
