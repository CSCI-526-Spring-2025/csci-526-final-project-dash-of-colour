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
                    gameObject.GetComponent<Collider>().isTrigger = false;
                    break;
                case CustomColor.BlueShade:
                    gameObject.tag = "Slightly_Bouncy";
                    gameObject.GetComponent<Collider>().isTrigger = false;
                    break;
                default:
                    gameObject.tag = "Not_Bouncy"; // this tag mainly for documentation purpose
                    gameObject.GetComponent<Collider>().isTrigger = true;
                    break;
            }
            renderer.material.color = ColorData.currColor.ToColor(); 
            
            AnalyticsManager.Instance.IncrementColorChange(ColorData.currColor.ToString());
        }
        else
        {
            Debug.LogWarning("No Renderer found on this GameObject!");
        }
    }
}
