using UnityEngine;
using System.Collections;
 
[ExecuteAlways]
public class Dev_SpecsDisplay : MonoBehaviour
{
	[SerializeField] private Camera cam;
	
	float deltaTime = 0.0f;
 
	void Awake()
    {
        cam = GetComponent<Camera>();
    }
	
	void Update()
	{
		// if (!Const_Dev.IsDevMode)   return;
        
        deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
	}
 
	void OnGUI()
	{
		// if (!Const_Dev.IsDevMode)   return;
        
        int w = Screen.width, h = Screen.height;
 
		GUIStyle style = new GUIStyle();
 
		int fontSize = h * 1 / 100;
		Rect rect = new Rect(0, 0, w, fontSize * 2);
		style.alignment = TextAnchor.UpperRight;
		style.fontSize = fontSize;
		style.normal.textColor = new Color (255f, 255f, 255f, 1.0f);
		float msec = deltaTime * 1000.0f;
		float fps = 1.0f / deltaTime;
		string fpsText = string.Format("{0:0.0} ms ({1:0.} fps)", msec, fps);
		
        float screenWidth = Screen.currentResolution.width;
        float screenHeight = Screen.currentResolution.height;
        string screenWidthText = string.Format("{0}", screenWidth);
        string screenHeightText = string.Format("{0}", screenHeight);
		string resText = $"{screenWidthText}X{screenHeightText}";
		
		float cameraPixelWidth = cam.pixelWidth;
        float cameraPixelHeight = cam.pixelHeight;
        string cameraWidthText = string.Format("{0}", cameraPixelWidth);
        string cameraHeightText = string.Format("{0}", cameraPixelHeight);
		string cameraPixelsText = $"camera pixels: {screenWidthText}X{screenHeightText}";
        
		string build = Const_Dev.IsDevMode ? "dev" : "prod";
		string buildText = $"{build} build";

        GUI.Label(rect, $"{fpsText}, {resText}\n{cameraPixelsText}\n{buildText}", style);
	}
}