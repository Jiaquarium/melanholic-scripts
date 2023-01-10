using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Experimental.Rendering.Universal;
 
[ExecuteAlways]
public class Dev_SpecsDisplay : MonoBehaviour
{
	[SerializeField] private Camera cam;
	[SerializeField] private Script_GraphicsManager graphics;
	[SerializeField] private PixelPerfectCamera pixelPerfectCamera;
	[SerializeField] private float fpsRefreshTimer;
	[SerializeField] private bool isFixedBigFont;
	[SerializeField] private int fixedFontSize;
	[SerializeField] private bool isStartHidden;

	[SerializeField] private bool isSingleLineDisplay;

	[SerializeField] private Dev_GameHelper gameHelper;
	
	private float fps;
	private float frameTimeMs;
	private float timer;
	private string version;
	private bool isHidden;
 
	void Awake()
    {
        cam = GetComponent<Camera>();
		this.enabled = Const_Dev.IsSpecsDisplayOn || Const_Dev.IsPublisherSpecsOn;

		version = Version();

		isHidden = isStartHidden;
    }
	
	void Update()
	{
		timer -= Time.unscaledDeltaTime;

		if (timer <= 0)
		{
			RefreshFrameData();
			timer = fpsRefreshTimer;
		}

		HandleSpecsDevInput();

		if (Const_Dev.IsTrailerMode)
			HandleTrailerDevInput();
		
		if (Const_Dev.IsDevMode || Debug.isDebugBuild)
			HandleDevInput();
	}
 
	void OnGUI()
	{
        if (isHidden)
			return;
		
		int w = Screen.width, h = Screen.height;
 
		GUIStyle style = new GUIStyle();
 
		int fontSize = isFixedBigFont ? fixedFontSize : h * 1 / 75;
		Rect rect = new Rect(0, 0, w, fontSize * 2);
		style.alignment = TextAnchor.UpperRight;
		style.fontSize = fontSize;
		style.normal.textColor = new Color (255f, 255f, 255f, 1.0f);
		
        string label;

		if (Const_Dev.IsTrailerMode)
		{
			label =
TrailerModeSpecs();
		}
		else if (Const_Dev.IsPublisherSpecsOn)
		{
			label =
@$"{version}";
		}
		else
		{
			label =
@$"{FrameData()}, {VSyncData()}
{Resolution()}
{DisplayInfo()}
{PixelPerfectData()}
ortho: {CameraSize()}
{ScreenMode()}
{BuildText()}
{version}";
		}

		GUI.Label(rect, label, style);
	}

	private string TrailerModeSpecs() => isSingleLineDisplay ?
		@$"{Fps()}, {CameraSize()}x, pxRt:{PixelRatio()}x, {VP()}, lt:{Script_LightFXManager.Control?.CurrentIntensity.ToString()}"
		: @$"{Fps()}
{CameraSize()}x
pxRt:{PixelRatio()}x
{VP()}
lt:{Script_LightFXManager.Control?.CurrentIntensity.ToString()}";

	private string Version() => $"v{Application.version}";
	
	private void RefreshFrameData()
	{
		frameTimeMs = Time.smoothDeltaTime * 1000f;
		fps = 1f / Time.smoothDeltaTime;
	}
	
	private string FrameData()
	{
		string fpsText = string.Format("{0:0.0} ms", frameTimeMs) + $" ({Fps()})";
		
		return fpsText;
	}

	private string Fps() => string.Format("{0:0.} fps", fps);

	private string VSyncData()
	{
		int vSyncCount = QualitySettings.vSyncCount;
		int targetFrameRate = Application.targetFrameRate;

		string vSyncText = $"vSync: {vSyncCount}, targetFps: {targetFrameRate}";

		return vSyncText;
	}

