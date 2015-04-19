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
			mainScript.SetCursorActive();
		}
	}

	void OnMouseExit() {
		if (mainScript.hotspotsActive) {
			mainScript.SetCursorInactive();
		}
	}

	// Functions for Dialog UI
	public void MouseEnter() {
		mainScript.SetCursorActive();
	}
	
	public void MouseExit() {
		mainScript.SetCursorInactive();
	}
}
