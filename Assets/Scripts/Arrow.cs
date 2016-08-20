using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
	public GameObject spiderPrefab;
	private SpriteRenderer sr;
	private Vector2 startPoint;
	private GameObject heldSpider;

	void Awake()
	{
		sr = this.GetComponent<SpriteRenderer>();
		hide(false);
	}

	public void show(Vector2 pt)
	{		
		startPoint = pt;
		sr.enabled = true; // show this dot
		addSpider(); // draw the spider
		redraw(pt); // draw the line
	}

	public GameObject hide(bool killSpider)
	{
		sr.enabled = false;
		GameObject temp = heldSpider;
		if(killSpider && heldSpider != null)
			Destroy(heldSpider);
		heldSpider = null;
		return temp;
	}

	public void redraw(Vector2 pt)
	{
		if(heldSpider == null)
			return;
		
		transform.position = new Vector3(pt.x, pt.y, transform.position.z);
		Vector2 diff = pt - (Vector2) heldSpider.transform.position;
		diff.Normalize();
		float rot = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
		heldSpider.transform.rotation = Quaternion.Euler(0f, 0f, rot - 90);

	}

	private void addSpider()
	{
		Vector3 pt = new Vector3(startPoint.x, startPoint.y, spiderPrefab.transform.position.z);
		heldSpider = (GameObject) Instantiate(spiderPrefab, pt, Quaternion.identity);
	}
}
