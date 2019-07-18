using UnityEngine;

namespace Assets.Scripts.UI
{
    public class Skill : MonoBehaviour
    {
        public int SkillID;
        [SerializeField] private AbilityLibrary _abilitiesLibraryReference;

        public void SetSkillID(int ID)
        {
            SkillID = ID;
        }

        public void SetAbilitiesLibraryReference(AbilityLibrary reference)
        {
            _abilitiesLibraryReference = reference;
        }
    }
}
