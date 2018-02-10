// © 2015 Mario Lelas
using UnityEngine;


namespace MLSpace
{
    /// <summary>
    /// Pauses and unpauses game
    /// Toggles controls info text
    /// Sets player hit interval 
    /// </summary>
    public class GameControl : MonoBehaviour
    {
        // Update is called once per frame
        void Update()
        {
            // quit 
            if (Input.GetKeyDown (KeyCode.Escape))
                Application.Quit();

            // restart
            if (Input.GetButtonDown("Submit"))
                Application.LoadLevel(0);
        }
    } 
}
