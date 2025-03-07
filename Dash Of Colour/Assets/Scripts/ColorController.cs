using UnityEngine;

public class ColorController : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void OnMouseDown()
    {
        Renderer renderer = GetComponent<Renderer>(); 

        if (renderer != null) 
        {
            switch (ColorData.currColor){
                case CustomColor.PinkShade:
                    gameObject.tag = "Bouncy";
                    break;
                case CustomColor.BlueShade:
                    gameObject.tag = "Slightly_Bouncy";
                    break;
                default:
                    gameObject.tag = "Not_Bouncy"; // this tag mainly for documentation purpose
                    break;
            }
            renderer.material.color = ColorData.currColor.ToColor(); 
            
        }
        else
        {
            Debug.LogWarning("No Renderer found on this GameObject!");
        }
    }
}
