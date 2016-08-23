using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spewnity;

public class CheckInput : MonoBehaviour
{
	public Arrow arrow;

	private bool isDragging = false;
	private float penRadius;
	private Vector2 centerPos;
	private Game game;

	void Awake()
	{
		penRadius = GameObject.Find("Pen").GetComponent<CircleCollider2D>().radius;
		centerPos = (Vector2) GameObject.Find("Pen").transform.position;
		game = GameObject.Find("Game").GetComponent<Game>();
	}

	void Update()
	{
		bool rmb = Input.GetMouseButtonDown(1);
		if(game.gameOver || rmb)
		{
			if(isDragging) unspawnSpider();
			return;
		}

		Vector2 mousePt = Camera.main.ScreenToWorldPoint(Input.mousePosition);

		if(Input.GetMouseButtonDown(0)) // Button pressed
		{
			spawnSpider(mousePt);
		}

		else if(Input.GetMouseButton(0)) // Button held
		{
			if(isDragging)
			{
				if(rmb) unspawnSpider();
				else arrow.redraw(mousePt);
			}
		}

		else if(isDragging) flingSpider(); // Button released
	}

	private void startDragging()
	{
		isDragging = true;
		Cursor.visible = false;
	}

	private void spawnSpider(Vector2 pt)
	{
		// Adjust spider position to edge of pen
		Vector2 snapToBoundary = (pt - centerPos).normalized * penRadius + centerPos;

		// And spawn a panicky spider
		startDragging();
		arrow.show(snapToBoundary);
		scritch();
	}

	private void stopDragging()
	{
		isDragging = false;
		Cursor.visible = true;
	}

	private void unspawnSpider()
	{
		stopDragging();
		arrow.hide(true);
	}

	private void flingSpider()
	{
		stopDragging();
		GameObject go = arrow.hide(false);
		Debug.Assert(go != null);
		Spider spider = go.GetComponent<Spider>();
		spider.fling(arrow.lineLength);
		SoundManager.instance.Play("flick");
	}

	private void scritch()
	{
		if(!isDragging) return;
		SoundManager.instance.Play("scritch", (Sound s) => scritch());
	}
}