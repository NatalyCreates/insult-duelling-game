using UnityEngine;
using System.Collections;

public class HotspotBehavior : MonoBehaviour {

	GameObject mainObj;
	InitGame mainScript;

	// Use this for initialization
	void Start () {
		mainObj = GameObject.Find("GameManager");
		mainScript = mainObj.GetComponent<InitGame>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	// Functions for Hotspots
	void OnMouseEnter() {
		if (mainScript.hotspotsActive) {
			//Debug.Log("Enter hostpot " + gameObject.name);
			mainScript.SetCursorActive();
		}
	}

	void OnMouseExit() {
		if (mainScript.hotspotsActive) {
			//Debug.Log("Exit hostpot " + gameObject.name);
			mainScript.SetCursorInactive();
		}
	}

	// Functions for Dialog UI
	public void MouseEnter() {
		//Debug.Log("Enter UI hostpot " + gameObject.name);
		mainScript.SetCursorActive();
	}
	
	public void MouseExit() {
		//Debug.Log("Exit UI hostpot " + gameObject.name);
		mainScript.SetCursorInactive();
	}
}
