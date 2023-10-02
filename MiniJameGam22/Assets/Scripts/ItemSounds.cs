using UnityEngine;
using Utilities;

public class ItemSounds : MonoBehaviour
{
    private AudioSource audioSource;
    [SerializeField] private AudioClip pickupAudio;
    [SerializeField] private AudioClip scatterAudio;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayPickup()
    {
        audioSource.pitch = Rand.Between(0.5f, 1.5f);
        audioSource.clip = pickupAudio;
        audioSource.Play();
    }

    public void PlayScatter()
    {
        audioSource.pitch = Rand.Between(0.5f, 1.5f);
        audioSource.clip = scatterAudio;
        audioSource.Play();
    }
}