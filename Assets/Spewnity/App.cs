/**
 * "Global" scene bootstrapper that lets you run any scene independently,
 * whether for testing/debugging or to rearrange scenes. To use:
 *   - Create an App scene
 *   - Add to this scene a GameObject with this script on it
 *   - From each of your other scenes: call App.load() in your Awake()
 * 
 * The objects in the app scene will be addively loaded.
 * 
 * This is better than DontDestroyOnLoad in that it ensures any GameObjects in
 * the App scene are available no matter which scene you run first, and does
 * not require you to maintain duplicate objects.
 * 
 * To call your App Scene something other than "App," pass the correct name
 * in the App.load() method. If the default is ok, you can use the AppLoader class.
 * 
 * Subscribe to appInitialized to be called when the app is initialized for the 
 * first time. Subscribe to SceneManager.activeSceneChanged to be called whenever
 * the scene is changed.
 **/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

namespace Spewnity
{
	public class App : MonoBehaviour
	{
		public static App instance = null;
		public static string curScene = null;
		public static string lastScene = null;

		public UnityEvent appInitialized;

		public static void load(string sceneName = "App")
		{
			if(instance == null) SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);

			App.lastScene = App.curScene;
			App.curScene = SceneManager.GetActiveScene().name;
		}

		public void Awake()
		{
			App.instance = this;
			App.instance.appInitialized.Invoke();
		}
	}
}