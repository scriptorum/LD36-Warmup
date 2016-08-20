using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
	public float dotLength = 0.5f;
	public GameObject spiderPrefab;
	public GameObject dotPrefab;
	private SpriteRenderer sr;
	private Vector2 startPoint;
	private GameObject heldSpider;
	private List<GameObject> dots = new List<GameObject>();

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
		GameObject temp = heldSpider;
		if(killSpider && heldSpider != null)
			Destroy(heldSpider);
		heldSpider = null;

		// Kill arrow
		sr.enabled = false;
		foreach(GameObject go in dots)
			go.SetActive(false);

		return temp;
	}

	public void redraw(Vector2 pt)
	{
		if(heldSpider == null)
			return;

		// Rotate spider
		Vector2 diff = pt - (Vector2) heldSpider.transform.position;
		diff.Normalize();
		float rot = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
		heldSpider.transform.rotation = Quaternion.Euler(0f, 0f, rot - 90);

		// Move main dot
		transform.position = new Vector3(pt.x, pt.y, transform.position.z);

		// Determine optimal number of smaller dots
		Vector2 vec = ((Vector2) heldSpider.transform.position) - pt;
		int numDots = Mathf.Max(0, (int) Mathf.Floor(vec.magnitude / dotLength) - 1);
		Vector2 dotOffset = vec / numDots;

		// Ensure correct number of dots exist
		if(dots.Count > numDots)
		{
			for(int d = numDots; d < dots.Count; d++)
				dots[d].SetActive(false);
		}
		else if(numDots > dots.Count)
		{
			while(numDots > dots.Count)
			{
				GameObject dot = Instantiate(dotPrefab);
				dots.Add(dot);
			}
		}
			
		// Align dots
		for(int d = 0; d < numDots; d++)
		{
			if(!dots[d].activeSelf)
				dots[d].SetActive(true);
			dots[d].transform.position = new Vector3(pt.x + dotOffset.x * d, pt.y + dotOffset.y * d, dotPrefab.transform.position.z);
		}
	}

	private void addSpider()
	{
		Vector3 pt = new Vector3(startPoint.x, startPoint.y, spiderPrefab.transform.position.z);
		heldSpider = (GameObject) Instantiate(spiderPrefab, pt, Quaternion.identity);
	}
}
