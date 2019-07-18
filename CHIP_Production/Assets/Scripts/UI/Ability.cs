using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
    public class Ability : MonoBehaviour//, IPointerClickHandler
    {
        public int AbilityID;
        public Sprite AbilitySprite;

        private SlottingMachine _slottingMachine;

//         private void Awake()
//         {
//             AbilitySprite = GetComponent<Button>().image.sprite;
//         }

        public void SetSlottingMachine(SlottingMachine slotter)
        {
            _slottingMachine = slotter;
        }

        public void AddAbilityToSlottingMachine()
        {
            _slottingMachine.AddAbility(this);
        }

    }
}
