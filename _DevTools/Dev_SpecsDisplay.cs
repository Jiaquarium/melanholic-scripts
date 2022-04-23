using UnityEngine;
using System.Collections;
using UnityEngine.Experimental.Rendering.Universal;
 
[ExecuteAlways]
public class Dev_SpecsDisplay : MonoBehaviour
{
	[SerializeField] private Camera cam;
	[SerializeField] private Script_GraphicsManager graphics;
	[SerializeField] private PixelPerfectCamera pixelPerfectCamera;
	[SerializeField] private float fpsRefreshTimer;

	[SerializeField] private bool isSingleLineDisplay;
	
	private float fps;
	private float frameTimeMs;
	private float timer;
 
	void Awake()
    {
        cam = GetComponent<Camera>();
		this.enabled = Const_Dev.IsSpecsDisplayOn;
    }
	
	void Update()
	{
		timer -= Time.unscaledDeltaTime;

		if (timer <= 0)
		{
			RefreshFrameData();
			timer = fpsRefreshTimer;
		}
	}
 
	void OnGUI()
	{
        int w = Screen.width, h = Screen.height;
 
		GUIStyle style = new GUIStyle();
 
		int fontSize = h * 1 / 75;
		Rect rect = new Rect(0, 0, w, fontSize * 2);
		style.alignment = TextAnchor.UpperRight;
		style.fontSize = fontSize;
		style.normal.textColor = new Color (255f, 255f, 255f, 1.0f);
		
        var label = Const_Dev.IsTrailerMode ?
TrailerModeSpecs()
: @$"{FrameData()}, {VSyncData()}
{Resolution()}
{PixelPerfectData()}
ortho: {CameraSize()}
{ScreenMode()}
{BuildText()}";

		GUI.Label(rect, label, style);
	}

	private string TrailerModeSpecs() => isSingleLineDisplay ?
		@$"{Fps()}, {CameraSize()}x, pxRt:{PixelRatio()}x, {VP()}, lt:{Script_LightFXManager.Control?.CurrentIntensity.ToString()}"
		: @$"{Fps()}
{CameraSize()}x
pxRt:{PixelRatio()}x
{VP()}
lt:{Script_LightFXManager.Control?.CurrentIntensity.ToString()}";

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
}