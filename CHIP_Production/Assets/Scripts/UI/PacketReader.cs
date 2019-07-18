using UnityEngine;

namespace Assets.Scripts.UI
{
    public class PacketReader : MonoBehaviour
    {

        private Slot[] _chipsSlots;
        private SlottingMachine _currentSlotMachine;
        private int _numSlots = 0;
        
        private void Awake()
        {
            if (_currentSlotMachine == null)
                _currentSlotMachine =
                    GameObject.FindGameObjectWithTag("SlottingMachine").GetComponent<SlottingMachine>();

            _chipsSlots = new Slot[_currentSlotMachine.GetSlottingMachineSize()];

            int numberOfChildren = transform.childCount;
            for (int childIndex = 0; childIndex < numberOfChildren; childIndex++)
            {
                _chipsSlots[childIndex] = transform.GetChild(childIndex).GetComponent<Slot>();
            }
        }

        public void ReadPacket()
        {
            for (int slotID = 0; slotID < _chipsSlots.Length; slotID++)
            {
                if (_currentSlotMachine.Slots[slotID].AbilityID == -1)
                    break;

                _chipsSlots[slotID].SetAbility(_currentSlotMachine.Slots[slotID]);
            }
        }

        public void ClearSlots()
        {
            for (int slotID = 0; slotID < _chipsSlots.Length; slotID++)
            {
                _chipsSlots[slotID].ClearAbility();
            }
        }

        public int GetPacketSize()
        {
            return _chipsSlots.Length;
        }

        public int GetPackAbilityIDFromIndex(int id)
        {
            return _chipsSlots[id].AbilityID;
        }

    }
}
