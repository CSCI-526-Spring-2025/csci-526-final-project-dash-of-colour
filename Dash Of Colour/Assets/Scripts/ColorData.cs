using UnityEngine;
public enum CustomColor
{
    PinkShade,
    BlueShade,
    GreyShade
}

public static class ColorData 
{
    public static CustomColor currColor = CustomColor.PinkShade;
    public static Color ToColor(this CustomColor customColor)
    {
        switch (customColor)
        {
            case CustomColor.PinkShade:
                return new Color32(255, 152, 222, 255); 
            case CustomColor.BlueShade:
                return new Color32(142, 185, 255, 255); 
            case CustomColor.GreyShade:
                return new Color32(128, 128, 128, 255); 
            default:
                return Color.gray; // Fallback Color
        }
    }
}
