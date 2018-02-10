using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class StrobeEffect : MonoBehaviour
{
	[Serializable]
	public class StrobeGroup
    {
        public bool onByDefault = true;
        [Tooltip("PWM would be the percentage of the time light is ON vs OFF.")] [Range(0.0f,1.0f)] public float PWM;
        [Tooltip("Beats per minute")] [Range(0,200)] public int BPM;
        public float offsetTiming = 0.25f;
		public List<GameObject> StrobeObjects;

        public float GetStrobeDuration()
        {
            return (60.0f / BPM);
        }
	}

	public List<StrobeGroup> StrobeGroups;

    private bool isStrobing = false;

    IEnumerator StrobeLightFlash( StrobeGroup strobeGroup )
    {
        while(true)
        {
            if(isStrobing)
            {
                foreach (GameObject strobeObject in strobeGroup.StrobeObjects)
                {
                    Transform[] transforms = strobeObject.GetComponentsInChildren<Transform>();

                    int i = 0;

                    foreach (Transform childTransform in transforms)
                    {
                        //Debug.Log(childTransform.gameObject.name);
                        Renderer renderer = childTransform.gameObject.GetComponent<Renderer>();
                        if( renderer )
                        {
                            float offset = i * strobeGroup.offsetTiming;
                            float t = (Time.time + offset) % strobeGroup.GetStrobeDuration();

                            bool show = t < (strobeGroup.GetStrobeDuration() * 0.5f);
                            show = show ^ strobeGroup.onByDefault;

                            renderer.enabled = show;
                        }

                        i++;
                    }
                }
            }
            yield return null;
        }
    }

    void Start()
    {
        foreach (StrobeGroup strobeGroup in StrobeGroups)
        {
           StartCoroutine( StrobeLightFlash(strobeGroup) );
        }
    }
        
	void Update ()
    {
        isStrobing = Input.GetButton("Fire2");

        // if button released, reset strobe
        if( isStrobing == false )
        {
            foreach (StrobeGroup strobeGroup in StrobeGroups)
            {
                foreach (GameObject strobeObject in strobeGroup.StrobeObjects)
                {
                    Transform[] transforms = strobeObject.GetComponentsInChildren<Transform>();

                    foreach (Transform childTransform in transforms)
                    {
                        Renderer renderer = childTransform.gameObject.GetComponent<Renderer>();
                        if( renderer )
                        {
                            renderer.enabled = strobeGroup.onByDefault;
                        }
                    }
                }
            }
        }

	}
}






