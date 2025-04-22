using System;
using UnityEngine;

public static class DummyNameGenerator
{
	private static string[] adjectives = {
		"Swift", "Mighty", "Sneaky", "Glowing", "Happy", "Chill", "Witty", "Brave", "Loyal", "Wild"
	};

	private static string[] nouns = {
		"Falcon", "Panda", "Carrot", "Rocket", "Knight", "Lizard", "Rhino", "Wolf", "Penguin", "Tiger"
	};

	public static string GenerateName()
	{
		string adj = adjectives[UnityEngine.Random.Range(0, adjectives.Length)];
		string noun = nouns[UnityEngine.Random.Range(0, nouns.Length)];
		int number = UnityEngine.Random.Range(1000, 9999);

		return $"{adj}{noun}{number}";
	}
}
