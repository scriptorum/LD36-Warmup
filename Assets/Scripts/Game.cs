using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Game : MonoBehaviour {
	public Text scoreUI;
	public int MAX_DEAD_SPIDERS = 3;
	public AnimationCurve spiderSpeed = new AnimationCurve(new Keyframe[] { new Keyframe(0, 0.2f), new Keyframe(1, 3.5f) });
	public bool gameOver = true;


	private int score = 0;
	private int deadSpiders = 0;

	void Start()
	{
		startGame();
	}

	public void startGame()
	{
		deadSpiders = 0;
		updateScore(0);
		gameOver = false;
	}

	public void addSpider()
	{
		if(gameOver)
			return;
		
		updateScore(score + 1);
	}

	public void removeSpider()
	{
		if(gameOver)
			return;
		
		updateScore(score - 1);
		if(++deadSpiders > MAX_DEAD_SPIDERS)
		{
			Debug.Log("Too many dead spiders! Game over!");
			gameOver = true;
		}
	}

	public void updateScore(int amount)
	{
		score = amount;
		scoreUI.text = amount.ToString();
	}
}
