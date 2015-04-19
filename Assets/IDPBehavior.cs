using UnityEngine;
using System.Collections;

public class IDPBehavior : MonoBehaviour {

	bool firstClick = true;
	
	GameObject mainObj;
	DialogManager dialogMgr;
	InitGame mainScript;

	// Use this for initialization
	void Start () {
		mainObj = GameObject.Find("GameManager");
		dialogMgr = mainObj.GetComponent<DialogManager>();
		mainScript = mainObj.GetComponent<InitGame>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnMouseDown() {
		if (mainScript.hotspotsActive) {
			if (firstClick) {
				dialogMgr.FirstIDPDialog();
				firstClick = false;
			}
			else {
				dialogMgr.IDPDialog();
			}
		}
	}

}
