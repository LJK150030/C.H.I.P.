using Assets.Scripts.Utilities;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
    public class SlottingMachine : MonoBehaviour
    {
        [HideInInspector] public Slot[] Slots;
        private Button Play_Button;
        private Button Eject_Button;
        public AudioClip SFX_error;

        private void Awake()
        {
            Play_Button = GameObject.FindGameObjectWithTag("PlayButton").GetComponent<Button>();
            
            Eject_Button = GameObject.FindGameObjectWithTag("EjectButton").GetComponent<Button>();
            //Eject_Button.onClick.AddListener(BackSpace);
            Eject_Button.onClick.AddListener(ResetSlots);

            int numberOfChildren = transform.childCount;

            Slots = new Slot[numberOfChildren];

            for (int childIndex = 0; childIndex < numberOfChildren; childIndex++)
            {
                Slots[childIndex] = transform.GetChild(childIndex).GetComponent<Slot>();
            }
        }

        public void AddAbility(Ability ability)
        {
            bool addedAbility = false;

            for (int nonEmptySlotID = 0; nonEmptySlotID < Slots.Length; nonEmptySlotID++)
            {
                if (Slots[nonEmptySlotID].AbilityID != -1) continue;

                Slots[nonEmptySlotID].SetAbility(ability);
                addedAbility = true;
                break;
            }

            if(!addedAbility)
                AudioUtil.PlayOneOff(SFX_error);
        }

        public void ResetSlots()
        {
            for (int slotID = 0; slotID < Slots.Length; slotID++)
            {
                if (Slots[slotID].AbilityID == -1) break;

                Slots[slotID].ClearAbility();
            }
        }

        public int GetAbilityIDInSlot(int slotID)
        {
            return Slots[slotID].AbilityID;
        }

        public int GetSlottingMachineSize()
        {
            return Slots.Length;
        }

        public void BackSpace()
        {
            for (int slotID = Slots.Length - 1; slotID >= 0; slotID--)
            {
                if (Slots[slotID].AbilityID == -1) continue;

                Slots[slotID].ClearAbility();
                break;
            }
        }

        public bool IsThereFreeSpace()
        {
            for (int slotID = Slots.Length - 1; slotID >= 0; slotID--)
            {
                if (Slots[slotID].AbilityID == -1) return true;
            }

            return false;
        }

        public bool EnableOrDisablePlayButton()
        {
            return Slots[0].AbilityID != -1;
        }
    }
}