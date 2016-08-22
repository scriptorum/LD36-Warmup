using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppLoader : MonoBehaviour 
{
	void Awake()
	{
		App.load();
	}
}

