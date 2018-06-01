using UnityEngine;


public class Inventory {

	public bool addItem(string name) {
		if (checkItem(name) == 0) {
			PlayerPrefs.SetInt (name, 1);
            return true;
		} else {
            return false;
		}
	}

	public int checkItem(string name) {
		return PlayerPrefs.GetInt (name, 0);
	}

}