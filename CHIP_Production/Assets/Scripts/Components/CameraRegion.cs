using UnityEngine;

namespace Assets.Scripts.Components
{
    public class CameraRegion : MonoBehaviour
    {
        public Color regionColor;
        [HideInInspector] public Bounds bounds;

        private void Start()
        {
            bounds = new Bounds(transform.position, transform.localScale);
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = regionColor;
            Gizmos.DrawWireCube(transform.position, transform.localScale);
            Gizmos.DrawLine(bounds.min, bounds.max);
        }
    }
}
