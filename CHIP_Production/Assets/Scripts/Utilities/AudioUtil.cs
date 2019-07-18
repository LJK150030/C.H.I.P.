using UnityEngine;

namespace Assets.Scripts.Utilities
{
    [RequireComponent(typeof(AudioSource))]
    public class AudioUtil : MonoBehaviour
    {

        private static AudioSource audioSource;

        private void Awake()
        {
            audioSource = GetComponent<AudioSource>();
        }

        // Will play an audio clip located at the supplied world position
        public static void PlayOneOffAt(Vector3 position, AudioClip clip)
        {
            if (clip == null) return;

            var go = new GameObject("__audio[" + clip.name + "]");
            go.transform.SetPositionAndRotation(position, Quaternion.identity);

            var source = go.AddComponent<AudioSource>();
            source.clip = clip;
            source.Play();

            go.AddComponent<DestroyOnAudioFinished>();
        }

        // Will play an audio clip located at where the transform is; 
        public static void PlayOneOffAt(Transform pos, AudioClip clip)
        {
            PlayOneOffAt(pos.position, clip);
        }

        public static void PlayOneOffNow(AudioClip clip)
        {
            PlayOneOffAt(Vector3.zero, clip);
        }

        public static void PlayOneOff(AudioClip clip)
        {
            audioSource.PlayOneShot(clip);
        }

        public void PlaySoundNow(AudioClip clip)
        {
            audioSource.PlayOneShot(clip);
        }

        public void ReplaySound(AudioClip clip)
        {
            audioSource.clip = clip;
        }

        public void SetLoop(bool isLooping)
        {
            audioSource.loop = isLooping;
        }
    }
}
