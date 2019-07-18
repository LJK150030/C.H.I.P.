using UnityEngine;

namespace Assets.Scripts.Utilities
{
    [RequireComponent(typeof(AudioSource))]
    public class DestroyOnAudioFinished : MonoBehaviour {

        private AudioSource source;

        private void Start()
        {
            source = GetComponent<AudioSource>();
        }

        private void Update()
        {
            if (source == null || !source.isPlaying) Destroy(gameObject);
        }
    }
}
