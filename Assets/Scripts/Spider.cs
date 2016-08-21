using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spewnity;

public class Spider : MonoBehaviour
{
	private static float FLING_MULTIPLIER = 2.0f;

	private bool readyToBoogie = false;
	private Rigidbody2D rb;
	private Game game;
	private int permeableLayer;
	private int spiderLayer;
	private int boundaryLayer;
	private int deathLayer;
	private int dyingLayer;
	private Animator anim;

	public float walkingSpeed;

	void Awake()
	{
		game = GameObject.Find("Game").GetComponent<Game>();
		rb = gameObject.GetComponent<Rigidbody2D>();
		spiderLayer = LayerMask.NameToLayer("Spiders");
		permeableLayer = LayerMask.NameToLayer("Permeable");
		boundaryLayer = LayerMask.NameToLayer("Boundary");
		deathLayer = LayerMask.NameToLayer("Death");
		dyingLayer = LayerMask.NameToLayer("Dying");
		walkingSpeed = game.spiderSpeed.Evaluate(Random.value);
		anim = gameObject.GetComponent<Animator>();
	}

	void Start()
	{
		anim.SetTrigger("panic");
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
			game.spiderBirthed();
			anim.SetTrigger("walk");
		}
	}

	void OnTriggerEnter2D(Collider2D c)
	{
		if(c.gameObject.layer == deathLayer)
		{
			anim.SetTrigger("panic");
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

// This gets out of control quickly
//	void OnCollisionEnter2D(Collision2D c)
//	{
//		if(c.gameObject.layer == spiderLayer)
//		{
//			SoundManager.instance.Play("scritch");
//		}
//	}

	public void fling(float force)
	{
		readyToBoogie = true;
		rb.AddRelativeForce(new Vector2(0f, force * FLING_MULTIPLIER), ForceMode2D.Impulse);
	}
}
