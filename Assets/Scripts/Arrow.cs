using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
	private SpriteRenderer sr;

	void Awake()
	{
		sr = this.GetComponent<SpriteRenderer>();
		hide();
	}

	public void show(Vector2 pt)
	{		
		sr.enabled = true;
		redraw(pt);
	}

	public void hide()
	{
		sr.enabled = false;
	}

	public void redraw(Vector2 pt)
	{
		transform.position = new Vector3(pt.x, pt.y, transform.position.z);
	}
}
