using UnityEngine;

public class UIButtonClickSound : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip clickClip;

    public void PlayClick()
    {
        if (audioSource != null && clickClip != null)
        {
            audioSource.PlayOneShot(clickClip);
        }
    }
}
