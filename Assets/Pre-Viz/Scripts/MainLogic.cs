 using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using RenderHeads.Media.AVProVideo;
using System;
using System.IO;
using UnityEngine.VR;
using UnityStandardAssets.Characters.FirstPerson;

//Previz Project Version5.3
public class MainLogic : MonoBehaviour {

    [Header("General Settings")]
    [Tooltip("Select the Build Platform")]
    public Platform BuildPlatform;
    [Tooltip("Select manual control type")] 
    public ManualControlType manualControlType;
	[Header("Background Audio")]
	[Tooltip("The audio that will be played over other scene sounds")]
	public AudioClip backgroundAudio;
	public bool isBackgroundAudioPlaying;
	private AudioSource backgroundAudioSource;
    [Header("Video Surfaces")]
	[Tooltip("Materials that will be used as Video Surfaces")]
	public List<Material> VideoMaterialSurface;

    [Header("Non Video Surfaces")]
	[Tooltip("Objects for regular textures")]
	public List<GameObject> ObjectList;

	[Header("Video Scenes")]
    [Tooltip("Each Video Scene is composed of its name and a list of Video Names")]
    public VideoSurfacesStruct[] Scenes;

	[Header("Camera Objects")]
    [Tooltip("Selectable Camera Objects")]
	public List<GameObject> Cameras;

    [Header("Other References")]
    [Tooltip("Defines incial starting point for First Person Player.")]
	public GameObject Player;
	public GameObject OculusSettings;

	[Header("Prefabs used for runtime instatiation")]
	public GameObject AVFProVideoPrefab;
	public GameObject SelectCameraItemPrefab;
	public GameObject VideoSurfaceItemPrefab;

	[HideInInspector]
	public MenuLogic menuLogic;

	[Header("Camera References")]
	public GameObject MainCamera;
	public GameObject FirstPersonCamera;
	public GameObject OculusCamera;

	[HideInInspector]
	public bool ManualControlActivated = false;

	[HideInInspector]
	public List<MediaPlayer> MediaPlayers;

	[Serializable]
	public struct VideoSurfacesStruct{
		public string VideoSceneName;
		public string[] VideoFiles;
		public Material[] OtherMaterials;
	}

    public enum Platform{NonVR, Oculus, Vive};
    public enum ManualControlType { Walking, FreeRoam };

    private int maxCameras = 8;
    private int maxMovieScenes = 8;

    private Vector3 InicialPlayerPosition;
    private Quaternion InicialPlayerRotation;
    

	void Awake(){
		foreach (GameObject camera in Cameras) {
			Debug.Log ("Camera: " + camera.name + "Original Rotation: " + camera.transform.rotation.eulerAngles);
		}
		LoadCameras ();
	}

	#region Start
    void Start ()
    {
		backgroundAudioSource = gameObject.AddComponent<AudioSource> ();
		MainCamera = GameObject.Find ("Main Camera").gameObject;
		menuLogic = gameObject.GetComponentInChildren<MenuLogic> (true);
		menuLogic.OVRPlayerReticle = GameObject.Find ("CanvasReticle");
        
       
        StartMainCamera ();
		LoadVideoPlayers ();
		//LoadCameras ();
		LoadVideoSurfaceItems ();
		DisableAllCameriasInScene ();
        DisableCameras();
        menuLogic.CloseMenu ();
		StartObjectMaterials ();
		MainCamera.SetActive (true);

        switch (BuildPlatform)
        {
		case Platform.NonVR:
			FirstPersonCamera.SetActive (true);
			OculusSettings.SetActive (false);
                break;
		case Platform.Oculus:
			FirstPersonCamera.SetActive (true);
			OculusSettings.SetActive (true);
                MainCamera.gameObject.GetComponent<VRMouseLook>().enabled = false;
                break;
		case Platform.Vive:
			OculusSettings.SetActive (false);
                break;
        }
        if (Cameras.Count == 0)
        {
            ManualControl();
        }
        InicialPlayerPosition = Player.gameObject.transform.position;
        InicialPlayerRotation = Player.gameObject.transform.rotation;
    }

