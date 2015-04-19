using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class InitGame : MonoBehaviour {

	public Texture2D cursorActive;
	public Texture2D cursorInactive;

	CursorMode cursorMode = CursorMode.Auto;
	Vector2 cursorEdge = Vector2.zero;

	bool isFirstUpdate = true;

	public GameObject bg, rhino, idp, sign, playerWords, rhinoWords;
	public GameObject[] options;

	DialogManager dialogMgr;

	public bool hotspotsActive = true;

	// Use this for initialization
	void Start () {
		// Make sure cursor is visible
		Cursor.visible = true;

		// find all objects
		rhinoWords = GameObject.Find("RhinoWords");
		playerWords = GameObject.Find("PlayerWords");

		bg = GameObject.Find("Bg");
		rhino = GameObject.Find("Rhino");
		idp = GameObject.Find("IDP");
		sign = GameObject.Find("Sign");

		// set dialog options
		options = new GameObject[5];
		
		options[0] = GameObject.Find("Option1");
		options[1] = GameObject.Find("Option2");
		options[2] = GameObject.Find("Option3");
		options[3] = GameObject.Find("Option4");
		options[4] = GameObject.Find("Option5");

		// Hide dialog options, rhino, bg, IDP, sign

		options[0].SetActive(false);
		options[1].SetActive(false);
		options[2].SetActive(false);
		options[3].SetActive(false);
		options[4].SetActive(false);

		rhino.SetActive(false);
		bg.SetActive(false);
		idp.SetActive(false);
		sign.SetActive(false);

		// dialog manager
		dialogMgr = gameObject.GetComponent<DialogManager>();

		// Start Intro sequence
		dialogMgr.Intro();
	}

	
	// Update is called once per frame
	void Update () {
		if (isFirstUpdate) {
			// Set the cursor to a sprite
			SetCursorInactive();
			isFirstUpdate = false;
		}
	
	}

	public void SetCursorActive() {
		Cursor.SetCursor(cursorActive, cursorEdge, cursorMode);
	}

	public void SetCursorInactive() {
		Cursor.SetCursor(cursorInactive, cursorEdge, cursorMode);
	}
}
