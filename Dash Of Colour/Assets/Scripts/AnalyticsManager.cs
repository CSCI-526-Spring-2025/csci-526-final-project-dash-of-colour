using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System;

public class AnalyticsManager : MonoBehaviour
{
    public static AnalyticsManager Instance;
    private long sessionID;
    public int colorChangeCount = 0;
    public float levelStartTime;
    [SerializeField] private string URL;
    // private const string GOOGLE_FORM_URL = "https://docs.google.com/forms/u/0/d/1rM4mBOoHWSpxw4whg-S04rYa1HBynEd0aOD3ff24k4M/previewResponse";
    private const string GOOGLE_FORM_URL = "https://docs.google.com/forms/d/e/1FAIpQLScyfAKp0VpPWfeh1AD49GZqlhq3O3NSKNYMwiPxgNXfHeYSfA/formResponse";

    private int resetsUsed = 0;
    
    private Dictionary<string, int> colorUsage = new Dictionary<string, int>()
    {
        { CustomColor.PinkShade.ToString(), 0 },
        { CustomColor.BlueShade.ToString(), 0 },
        { CustomColor.GreyShade.ToString(), 0 }
    };

    
    private const string SESSION_ID_FIELD = "entry.1437283602";
    // private const string COLOR_CHANGE_FIELD = "entry.14387337";
    private const string COLOR_USAGE = "entry.1221318288";
    private const string LEVEL_TIME_FIELD = "entry.187738284";

    private const string RESETS_USED_FIELD = "entry.1131877411";

    private void Awake()
{
    if (Instance != null && Instance != this)
    {
        Destroy(gameObject);
        return;
    }

    Instance = this;
    DontDestroyOnLoad(gameObject);

    sessionID = DateTime.Now.Ticks;
}


     public void LevelStart()
    {
        // Start tracking level time
        levelStartTime = Time.time;
        // Reset color change count
        // colorChangeCount = 0;

        // resetsUsed = 0;

        colorUsage[CustomColor.PinkShade.ToString()] = 0;
        colorUsage[CustomColor.BlueShade.ToString()] = 0;
        colorUsage[CustomColor.GreyShade.ToString()] = 0;
    }

    //Method to increase resets 
    public void IncrementResetCount()
    {
        resetsUsed++;
        Debug.Log($"Resets used: {resetsUsed}");
    }

    public void IncrementColorChange(string colorUsed)
    {
        if (colorUsage.ContainsKey(colorUsed))
        {
            colorUsage[colorUsed]++;
        }
        else
        {
            // In case you have a color that's not predefined
            colorUsage[colorUsed] = 1;
        }
        Debug.Log($"Updated {colorUsed} count: {colorUsage[colorUsed]}");
    }

    public void LevelComplete()
    {
        float timeTaken = Time.time - levelStartTime;
        Debug.Log("Level completed in " + timeTaken + " seconds");

        //Combining the color usage
        string colorUsageString = "";
        foreach (var kvp in colorUsage)
        {
            colorUsageString += $"{kvp.Key}: {kvp.Value}, ";
        }
        // Remove trailing comma and space
        colorUsageString = colorUsageString.TrimEnd(',', ' ');


        StartCoroutine(PostAnalyticsData(sessionID.ToString(), colorUsageString, resetsUsed,timeTaken));
    }

    
    private IEnumerator PostAnalyticsData( string sessionID, string colorUsageString, int resetsUsed, float levelTime)
    {
        WWWForm form = new WWWForm();
        form.AddField(SESSION_ID_FIELD, sessionID);
        // form.AddField(COLOR_CHANGE_FIELD, colorChanges);
        form.AddField(COLOR_USAGE, colorUsageString);
        form.AddField(RESETS_USED_FIELD, resetsUsed);
        form.AddField(LEVEL_TIME_FIELD, levelTime.ToString("F2"));
        Debug.Log(sessionID);


        using (UnityWebRequest www = UnityWebRequest.Post(GOOGLE_FORM_URL, form))
        {
            // Debug.Log(form);
            yield return www.SendWebRequest();
            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Error sending analytics: " + www.error);
                Debug.LogError("Response: " + www.downloadHandler.text);
            }
            else
            {
                Debug.Log("Analytics data sent successfully!");
            }
        }
    }
}
