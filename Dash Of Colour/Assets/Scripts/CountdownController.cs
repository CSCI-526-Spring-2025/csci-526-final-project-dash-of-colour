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
		StartCoroutine(CountdownToStart());
	}

	IEnumerator CountdownToStart()
	{
		while(countdownTime > 0)
		{
			countdownDisplay.text = countdownTime.ToString();

			yield return new WaitForSeconds(1F);

			countdownTime--;
		}

		countdownDisplay.text = "GO!";

		GameManager.instance.StartGame(); // Start the game for all objects


		yield return new WaitForSeconds(0.1F);

		countdownDisplay.gameObject.SetActive(false);
	}
}