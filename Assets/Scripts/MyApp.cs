/**
 * Spitballing here.
 * I just want a way to ensure SoundManager is loaded when I test the Play scene (without first running Main).
 **/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MyApp : App
{
	// Shared variables here


	private void appAwake()
	{
		// Stuff to do just once
	}

	private static void sceneLoaded()
	{
		// Stuff to do every time a new scene is loaded				
	}
}


public class App : MonoBehaviour
{
	public static App instance = null;
	public static string curScene = null;
	public static string lastScene = null;

	public static App load()
	{
		if(instance == null) SceneManager.LoadScene("App", LoadSceneMode.Additive);
		App.lastScene = App.curScene;
		App.curScene = SceneManager.GetActiveScene().name;

		sceneLoaded();

		return instance;
	}

	public void Awake()
	{
		App.instance = this;
		appAwake();
	}

	public void Start()
	{
	}

	private void appAwake()
	{
		// Override with stuff to do once the App awakens
	}

	private static void sceneLoaded()
	{
		// Override with stuff to do every time a new scene is loaded
	}
}
