using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WheelPointGenerator : MonoBehaviour {

	// when started, generate random point values for each segment whose value is not 100
	void Start () {
		ResetValues ();
	}

	public void ResetValues(){
		TextMeshPro[] tm = this.GetComponentsInChildren<TextMeshPro> ();

		for (var i = 0; i < tm.Length; i++) {
			if(!tm[i].text.Contains("100")){
				tm[i].text = (Random.Range(2,9) * 100).ToString();
			}
		}
	}
}