	private string Resolution()
	{
		float screenPixelWidth = cam.pixelWidth;
		float screenPixelHeight = cam.pixelHeight;
		string screenPixelWidthText = string.Format("{0}", screenPixelWidth);
		string screenPixelHeightText = string.Format("{0}", screenPixelHeight);
		
		string resText = $"VP: {VP()} | Screen: {Screen.currentResolution.ToString()}";

		return resText;
	}

	private string VP()
	{
		float screenPixelWidth = cam.pixelWidth;
		float screenPixelHeight = cam.pixelHeight;
		string screenPixelWidthText = string.Format("{0}", screenPixelWidth);
		string screenPixelHeightText = string.Format("{0}", screenPixelHeight);
		
		return $"{screenPixelWidthText}X{screenPixelHeightText}";
	}

	private string BuildText()
	{
		string build = Const_Dev.IsDevMode ? "dev" : "prod";
		string buildText = $"{build} build";

		return buildText;
	}

	private string CameraSize()
	{
		string cameraSizeText = $"{cam.orthographicSize}";

		return cameraSizeText;
	}

	private string PixelPerfectData()
	{
		if (graphics == null)
			return string.Empty;
		
		int UIScale;
		int PPCamRatio;
		string pixelPerfectData = string.Empty;
		
		try
		{
			UIScale = graphics.UIDefaultScaleFactor;
			PPCamRatio = pixelPerfectCamera.pixelRatio;
			pixelPerfectData = $"pixel ratios (PPCam): {PixelRatio()}x ({PPCamRatio}x)\nUI scale: {UIScale}x";
		}
		catch
		{
			PPCamRatio = 0;
		}

		return pixelPerfectData;
	}

	private string PixelRatio()
	{
		int calcedPixelRatio;
		
		try
		{
			calcedPixelRatio = graphics.PixelRatio;
		}
		catch
		{
			calcedPixelRatio = 0;
		}

		return $"{calcedPixelRatio}";
	}

	private string ScreenMode()
	{
		string screenMode = Screen.fullScreenMode.ToString();
		
		return screenMode;
	}

	private string DisplayInfo()
	{
		DisplayInfo info = Screen.mainWindowDisplayInfo;
		
        var displayIdx = Script_Utils.GetCurrentDisplayIdx();

		return $"Display {displayIdx}, {info.name} {info.width}x{info.height}";
	}

	private void HandleSpecsDevInput()
	{
		if (Input.GetButtonDown(Const_KeyCodes.SpecsDisplay))
		{
			isHidden = !isHidden;
		}
	}
	
	private void HandleTrailerDevInput()
	{
		if (Input.GetButtonDown(Const_KeyCodes.PlayerVisibility))
		{
			if (Script_Game.Game != null)
			{
				var isInvisible = Script_Game.Game.GetPlayer().isInvisible;
				Script_Game.Game.GetPlayer().SetInvisible(!isInvisible);
			}			
		}

		if (Input.GetButtonDown(Const_KeyCodes.UIVisibility))
		{
			if (Script_Game.Game != null)
				Script_Game.Game.IsHideHUD = !Script_Game.Game.IsHideHUD;
		}
	}

	private void HandleDevInput()
	{
		if (Input.GetKey(KeyCode.V) && Input.GetKeyDown(KeyCode.B))
		{
			if (Script_SaveGameControl.control != null)
			{
				Script_SaveGameControl.control.Save();
				ShowSavedNotification();
			}
		}
		else if (Input.GetKey(KeyCode.V) && Input.GetKeyDown(KeyCode.N))
		{
			Script_Game game = Script_Game.Game;
                    
			Model_Exit exitData = new Model_Exit(
				game.level,
				game.GetPlayer().transform.position,
				game.GetPlayer().FacingDirection
			);
			
			game.ElevatorCloseDoorsCutScene(
				null,
				null,
				Script_Elevator.Types.Last,
				exitData,
				Script_Exits.ExitType.SaveAndRestartOnLevel
			);
		}
	}

	private void ShowSavedNotification()
	{
		if (gameHelper != null)
			gameHelper.ShowSaveDevCanvas();
	}
}