using UnityEngine;

namespace Assets.Project_Assets_Folder.Scripts
{
    public class GroundDetector : MonoBehaviour {
        public float detectLength = 0.1f;
        public float sideDetectLength = 0.2f;
        public LayerMask layerMask = -1;
        public string targetTag = "Ground";

        public bool isGrounded;
        public bool enterGrounded;
        public Vector2 groundNormal;
        public float groundAngle;
        public Vector2 RaycastOffset;

        public Vector3 CenterOffset;
        public Vector3 RightOffset;
        public Vector3 LeftOffset;

        //         public bool isLeft = false;
        //         public bool isRight = false;
        //         public bool isCenter = false;

        void Update () {
            GroundDetection();
        }

        private void GroundDetection()
        {

            RaycastHit2D[] centerHit = Physics2D.RaycastAll(transform.position + CenterOffset, Vector2.down, detectLength, layerMask);
            RaycastHit2D[] leftHit = Physics2D.RaycastAll(transform.position + LeftOffset, Vector2.down, sideDetectLength, layerMask);
            RaycastHit2D[] rightHit = Physics2D.RaycastAll(transform.position + RightOffset, Vector2.down, sideDetectLength, layerMask);

            for (int i = 0; i < centerHit.Length; i++)
            {
                if (centerHit[i].transform.tag == targetTag)
                {
                    if (!isGrounded)
                    {
                        enterGrounded = true;
                    }
                    else
                    {
                        enterGrounded = false;
                    }
                    isGrounded = true;
                    groundNormal = centerHit[i].normal;
                    groundAngle = -Vector2.SignedAngle(groundNormal, Vector2.up);
                    return;
                }
            }

            for (int i = 0; i < leftHit.Length; i++)
            {
                if (leftHit[i].transform.tag == targetTag)
                {
                    if (!isGrounded)
                    {
                        enterGrounded = true;
                    }
                    else
                    {
                        enterGrounded = false;
                    }
                    isGrounded = true;
                    groundNormal = leftHit[i].normal;
                    groundAngle = -Vector2.SignedAngle(groundNormal, Vector2.up);
                    return;
                }
            }

            for (int i = 0; i < rightHit.Length; i++)
            {
                if (rightHit[i].transform.tag == targetTag)
                {
                    if (!isGrounded)
                    {
                        enterGrounded = true;
                    }
                    else
                    {
                        enterGrounded = false;
                    }
                    isGrounded = true;
                    groundNormal = rightHit[i].normal;
                    groundAngle = -Vector2.SignedAngle(groundNormal, Vector2.up);
                    return;
                }
            }

            isGrounded = false;
            enterGrounded = false;
            groundNormal = Vector2.zero;
            groundAngle = 0;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;

            Gizmos.DrawLine(CenterOffset + transform.position, CenterOffset + transform.position + Vector3.down * detectLength);
            Gizmos.DrawLine(RightOffset + transform.position, RightOffset + transform.position + Vector3.down * sideDetectLength);
            Gizmos.DrawLine(LeftOffset + transform.position, LeftOffset + transform.position + Vector3.down * sideDetectLength);

        }
    }
}
