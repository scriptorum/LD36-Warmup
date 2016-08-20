using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckInput : MonoBehaviour
{
	public Arrow arrow;

	private bool isDragging = false;
	private float outerPenRadius;
	private float penRadius;

	void Awake()
	{
//		arrow = GameObject.Find("/Arrow").GetComponent<Arrow>();

		penRadius = GameObject.Find("Pen").GetComponent<CircleCollider2D>().radius;
		outerPenRadius = GameObject.Find("Outer Pen").GetComponent<CircleCollider2D>().radius;
	}

	void Update()
	{
		Vector2 mousePt = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		float radius = Vector2.Distance(transform.position, mousePt);
		bool mouseHeld = Input.GetMouseButton(0);

		// In inner pen
		if(radius < penRadius)
		{
			if(isDragging)
			{
				if(mouseHeld) arrow.redraw(mousePt);
				else stopDrag(false);
			}
		}

		// In outer pen
		else if(radius < outerPenRadius)
		{
			if(isDragging)
			{
				if(mouseHeld) arrow.redraw(mousePt);
				else stopDrag();
			}
			else if(mouseHeld) startDrag(mousePt);
		}

		// Outside both
		else if(isDragging) stopDrag();
	}

	private void startDrag(Vector2 pt)
	{
		isDragging = true;
		Cursor.visible = false;
		arrow.show(pt);
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
		}
	}
}
