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
		hide();
	}

	public void show(Vector2 pt)
	{		
		startPoint = pt;
		sr.enabled = true; // show this dot
		addSpider(); // draw the spider
		redraw(pt); // draw the line
	}

	public void hide()
	{
		Debug.Log("Spider gone");
		sr.enabled = false;
//		Destroy(heldSpider);
		heldSpider = null;
	}

	public void redraw(Vector2 pt)
	{
		transform.position = new Vector3(pt.x, pt.y, transform.position.z);

//		float spiderAngle = Mathf.Atan2(startPoint.y, startPoint.x) * Mathf.Rad2Deg;
//		heldSpider.transform.LookAt(new Vector3(pt.x, pt.y, heldSpider.transform.position.z));

		Vector2 diff = pt - (Vector2) heldSpider.transform.position;
		diff.Normalize();
		float rot = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
		heldSpider.transform.rotation = Quaternion.Euler(0f, 0f, rot - 90);

	}

	private void addSpider()
	{
		Vector3 pt = new Vector3(startPoint.x, startPoint.y, spiderPrefab.transform.position.z);
		heldSpider = (GameObject) Instantiate(spiderPrefab, pt, Quaternion.identity);
//		heldSpider.transform.LookAt(ctr);
//		Debug.Log("Looking from " + pt + " to " + ctr + "! New rotation:" + heldSpider.transform.localRotation);
	}
}
