﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spider : MonoBehaviour
{
	private static float FLING_MULTIPLIER = 2.0f;

	private bool readyToBoogie = false;
	private Rigidbody2D rb;
	private int permeableLayer;
	private int spiderLayer;
	private int boundaryLayer;
	private int deathLayer;
	private int dyingLayer;

	public float walkingSpeed;

	void Awake()
	{
		rb = gameObject.GetComponent<Rigidbody2D>();
		spiderLayer = LayerMask.NameToLayer("Spiders");
		permeableLayer = LayerMask.NameToLayer("Permeable");
		boundaryLayer = LayerMask.NameToLayer("Boundary");
		deathLayer = LayerMask.NameToLayer("Death");
		dyingLayer = LayerMask.NameToLayer("Dying");
		walkingSpeed = Random.Range(0.2f, 0.5f);
	}

	void FixedUpdate()
	{
		if(!readyToBoogie) return;

		rb.AddRelativeForce(new Vector2(0f, walkingSpeed));
	}

	// Pass through permeable barrier?
	void OnTriggerExit2D(Collider2D c)
	{
		if(c.gameObject.layer == permeableLayer)
		{
//			readyToBoogie = true;
			gameObject.layer = spiderLayer;
		}
	}

	void OnTriggerEnter2D(Collider2D c)
	{
		if(c.gameObject.layer == deathLayer)
		{
			gameObject.layer = dyingLayer;
			gameObject.AddComponent<Dying>();
			rb.velocity = rb.velocity.normalized;
			rb.angularVelocity = 0f;
			Destroy(this);
		}
	}

	// Walk into wall?
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
		readyToBoogie = true;
		rb.AddRelativeForce(new Vector2(0f, force * FLING_MULTIPLIER), ForceMode2D.Impulse);
	}
}
