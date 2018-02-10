using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class DoorController : MonoBehaviour {

	public PlayableDirector director;
	public bool isDoorOpen = false;
	private bool  playingBackwards;
	public bool playing = false;
	// Use this for initialization
	void Start() {
		director = GetComponent<PlayableDirector>();
		director.Stop();
		director.time = isDoorOpen ?  director.playableAsset.duration - 0.01 : 0f ;
		director.Evaluate ();
	}

	// Update is called once per frame
	void Update() {
		if (playing) {
			if (playingBackwards) {
				double t = director.time - Time.deltaTime;
				if (t < 0)
					t = 0;

				director.time = t;
				director.Evaluate ();

				if (t == 0) {
					playing = false;
				}	
			} else {
				double t = director.time + Time.deltaTime;
				if (t > director.playableAsset.duration - 0.01)
					t = director.playableAsset.duration - 0.01;

				director.time = t;
				director.Evaluate ();

				if (t == director.playableAsset.duration - 0.01) {
					playing = false;
				}	
			}
		}
	}
	public void  ToggleDoor(){
		var duration = isDoorOpen ?  director.playableAsset.duration - 0.01 : 0f ;
		playingBackwards = isDoorOpen;
		director.time = duration;
		isDoorOpen = !isDoorOpen;
		playing = true;
	}

}
