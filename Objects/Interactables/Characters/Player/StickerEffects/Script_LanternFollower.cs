using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class Script_LanternFollower : MonoBehaviour
{
    private enum States
    {
        Inactive            = 0,
        Active              = 1
    }
    
    [SerializeField] private States state;
    [SerializeField] private bool isLightOn;

    [SerializeField] private AnimationCurve progressCurve;
    [SerializeField] private float speed;

    [SerializeField] private Script_Game game;
    [SerializeField] private Light myLight;
    [SerializeField] private Renderer graphics;

    [SerializeField] private Sprite lightOnSprite;
    [SerializeField] private Sprite lightOffSprite;

    private Vector3 startLocation;
    private Vector3 endLocation;
    private Vector3 newEndLocation;
    private float progress;

    public bool IsLightOn
    {
        get => isLightOn;
        private set => isLightOn = value;
    }
    
    void OnEnable()
    {
        Script_GameEventsManager.OnLevelBeforeDestroy += HideOnLevelDestroy;
        Script_GameEventsManager.OnPlayerSetupOnLevel += UnhideOnPlayerSetup;
    }

    void OnDisable()
    {
        Script_GameEventsManager.OnLevelBeforeDestroy -= HideOnLevelDestroy;
        Script_GameEventsManager.OnPlayerSetupOnLevel -= UnhideOnPlayerSetup;
    }
    
    void Update()
    {
        // Follow Player
        newEndLocation = game.GetPlayer().transform.position;

        Move();

        void Move()
        {
            // If new end location, then update my end location
            if (!newEndLocation.Equals(endLocation))
            {
                endLocation = newEndLocation;
                progress = 0f;
                startLocation = transform.position;
            }
            
            if (progress < 1f)
            {
                progress += speed * Time.deltaTime;
                if (progress >= 1f)     progress = 1f;
                
                transform.position = Vector3.Lerp(
                    startLocation,
                    endLocation,
                    progressCurve.Evaluate(progress)
                );
            }

        }
    }

    public void SwitchLightState()
    {
        if (IsLightOn)      LightOff();
        else                LightOn();
    }

    public void LightOff()
    {
        myLight.gameObject.SetActive(false);
        
        var spriteRenderer = (SpriteRenderer)graphics;
        spriteRenderer.sprite = lightOffSprite;
        
        IsLightOn = false;
    }

    private void LightOn()
    {
        myLight.gameObject.SetActive(true);

        var spriteRenderer = (SpriteRenderer)graphics;
        spriteRenderer.sprite = lightOnSprite;

        IsLightOn = true;
    }   

    public void Activate()
    {
        state = States.Active;

        InitialState();
        gameObject.SetActive(true);
    }

    public void Deactivate()
    {
        state = States.Inactive;
        gameObject.SetActive(false);
    }

    // Before destroying level we need to clear this.
    private void HideOnLevelDestroy()
    {
        Debug.Log($"{name} reacts to LevelBeforeDestroy");
        
        // Only hide Graphics if is Active so we can still detect the OnPlayerSetupOnLevel event.
        graphics.gameObject.SetActive(false);
    }

    private void UnhideOnPlayerSetup()
    {
        Debug.Log($"{name} reacts to PlayerSetupOnLevel");
        
        if (state == States.Active)
        {
            InitialState();
            graphics.gameObject.SetActive(true);
        }
    }

    private void InitialState()
    {
        transform.position = game.GetPlayer().transform.position;
        startLocation = transform.position;
        endLocation = startLocation;
        progress = 1f;
    }

    public void Setup()
    {
        Deactivate();
        LightOff();
        InitialState();
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(Script_LanternFollower))]
public class Script_LanternFollowerTester : Editor
{
    public override void OnInspectorGUI() {
        DrawDefaultInspector();

        Script_LanternFollower t = (Script_LanternFollower)target;
        if (GUILayout.Button("Activate"))
        {
            t.Activate();
        }

        if (GUILayout.Button("Deactivate"))
        {
            t.Deactivate();
        }
    }
}
#endif