using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SelectCameraItem : MonoBehaviour {

	public Text Name;
	public Text Position;

	[HideInInspector]
	public Camera camera;
    [HideInInspector]
    public Vector3 originalPosition;
    [HideInInspector]
    public Vector3 originalRotation;

	public void SelectCamera(){
		MainLogic mainLogic = gameObject.GetComponentInParent<MainLogic> ();
		mainLogic.SelectCamera (this);	
	}
}
