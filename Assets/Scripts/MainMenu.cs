using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Spewnity;

public class MainMenu : MonoBehaviour
{
	private GameObject playButtonGO;
	private GameObject rulesButtonGO;
	private GameObject titleGO;
	private GameObject authorGO;
	private GameObject playTextGO;
	private GameObject rulesTextGO;
	private GameObject[] elements;
	private ActionQueue aq;
	private float fadeTime = 1.4f;

	public Text scoreText;

	void Awake()
	{
		playButtonGO = GameObject.Find("/Menu Play Button");
		playTextGO = playButtonGO.GetChild("Label");
		rulesButtonGO = GameObject.Find("/Menu Rules Button");
		rulesTextGO = rulesButtonGO.GetChild("Label");
		titleGO = GameObject.Find("/Menu Title");
		authorGO = GameObject.Find("/Menu Author");
		aq = gameObject.AddComponent<ActionQueue>();
		elements = new GameObject[] {
			titleGO,
			authorGO,
			playButtonGO,
			playTextGO,
			rulesButtonGO,
			rulesTextGO
		};
		Game.loadPrefs();
	}

	void Update()
	{
		if(Input.GetKeyDown(KeyCode.BackQuote))
		{
			Debug.Log("Deleting all prefs");
			PlayerPrefs.DeleteAll();
			Game.loadPrefs();
			updateScore();
		}
	}

	void Start()
	{
		scoreText.text = "";
		TransitionToScene tts = playButtonGO.GetComponent<TransitionToScene>();
		tts.clickEvent.AddListener(() => SoundManager.instance.Stop("theme"));

		// Show long intro ONLY if this is the first scene loaded.
		// Otherwise show the fast intro.
		if(string.IsNullOrEmpty(App.instance.lastScene))
		{
			aq.Delay(0.5f);
			float d = fadeTime;

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
					StartCoroutine(tf.LerpScale(new Vector3(1f, 1f, 1f), d));
					StartCoroutine(sr.color.LerpColor(origColor, d, (v) => sr.color = v));
				});
				aq.PlaySound("drop");
				aq.Delay(d);

				d *= 0.7f;
			}
		}

		aq.Add(() => updateScore());
		aq.Delay(0.35f);
		if(!SoundManager.instance.GetSource("theme").isPlaying)
			aq.PlaySound("theme");
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

