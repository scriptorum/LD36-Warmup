using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Spewnity;

public class MainMenu : MonoBehaviour
{
	private GameObject buttonGO;
	private GameObject titleGO;
	private GameObject authorGO;
	private GameObject[] elements;
	private ActionQueue aq;
	private float fadeTime = 1.0f;

	public Text scoreText;

	void Awake()
	{
		buttonGO = GameObject.Find("/Menu Play Button");
		titleGO = GameObject.Find("/Menu Title");
		authorGO = GameObject.Find("/Menu Author");
		aq = gameObject.AddComponent<ActionQueue>();
		elements = new GameObject[] { titleGO, authorGO, buttonGO };
	}

	void Start()
	{
		scoreText.text = "";
		TransitionToScene tts = buttonGO.GetComponent<TransitionToScene>();
		tts.enabled = false;
		float dropTime = 0.65f;
		Debug.Assert(dropTime < fadeTime);

		if(Game.lastScore == 0)
		{
			aq.Delay(0.5f);

			foreach(GameObject element in elements)
			{
				Transform tf = element.transform;
				tf.localScale = new Vector3(4f, 4f, 1f);
				SpriteRenderer sr = element.GetComponent<SpriteRenderer>();
				Color origColor = sr.color;
				Color tempColor = origColor;
				tempColor.a = 0;
				sr.color = tempColor;

				aq.Add(() =>
				{
					StartCoroutine(tf.LerpScale(new Vector3(1f, 1f, 1f), fadeTime));
					StartCoroutine(sr.color.LerpColor(origColor, fadeTime, (v) => sr.color = v));
				});
				aq.Delay(dropTime);
				aq.PlaySound("drop");
				aq.Delay(fadeTime - dropTime);
			}
		}

		aq.Add(() => updateScore());
		aq.Add(() => tts.enabled = true);
		aq.Run();
	}

	public void updateScore()
	{
		string msg = "";

		if(Game.highScore > 0)
		{
			msg += "High Score:\n" + Game.highScore;
			if(Game.lastScore > 0) msg += "\n\n";
		}
		if(Game.lastScore > 0) msg += "Last Score:\n" + Game.lastScore;

		scoreText.text = msg;
	}
}

