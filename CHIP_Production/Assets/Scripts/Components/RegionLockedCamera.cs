using System;
using System.Linq.Expressions;
using Assets.Scripts.Controllers;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Components
{
    public class RegionLockedCamera : MonoBehaviour
    {
        public float CameraVerticalSnap = 1.0f;
        public float CameraHorizontalSnap = 1.0f;

        private CameraRegion[] WorldBounds;
        private GameObject targetPlayer;
        private Vector2 cameraMins;
        private Vector2 cameraMaxs;
        private Camera camera;
        private float aspect = 0.0f;
        private float cameraScale = 0.0f;
        private Vector2 localMax = Vector2.zero;
        private Bounds CameraContainBounds;

        private int TargetInRegion = -1;
        // Use this for initialization
        void Awake()
        {
            camera = GetComponent<Camera>();
            GameObject region = GameObject.FindGameObjectWithTag("Regions");
            int numRegions = region.transform.childCount;
            WorldBounds = new CameraRegion[numRegions];

            for (int childID = 0; childID < numRegions; childID++)
            {
                WorldBounds[childID] = region.transform.GetChild(childID).GetComponent<CameraRegion>();
            }

            targetPlayer = GameObject.FindGameObjectWithTag("TargetPlayer");

            cameraMins = Vector2.zero;
            cameraMaxs = Vector2.one;
            aspect = camera.aspect;
            cameraScale = camera.orthographicSize;
            localMax = new Vector2(cameraScale * aspect, cameraScale);
        }

        // Update is called once per frame
        void Update()
        {
            if (targetPlayer == null) return;

            
            Vector2 worldPosition = transform.position;
            cameraMins = worldPosition + (-localMax);
            cameraMaxs = worldPosition + localMax;

            for (int regionID = 0; regionID < WorldBounds.Length; regionID++)
            {
                if (WorldBounds[regionID].bounds.Contains(targetPlayer.transform.position))
                {
                    TargetInRegion = regionID;
                    break;
                }
            }

            if (Math.Abs(WorldBounds[TargetInRegion].bounds.size.y - WorldBounds[TargetInRegion].bounds.size.x) < 1.1f)
            {
                CameraContainBounds = new Bounds(
                    new Vector2(WorldBounds[TargetInRegion].bounds.center.x,
                        WorldBounds[TargetInRegion].bounds.center.y),
                    new Vector2(WorldBounds[TargetInRegion].bounds.size.y * aspect,
                        WorldBounds[TargetInRegion].bounds.size.y));
            }
            else if (WorldBounds[TargetInRegion].bounds.size.y < camera.orthographicSize * 2.0f)
            {
                float calculatedX = Mathf.Clamp(targetPlayer.transform.position.x,
                    WorldBounds[TargetInRegion].bounds.min.x + (-localMax.x),
                    WorldBounds[TargetInRegion].bounds.max.x + (localMax.x));

                CameraContainBounds = new Bounds(
                    new Vector2(calculatedX, WorldBounds[TargetInRegion].bounds.center.y),
                    new Vector2(WorldBounds[TargetInRegion].bounds.size.y,
                        WorldBounds[TargetInRegion].bounds.size.y / aspect));
            }
            else if (WorldBounds[TargetInRegion].bounds.size.x < camera.orthographicSize * 2.0f)
            {
                float calculatedY = Mathf.Clamp(targetPlayer.transform.position.y,
                      WorldBounds[TargetInRegion].bounds.min.y + localMax.y,
                      WorldBounds[TargetInRegion].bounds.max.y + (-localMax.y));

                CameraContainBounds = new Bounds(
                    new Vector2(WorldBounds[TargetInRegion].bounds.center.x, calculatedY),
                    new Vector2(WorldBounds[TargetInRegion].bounds.size.y,
                        WorldBounds[TargetInRegion].bounds.size.y / aspect));
            }
            else
            {
                float calculatedX = Mathf.Clamp(targetPlayer.transform.position.x,
                    WorldBounds[TargetInRegion].bounds.min.x + (localMax.x),
                    WorldBounds[TargetInRegion].bounds.max.x + (-localMax.x));

                float calculatedY = Mathf.Clamp(targetPlayer.transform.position.y,
                    WorldBounds[TargetInRegion].bounds.min.y + (localMax.y),
                    WorldBounds[TargetInRegion].bounds.max.y + (-localMax.y));

                CameraContainBounds = new Bounds(
                    new Vector2(calculatedX, calculatedY),
                    new Vector2(WorldBounds[TargetInRegion].bounds.size.y,
                        WorldBounds[TargetInRegion].bounds.size.y / aspect));

                //                 if (WorldBounds[TargetInRegion].bounds.size.y < WorldBounds[TargetInRegion].bounds.size.x)
                //                 {
                //                     float calculatedX = Mathf.Clamp(targetPlayer.transform.position.x,
                //                         WorldBounds[TargetInRegion].bounds.min.x + localMax.x,
                //                         WorldBounds[TargetInRegion].bounds.max.x + (-localMax.x));
                // 
                //                     float calculatedY = Mathf.Clamp(targetPlayer.transform.position.y,
                //                         WorldBounds[TargetInRegion].bounds.min.y + localMax.y,
                //                         WorldBounds[TargetInRegion].bounds.max.y + (-localMax.y));
                // 
                //                     CameraContainBounds = new Bounds(
                //                         new Vector2(calculatedX, calculatedY),
                //                         new Vector2(WorldBounds[TargetInRegion].bounds.size.y,
                //                             WorldBounds[TargetInRegion].bounds.size.y / aspect));
                //                 }
                //                 else
                //                 {
                //                     float calculatedY = Mathf.Clamp(targetPlayer.transform.position.y,
                //                         WorldBounds[TargetInRegion].bounds.min.y + localMax.y,
                //                         WorldBounds[TargetInRegion].bounds.max.y + (-localMax.y));
                // 
                //                     CameraContainBounds = new Bounds(
                //                         new Vector2(WorldBounds[TargetInRegion].bounds.center.x, calculatedY),
                //                         new Vector2(WorldBounds[TargetInRegion].bounds.size.y,
                //                             WorldBounds[TargetInRegion].bounds.size.y / aspect));
                //                 }
            }

            float calculated_y_position = Mathf.Lerp(transform.position.y, CameraContainBounds.center.y,
                Time.deltaTime * CameraVerticalSnap);

            float calculated_x_position = Mathf.Lerp(transform.position.x, CameraContainBounds.center.x,
                Time.deltaTime * CameraHorizontalSnap);

            transform.position = new Vector3(calculated_x_position, calculated_y_position, -10.0f);
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawLine(cameraMins, cameraMaxs);

            Gizmos.color = Color.red;
            Gizmos.DrawLine(CameraContainBounds.min, CameraContainBounds.max);
        }

        //         public static void SwitchCorner(bool state)
        //         {
        //             switch_sides = state;
        //         }
    }
}
