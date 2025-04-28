using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System;
using System.Diagnostics;
using System.Collections;

[Serializable]
public class LeaderboardEntry
{
    public string player_name;
    public float time_seconds;
}

[Serializable]
public class LeaderboardData
{
    public List<LeaderboardEntry> data;
}

public class Leaderboard : MonoBehaviour
{
    [SerializeField] private string supabaseUrl = "https://pbzqzzgkpseijrjwickx.supabase.co";
    [SerializeField] private string supabaseKey = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJzdXBhYmFzZSIsInJlZiI6InBienF6emdrcHNlaWpyandpY2t4Iiwicm9sZSI6ImFub24iLCJpYXQiOjE3NDUzMDEyMDUsImV4cCI6MjA2MDg3NzIwNX0.XbCmXzMEyEMrGNgpomrP-Hy8rxx1th78aoAp5KmTse4";

    public void SubmitScore(string level, string playerName, float time)
    {
        string json = $"{{\"level_name\": \"{level}\", \"player_name\": \"{playerName}\", \"time_seconds\": {time}}}";
        StartCoroutine(PostScore(json));
    }

    IEnumerator PostScore(string json)
    {
        using UnityWebRequest www = new UnityWebRequest(supabaseUrl + "/rest/v1/leaderboard", "POST");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(json);
        www.uploadHandler = new UploadHandlerRaw(bodyRaw);
        www.downloadHandler = new DownloadHandlerBuffer();
        www.SetRequestHeader("Content-Type", "application/json");
        www.SetRequestHeader("apikey", supabaseKey);
        www.SetRequestHeader("Authorization", "Bearer " + supabaseKey);

        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
            UnityEngine.Debug.LogError($"Error uploading score: " + www.error);
        //Debug.Log($"Resets used: {resetsUsed}");
        else
            UnityEngine.Debug.Log($"Score submitted!");
    }

    public void FetchTopScores(string level, Action<List<LeaderboardEntry>> callback)
    {
        StartCoroutine(GetTopScores(level, callback));
    }

    public void FetchCurrentPosition(string level, float time, Action<int> callback)
    {
        StartCoroutine(PredictPlayerRank(level, time, callback));
    }

    IEnumerator GetTopScores(string level, Action<List<LeaderboardEntry>> callback)
    {
        string url = $"{supabaseUrl}/rest/v1/leaderboard?level_name=eq.{level}&order=time_seconds.asc&limit=5";

        UnityWebRequest www = UnityWebRequest.Get(url);
        www.SetRequestHeader("apikey", supabaseKey);
        www.SetRequestHeader("Authorization", "Bearer " + supabaseKey);

        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            UnityEngine.Debug.LogError("Error fetching leaderboard: " + www.error);
            callback?.Invoke(new List<LeaderboardEntry>());
        }
        else
        {
            string json = "{\"data\":" + www.downloadHandler.text + "}";
            var entries = JsonUtility.FromJson<LeaderboardData>(json).data;
            callback?.Invoke(entries);
        }
    }

    IEnumerator PredictPlayerRank(string level, float time, Action<int> callback)
    {
        string url = $"{supabaseUrl}/rest/v1/leaderboard?level_name=eq.{level}&order=time_seconds.asc";

        UnityWebRequest www = UnityWebRequest.Get(url);
        www.SetRequestHeader("apikey", supabaseKey);
        www.SetRequestHeader("Authorization", "Bearer " + supabaseKey);

        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            UnityEngine.Debug.LogError("Error fetching leaderboard: " + www.error);
            callback?.Invoke(-1);
        }
        else
        {
            string json = "{\"data\":" + www.downloadHandler.text + "}";
            var entries = JsonUtility.FromJson<LeaderboardData>(json).data;

            int position = 1;
            foreach (var entry in entries)
            {
                if (time < entry.time_seconds)
                {
                    callback?.Invoke(position);
                    yield break;
                }
                position++;
            }

            // If time is greater than all existing scores, you would be at the end
            callback?.Invoke(position);
        }
    }


    //IEnumerator GetPlayerRankByName(string level, string playerName, Action<int> callback)
    //{
    //    string url = $"{supabaseUrl}/rest/v1/leaderboard?level_name=eq.{level}&order=time_seconds.asc";

    //    UnityWebRequest www = UnityWebRequest.Get(url);
    //    www.SetRequestHeader("apikey", supabaseKey);
    //    www.SetRequestHeader("Authorization", "Bearer " + supabaseKey);

    //    yield return www.SendWebRequest();

    //    if (www.result != UnityWebRequest.Result.Success)
    //    {
    //        UnityEngine.Debug.LogError("Error fetching leaderboard: " + www.error);
    //        callback?.Invoke(-1);
    //    }
    //    else
    //    {
    //        string json = "{\"data\":" + www.downloadHandler.text + "}";
    //        var entries = JsonUtility.FromJson<LeaderboardData>(json).data;

    //        int position = 1;
    //        foreach (var entry in entries)
    //        {
    //            if (entry.player_name == playerName)
    //            {
    //                callback?.Invoke(position);
    //                yield break;
    //            }
    //            position++;
    //        }

    //        // Player name not found
    //        callback?.Invoke(-1);
    //    }
    //}

}