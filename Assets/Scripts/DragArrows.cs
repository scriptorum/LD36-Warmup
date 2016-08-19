using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragArrows : MonoBehaviour
{
	public Arrow arrow;
	public CircleCollider2D penCollider;
	public bool isDragging = false;

	void Awake()
	{
//		penCollider = GameObject.Find("/Pen").GetComponent<CircleCollider2D>();
//		arrow = GameObject.Find("/Arrow").GetComponent<Arrow>();
	}

	void OnMouseDown()
	{
		Debug.Log("On Mouse Down");
		Vector2 pt = getPoint();
		if(!isWithinPen(pt))
		{
			startDrag(pt);
		}
	}
		
	void OnMouseUp()
	{
		Debug.Log("On Mouse Up");
		if(isDragging)
		{
			stopDrag();
		}
	}

	void OnMouseDrag()
	{
		if(isDragging)
		{
			arrow.redraw(getPoint());
		}
	}
		
	void OnMouseExit()
	{
		Debug.Log("On Mouse Exit");
		if(isDragging && !isWithinPen(getPoint()))
		{
			stopDrag();
		}
	}

	private bool isWithinPen(Vector2 pt)
	{
		return penCollider.OverlapPoint(pt);
	}

	private void startDrag(Vector2 pt)
	{
		isDragging = true;
		Cursor.visible = false;
		arrow.show(pt);
	}

	private void stopDrag()
	{
		isDragging = false;
		Cursor.visible = true;
		arrow.hide();
	}

	private Vector2 getPoint()
	{
		return (Vector2) Camera.main.ScreenToWorldPoint(Input.mousePosition);
	}
}
