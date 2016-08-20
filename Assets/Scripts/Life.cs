using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spewnity;

public class Life : MonoBehaviour 
{
	private GameObject crossout;

	void Awake()
	{
		crossout = gameObject.GetChild("Crossout");
	}

	void Start()
	{
		// Hide crossout
		crossout.SetActive(false);
	}

	public void show()
	{
		crossout.SetActive(true);
	}
}
