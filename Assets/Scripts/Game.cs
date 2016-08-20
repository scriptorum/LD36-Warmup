using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Game : MonoBehaviour {
	private static int MAX_DEAD_SPIDERS = 3;
	private static float SCORE_SECONDS = 1.0f;
	private static int SCORE_MULTIPLIER = 1;

	public Text scoreUI;
	public AnimationCurve spiderSpeed = new AnimationCurve(new Keyframe[] { new Keyframe(0, 0.2f), new Keyframe(1, 3.5f) });
	public bool gameOver = true;

	private int score = 0;
	private int deadSpiders = 0;
	private int liveSpiders = 0;
	private float timeAccrued;

	void Start()
	{
		startGame();
	}

	public void startGame()
	{
		liveSpiders = deadSpiders = score = 0;
		updateScoreBox();
		gameOver = false;
	}

	public void addSpider()
	{
		if(gameOver)
			return;

		liveSpiders++;
	}

	public void removeSpider()
	{
		if(gameOver)
			return;

		--liveSpiders;
		if(++deadSpiders > MAX_DEAD_SPIDERS)
		{
			Debug.Log("Too many dead spiders! Game over!");
			gameOver = true;
		}
	}

	void Update()
	{
		if(gameOver)
			return;

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
		scoreUI.text = score.ToString();
	}
}
