using UnityEngine;


public class Inventory {

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
        addItem(name);

		return true;
	}

}