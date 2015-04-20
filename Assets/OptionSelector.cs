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

		int i;
		bool found = false;

		if (dialogMgr.myGameState.modeIsPlayerAsking) {
			for (i = 1; i <= dialogMgr.numberOfInsults; i++) {
				//Debug.Log("****** i = " + i + " ********");
				//Debug.Log("Looking for the insult");
				if (dialogMgr.insultDict[i].insult == t) {
					dialogMgr.myGameState.playerLastInsultGivenId = i;
					//Debug.Log(dialogMgr.insultDict[i].insult);
					//Debug.Log(dialogMgr.myGameState.playerLastInsultGivenId);
					found = true;
					break;
				}
			}
		}
		else {
			if (dialogMgr.myGameState.askerIsRhino) {
				for (i = 1; i <= dialogMgr.numberOfInsults; i++) {
					//Debug.Log("****** i = " + i + " ********");
					//Debug.Log("Looking for the retort");
					if (dialogMgr.insultDict[i].retort == t) {
						dialogMgr.myGameState.playerLastRetortGivenId = i;
						//Debug.Log(dialogMgr.insultDict[i].retort);
						//Debug.Log(dialogMgr.myGameState.playerLastRetortGivenId);
						found = true;
						break;
					}
				}
			}
			else {
				for (i = 1; i <= dialogMgr.numberOfInsults; i++) {
					//Debug.Log("****** i = " + i + " ********");
					//Debug.Log("Looking for the retort");
					if (dialogMgr.insultDict[i].retort == t) {
						dialogMgr.myGameState.playerLastRetortGivenId = i;
						//Debug.Log(dialogMgr.insultDict[i].retort);
						//Debug.Log(dialogMgr.myGameState.playerLastRetortGivenId);
						found = true;
						break;
					}
				}
			}
		}

		if (!found) {
			// player chose a generic response
			//Debug.Log("player chose a generic response: " + t);
			dialogMgr.myGameState.playerLastRetortGivenId = -1;
			dialogMgr.myGameState.playerLastInsultGivenId = -1;
		}

		dialogMgr.PlayerSpeak(t);
	}

}
