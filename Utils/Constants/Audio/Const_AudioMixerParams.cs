/// <summary>
/// No Audio Source should output in the Master mixer.
/// </summary>
public static class Const_AudioMixerParams
{
    // ------------------------------------------------------------------
    // Game Consts

    //////////////////////////////////////////////////////////////////////////////////
    // NOTE: When adding new params, MUST initialize them in BackgroundMusicManager //
    //////////////////////////////////////////////////////////////////////////////////
    
    public const string ExposedGameVolume = "ExposedMasterVolume";
    
    // Should only be used by the highest level controllers where there will be no conflict.
    public const string ExposedMusicVolume = "ExposedMusicVolume";
    public const string ExposedBGVolume = "ExposedBGVolume";
    public const string ExposedBG2Volume = "ExposedBG2Volume";
    
    public const string ExposedFXVolume = "ExposedFXVolume";
    public const string ExposedSFXVolume = "ExposedSFXVolume";
    
    public const string ExposedInteractionUIVolume = "ExposedInteractionUIVolume";
    public const string ExposedBGMUIVolume = "ExposedBGMUIVolume";

    // ------------------------------------------------------------------
    // Settings Consts
    // These are the toplevel parent setting params that should ONLY ever be used by settings.
    // These should not be reset/initialized by BGM Manager because they must load from state.
    
    // Music Setting and UI Music Setting should always be set together
    public const string ExposedMusicSettingVolume = "ExposedMusicSettingVolume";
    public const string ExposedUIMusicSettingVolume = "ExposedUIMusicSettingVolume";
    
    // FX Setting and UI FX Setting should always be set together
    public const string ExposedFXSettingVolume = "ExposedFXSettingVolume";
    public const string ExposedUIFXSettingVolume = "ExposedUIFXSettingVolume";
}