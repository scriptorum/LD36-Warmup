using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spewnity;

public class CheckInput : MonoBehaviour
{
	public Arrow arrow;

	private bool isDragging = false;
	private float penRadius;
	private Game game;

	void Awake()
	{
//		arrow = GameObject.Find("/Arrow").GetComponent<Arrow>();

		penRadius = GameObject.Find("Pen").GetComponent<CircleCollider2D>().radius;
		game = GameObject.Find("Game").GetComponent<Game>();
	}

	void Update()
	{
		if(game.gameOver)
		{
			if(isDragging) stopDrag();
			return;
		}

		Vector2 mousePt = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		float radius = Vector2.Distance(transform.position, mousePt);
		bool mouseHeld = Input.GetMouseButton(0);
		bool rmbCheck = Input.GetMouseButtonDown(1);

		if(rmbCheck)
		{
			stopDrag();
			return;
		}


		// In pen
		if(radius < penRadius)
		{
			if(isDragging)
			{
				if(mouseHeld) arrow.redraw(mousePt);
				else stopDrag(false);
			}
		}

		// Beyond pen
		else
		{
			if(isDragging)
			{
				if(mouseHeld) arrow.redraw(mousePt);
				else stopDrag();
			}
			else if(mouseHeld) startDrag(mousePt);
		}
	}

	private void startDrag(Vector2 pt)
	{
		isDragging = true;
		Cursor.visible = false;
		arrow.show(pt);
		scritch();
	}

	private void stopDrag(bool killSpider = true)
	{
		isDragging = false;
		Cursor.visible = true;
		GameObject go = arrow.hide(killSpider);

		if(go != null)
		{
			// Fling spider
			Spider spider = go.GetComponent<Spider>();
			spider.fling(arrow.lineLength);
			SoundManager.instance.Play("flick");
		}
	}

	private void scritch()
	{
		if(!isDragging)
			return;
		SoundManager.instance.Play("scritch", (Sound s) => scritch());
	}
}
