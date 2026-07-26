using SmallHedge.SoundManager;
using UnityEngine;

public class AnimationSoundEvents : MonoBehaviour
{
    // Called from an Animation Event — pass the sound name as a string parameter
    public void PlaySoundEvent(string soundName)
    {
        if (System.Enum.TryParse(soundName, out SoundType sound))
        {
            SoundManager.PlaySound(sound);
        }
        else
        {
            Debug.LogWarning("Unknown sound type: " + soundName);
        }
    }
}
