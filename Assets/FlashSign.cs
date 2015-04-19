using UnityEngine;
using System.Collections;

public class FlashSign : MonoBehaviour {
	public Sprite sign1;
	public Sprite sign2;
	public Sprite sign3;
	private int num = 1;

	// Use this for initialization
	void Start () {

	}

	public void InitSign() {
		InvokeRepeating("CallFlash", 3, 4);
	}

	void CallFlash() {
		StartCoroutine(FlashingSign());
	}

	IEnumerator FlashingSign() {
		gameObject.GetComponent<AudioSource>().Play();
		gameObject.GetComponent<SpriteRenderer>().sprite = sign2;
		yield return new WaitForSeconds(0.3f);
		gameObject.GetComponent<SpriteRenderer>().sprite = sign3;
		yield return new WaitForSeconds(0.3f);
		gameObject.GetComponent<SpriteRenderer>().sprite = sign1;
	}

	void FlashingSign2() {
		if (num == 1) {
			// switch to sign 2
			num = 2;
			gameObject.GetComponent<SpriteRenderer>().sprite = sign2;
		}
		else if (num == 2) {
			// switch to sign 3
			num = 3;
			gameObject.GetComponent<SpriteRenderer>().sprite = sign3;
		}
		else if (num == 3) {
			// switch to sign 1
			num = 1;
			gameObject.GetComponent<SpriteRenderer>().sprite = sign1;
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}


}
