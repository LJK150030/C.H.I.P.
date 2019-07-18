using Assets.Scripts.Utilities;
using UnityEngine;

namespace Assets.Scripts.Components
{
    public class PlaySoundOnCollision : MonoBehaviour {

        public AudioClip SFX_passed;

        void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.tag == "Player")
            {
                AudioUtil.PlayOneOff(SFX_passed);
            }
        }
    }
}
