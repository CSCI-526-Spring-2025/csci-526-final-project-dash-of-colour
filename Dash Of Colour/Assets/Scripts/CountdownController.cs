using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CountdownController : MonoBehaviour
{
	public int countdownTime = 3;
	public Text countdownDisplay;

	private void Start()
	{
		countdownTime = 3;
		StartCoroutine(CountdownToStart());
	}

	IEnumerator CountdownToStart()
	{
		Debug.Log("Started countdown");
		while (countdownTime > 0)
		{
            countdownDisplay.text = countdownTime.ToString();
            yield return new WaitForSecondsRealtime(1.0f);
            countdownTime--;
		}
		countdownDisplay.text = "GO!";
		GameManager.instance.StartGame(); // Start the game for all objects
		yield return new WaitForSecondsRealtime(0.1F);
		countdownDisplay.gameObject.SetActive(false);
	}
}