using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class DialogManager : MonoBehaviour {

	GameObject rhinoWords, playerWords;

	InitGame mainScript;

	Dictionary<int, InsultRetort> insultDict = new Dictionary<int, InsultRetort>();

	int numberOfInsults;

	List<int> knownRetorts = new List<int>();
	List<int> knownInsults = new List<int>();

	int randInt;

	GameObject rhino, idp;
	HotspotBehavior rhinoHotspot, idpHotspot;
	RhinoBehavior rhinoBehave;
	IDPBehavior idpBehave;

	GameObject[] options;

	// Use this for initialization
	void Start () {

		insultDict.Add(1, new InsultRetort("Leave my sight, my eyes can’t stand to see you!",
		                                   "I should poke your eyes out and solve that problem.",
		                                   "You’re too cowardly to look me in the eye!",
		                                   1));
		insultDict.Add(2, new InsultRetort("You cannot bear to face me, coward!",
		                                   "Have you looked in the mirror lately?",
		                                   "My presence is so bright, people avert their eyes when I pass.",
		                                   2));
		insultDict.Add(3, new InsultRetort("I'll pound you like a punching bag!",
		                                   "You'll be hitting the bottle after your defeat!",
		                                   "Soon I shall drink to your demise!",
		                                   3));
		insultDict.Add(4, new InsultRetort("I'll teach you a lesson, punk!",
		                                   "It's nice to hear you're investing in education.",
		                                   "Every child in the world has learnt my name.",
		                                   4));
		insultDict.Add(5, new InsultRetort("Never in my life have I met someone as stupid as you.",
		                                   "You'll have lots of time to meet new people soon!",
		                                   "Only few people dare come here and challenge me.",
		                                   5));

		// count insult sets
		numberOfInsults = insultDict.Keys.Count;

		mainScript = gameObject.GetComponent<InitGame>();

		rhinoWords = mainScript.rhinoWords;
		playerWords = mainScript.playerWords;

		//rhino = mainScript.rhino;
		//idp = mainScript.idp;

		options = new GameObject[5];
		options = mainScript.options;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void disableInteaction() {
		mainScript.hotspotsActive = false;
		mainScript.SetCursorInactive();
		/*
		if (rhino.activeInHierarchy) {
			Debug.Log("in rhino disable");
			rhinoHotspot = rhino.GetComponent<HotspotBehavior>();
			rhinoBehave = rhino.GetComponent<RhinoBehavior>();
			rhinoBehave.enabled = false;
			rhinoHotspot.enabled = false;
		}
		if (idp.activeInHierarchy) {
			Debug.Log("in idp disable");
			idpHotspot = idp.GetComponent<HotspotBehavior>();
			idpBehave = idp.GetComponent<IDPBehavior>();
			Debug.Log("object idpBehave " + idpBehave + " and enabled is " + idpBehave.enabled);
			Debug.Log("object idpHotspot " + idpHotspot + " and enabled is " + idpHotspot.enabled);
			idpBehave.enabled = false;
			Debug.Log("object idpBehave " + idpBehave + " and enabled is " + idpBehave.enabled);
			idpHotspot.enabled = false;
			Debug.Log("object idpHotspot " + idpHotspot + " and enabled is " + idpHotspot.enabled);
		}
		*/
	}

	void enableInteaction() {
		mainScript.hotspotsActive = true;
		/*
		if (rhino.activeInHierarchy) {
			rhinoHotspot = rhino.GetComponent<HotspotBehavior>();
			rhinoBehave = rhino.GetComponent<RhinoBehavior>();
			rhinoBehave.enabled = true;
			rhinoHotspot.enabled = true;
		}
		if (idp.activeInHierarchy) {
			idpHotspot = idp.GetComponent<HotspotBehavior>();
			idpBehave = idp.GetComponent<IDPBehavior>();
			Debug.Log("object idpBehave " + idpBehave + " and enabled is " + idpBehave.enabled);
			Debug.Log("object idpHotspot " + idpHotspot + " and enabled is " + idpHotspot.enabled);
			idpBehave.enabled = true;
			Debug.Log("object idpBehave " + idpBehave + " and enabled is " + idpBehave.enabled);
			idpHotspot.enabled = true;
			Debug.Log("object idpHotspot " + idpHotspot + " and enabled is " + idpHotspot.enabled);
		}
		*/
	}

	void ChangeText(GameObject dialogBox, string newText) {
		dialogBox.GetComponent<Text>().text = newText;
	}

	public void Intro() {
		StartCoroutine(IntroSequence());
	}

	IEnumerator IntroSequence() {
		Cursor.visible = false;
		yield return new WaitForSeconds(1.0f);
		gameObject.GetComponent<AudioSource>().Play();
		yield return new WaitForSeconds(3.0f);
		playerWords.GetComponent<Text>().text = "At last, I’ve arrived in the great city of Aspersia!";
		yield return new WaitForSeconds(2.5f);
		playerWords.GetComponent<Text>().text = "";
		yield return new WaitForSeconds(0.8f);
		playerWords.GetComponent<Text>().text = "Here lives the great master of insults, the one, the king.";
		yield return new WaitForSeconds(2.5f);
		playerWords.GetComponent<Text>().text = "";
		yield return new WaitForSeconds(0.8f);
		playerWords.GetComponent<Text>().text = "To this day, many have challenged him. None have prevailed.";
		yield return new WaitForSeconds(2.5f);
		playerWords.GetComponent<Text>().text = "";
		yield return new WaitForSeconds(0.8f);
		playerWords.GetComponent<Text>().text = "I plan to change that.";
		yield return new WaitForSeconds(2.5f);
		playerWords.GetComponent<Text>().text = "";
		yield return new WaitForSeconds(1.0f);
		mainScript.bg.SetActive(true);
		mainScript.idp.SetActive(true);
		mainScript.sign.SetActive(true);
		Cursor.visible = true;
	}

	public void FirstRhinoDialog() {
		StartCoroutine(FirstRhinoSequence());
	}

	public void RhinoDialog() {
		StartCoroutine(RhinoSequence());
	}

	public void FirstIDPDialog() {
		StartCoroutine(FirstIDPSequence());
	}

	public void IDPDialog() {
		StartCoroutine(IDPSequence());
	}

	IEnumerator RhinoSequence() {
		disableInteaction();
		ChangeText(rhinoWords, "Coming back for more?");
		yield return new WaitForSeconds(2.5f);
		ChangeText(rhinoWords, "");
		RandomRhinoInsult();
	}

	IEnumerator FirstRhinoSequence() {
		disableInteaction();
		ChangeText(rhinoWords, "Your insults can’t harm me, I have the Hide of the Rhinoceros!");
		yield return new WaitForSeconds(2.5f);
		ChangeText(rhinoWords, "");
		RandomRhinoInsult();
	}

	IEnumerator IDPSequence() {
		disableInteaction();
		ChangeText(rhinoWords, "Let's begin!");
		yield return new WaitForSeconds(2.5f);
		ChangeText(rhinoWords, "");
		RandomIDPInsult();
	}

	IEnumerator FirstIDPSequence() {
		disableInteaction();
		Debug.Log("disabled interaction");
		ChangeText(rhinoWords, "Welcome to Insult Duelling Practice 2.0.");
		yield return new WaitForSeconds(2.5f);
		ChangeText(rhinoWords, "");
		yield return new WaitForSeconds(0.8f);
		ChangeText(rhinoWords, "Generating random personality… Let’s begin!");
		yield return new WaitForSeconds(2.5f);
		ChangeText(rhinoWords, "");
		RandomIDPInsult();
	}


	void RandomRhinoInsult() {
		randInt = Random.Range(1, numberOfInsults + 1);
		StartCoroutine(RandomInsultSequence(insultDict[randInt].masterInsult, randInt));
	}

	void RandomIDPInsult() {
		randInt = Random.Range(1, numberOfInsults + 1);
		Debug.Log("random insult chosen " + randInt);
		StartCoroutine(RandomInsultSequence(insultDict[randInt].insult, randInt));
		if (!knownInsults.Contains(randInt)) {
			knownInsults.Add(randInt);
		}
	}

	IEnumerator RandomInsultSequence(string insult, int insultId) {
		ChangeText(rhinoWords, insult);
		yield return new WaitForSeconds(2.5f);
		ChangeText(rhinoWords, "");
		ShowRetortDialogOptions(insultId);
	}

	public void PlayerSpeak(string text) {
		StartCoroutine(PlayerRetortSequence(text));
	}

	IEnumerator PlayerRetortSequence(string text) {
		HideDialogOptions();
		ChangeText(playerWords, text);
		yield return new WaitForSeconds(2.5f);
		ChangeText(playerWords, "");
		enableInteaction();
	}

	void ShowRetortDialogOptions(int insultId) {
		ShowDialogOptions(insultId, true);
	}

	void ShowInsultDialogOptions() {
		ShowDialogOptions(0, false);
	}

	void ShowDialogOptions(int insultId, bool isRetortMode) {

		int i = 0;
		int j = 0;

		int randPosCorrect = -1;
		int randOpt;

		List<int> usedAlready = new List<int>();

		int maxOptions;

		if (isRetortMode) {
			maxOptions = knownRetorts.Count + 1;
		}
		else {
			maxOptions = knownInsults.Count + 1;
		}

		Debug.Log("Number of options " + maxOptions);

		if (maxOptions > 5) {
			maxOptions = 5;
			Debug.Log("Number of options changed to " + maxOptions);
		}


		if (isRetortMode) {
			// last option is always give up
			randPosCorrect = Random.Range(0, maxOptions - 1);
		}
		else {
			randPosCorrect = -1;
		}
		Debug.Log("correct answer pos " + randPosCorrect);

		while (i < maxOptions) {
			// infinite loop safety
			j++;
			Debug.Log("In loop cycle " + j);
			if (j > 10) {
				i = maxOptions;
				break;
			}
			
			if (i == maxOptions - 1) {
				if (isRetortMode) {
					options[i].GetComponent<Text>().text = "I give up.";
					i++;
				}
				else {
					options[i].GetComponent<Text>().text = "I got nothing.";
					i++;
				}
			}
			
			else if ((i == randPosCorrect) && (isRetortMode)) {
				if (knownRetorts.Contains(insultId)) {
					options[i].GetComponent<Text>().text = insultDict[insultId].retort;
					usedAlready.Add(insultId);
					i++;
				}
			}
			
			// the other options are random
			else {
				randOpt = Random.Range(1, numberOfInsults);
				if (isRetortMode) {
					if ((randOpt != insultId) &&
					    (knownRetorts.Contains(randOpt)) &&
					    (!usedAlready.Contains(randOpt))) {
						options[i].GetComponent<Text>().text = insultDict[randOpt].retort;
						usedAlready.Add(randOpt);
						i++;
					}
				}
				else {
					if ((knownInsults.Contains(randOpt)) &&
					    (!usedAlready.Contains(randOpt))) {
						options[i].GetComponent<Text>().text = insultDict[randOpt].insult;
						usedAlready.Add(randOpt);
						i++;
					}
				}
			}
		}

		
		i = 0;
		for (i = 0; i < maxOptions; i++) {
			options[i].SetActive(true);
		}

	}

	void HideDialogOptions() {
		int i = 0;
		for (i = 0; i < 5; i++) {
			options[i].GetComponent<Text>().text = "";
			options[i].SetActive(false);
		}
	}

	void endDialog() {
		enableInteaction();
	}

}
