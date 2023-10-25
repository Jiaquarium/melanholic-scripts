using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Discord;
using System;

public class Script_DiscordManager : MonoBehaviour
{
    private static long ClientId = 1166477781950005341;
    private static string LargeImageKey = "psychic-duck_512x512";
    private Discord.Discord discord;

    private static Script_DiscordManager m_Instance;
    
    void OnApplicationQuit()
    {
        try
        {
            if (discord != null)
            {
                discord.Dispose();
                Debug.Log($"Discord SDK successfully disposed");
                return;
            }
            
            Debug.Log($"No Discord SDK to dispose of");
        }
        catch (Exception e)
        {
            Debug.LogError($"Error when disposing of Discord SDK: {e}");
        }
    }
    
    void Awake()
    {
        if (m_Instance != null)
        {
			Destroy(gameObject);
			return;
		}
		m_Instance = this;
        
        DontDestroyOnLoad(gameObject);
    }
    
    // Start is called before the first frame update
    void Start()
    {
        try
        {
            discord = new Discord.Discord(ClientId, (UInt64)Discord.CreateFlags.NoRequireDiscord);

            if (discord == null)
                return;

            Debug.Log($"Initialized Discord SDK!");

            var activityManager = discord.GetActivityManager();
            var activity = new Discord.Activity {
                Assets = {
                    LargeImage = LargeImageKey
                },
            };

            activityManager.UpdateActivity(activity, res => {
                if (res == Discord.Result.Ok)
                    Debug.Log("Discord Activity successfully initialized!");
                else
                    Debug.LogError("Discord Activity failed to initialize");
            });
        }
        catch
        {
            Debug.Log($"Discord not open, Discord SDK not initialized");
            CleanDestroy();
        }
    }

    // Update is called once per frame
    void LateUpdate()
    {
        try
        {
            if (discord != null)
                discord.RunCallbacks();
            else
                CleanDestroy();
        }
        catch (Exception e)
        {
            Debug.LogError($"Error when running Discord SDK callbacks: {e}");
            CleanDestroy();
        }
    }

    private void CleanDestroy()
    {
        Debug.Log($"Destroying Discord SDK manager");
        
        if (discord != null)
            discord.Dispose();

        Destroy(gameObject);
    }
}
