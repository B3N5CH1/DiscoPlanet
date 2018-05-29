using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Inventory : MonoBehaviour {

	public void addItem(string name) {
		PlayerPrefs.SetInt (name, 1);
	}

}