using UnityEngine;
using UnityEngine.UI;

namespace Assets.Project_Assets_Folder.Scripts
{
    public class Slotting : MonoBehaviour
    {
        public Sprite EmptySlotSprite;
        public bool[] SlotUsed;

        private Image[] SpriteRendererImages;

        private void Start()
        {


            SpriteRendererImages = new Image[2];
            SpriteRendererImages[0] = transform.Find("Slot_1").GetChild(0).GetComponent<Image>();
            SpriteRendererImages[1] = transform.Find("Slot_2").GetChild(0).GetComponent<Image>();

            SlotUsed = new bool[2] {false, false};
        }

        public void Clean(int index)
        {
            SpriteRendererImages[index].color = Color.black;
            SlotUsed[index] = false;
        }

        public void SetVisual(Color image, int i)
        {
            SpriteRendererImages[i].color = Color.white;
            SlotUsed[i] = true;
        }
    }
}
