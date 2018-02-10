using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using RenderHeads.Media.AVProVideo;

public class MenuLogic : MonoBehaviour {

	public GameObject MainMenu;
	public GameObject ChangeVideoMenu;
	public GameObject ChangeCameraMenu;
	public GameObject Reticle;
	public MainLogic mainLogic;
	public Scrollbar ChangeCameraScrollBar;
	public Scrollbar ChangeVideoScrollBar;
	public float ScrollSpeed = 0.01f;

	public AudioSource backgroundAudioSource;
	[HideInInspector]
	public GameObject OVRPlayerReticle;

	void Start () {
		if (backgroundAudioSource == null) {
			backgroundAudioSource = gameObject.AddComponent<AudioSource> ();
		}
		backgroundAudioSource.loop = true;
		Cursor.visible = false;
		Cursor.lockState = CursorLockMode.Confined;
	}

	void Update () {
		if(Input.GetKeyDown(KeyCode.JoystickButton1) || Input.GetKeyDown(KeyCode.Backspace) || Input.GetKeyDown(KeyCode.Mouse1)){//B-Button or Backspace or right click
			if (ChangeVideoMenu.activeInHierarchy || ChangeCameraMenu.activeInHierarchy) {
				SelectMainMenu ();
			} else if (MainMenu.activeInHierarchy) {
				CloseMenu ();
			}
		}
		if (Input.GetKeyDown (KeyCode.JoystickButton7) || Input.GetKeyDown(KeyCode.Return)) {//Start-Button or Enter 
			if (ChangeVideoMenu.activeInHierarchy || ChangeCameraMenu.activeInHierarchy || MainMenu.activeInHierarchy) {
				CloseMenu ();
			} else {
				if (mainLogic.ManualControlActivated) {
					mainLogic.ManualControlOpenMenu ();
				} 
				SelectMainMenu ();
			}
		}
		if(Input.GetAxis("Vertical") != 0f){
			if (ChangeVideoMenu.activeInHierarchy) {
				ChangeVideoScrollBar.value += Input.GetAxis ("Vertical") * ScrollSpeed;
			}
			if (ChangeCameraMenu.activeInHierarchy) {
				ChangeCameraScrollBar.value += Input.GetAxis ("Vertical") * ScrollSpeed;
			}
		}

	}

	public void SelectMainMenu(){
        if (mainLogic.BuildPlatform == MainLogic.Platform.NonVR)
        {
            MainMenu.SetActive(true);
            ChangeVideoMenu.SetActive(false);
            ChangeCameraMenu.SetActive(false);
            Reticle.SetActive(true);
            if (mainLogic.ManualControlActivated)
            {
                gameObject.transform.Find("Menu").position = mainLogic.Player.gameObject.transform.position;
                gameObject.transform.Find("Menu").Translate(0, 0.5f, 0);
                gameObject.transform.Find("Menu").rotation = mainLogic.Player.gameObject.transform.rotation;
                gameObject.transform.Find("Menu").rotation = Quaternion.Euler(0f, gameObject.transform.Find("Menu").rotation.eulerAngles.y, gameObject.transform.Find("Menu").rotation.eulerAngles.z);

            }
            else
            {
                gameObject.transform.Find("Menu").position = mainLogic.MainCamera.gameObject.transform.position;
                gameObject.transform.Find("Menu").rotation = mainLogic.MainCamera.gameObject.transform.rotation;
                gameObject.transform.Find("Menu").rotation = Quaternion.Euler(0f, gameObject.transform.Find("Menu").rotation.eulerAngles.y, gameObject.transform.Find("Menu").rotation.eulerAngles.z);

            }
        }else if(mainLogic.BuildPlatform == MainLogic.Platform.Oculus)
        {

            MainMenu.SetActive(true);
            ChangeVideoMenu.SetActive(false);
            ChangeCameraMenu.SetActive(false);
            Reticle.SetActive(true);
            mainLogic.Player.GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>().enabled = false;
            if (mainLogic.ManualControlActivated)
            {
                gameObject.transform.Find("Menu").position = mainLogic.Player.gameObject.transform.position;
                gameObject.transform.Find("Menu").Translate(0, 0.5f, 0);

                gameObject.transform.Find("Menu").rotation = mainLogic.Player.gameObject.transform.GetComponentInChildren<Camera>().gameObject.transform.rotation;//mainLogic.Player.gameObject.transform.rotation;

                gameObject.transform.Find("Menu").rotation = Quaternion.Euler(0f, gameObject.transform.Find("Menu").rotation.eulerAngles.y, 0f);// gameObject.transform.Find("Menu").rotation.eulerAngles.z);

            }
            else
            {
                gameObject.transform.Find("Menu").position = mainLogic.MainCamera.gameObject.transform.position;
                // gameObject.transform.Find("Menu").rotation = mainLogic.MainCamera.gameObject.transform.rotation;
                //gameObject.transform.Find("Menu").rotation = Quaternion.Euler(0f, gameObject.transform.Find("Menu").rotation.eulerAngles.y, gameObject.transform.Find("Menu").rotation.eulerAngles.z);
                gameObject.transform.Find("Menu").rotation = mainLogic.MainCamera.gameObject.transform.rotation;//gameObject.transform.GetComponentInChildren<Camera>().gameObject.transform.rotation;
                gameObject.transform.Find("Menu").rotation = Quaternion.Euler(0f, gameObject.transform.Find("Menu").rotation.eulerAngles.y, 0f);
            }
        }

//		gameObject.transform.Find("MainMenu").position = mainLogic.MainCamera.GetComponentInChildren<Camera> ().gameObject.transform.position;
//		gameObject.transform.Find ("MainMenu").Translate (0f, 0f, 2f);
//		gameObject.transform.Find("MainMenu").rotation = mainLogic.MainCamera.GetComponentInChildren<Camera> ().gameObject.transform.rotation;
//		gameObject.transform.Find("MainMenu").rotation = Quaternion.Euler (0f, gameObject.transform.Find("MainMenu").rotation.eulerAngles.y, gameObject.transform.Find("MainMenu").rotation.eulerAngles.z);
	}

