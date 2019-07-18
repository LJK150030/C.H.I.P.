using System.Collections;
using Assets.Scripts.Utilities;
using UnityEngine;

namespace Assets.Scripts.Components
{
    public class PlaySoundOnStart : MonoBehaviour {

        public AudioClip clipToPlay;
        public float SecondsToDelay = 0.0f;

        public void Start()
        {
            StartCoroutine(PlayOnDelay());
        }

        public IEnumerator PlayOnDelay()
        {
            yield return new WaitForSeconds(SecondsToDelay);
            AudioUtil.PlayOneOffAt(transform, clipToPlay);
        }

        public void PlayImmediately()
        {
            AudioUtil.PlayOneOffAt(transform, clipToPlay);
        }
    }
}
