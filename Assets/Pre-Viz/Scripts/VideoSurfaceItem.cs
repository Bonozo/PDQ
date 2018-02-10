using UnityEngine;
using System.Collections;
using RenderHeads.Media.AVProVideo;
using System.Collections.Generic;
using UnityEngine.UI;

public class VideoSurfaceItem : MonoBehaviour {

	public Text NameText;

	[HideInInspector]
	public string[] VideoNames;

	[HideInInspector]
	public Material[] OtherMaterials;

	[HideInInspector]
	public MainLogic mainLogic;

	void Start () {
		mainLogic = gameObject.GetComponentInParent<MainLogic> ();
	}

    public void SelectVideoItem()
    {
        mainLogic.SelectVideoItem(VideoNames, OtherMaterials);
    }
}
