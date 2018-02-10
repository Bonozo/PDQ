using UnityEngine;
using System.Collections;

public class LEDPixelSystem : MonoBehaviour {

    public Material LEDPixelMaterial;
    [ColorUsageAttribute(true,true,0f,8f,0.125f,3f)] public Color fromColor= new Color(0.0f, 0.0f, 1.0f, 1.0f);
    [ColorUsageAttribute(true,true,0f,8f,0.125f,3f)] public Color toColor= new Color(1.0f, 0.0f, 0.0f, 1.0f);
    [Range(.1f, 10f)] public float speed = 1.0f;
    public bool sinewave = true;
    private float value = 0.0f;
    private int direction = 1;

	// Use this for initialization
	void Start ()
    {
	       
	}
	
	// Update is called once per frame
	void Update ()
    {
        value += Time.deltaTime * direction * speed;

        if ( value > 1 && direction > 0 )
        {
            direction = -1;
            value = 1;
        }
        else if ( value < 0 && direction < 0 )
        {
            direction = 1;
            value = 0;
        }

        float lerp = value;
        if( sinewave )
        {
            lerp = Mathf.Sin( Mathf.PI * value );
        }

        Color newColor = Color.Lerp( fromColor, toColor, lerp );
        LEDPixelMaterial.SetColor( "_EmissionColor", newColor );
	}
}
