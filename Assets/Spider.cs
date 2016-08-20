using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spider : MonoBehaviour
{
	private static float FORCE_MULTIPLIER = 2.0f;
	private static float WALK_FORCE = 0.25f;

	public bool readyToBoogie = false;
	private Rigidbody2D rb;

	void Awake()
	{
		rb = gameObject.GetComponent<Rigidbody2D>();
	}

	void Update()
	{
		if(readyToBoogie)// && rb.velocity + rb.angularVelocity < 
		{
			rb.AddRelativeForce(new Vector2(0f, WALK_FORCE));
		}
	}

	public void fling(float force)
	{
		readyToBoogie = true;
		rb.AddRelativeForce(new Vector2(0f, force * FORCE_MULTIPLIER), ForceMode2D.Impulse);
	}
}
