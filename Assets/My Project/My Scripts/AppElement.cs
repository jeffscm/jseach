using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppElement : MonoBehaviour {

	// Gives access to the application and all instances.
	public App app { get { return GameObject.FindObjectOfType<App>(); }}
}
