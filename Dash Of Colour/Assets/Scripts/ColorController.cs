using UnityEngine;

public class ColorController : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void OnMouseDown()
    {
        Renderer renderer = GetComponent<Renderer>(); 

        if (renderer != null) 
        {
            renderer.material.color = ColorData.currColor; 
        }
        else
        {
            Debug.LogWarning("No Renderer found on this GameObject!");
        }
    }
}
