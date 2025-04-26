using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using System.Collections;

public class ColorController_ComplexObst : MonoBehaviour
{
    private Renderer objRend;
    private CustomColor origColor;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        objRend = GetComponent<Renderer>();
        origColor = objRend.GetComponent<CustomColor>();
    }

    private void OnMouseDown()
    {
        Renderer renderer = GetComponent<Renderer>();

        if (renderer != null)
        {
            switch (ColorData.currColor)
            {
                case CustomColor.PinkShade:
                    gameObject.tag = "Bouncy";
                    gameObject.GetComponent<Collider>().enabled = true;
                    break;
                case CustomColor.BlueShade:
                    gameObject.tag = "Slightly_Bouncy";
                    gameObject.GetComponent<Collider>().enabled = true;
                    break;
                default:
                    StartCoroutine(graySetter());
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
    IEnumerator graySetter()
    {
        gameObject.tag = "Not_Bouncy"; // this tag mainly for documentation purpose
        gameObject.GetComponent<Collider>().enabled = false;
        yield return new WaitForSeconds(2f); // wait duration
        gameObject.GetComponent<Collider>().enabled = true;
        objRend.material.color = origColor.ToColor();
    }
    }
