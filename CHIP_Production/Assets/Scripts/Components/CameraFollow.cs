using UnityEngine;

namespace Assets.Scripts.Components
{
    public class CameraFollow : MonoBehaviour
    {
        public float LeftBound = 0.0f;
        public float RightBound = 1.0f;
        public float bottomBound = 0.0f;
        public float cameraSnap = 1.0f;

        public static GameObject target;
        public static bool switch_corner = false;

        private void Awake()
        {

        }

        // Update is called once per frame
        private void Update ()
        {
            var targetPosition = !switch_corner ? new Vector2(target.transform.position.x - LeftBound, target.transform.position.y - bottomBound) : 
                new Vector2(target.transform.position.x - RightBound, target.transform.position.y - bottomBound);
                
            transform.position = Vector2.LerpUnclamped(transform.position, targetPosition, Time.deltaTime * cameraSnap);
            transform.position = new Vector3(transform.position.x, targetPosition.y, -1.0f);
        }

        public static void SwitchCorner(bool state)
        {
            switch_corner = state;
        }

        public static void SetTarget(GameObject givenTarget)
        {
            target = givenTarget;
        }

        void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(new Vector2(LeftBound + transform.position.x, transform.position.y - 5), new Vector2(LeftBound + transform.position.x, transform.position.y + 5));
            Gizmos.DrawLine(new Vector2(RightBound + transform.position.x, transform.position.y - 5), new Vector2(RightBound + transform.position.x, transform.position.y + 5));

            Gizmos.color = Color.red;
            Gizmos.DrawLine(new Vector2(LeftBound + transform.position.x, transform.position.y + bottomBound), new Vector2(RightBound + transform.position.x, transform.position.y + bottomBound));
        }
    }
}
