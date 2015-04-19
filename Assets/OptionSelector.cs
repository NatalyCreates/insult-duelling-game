using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class OptionSelector : MonoBehaviour {

	GameObject rhinoWords;
	GameObject playerWords;

	DialogManager dialogMgr;
	GameObject mainObj;

	// Use this for initialization
	void Start () {
		mainObj = GameObject.Find("GameManager");
		dialogMgr = mainObj.GetComponent<DialogManager>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void MouseDown() {
		string t = gameObject.GetComponent<Text>().text;
		dialogMgr.PlayerSpeak(t);
	}

}
