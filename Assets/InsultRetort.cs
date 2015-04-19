using UnityEngine;
using System.Collections;

public class InsultRetort {

	public string insult;
	public string retort;
	public string masterInsult;
	public int id;

	public InsultRetort(string newInsult, string newRetort, string newMasterInsult, int newId) {
		insult = newInsult;
		retort = newRetort;
		masterInsult = newMasterInsult;
		id = newId;
	}

	public int getId() {
		return id;
	}

}
