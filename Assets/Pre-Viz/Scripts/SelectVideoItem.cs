using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using RenderHeads.Media.AVProVideo;

public class SelectVideoItem : MonoBehaviour {

	public string VideoName;
	// Use this for initialization
	public ToggleGroup tg;
	void Start () {
		Debug.Log ("Start");
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void Select(){
		Debug.Log ("Click");
		if (tg.AnyTogglesOn()) {
			Debug.Log ("Toggle On");
			foreach (Toggle t in tg.ActiveToggles()) {
				Debug.Log ("Toggle Found");
//				t.gameObject.GetComponent<ResetVideoItem> ().mediaPlayer.OpenVideoFromFile (MediaPlayer.FileLocation.RelativeToStreamingAssetsFolder, VideoName, true);
				tg.SetAllTogglesOff ();
				//mediaPlayer.m_VideoPath = VideoName;
				//mediaPlayer.Play ();
			}
		}
	}
}
