using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class DimmerEffect : MonoBehaviour
{
    public bool onByDefault = true;
    public float offsetTiming = 0.25f;

	[Serializable]
	public class DimmerGroup
    {
        [ColorUsage(true,true,0f,8f,0.125f,3f)] public Color lightColor = new Color(0.0f, 0.0f, 1.0f, 1.0f);
        [Tooltip("Beats per minute")] [Range(0,200)] public int BPM;
		public List<GameObject> DimmerObjects;

        public float GetBeatsPerSecond()
        {
            return (BPM / 60.0f);
        }
	}

	public List<DimmerGroup> DimmerGroups;

    private bool isDimming = false;

    IEnumerator DimmerLightSine( DimmerGroup dimmerGroup )
    {
        while(true)
        {
            if(isDimming)
            {
                foreach (GameObject dimmerObject in dimmerGroup.DimmerObjects)
                {
                    Transform[] transforms = dimmerObject.GetComponentsInChildren<Transform>();

                    int i = 0;

                    foreach (Transform childTransform in transforms)
                    {
                        //Debug.Log(childTransform.gameObject.name);
                        Renderer renderer = childTransform.gameObject.GetComponent<Renderer>();
                        if( renderer )
                        {
                            Material material = renderer.material;
                        
                            Color emissionColor = dimmerGroup.lightColor;

                            float t = Time.time * (Mathf.PI * 2) * dimmerGroup.GetBeatsPerSecond();
                            float offset = i * offsetTiming;

                            emissionColor *= Mathf.Sin(t + offset);
                            material.SetColor( "_EmissionColor", emissionColor );
                        }

                        i++;
                    }
                }
                yield return null;

            }
            else
            {
                yield return null;
            }
        }
    }

    void Start()
    {
        foreach (DimmerGroup dimmerGroup in DimmerGroups)
        {
            StartCoroutine( DimmerLightSine(dimmerGroup) );
        }
    }
        
	void Update ()
    {
        isDimming = Input.GetButton("Fire1");

        // if button released, reset color
        if( Input.GetButtonUp("Fire1") )
        {
            foreach (DimmerGroup dimmerGroup in DimmerGroups)
            {
                foreach (GameObject dimmerObject in dimmerGroup.DimmerObjects)
                {
                    Transform[] transforms = dimmerObject.GetComponentsInChildren<Transform>();

                    foreach (Transform childTransform in transforms)
                    {
                        Renderer renderer = childTransform.gameObject.GetComponent<Renderer>();
                        if( renderer )
                        {
                            Material material = renderer.material;
                        
                            Color originalColor = dimmerGroup.lightColor;
                            material.SetColor( "_EmissionColor", originalColor );
                        }                            
                    }
                }
            }
        }
	}
}






