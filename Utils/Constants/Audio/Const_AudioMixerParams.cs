/// <summary>
/// No Audio Source should output in the Master mixer.
/// </summary>
public static class Const_AudioMixerParams
{
    public const string ExposedGameVolume = "ExposedMasterVolume";
    
    // Should only be used by the highest level controllers where there will be no conflict.
    public const string ExposedMusicVolume = "ExposedMusicVolume";
    
    public const string ExposedBGVolume = "ExposedBGVolume";
    public const string ExposedBG2Volume = "ExposedBG2Volume";
    public const string ExposedSFXVolume = "ExposedSFXVolume";
    
    public const string ExposedInteractionUIVolume = "ExposedInteractionUIVolume";
    public const string ExposedBGMUIVolume = "ExposedBGMUIVolume";
}