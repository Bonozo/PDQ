using UnityEngine;
using System.Collections;

public class CameraOffsetCorrection : MonoBehaviour {

	public float xOffset;
	public float yOffset;
	public float zOffset;
	public MainLogic mainlogic;
	// Use this for initialization
	void Start () {
		if (mainlogic.BuildPlatform == MainLogic.Platform.Oculus) {
			gameObject.transform.Translate (-xOffset, -yOffset, -zOffset);
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
