using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spewnity;

public class Spider : MonoBehaviour
{
	private static float FLING_MULTIPLIER = 2.0f;

	private Rigidbody2D rb;
	private Game game;
	private int permeableLayer;
	private int spiderLayer;
	private int boundaryLayer;
	private int deathLayer;
	private int dyingLayer;
	private Animator anim;
	private float penRadius;
	private bool isWalking = false;
	private bool isOutsidePen = true;
	private bool isHeld = true;
	private Vector2 centerPos;

	public float walkingSpeed;

	void Awake()
	{
		game = GameObject.Find("/Game").GetComponent<Game>();
		rb = gameObject.GetComponent<Rigidbody2D>();
		spiderLayer = LayerMask.NameToLayer("Spiders");
		permeableLayer = LayerMask.NameToLayer("Permeable");
		boundaryLayer = LayerMask.NameToLayer("Boundary");
		deathLayer = LayerMask.NameToLayer("Death");
		dyingLayer = LayerMask.NameToLayer("Dying");
		walkingSpeed = game.spiderSpeed.Evaluate(Random.value);
		anim = gameObject.GetComponent<Animator>();
		penRadius = GameObject.Find("Pen").GetComponent<CircleCollider2D>().radius;
		centerPos = (Vector2) GameObject.Find("Pen").transform.position;

	}

	private bool isSpiderViable()
	{
		float spiderRadius = Vector2.Distance(centerPos, (Vector2) transform.position);
		if(spiderRadius > penRadius)
		{
			killSpider();
			return false;
		}
		return true;
	}

	private void killSpider()
	{
		Destroy(gameObject);
	}

	void Start()
	{
		anim.SetTrigger("panic");
		anim.speed = 1.0f;
	}

	void FixedUpdate()
	{
		if(isOutsidePen)
		{
			if(!isHeld && rb.velocity.magnitude <= 0.0001f) killSpider();
			return;
		}

		if(!isWalking)
		{
			if(rb.velocity.magnitude > walkingSpeed) return;

			float spiderRadius = Vector2.Distance(centerPos, (Vector2) transform.position);
			if(spiderRadius > penRadius) killSpider();
			else
			{
				isWalking = true;
				anim.SetTrigger("walk");
				anim.speed = 1 + walkingSpeed;
				game.spiderBirthed();
			}
		}

		rb.AddRelativeForce(new Vector2(0f, walkingSpeed));
	}

	// Pass through permeable barrier?
	void OnTriggerExit2D(Collider2D c)
	{
		if(c.gameObject.layer == permeableLayer)
		{
			isOutsidePen = false;
			gameObject.layer = spiderLayer;
		}
	}

	// Collide with pit? Not if still "airborne"
	void OnTriggerStay2D(Collider2D c)
	{
		if(c.gameObject.layer == deathLayer && isWalking)
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
		if(c.gameObject.layer == boundaryLayer) rb.AddTorque(0.8f * Random.value > 0.5 ? 1 : -1);
	}

	public void fling(float force)
	{
		if(force < Game.minLineLength) killSpider();
		else
		{
			isHeld = false;
			rb.AddRelativeForce(new Vector2(0f, force * FLING_MULTIPLIER), ForceMode2D.Impulse);
		}
	}
}
