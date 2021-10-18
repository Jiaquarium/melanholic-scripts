﻿using UnityEngine;
using System.Collections;
using UnityEngine.Experimental.Rendering.Universal;
 
[ExecuteAlways]
public class Dev_SpecsDisplay : MonoBehaviour
{
	[SerializeField] private Camera cam;
	[SerializeField] private Script_GraphicsManager graphics;
	[SerializeField] private PixelPerfectCamera pixelPerfectCamera;
	[SerializeField] private float fpsRefreshTimer;
	
	private float fps;
	private float frameTimeMs;
	private float timer;
 
	void Awake()
    {
        cam = GetComponent<Camera>();
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
 
		int fontSize = h * 1 / 100;
		Rect rect = new Rect(0, 0, w, fontSize * 2);
		style.alignment = TextAnchor.UpperRight;
		style.fontSize = fontSize;
		style.normal.textColor = new Color (255f, 255f, 255f, 1.0f);
		
        GUI.Label(rect,
@$"{FrameData()}, {VSyncData()}
{Resolution()}
{PixelPerfectData()}
{CameraSize()}
{ScreenMode()}
{BuildText()}",
		style);
	}

	private void RefreshFrameData()
	{
		frameTimeMs = Time.smoothDeltaTime * 1000f;
		fps = 1f / Time.smoothDeltaTime;
	}
	
	private string FrameData()
	{
		string fpsText = string.Format("{0:0.0} ms ({1:0.} fps)", frameTimeMs, fps);
		
		return fpsText;
	}

	private string VSyncData()
	{
		int vSyncCount = QualitySettings.vSyncCount;
		int targetFrameRate = Application.targetFrameRate;

		string vSyncText = $"vSync: {vSyncCount}, targetFps: {targetFrameRate}";

		return vSyncText;
	}

	private string Resolution()
	{
		float screenWidth = Screen.currentResolution.width;
        float screenHeight = Screen.currentResolution.height;
        string screenWidthText = string.Format("{0}", screenWidth);
        string screenHeightText = string.Format("{0}", screenHeight);
		string resText = $"{screenWidthText}X{screenHeightText}";

		return resText;
	}

	private string BuildText()
	{
		string build = Const_Dev.IsDevMode ? "dev" : "prod";
		string buildText = $"{build} build";

		return buildText;
	}

	private string CameraSize()
	{
		string cameraSizeText = $"ortho: {cam.orthographicSize}";

		return cameraSizeText;
	}

	private string PixelPerfectData()
	{
		if (graphics == null)
			return string.Empty;
		
		int UIScale = graphics.UIDefaultScaleFactor;
		int calcedPixelRatio = graphics.PixelRatio;
		
		int PPCamRatio;
		
		try
		{
			PPCamRatio = pixelPerfectCamera.pixelRatio;
		}
		catch
		{
			PPCamRatio = 0;
		}
		
		string pixelPerfectData = $"pixel ratios (PPCam): {calcedPixelRatio}x ({PPCamRatio}x)\nUI scale: {UIScale}x";

		return pixelPerfectData;
	}

	private string ScreenMode()
	{
		string screenMode = Screen.fullScreenMode.ToString();
		
		return screenMode;
	}
}