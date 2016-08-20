using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spider : MonoBehaviour
{
	private static float FORCE_MULTIPLIER = 2.0f;
	private static float WALK_FORCE = 0.35f;

	private bool readyToBoogie = false;
	private Rigidbody2D rb;
	private int permeableLayer;
	private int spiderLayer;
	private int boundaryLayer;

	void Awake()
	{
		rb = gameObject.GetComponent<Rigidbody2D>();
		spiderLayer = LayerMask.NameToLayer("Spiders");
		permeableLayer = LayerMask.NameToLayer("Permeable");
		boundaryLayer = LayerMask.NameToLayer("Boundary");
	}

	void FixedUpdate()
	{
		if(!readyToBoogie) return;

		rb.AddRelativeForce(new Vector2(0f, WALK_FORCE));
	}

	void OnTriggerExit2D(Collider2D c)
	{
		if(c.gameObject.layer == permeableLayer)
		{
			readyToBoogie = true;
			gameObject.layer = spiderLayer;
		}
	}

	void OnCollisionStay2D(Collision2D c)
	{
		if(c.gameObject.layer == boundaryLayer)
		{
			rb.AddTorque(0.8f * Random.value > 0.5 ? 1 : -1);
//			rb.AddRelativeForce(new Vector2(0f, WALK_FORCE * 100));
		}
	}

	public void fling(float force)
	{
		rb.AddRelativeForce(new Vector2(0f, force * FORCE_MULTIPLIER), ForceMode2D.Impulse);
	}
}
