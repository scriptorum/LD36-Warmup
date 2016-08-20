using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spewnity;

public class Dying : MonoBehaviour
{
	private ActionQueue aq;
	private SpriteRenderer sr;
	private Game game;

	void Awake()
	{
		sr = gameObject.GetComponent<SpriteRenderer>();
		game = GameObject.Find("Game").GetComponent<Game>();
	}

	void Start()
	{
		aq = gameObject.AddComponent<ActionQueue>();
		aq
			.Add(() => StartCoroutine(sr.color.LerpColor(new Color(1f, 1f, 1f, 0f), 1.0f, (v) => sr.color = v)))
			.Add(() => StartCoroutine(gameObject.transform.LerpScale(Vector3.zero, 1.0f)))
			.Delay(1.0f)
			.Add(() => game.removeSpider())
			.Destroy(gameObject)
			.Run();
	}

	void FixedUpdate()
	{
		gameObject.transform.Rotate(0f, 0f, 3.5f);
	}
}
