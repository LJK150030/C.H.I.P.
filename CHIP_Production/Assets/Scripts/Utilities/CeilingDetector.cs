using UnityEngine;

namespace Assets.Scripts.Utilities
{
    public class CeilingDetector : MonoBehaviour
    {
        public float detectLength = 0.1f;
        public LayerMask layerMask = -1;
        public string targetTag = "Ground";

        public bool isHittingCeiling;
        public bool justHitCeiling;
        public Vector2 groundNormal;
        public float groundAngle;

        void Update()
        {
            RaycastHit2D[] hit = Physics2D.RaycastAll(transform.position, Vector2.up, detectLength, layerMask);
            for (int i = 0; i < hit.Length; i++)
            {
                if (hit[i].transform.tag == targetTag)
                {
                    if (!isHittingCeiling)
                    {
                        justHitCeiling = true;
                    }
                    else
                    {
                        justHitCeiling = false;
                    }
                    isHittingCeiling = true;
                    groundNormal = hit[i].normal;
                    groundAngle = -Vector2.SignedAngle(groundNormal, Vector2.up);
                    return;
                }
            }
            isHittingCeiling = false;
            justHitCeiling = false;
            groundNormal = Vector2.zero;
            groundAngle = 0;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(transform.position, transform.position + Vector3.up * detectLength);
        }
    }
}