    public void Update()
    {

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("Quit");
            Application.Quit();
        }
        if (Input.GetKeyDown(KeyCode.R) || Input.GetKeyDown(KeyCode.JoystickButton6))
        {
            //Player.gameObject.transform = PlayerInicialPosition;
            Debug.Log("Reset");
           
            Player.gameObject.transform.position = InicialPlayerPosition;
            Player.gameObject.transform.rotation = InicialPlayerRotation;
        }
    }
  
	private void StartMainCamera(){
		Player = GameObject.Find ("PlayerControllers");
        GameObject StandardPlayer = Player.transform.Find("StandardPlayerController").gameObject;
        GameObject OculusPlayer = Player.transform.Find("OculusPlayerController").gameObject;
        switch (BuildPlatform)
        {
            case Platform.NonVR:
                Player = StandardPlayer;
                OculusPlayer.SetActive(false);
                StandardPlayer.SetActive(true);
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
                FirstPersonCamera.SetActive(true);
                if(manualControlType == ManualControlType.Walking)
                {
                    StandardPlayer.GetComponent<FirstPersonController>().FlyMode = false;
                }
                else if(manualControlType == ManualControlType.FreeRoam)
                {
                    StandardPlayer.GetComponent<FirstPersonController>().FlyMode = true;
                }
                
                Debug.Log("Standard Version Activated");
                break;
            case Platform.Oculus:
                //Player = OculusPlayer;
                //StandardPlayer.SetActive(false);
                //OculusPlayer.SetActive(true);
                //MainCamera.GetComponent<VRMouseLook>().enabled = false;
                //OculusCamera.SetActive(true);
                Player = StandardPlayer;
                OculusPlayer.SetActive(false);
                StandardPlayer.SetActive(true);
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
                FirstPersonCamera.SetActive(true);
                if(manualControlType == ManualControlType.Walking)
                {
                    StandardPlayer.GetComponent<FirstPersonController>().FlyMode = false;
                }else if(manualControlType == ManualControlType.FreeRoam)
                {
                    StandardPlayer.GetComponent<FirstPersonController>().FlyMode = true;
                }
                Debug.Log("Oculus Version Activated");
                break;
            case Platform.Vive:
                break;
        }
	}

    private void LoadVideoPlayers()
    {
        int surfaceNumber = 0;
        foreach (Material m in VideoMaterialSurface)
        {

            //if (VideoSurfaces[0].VideoFiles.Length > surfaceNumber)
            //{
                AVFProVideoPrefab.GetComponent<ApplyToMaterial>()._material = m;
                GameObject mediaPlayer = Instantiate(AVFProVideoPrefab);
            string videoName = "";
            if (Scenes[0].VideoFiles.Length > surfaceNumber)
            {
                videoName = GetVideoName(Scenes[0].VideoFiles[surfaceNumber]);
            }
                Debug.Log("Vide Name:" + videoName);
                mediaPlayer.GetComponent<MediaPlayer>().OpenVideoFromFile(MediaPlayer.FileLocation.RelativeToStreamingAssetsFolder, videoName, true);
                surfaceNumber++;
                MediaPlayers.Add(mediaPlayer.GetComponent<MediaPlayer>());
           // }
        }
    }

    private void LoadCameras()
    {
        GameObject SelectCameraContent = GameObject.Find("SelectCameraContent");
        int count = 0;
        foreach (GameObject camera in Cameras)
        {
            if (camera != null && camera.GetComponent<Camera>() && count < maxCameras)
            {
                GameObject SelectCameraItem = GameObject.Instantiate(SelectCameraItemPrefab);
                SelectCameraItem.transform.SetParent(SelectCameraContent.transform);
                SelectCameraItem.transform.localScale = new Vector3(1f, 1f, 1f);
                SelectCameraItem.GetComponent<SelectCameraItem>().camera = camera.GetComponent<Camera>();
                SelectCameraItem.GetComponent<SelectCameraItem>().Name.text = camera.name;
                SelectCameraItem.GetComponent<SelectCameraItem>().originalPosition = camera.transform.position;
                SelectCameraItem.GetComponent<SelectCameraItem>().originalRotation = camera.transform.eulerAngles;
				Debug.Log ("Camera: " + camera.name + "OriginalRotation: " + camera.transform.rotation.eulerAngles);
                SelectCameraItem.GetComponent<SelectCameraItem>().Position.text = "Position: " + Convert.ToInt32(camera.transform.position.x) + ", "
                    + Convert.ToInt32(camera.transform.position.y) + ", " + Convert.ToInt32(camera.transform.position.z);
                count++; 
            }
        }
    }

    private void LoadVideoSurfaceItems()
    {
        GameObject SelectVideoContent = GameObject.Find("SelectVideoContent");
        int count = 0;
        foreach (VideoSurfacesStruct videoStruct in Scenes)
        {
            if (count < maxMovieScenes)
            {
                GameObject videoSurfaceItem = GameObject.Instantiate(VideoSurfaceItemPrefab);
                videoSurfaceItem.transform.SetParent(SelectVideoContent.transform);
                videoSurfaceItem.transform.localScale = new Vector3(1f, 1f, 1f);
                videoSurfaceItem.GetComponent<VideoSurfaceItem>().NameText.text = videoStruct.VideoSceneName;
                videoSurfaceItem.GetComponent<VideoSurfaceItem>().VideoNames = ConvertVideoNames(videoStruct.VideoFiles);
                videoSurfaceItem.GetComponent<VideoSurfaceItem>().OtherMaterials = videoStruct.OtherMaterials;
                count++;
            }
        }
    }

    public void DisableAllCameriasInScene()
    {
        foreach (Camera camera in GameObject.FindObjectsOfType<Camera>())
        {
            camera.gameObject.SetActive(false);
        }
    }

    public void DisableCameras(){
		foreach (GameObject camera in Cameras) {
            if (camera != null)
            {
                camera.SetActive(false);
            }
		}
		Player.SetActive (false);
	}

    public void StartObjectMaterials()
    {
        int count = 0;
        foreach (GameObject g in ObjectList)
        {
            if (Scenes.Length > 0 && Scenes[0].OtherMaterials.Length > count)
            {
                if (g != null && Scenes != null && Scenes[0].OtherMaterials != null)
                {
                    g.GetComponent<Renderer>().material = Scenes[0].OtherMaterials[count];
                }
            }
            count++;
        }
    }

    #endregion

    #region MenuFunctions
    public void SelectCamera(SelectCameraItem CameraItem){
		StartCoroutine(SelectCameraAfterFade (CameraItem));
	}

    public void SelectVideoItem(string[] VideoNames, Material[] OtherMaterials)
    {
        int count = 0;
        foreach (MediaPlayer g in MediaPlayers)
        {
            if (VideoNames.Length > count)
            {
                Debug.Log("VideoName: " + count + VideoNames[count]);
                g.OpenVideoFromFile(MediaPlayer.FileLocation.RelativeToStreamingAssetsFolder, VideoNames[count], true);
                Debug.Log("Debug: Play " + VideoNames[count]);
            }else//Video Empty
            {
                g.OpenVideoFromFile(MediaPlayer.FileLocation.RelativeToStreamingAssetsFolder, "", true);
            }
            count++;
        }
        int count2 = 0;
        foreach (GameObject obj in ObjectList)
        {
            if (OtherMaterials.Length > count2)
            {
                if (OtherMaterials[count2] != null && obj != null)
                {
                    obj.GetComponent<Renderer>().material = OtherMaterials[count2];
                }
            }
            count2++;
        }

        menuLogic.CloseMenu();
    }

    public IEnumerator SelectCameraAfterFade(SelectCameraItem CameraItem){

			Debug.Log ("Start Fade Out");
			yield return MainCamera.GetComponent<OVRScreenFade> ().FadeOut ();
			Debug.Log ("Start Fade In");


        //gameObject.transform.position = Camera.transform.position;//IMPORTANTE
        gameObject.transform.Find("MenuSystem").position = CameraItem.originalPosition;

        if (BuildPlatform == Platform.NonVR)
        {
            gameObject.transform.Find("MenuSystem").eulerAngles = CameraItem.originalRotation;
            MainCamera.gameObject.GetComponent<VRMouseLook>().ResetCamera();
        } else if (BuildPlatform == Platform.Oculus){

			//gameObject.transform.Find ("MenuSystem").localEulerAngles.Set (0, CameraItem.originalRotation.y, 0);
           
			gameObject.transform.Find ("MenuSystem").eulerAngles = new Vector3 (0, CameraItem.originalRotation.y, 0);//CameraItem.originalRotation;
			Debug.Log("Rotate MenuSystem to: 0, " + CameraItem.originalRotation.y + ", 0");

		//	float y = gameObject.transform.Find ("MenuSystem").localEulerAngles.y;
		//	gameObject.transform.Find ("MenuSystem").localEulerAngles.Set (0, y, 0);
         //   MainCamera.gameObject.GetComponent<VRMouseLook>().ResetCamera();
			//gameObject.transform.Find ("MenuSystem").localEulerAngles.Set (0, y, 0);
		

			//Test Code.
			//UnityEngine.VR.InputTracking.Recenter();


        }


		menuLogic.CloseMenu ();
		ManualControlActivated = false;
		MainCamera.SetActive (true);
		Player.SetActive (false);
		menuLogic.SetInputCamera (MainCamera.GetComponent<Camera> ());
		transform.rotation = new Quaternion (0f, 180f, 0f, 0f);
		Debug.Log ("Finish FadeOut");
		MainCamera.GetComponent<OVRScreenFade> ().FadeBackIn ();
       
	}

	public void RestartVideos(){
		foreach (MediaPlayer mp in MediaPlayers) {
			mp.Rewind (false);
		}
	}

	public void ManualControlOpenMenu(){
		

		if (BuildPlatform == Platform.Oculus) {
            gameObject.transform.Find("MenuSystem").position = Player.GetComponentInChildren<Camera>().gameObject.transform.position;
            gameObject.transform.Find("MenuSystem").rotation = Player.GetComponentInChildren<Camera>().gameObject.transform.rotation;
            gameObject.transform.Find("MenuSystem").rotation = Quaternion.Euler(0f, gameObject.transform.Find("MenuSystem").rotation.eulerAngles.y, gameObject.transform.Find("MenuSystem").rotation.eulerAngles.z);
            Player.GetComponent<FirstPersonController>().m_WalkSpeed = 0f;
            Player.GetComponent<FirstPersonController>().m_RunSpeed = 0f;
            Player.gameObject.GetComponentInChildren<Canvas>(true).gameObject.SetActive(true);
        } else if(BuildPlatform == Platform.NonVR){
			gameObject.transform.Find("MenuSystem").position = Player.GetComponentInChildren<Camera> ().gameObject.transform.position;
			gameObject.transform.Find ("MenuSystem").rotation = Player.GetComponentInChildren<Camera> ().gameObject.transform.rotation;
			gameObject.transform.Find ("MenuSystem").rotation = Quaternion.Euler (0f, gameObject.transform.Find ("MenuSystem").rotation.eulerAngles.y, gameObject.transform.Find ("MenuSystem").rotation.eulerAngles.z);
			Player.GetComponent<FirstPersonController> ().m_WalkSpeed = 0f;
			Player.GetComponent<FirstPersonController> ().m_RunSpeed = 0f;
			Player.gameObject.GetComponentInChildren<Canvas>(true).gameObject.SetActive(true);
		}
	}

	public void ManualControlCloseMenu(){
		if (BuildPlatform == Platform.Oculus) {
            Player.GetComponent<FirstPersonController>().m_WalkSpeed = 5f;
            Player.GetComponent<FirstPersonController>().m_RunSpeed = 10f;
            Player.gameObject.GetComponentInChildren<Canvas>(true).gameObject.SetActive(false);
            Player.GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>().enabled = true;
        } else if(BuildPlatform == Platform.NonVR) {
			Player.GetComponent<FirstPersonController> ().m_WalkSpeed = 5f;
			Player.GetComponent<FirstPersonController> ().m_RunSpeed = 10f;
			Player.gameObject.GetComponentInChildren<Canvas>(true).gameObject.SetActive(false);
		}
	}

	public void ManualControl(){
		DisableCameras ();
		MainCamera.SetActive (false);
		Player.SetActive (true);
		ManualControlActivated = true;
		menuLogic.SetInputCamera (Player.GetComponentInChildren<Camera> ());
		if (BuildPlatform == Platform.Oculus) {
            Player.GetComponent<FirstPersonController>().m_WalkSpeed = 5f;
            Player.GetComponent<FirstPersonController>().m_RunSpeed = 10f;
            Player.gameObject.GetComponentInChildren<Canvas>(true).gameObject.SetActive(false);
            Player.GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>().enabled = true;

        } else if (BuildPlatform == Platform.NonVR){
			Player.GetComponent<FirstPersonController> ().m_WalkSpeed = 5f;
			Player.GetComponent<FirstPersonController> ().m_RunSpeed = 10f;
			Player.gameObject.GetComponentInChildren<Canvas>(true).gameObject.SetActive(false);

		}
		menuLogic.OVRPlayerReticle.SetActive (false);
        
	}
    #endregion


    #region Helpers
    private string GetVideoName(string name){
		DirectoryInfo dir = new DirectoryInfo (Application.streamingAssetsPath);
		FileInfo[] info = dir.GetFiles ();
		foreach (FileInfo file in info) {
           // if (file.Name.Contains(name) && file.Extension != "meta") {
            //Debug.Log("File Name " + file.Name.Split('.')[0] + "==" + name);
            if(file.Name.Split('.')[0] == name && file.Name.Split('.').Length == 2) { 
			//	Debug.Log (file.Name);
				return file.Name;
            }

		}
		return "Error: File Not in Streaming Assets";
	}

	private string[] ConvertVideoNames(string[] videoFiles){
		string[] names = new string[videoFiles.Length];
		for(int i = 0; i<videoFiles.Length; i++){
			names [i] = GetVideoName (videoFiles [i]);
		}
		return names;
	}

    private bool VideoExists(string VideoName)
    {
        DirectoryInfo dir = new DirectoryInfo(Application.streamingAssetsPath);
        FileInfo[] info = dir.GetFiles();
        foreach(FileInfo file in info)
        {
            if(file.Name.Contains(VideoName) && file.Extension != "meta")
            {
                return true;
            }
        }
        return false;
    }
    #endregion
}
