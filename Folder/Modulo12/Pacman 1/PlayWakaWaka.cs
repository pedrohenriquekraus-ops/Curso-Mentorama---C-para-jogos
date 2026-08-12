using UnityEngine;

public class PlayWakaWaka : MonoBehaviour
{

    public AudioClip WakaClip1;
    public AudioClip WakaClip2;

    private AudioSource _audioSource;

    private static bool _SwitchClip;


    private void OnDestroy()
    {
        _audioSource = FindObjectOfType<AudioSource>();
        if (_audioSource != null)
        {
            _audioSource.PlayOneShot(_SwitchClip ? WakaClip1 : WakaClip2);
            _SwitchClip = !_SwitchClip;
        }

    }


    void Update()
    {

    }
}
