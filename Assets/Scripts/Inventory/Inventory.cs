using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Inventory : MonoBehaviour {

	public void addItem(string name) {
		if (checkItem(name) == 0) {
			PlayerPrefs.SetInt (name, 1);
		} else {

		}
	}

	public int checkItem(string name) {
		return PlayerPrefs.GetInt (name, 0);
	}

	public bool dialog(string name) {
		bool added = false;

		return added;
	}

}