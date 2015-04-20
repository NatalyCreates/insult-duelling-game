using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class DialogManager : MonoBehaviour {

	GameObject rhinoWords, playerWords;

	InitGame mainScript;

	public Dictionary<int, InsultRetort> insultDict = new Dictionary<int, InsultRetort>();

	public int numberOfInsults;

	HotspotBehavior rhinoHotspot, idpHotspot;
	RhinoBehavior rhinoBehave;
	IDPBehavior idpBehave;

	GameObject[] options;

	public int currentInsultGivenByIdp = -1;
	public bool modePlayerIsAsking = false;
	public int chosenPlayerRetort = -1;

	public int MAX_OPTIONS_TOTAL = 5;

	// Game State

	public List<int> knownRetorts = new List<int>();
	public List<int> knownInsults = new List<int>();

	public List<int> unknownInsults = new List<int>();

	public struct GameState {
		public bool modeIsPlayerAsking;
		public bool askerIsRhino;
		public int lastInsultGivenId;
		//public int idpLastInsultGivenId;
		//public int rhinoLastInsultGivenId;
		public int playerLastInsultGivenId;
		public int playerLastRetortGivenId;
		public int numTimesBeatRhino;
	}

	public GameState myGameState;


	GameObject menuBg, menuVicText, menuText, menuQuitButton, menuStartButton;

	bool isMenuOn = false;
	bool gameOver = false;
	bool introPlaying = false;

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

		// init main script
		mainScript = gameObject.GetComponent<InitGame>();

		// init textboxes
		rhinoWords = mainScript.rhinoWords;
		playerWords = mainScript.playerWords;

		// init options
		options = new GameObject[5];
		options = mainScript.options;

		// init game state

		myGameState.modeIsPlayerAsking = false;
		myGameState.askerIsRhino = false;
		myGameState.lastInsultGivenId = -1;
		myGameState.playerLastRetortGivenId = -1;
		myGameState.playerLastInsultGivenId = -1;

		myGameState.numTimesBeatRhino = 0;

		int i;
		for (i = 1; i <= numberOfInsults; i++) {
			// initially all insults are unknown
			unknownInsults.Add(i);

			// TODO - REMOVE THIS!!!
			/* SHORTCUT - Jump straight to Rhino! */
			//knownRetorts.Add(i);
			/* END SHORTCUT */
		}

		/*
		for (i = 0; i < unknownInsults.Count; i++) {
			Debug.Log("unknownInsults[" + i + "] = " + unknownInsults[i]);
		}
		*/

		// menu objects
		menuBg = GameObject.Find("MenuImg");
		menuVicText = GameObject.Find("WinText");
		menuText = GameObject.Find("MenuText");
		menuStartButton = GameObject.Find("ButtonNewGame");
		menuQuitButton = GameObject.Find("ButtonQuit");

		menuBg.SetActive(false);
		menuVicText.SetActive(false);
		menuText.SetActive(false);
		menuStartButton.SetActive(false);
		menuQuitButton.SetActive(false);

	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.Escape)) {
			Application.Quit();
		}
		if (!gameOver) {
			if ((Input.GetKeyDown(KeyCode.F1)) || (Input.GetKeyDown(KeyCode.M))) {
				if (isMenuOn) {
					isMenuOn = false;
					if (introPlaying) {
						// hide cursor
						Cursor.visible = false;
					}

					// enable the right one
					if (myGameState.askerIsRhino) {
						mainScript.rhino.GetComponent<PolygonCollider2D>().enabled = true;
					}
					else {
						mainScript.idp.GetComponent<PolygonCollider2D>().enabled = true;
					}

					menuBg.SetActive(false);
					menuText.SetActive(false);
					menuStartButton.SetActive(false);
					menuQuitButton.SetActive(false);
				}
				else {
					if (introPlaying) {
						// show cursor
						Cursor.visible = true;
					}

					isMenuOn = true;

					// show menu, disable the rest

					mainScript.rhino.GetComponent<PolygonCollider2D>().enabled = false;
					mainScript.idp.GetComponent<PolygonCollider2D>().enabled = false;
					
					menuBg.SetActive(true);
					menuText.SetActive(true);
					menuStartButton.SetActive(true);
					menuQuitButton.SetActive(true);

				}
			}
		}
	}


	/* DIALOG OPTIONS */

	void SetupRetortDialogOptions(int maxOptions) {
		
		int i = 0;
		int j = 0;
		
		int correctAnswerPos;
		int randOpt;
		
		List<int> usedAlready = new List<int>();

		List<int> unused = new List<int>();
		Debug.Log("adding " + knownRetorts.Count + " items to unused");
		for (int k = 0; k < knownRetorts.Count; k++) {
			unused.Add(knownRetorts[k]);
			Debug.Log("item " + k);
		}


		// get the right response position (out of max 4, last option is always give up)
		// do we even have the correct response available
		if (knownRetorts.Contains(myGameState.lastInsultGivenId)) {
			correctAnswerPos = Random.Range(0, maxOptions - 1);
			usedAlready.Add(myGameState.lastInsultGivenId);
			unused.Remove(myGameState.lastInsultGivenId);
		}
		else {
			correctAnswerPos = -1;
		}
		//Debug.Log("correctAnswerPos = " + correctAnswerPos);

		Debug.Log("SetupRetortDialogOptions: maxOptions = " + maxOptions);

		while (i < maxOptions) {
			Debug.Log("SetupRetortDialogOptions: in loop cycle " + i);
			// infinite loop safety
			j++;
			if (j > 10) {
				i = maxOptions;
				break;
			}

			// put the default answer in the last place
			if (i == maxOptions - 1) {
				//Debug.Log("SetupRetortDialogOptions: setting option " + i + " = 'I give up.'");
				options[i].GetComponent<Text>().text = "I give up.";
				i++;
			}

			// place the correct answer
			else if (i == correctAnswerPos) {
				options[i].GetComponent<Text>().text = insultDict[myGameState.lastInsultGivenId].retort;
				//Debug.Log("SetupRetortDialogOptions: setting option " + i + " = " + insultDict[myGameState.lastInsultGivenId].retort);
				i++;
			}
			
			// the other options are chosen at random from the list we know
			else {
				randOpt = Random.Range(0, unused.Count);
				int chosen = unused[randOpt];
				options[i].GetComponent<Text>().text = insultDict[chosen].retort;
				//Debug.Log("SetupRetortDialogOptions: setting option " + i + " = " + insultDict[chosen].retort);
				usedAlready.Add(chosen);
				unused.Remove(chosen);
				i++;

				//randOpt = Random.Range(0, knownRetorts.Count);
				//int chosen = knownRetorts[randOpt];

				/*
				if (!usedAlready.Contains(chosen)) {
					options[i].GetComponent<Text>().text = insultDict[chosen].retort;
					Debug.Log("SetupRetortDialogOptions: setting option " + i + " = " + insultDict[chosen].retort);
					usedAlready.Add(chosen);
					unused.Remove(chosen);
					i++;
				}
				*/
			}
		}
	}



	void SetupInsultDialogOptions(int maxOptions) {
		
		int i = 0;
		int j = 0;

		int randOpt;
		
		List<int> usedAlready = new List<int>();

		List<int> unused = new List<int>();
		Debug.Log("adding " + knownInsults.Count + " items to unused");
		for (int k = 0; k < knownInsults.Count; k++) {
			unused.Add(knownInsults[k]);
			Debug.Log("item " + k);
		}

		Debug.Log("SetupInsultDialogOptions: maxOptions = " + maxOptions);
		while (i < maxOptions) {
			Debug.Log("SetupInsultDialogOptions: in loop cycle " + i);
			// infinite loop safety
			j++;
			if (j > 10) {
				i = maxOptions;
				break;
			}

			// put the default answer in the last place
			if (i == maxOptions - 1) {
				//Debug.Log("SetupInsultDialogOptions: setting option " + i + " = 'I got nothing.'");
				options[i].GetComponent<Text>().text = "I got nothing.";
				i++;
			}
			
			// the other options are chosen at random from the list we know
			else {

				randOpt = Random.Range(0, unused.Count);
				int chosen = unused[randOpt];
				options[i].GetComponent<Text>().text = insultDict[chosen].insult;
				//Debug.Log("SetupInsultDialogOptions: setting option " + i + " = " + insultDict[chosen].insult);
				usedAlready.Add(chosen);
				unused.Remove(chosen);
				i++;

				//randOpt = Random.Range(0, knownInsults.Count);
				//int chosen = knownInsults[randOpt];

				/*
				if (!usedAlready.Contains(chosen)) {
					options[i].GetComponent<Text>().text = insultDict[chosen].insult;
					Debug.Log("SetupInsultDialogOptions: setting option " + i + " = " + insultDict[chosen].insult);
					usedAlready.Add(chosen);
					unused.Remove(chosen);
					i++;
				}
				*/
			}
		}
	}


	void DisplayDialogOptions() {

		// find number of options
		int maxOptions;

		if (myGameState.modeIsPlayerAsking) {
			maxOptions = GetMin(knownInsults.Count + 1, MAX_OPTIONS_TOTAL);
			SetupInsultDialogOptions(maxOptions);
		}
		else {
			maxOptions = GetMin(knownRetorts.Count + 1, MAX_OPTIONS_TOTAL);
			SetupRetortDialogOptions(maxOptions);
		}

		// WAIT FOR SETUP TO FINISH !!! ???
		// fixed - problem was loop repeating too many times

		int i = 0;
		// show all  available options (can be less than 5, max 5)
		for (i = 0; i < maxOptions; i++) {
			options[i].SetActive(true);
		}
		// show box
		mainScript.dialogImg.SetActive(true);
	}


	// Hide the dialog box
	void HideDialogOptions() {
		int i = 0;
		// hide all options
		for (i = 0; i < 5; i++) {
			options[i].GetComponent<Text>().text = "";
			options[i].SetActive(false);
		}
		// hide box
		mainScript.dialogImg.SetActive(false);
	}



	/* GENERAL FUNCTIONS */

	int GetMin(int num, int max) {
		if (num > max) {
			return max;
		}
		else {
			return num;
		}
	}

	void disableInteaction() {
		mainScript.hotspotsActive = false;
		mainScript.SetCursorInactive();
	}
	
	void enableInteraction() {
		mainScript.hotspotsActive = true;
	}
	
	void ChangeText(GameObject dialogBox, string newText) {
		dialogBox.GetComponent<Text>().text = newText;
	}


	/* INTRO */

	public void Intro() {
		StartCoroutine(IntroSequence());
	}
	
	IEnumerator IntroSequence() {
		introPlaying = true;
		Cursor.visible = false;
		yield return new WaitForSeconds(1.0f);
		gameObject.GetComponent<AudioSource>().Play();
		yield return new WaitForSeconds(3.0f);
		playerWords.GetComponent<Text>().text = "At last, I’ve arrived in the great city of Aspersia!";
		yield return new WaitForSeconds(2.8f);
		playerWords.GetComponent<Text>().text = "";
		yield return new WaitForSeconds(0.8f);
		playerWords.GetComponent<Text>().text = "Here lives the great master of insults, the one, the king.";
		yield return new WaitForSeconds(2.8f);
		playerWords.GetComponent<Text>().text = "";
		yield return new WaitForSeconds(0.8f);
		playerWords.GetComponent<Text>().text = "Many have challenged him. To this day, none have prevailed.";
		yield return new WaitForSeconds(2.8f);
		playerWords.GetComponent<Text>().text = "";
		yield return new WaitForSeconds(0.8f);
		playerWords.GetComponent<Text>().text = "I plan to change that.";
		yield return new WaitForSeconds(2.5f);
		playerWords.GetComponent<Text>().text = "";
		yield return new WaitForSeconds(1.0f);
		Cursor.visible = true;
		introPlaying = false;
		
		mainScript.bg.GetComponent<SpriteRenderer>().enabled = true;
		mainScript.idp.GetComponent<SpriteRenderer>().enabled = true;
		mainScript.sign.GetComponent<SpriteRenderer>().enabled = true;
		
		mainScript.idp.GetComponent<PolygonCollider2D>().enabled = true;
		
		yield return new WaitForSeconds(1.0f);
		mainScript.sign.GetComponent<FlashSign>().InitSign();
	}


	/* IDP */


	public void FirstIDPDialog() {
		StartCoroutine(IDPSequence(true));
	}
	
	public void IDPDialog() {
		StartCoroutine(IDPSequence(false));
	}

	void setGameModeRandom(float chancePlayerStarts) {
		int num = Random.Range(0, 10);
		float prob = (float) (num);

		/*
		Debug.Log("chancePlayerStarts = " + chancePlayerStarts);
		Debug.Log("num = " + num);
		Debug.Log("prob = " + prob);
		Debug.Log("chancePlayerStarts * 10 = " + chancePlayerStarts * 10);
		*/


		bool oldModeIsPlayerAsking = myGameState.modeIsPlayerAsking;
		//myGameState.modeIsPlayerAsking = !oldModeIsPlayerAsking;
		if (oldModeIsPlayerAsking) {
			chancePlayerStarts = 0.2f;
		}
		else {
			chancePlayerStarts = 0.8f;
		}
		Debug.Log("chancePlayerStarts = " + chancePlayerStarts);

		if (prob < chancePlayerStarts * 10 ) {
			myGameState.modeIsPlayerAsking = true;
		}
		else {
			myGameState.modeIsPlayerAsking = false;
		}


		Debug.Log("modeIsPlayerAsking = " + myGameState.modeIsPlayerAsking);
	}

	IEnumerator IDPSequence(bool firstTime) {
		disableInteaction();
		if (firstTime) {
			ChangeText(rhinoWords, "Welcome to Insult Duelling Practice 2.0.");
			yield return new WaitForSeconds(2.0f);
			ChangeText(rhinoWords, "");
			yield return new WaitForSeconds(0.5f);
			ChangeText(rhinoWords, "Generating random personality… Let’s begin!");
			yield return new WaitForSeconds(2.0f);
			ChangeText(rhinoWords, "");
			yield return new WaitForSeconds(0.5f);

			// first time IDP always starts
			myGameState.modeIsPlayerAsking = false;
			RandomIDPInsult();
		}
		else {
			ChangeText(rhinoWords, "Let's begin!");
			yield return new WaitForSeconds(2.5f);
			ChangeText(rhinoWords, "");
			yield return new WaitForSeconds(0.5f);

			// randomly decide who starts
			setGameModeRandom(0.6f);

			if (myGameState.modeIsPlayerAsking) {
				DisplayDialogOptions();
			}
			else {
				RandomIDPInsult();
			}
		}
	}

	void RandomIDPInsult() {

		int randInt;
		int chosen;

		int randKnow = Random.Range(0, 10);
		if (randKnow < 8) {

			if (unknownInsults.Count > 0) {
				randInt = Random.Range(0, unknownInsults.Count);
				//Debug.Log("randInt = " + randInt);
				//Debug.Log("unknownInsults.Count = " + unknownInsults.Count);
				chosen = unknownInsults[randInt];
			}
			else {
				randInt = Random.Range(0, knownInsults.Count);
				//Debug.Log("randInt = " + randInt);
				//Debug.Log("knownInsults.Count = " + knownInsults.Count);
				chosen = knownInsults[randInt];
			}
		}
		else {
			if (knownInsults.Count > 0) {
				randInt = Random.Range(0, knownInsults.Count);
				//Debug.Log("randInt = " + randInt);
				//Debug.Log("knownInsults.Count = " + knownInsults.Count);
				chosen = knownInsults[randInt];
			}
			else {
				randInt = Random.Range(0, unknownInsults.Count);
				//Debug.Log("randInt = " + randInt);
				//Debug.Log("unknownInsults.Count = " + unknownInsults.Count);
				chosen = unknownInsults[randInt];
			}
		}

		//int randInt = Random.Range(1, numberOfInsults + 1);

		myGameState.lastInsultGivenId = chosen;
		StartCoroutine(RandomInsultSequence(insultDict[chosen].insult));
		if (!knownInsults.Contains(chosen)) {
			knownInsults.Add(chosen);
			unknownInsults.Remove(chosen);
		}
	}
	
	IEnumerator RandomInsultSequence(string insult) {
		ChangeText(rhinoWords, insult);
		yield return new WaitForSeconds(2.5f);
		ChangeText(rhinoWords, "");
		yield return new WaitForSeconds(0.5f);
		DisplayDialogOptions();
	}


	/* RHINO */


	public void FirstRhinoDialog() {
		StartCoroutine(RhinoSequence(true));
	}
	
	public void RhinoDialog() {
		StartCoroutine(RhinoSequence(false));
	}

	IEnumerator RhinoSequence(bool firstTime) {
		disableInteaction();
		if (firstTime) {
			ChangeText(rhinoWords, "Your insults can’t harm me, I have the Hide Of The Rhinoceros(tm)!");
			yield return new WaitForSeconds(2.7f);
			ChangeText(rhinoWords, "");
			yield return new WaitForSeconds(0.5f);
			
			// rhino always starts
			RandomRhinoInsult();
		}
		else {
			ChangeText(rhinoWords, "Coming back for more?");
			yield return new WaitForSeconds(2.5f);
			ChangeText(rhinoWords, "");
			yield return new WaitForSeconds(0.5f);
			
			// rhino always starts
			RandomRhinoInsult();
		}
	}


	void RandomRhinoInsult() {

		int chosen;

		myGameState.modeIsPlayerAsking = false;

		chosen = Random.Range(1, numberOfInsults + 1);
		
		myGameState.lastInsultGivenId = chosen;
		StartCoroutine(RandomInsultSequence(insultDict[chosen].masterInsult));
	}



	/* CHECK BOSS TIME */

	void checkIfReadyForMaster() {
		Debug.Log("knownRetorts.Count = " + knownRetorts.Count);
		Debug.Log("numberOfInsults = " + knownRetorts.Count);
		if (knownRetorts.Count == numberOfInsults) {
			myGameState.askerIsRhino = true;
			StartCoroutine(PlayerReadyForMaster());
		}
	}

	IEnumerator PlayerReadyForMaster() {
		ChangeText(playerWords, "I have learnt much. The master will fall today, I’m sure.");
		yield return new WaitForSeconds(2.5f);
		ChangeText(playerWords, "");
		yield return new WaitForSeconds(0.5f);
		ChangeText(playerWords, "Summon the insult master! I challenge him to a duel.");
		yield return new WaitForSeconds(2.5f);
		ChangeText(playerWords, "");
		yield return new WaitForSeconds(0.5f);

		// Hide the IDP, show the rhino

		mainScript.idp.GetComponent<SpriteRenderer>().enabled = false;
		mainScript.idp.GetComponent<PolygonCollider2D>().enabled = false;

		mainScript.rhino.GetComponent<SpriteRenderer>().enabled = true;
		mainScript.rhino.GetComponent<PolygonCollider2D>().enabled = true;

		// play a sound
		mainScript.rhino.GetComponent<AudioSource>().Play();

	}


	/* PLAYER SPEECH */

	public void PlayerSpeak(string text) {
		if (myGameState.modeIsPlayerAsking) {
			StartCoroutine(PlayerAskSequence(text));
		}
		else {
			if (myGameState.askerIsRhino) {
				StartCoroutine(PlayerVsRhinoRetortSequence(text));
			}
			else {
				StartCoroutine(PlayerRetortSequence(text));
			}
		}
	}
	
	IEnumerator PlayerAskSequence(string text) {
		mainScript.SetCursorInactive();
		HideDialogOptions();
		ChangeText(playerWords, text);
		yield return new WaitForSeconds(2.5f);
		ChangeText(playerWords, "");
		yield return new WaitForSeconds(0.5f);
		answerPlayerInsult(0.8f);
	}

	IEnumerator PlayerVsRhinoRetortSequence(string text) {
		mainScript.SetCursorInactive();
		HideDialogOptions();
		ChangeText(playerWords, text);
		yield return new WaitForSeconds(2.5f);
		ChangeText(playerWords, "");
		yield return new WaitForSeconds(0.5f);
		if (myGameState.lastInsultGivenId == myGameState.playerLastRetortGivenId) {
			myGameState.numTimesBeatRhino = myGameState.numTimesBeatRhino + 1;
			ChangeText(rhinoWords, "That was an easy one..");
			yield return new WaitForSeconds(2.5f);
			ChangeText(rhinoWords, "");
			yield return new WaitForSeconds(0.5f);

			if (myGameState.numTimesBeatRhino == 3) {
				// victory!
				ChangeText(playerWords, "I win! I’m the new master!");
				yield return new WaitForSeconds(2.5f);
				ChangeText(playerWords, "");
				yield return new WaitForSeconds(0.5f);
				ChangeText(rhinoWords, "Hmm...");
				yield return new WaitForSeconds(1.5f);
				ChangeText(rhinoWords, "");
				yield return new WaitForSeconds(0.5f);
				ChangeText(rhinoWords, "Oh thank goodness. I’m free of this punishment. Enjoy your new life, kid.");
				yield return new WaitForSeconds(3.2f);
				ChangeText(rhinoWords, "");
				yield return new WaitForSeconds(0.5f);

				// game over
				gameOver = true;

				// hide the rhino
				mainScript.rhino.GetComponent<SpriteRenderer>().enabled = false;
				mainScript.rhino.GetComponent<PolygonCollider2D>().enabled = false;

				yield return new WaitForSeconds(1.5f);

				// play a sound?


				// show the victory screen + menu

				menuBg.SetActive(true);
				menuVicText.SetActive(true);
				menuText.SetActive(true);
				menuStartButton.SetActive(true);
				menuQuitButton.SetActive(true);

			}
			else {
				// start a new insult with the rhino right away
				RandomRhinoInsult();
			}
		}
		else {
			myGameState.numTimesBeatRhino = 0;
			ChangeText(rhinoWords, "Leave, scoundrel, you are defeated.");
			yield return new WaitForSeconds(2.2f);
			ChangeText(rhinoWords, "");
			yield return new WaitForSeconds(0.5f);
		}
		enableInteraction();
	}

	IEnumerator PlayerRetortSequence(string text) {
		mainScript.SetCursorInactive();
		HideDialogOptions();
		ChangeText(playerWords, text);
		yield return new WaitForSeconds(2.5f);
		ChangeText(playerWords, "");
		yield return new WaitForSeconds(0.5f);
		if (myGameState.lastInsultGivenId == myGameState.playerLastRetortGivenId) {
			ChangeText(rhinoWords, "You win!");
			yield return new WaitForSeconds(2.0f);
			ChangeText(rhinoWords, "");
			yield return new WaitForSeconds(0.5f);
		}
		else {
			ChangeText(rhinoWords, "I win!");
			yield return new WaitForSeconds(2.0f);
			ChangeText(rhinoWords, "");
			yield return new WaitForSeconds(0.5f);
		}
		enableInteraction();
		checkIfReadyForMaster();
	}


	void answerPlayerInsult(float chanceCorrectAnswer) {

		int num = Random.Range(0, 10);
		float prob = (float) (num);

		Debug.Log("playerLastInsultGivenId = " + myGameState.playerLastInsultGivenId);

		if (myGameState.playerLastInsultGivenId == -1) {
			StartCoroutine(DefeatSequence("Can't you do any better?"));
		}
		else {
			if (prob < chanceCorrectAnswer * 10) {
				StartCoroutine(DefeatSequence(insultDict[myGameState.playerLastInsultGivenId].retort));
				if (!knownRetorts.Contains(myGameState.playerLastInsultGivenId)) {
					knownRetorts.Add(myGameState.playerLastInsultGivenId);
				}
			}
			else {
				StartCoroutine(VictorySequence());
			}
		}
	}

	IEnumerator DefeatSequence(string text) {
		ChangeText(rhinoWords, text);
		yield return new WaitForSeconds(2.5f);
		ChangeText(rhinoWords, "");
		yield return new WaitForSeconds(0.5f);
		ChangeText(rhinoWords, "I win..");
		yield return new WaitForSeconds(2.0f);
		ChangeText(rhinoWords, "");
		yield return new WaitForSeconds(0.5f);
		enableInteraction();
		checkIfReadyForMaster();
	}

	IEnumerator VictorySequence() {
		ChangeText(rhinoWords, "You win..");
		yield return new WaitForSeconds(2.0f);
		ChangeText(rhinoWords, "");
		yield return new WaitForSeconds(0.5f);
		enableInteraction();
		checkIfReadyForMaster();
	}



}
