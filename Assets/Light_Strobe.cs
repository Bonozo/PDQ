using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class Light_Strobe : MonoBehaviour {


		public GameObject Light1;
		public GameObject Light2;

	

	void Start() {
		
	
		
	}
	void Update () {
		if(Input.GetKey(KeyCode.Z) == true)
		{
			Light1.SetActive (false);


	}
		if(Input.GetKey(KeyCode.Z) == false)
		{
			Light1.SetActive (true);
	}
		if(Input.GetKey(KeyCode.C) == true)
		{
			Light2.SetActive (false);


		}
		if(Input.GetKey(KeyCode.C) == false)
		{
			Light2.SetActive (true);
		}

}
}
