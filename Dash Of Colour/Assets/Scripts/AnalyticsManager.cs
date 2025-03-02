using System.Collections;
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
    
    private const string SESSION_ID_FIELD = "entry.1437283602";
    private const string COLOR_CHANGE_FIELD = "entry.14387337";
    private const string LEVEL_TIME_FIELD = "entry.187738284";

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
        colorChangeCount = 0;
    }

    public void IncrementColorChange()
    {
        colorChangeCount++;
        Debug.Log("Color changed count: " + colorChangeCount);
    }

    public void LevelComplete()
    {
        float timeTaken = Time.time - levelStartTime;
        Debug.Log("Level completed in " + timeTaken + " seconds");
        StartCoroutine(PostAnalyticsData(sessionID.ToString(), colorChangeCount, timeTaken));
    }

    
    private IEnumerator PostAnalyticsData( string sessionID, int colorChanges, float levelTime)
    {
        WWWForm form = new WWWForm();
        form.AddField(SESSION_ID_FIELD, sessionID);
        form.AddField(COLOR_CHANGE_FIELD, colorChanges);
        form.AddField(LEVEL_TIME_FIELD, levelTime.ToString("F2"));
        Debug.Log(sessionID);


        using (UnityWebRequest www = UnityWebRequest.Post(GOOGLE_FORM_URL, form))
        {
            Debug.Log(form);
            yield return www.SendWebRequest();
            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Error sending analytics: " + www.error);
            }
            else
            {
                Debug.Log("Analytics data sent successfully!");
            }
        }
    }
}
