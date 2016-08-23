using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Spewnity;
using UnityEngine.SceneManagement;

public class Game : MonoBehaviour
{
	private static int MAX_DEAD_SPIDERS = 3;
	private static float SCORE_SECONDS = 1.0f;
	private static int SCORE_MULTIPLIER = 1;

	public static int lastScore = 0;
	public static int highScore = 0;
	public static float minLineLength = 1.0f;

	public Text scoreText;
	public Text gameOverText;
	public AnimationCurve spiderSpeed = new AnimationCurve(new Keyframe[] {
		new Keyframe(0, 0.2f),
		new Keyframe(1, 3.5f)
	});
	public bool gameOver = true;

	private Life[] lives = new Life[] { null, null, null };
	private int score = 0;
	private int deadSpiders = 0;
	private int liveSpiders = 0;
	private float timeAccrued;
	private ActionQueue aq;

	public static void loadPrefs()
	{
		highScore = PlayerPrefs.GetInt("highScore", 0);
	}

	void Awake()
	{
		int id = 0;
		while(id < MAX_DEAD_SPIDERS) lives[id] = GameObject.Find("/Lives/Life" + (++id)).GetComponent<Life>();
		aq = gameObject.AddComponent<ActionQueue>();
	}

	void Start()
	{
		startGame();
	}

	public void startGame()
	{
		gameOverText.enabled = false;
		liveSpiders = deadSpiders = score = 0;
		updateScoreBox();
		gameOver = false;
	}

	public void spiderBirthed()
	{
		if(gameOver) return;

		liveSpiders++;
	}

	public void spiderDied()
	{
		if(gameOver) return;

		liveSpiders--;
		lives[deadSpiders++].show();
		if(deadSpiders >= MAX_DEAD_SPIDERS) endGame();
	}

	void Update()
	{
		if(gameOver) return;

		timeAccrued += Time.deltaTime;
		if(timeAccrued > SCORE_SECONDS)
		{
			score += liveSpiders * SCORE_MULTIPLIER;
			updateScoreBox();
			timeAccrued -= SCORE_SECONDS;
		}
	}

	public void updateScoreBox()
	{
		scoreText.text = score.ToString();
	}

	public void endGame()
	{
		gameOver = true;
		gameOverText.enabled = true;
		gameOverText.text = "";

		lastScore = score;
		if(lastScore > highScore)
		{
			highScore = lastScore;
			PlayerPrefs.SetInt("highScore", highScore);
		}

		aq.Delay(1f);
		string msg = "GAME OVER";
		int i = 0;
		foreach(char letter in msg)
		{
			aq.Add(() => gameOverText.text += msg[i++]);
			if(letter != ' ')
				aq.PlaySound("click");
			aq.Delay(0.2f);
		}
		aq.Delay(2f);
		aq.Add(() => SceneManager.LoadScene("Main"));
		aq.Run();
	}
}
