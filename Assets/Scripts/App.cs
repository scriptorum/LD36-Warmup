using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class App : MonoBehaviour
{
	public static App instance = null;

	public string lastScene = "";
	public string activeScene = "";

	public void Awake()
	{		
		if(instance == null)
		{
			instance = this;
			DontDestroyOnLoad(gameObject);
		}

		else if(instance != this)
		{
			instance.sceneInit();
			Destroy(gameObject);
			return;
		}

		sceneInit();
	}

	public void sceneInit()
	{
		lastScene = activeScene;
		activeScene = SceneManager.GetActiveScene().name;
	}
}

