using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
    public class AbilityLibrary : MonoBehaviour
    {
        [SerializeField] private Ability[] _abilities;
        [SerializeField] private SlottingMachine _slottingMachine;
        private Button[] _abilityButtons;

        private void Awake()
        {
            if (_slottingMachine == null)
                _slottingMachine = GameObject.FindGameObjectWithTag("SlottingMachine").GetComponent<SlottingMachine>();

            int numberOfChildren = transform.childCount;
            _abilities = new Ability[numberOfChildren];
            _abilityButtons = new Button[numberOfChildren];

            for (int child_index = 0; child_index < numberOfChildren; child_index++)
            {
                _abilities[child_index] = transform.GetChild(child_index).GetComponent<Ability>();
                _abilities[child_index].SetSlottingMachine(_slottingMachine);
                _abilityButtons[child_index] = transform.GetChild(child_index).GetComponent<Button>();
                //_abilityButtons[child_index].onClick.AddListener(EnableOrDisableButton);
            }
        }

        private void Update()
        {
        }

        public void EnableOrDisableButton()
        {
            bool enable = _slottingMachine.IsThereFreeSpace();

            for (int buttonIndex = 0; buttonIndex < _abilityButtons.Length; buttonIndex++)
            {
                _abilityButtons[buttonIndex].interactable = enable;
            }
        }

    }
}
