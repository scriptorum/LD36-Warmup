using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Spewnity
{
	public class AppLoader : MonoBehaviour
	{
		void Awake()
		{
			App.load();
		}
	}
}