    #region MenuFunctions
	public void ToggleBackgroundAudio(){
		mainLogic.isBackgroundAudioPlaying = !mainLogic.isBackgroundAudioPlaying;

		if (mainLogic.isBackgroundAudioPlaying) {
			if (!backgroundAudioSource.isPlaying) {
				var others = GameObject.FindObjectsOfType<AudioSource> ();
				foreach (AudioSource a in others) {
					a.Stop ();
				}
				backgroundAudioSource.clip = mainLogic.backgroundAudio;
				backgroundAudioSource.Play ();
			}
		} else {
			backgroundAudioSource.Stop ();
		}
	
	}
    public void SelectChangeVideo(){
		MainMenu.SetActive (false);
		ChangeVideoMenu.SetActive (true);
		ChangeCameraMenu.SetActive (false);
		Reticle.SetActive (true);
	}
	public void SelectChangeCamera(){
		MainMenu.SetActive (false);
		ChangeVideoMenu.SetActive (false);
		ChangeCameraMenu.SetActive (true);
		Reticle.SetActive (true);
	}
	public void SelectRestartVideo(){
		MainMenu.SetActive (false);
		ChangeVideoMenu.SetActive (false);
		ChangeCameraMenu.SetActive (false);
		Reticle.SetActive (false);
		mainLogic.RestartVideos ();
		if (mainLogic.ManualControlActivated) {
			mainLogic.ManualControlCloseMenu ();
		}
	}
	public void SelectManualControl(){
		MainMenu.SetActive (false);
		ChangeVideoMenu.SetActive (false);
		ChangeCameraMenu.SetActive (false);
		Reticle.SetActive (false);
		mainLogic.ManualControl ();
	}
	public void CloseMenu(){
		MainMenu.SetActive (false);
		ChangeVideoMenu.SetActive (false);
		ChangeCameraMenu.SetActive (false);
		Reticle.SetActive (false);
		if (mainLogic.ManualControlActivated) {
			mainLogic.ManualControlCloseMenu ();
		}
        mainLogic.Player.GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>().enabled = true;
    }
    #endregion

    #region Helpers
    public void SetInputCamera(Camera camera){
		MainMenu.GetComponent<Canvas> ().worldCamera = camera;
		ChangeVideoMenu.GetComponent<Canvas> ().worldCamera = camera;
		ChangeCameraMenu.GetComponent<Canvas> ().worldCamera = camera;
	}
    #endregion
}
