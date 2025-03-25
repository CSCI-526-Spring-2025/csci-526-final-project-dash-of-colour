using System;
using UnityEngine;
using UnityEngine.UI;
using System.Diagnostics; // Add this for Stopwatch

public class TimerController : MonoBehaviour
{
	public static TimerController instance;
	public Text timeCounter;
	private Stopwatch stopwatch;
	private String timeDisplayText = "Time: 00:00.00";

    private void Awake()
	{
		instance = this;
	}

	private void Start()
	{
		timeCounter.text = timeDisplayText;
		stopwatch = new Stopwatch();
	}

	public void BeginTimer()
	{
		stopwatch.Reset();
		stopwatch.Start();
	}

	public void EndTimer()
	{
		stopwatch.Stop();
		UpdateDisplay();  // Final update to show last time.
	}

	private void Update()
	{
		if (stopwatch.IsRunning)
		{
			UpdateDisplay();
		}
	}

	private void UpdateDisplay()
	{
		TimeSpan timePlaying = stopwatch.Elapsed;
		string timePlayingStr = "Time: " + timePlaying.ToString(@"mm\:ss\.ff");
		timeCounter.text = timePlayingStr;
	}

	public void PlayerWon()
	{
			EndTimer();
	}

    public string GetFinalTime()
    {
        TimeSpan timePlaying = stopwatch.Elapsed;
        return "Time: " + timePlaying.ToString(@"mm\:ss\.ff");
    }


}

