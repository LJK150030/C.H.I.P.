using System.Collections;
using Assets.Scripts.Utilities;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
    public class MenuButton : MonoBehaviour {
        private Button button;
        private Text text;
        private string originalText;
        public float flickerTime = 0.5f;
        //private AudioUtil audioUtil;
        //public AudioClip soundOnClick;


        private void Awake () {
            button = GetComponent<Button>();
            text = GetComponentInChildren<Text>();
            originalText = text.text;
            StartCoroutine(Flicker());
            //audioUtil = GameObject.FindGameObjectWithTag("Music").GetComponent<AudioUtil>();
            //button.onClick.AddListener(() => AudioUtil.PlayOneOff(soundOnClick));
            //button.onClick.AddListener(() => AudioUtil.PlayOneOff(soundOnClick));

        }

        private IEnumerator Flicker () {
            while (true)
            {
                text.text = originalText + "_";
                yield return new WaitForSeconds(flickerTime);
                text.text = originalText;
                yield return new WaitForSeconds(flickerTime);
            }
        }
    }
}
