using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
    public class Slot : MonoBehaviour
    {
        public int AbilityID = -1;

        [HideInInspector] public Image AbilityImage;

        private void Awake()
        {
            AbilityImage = transform.GetChild(0).GetComponent<Image>();
            AbilityImage.color = new Color(0.0f, 0.0f, 0.0f, 0.0f);
        }

        public void SetAbility(Ability ability)
        {
            AbilityID = ability.AbilityID;
            //AbilityImage.enabled = true;
            AbilityImage.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
            AbilityImage.sprite = ability.AbilitySprite;
        }

        public void SetAbility(Slot slotAbility)
        {
            AbilityID = slotAbility.AbilityID;

            if (AbilityID != -1)
            {
                if (slotAbility.AbilityImage.sprite != null)
                {
                    //AbilityImage.enabled = true;
                    AbilityImage.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
                    AbilityImage.sprite = slotAbility.AbilityImage.sprite;
                }
                else
                {
                    //AbilityImage.enabled = false;
                    AbilityImage.color = new Color(0.0f, 0.0f, 0.0f, 0.0f);
                }
            }
        }

        public void ClearAbility()
        {
            AbilityID = -1;
            //AbilityImage.enabled = false;
            AbilityImage.color = new Color(0.0f, 0.0f, 0.0f, 0.0f);
            AbilityImage.sprite = null;
        }
    }
}
