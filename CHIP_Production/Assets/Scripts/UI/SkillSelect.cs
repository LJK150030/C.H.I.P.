using Assets.Scripts.UI;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Project_Assets_Folder.Scripts
{
    public class SkillSelect : MonoBehaviour
    {
        public int NumSkills = 1;
        public Slotting SlottingGameObject;

        private Skill[] skillsToSelect;
        private Color[] skillImages;
        // Use this for initialization
        private void Awake()
        {
            skillsToSelect = new Skill[NumSkills];
            skillsToSelect[0] = transform.GetChild(0).GetComponent<Skill>();

            skillImages = new Color[NumSkills];
            skillImages[0] = transform.GetChild(0).GetComponent<Image>().color;
        }

        public void Select()
        {
            foreach (var skill in skillsToSelect)
            {
                for (var i = 0; i < 2; i++)
                {
                    if (SlottingGameObject.SlotUsed[i] == false)
                    {
                        SlottingGameObject.SetVisual(skillImages[0], i);
                        return;
                    }
                }
            }
        }
    }
}
