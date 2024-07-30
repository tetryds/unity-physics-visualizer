using System;
using System.Collections.Generic;
using UnityEngine;

namespace PhysicsVisualizer
{
    public static class PhysicsVisualizer
    {
        public static void Raycast(Ray ray, RaycastHit hitInfo, float maxDistance)
        {
            if (hitInfo.collider)
            {
                GizmosScheduler.Instance.ScheduleDraw(() =>
                {
                    Gizmos.color = Color.cyan;
                    Gizmos.DrawWireSphere(hitInfo.point, 0.05f);
                    Gizmos.DrawRay(hitInfo.point, hitInfo.normal * 0.1f);
                    Gizmos.DrawRay(ray.origin, ray.direction * hitInfo.distance);
                });
            }
            else
            {
                GizmosScheduler.Instance.ScheduleDraw(() =>
                {
                    Gizmos.color = Color.red;
                    Gizmos.DrawRay(ray.origin, ray.direction * maxDistance);
                });
            }

            /*
                    Raycast(Vector3 origin, Vector3 direction, float maxDistance, int layerMask);
                    Raycast(Vector3 origin, Vector3 direction, float maxDistance, int layerMask);
                    Raycast(Ray ray, out RaycastHit hitInfo);
                    Raycast(Ray ray, out RaycastHit hitInfo, float maxDistance);
                    Raycast(Ray ray, out RaycastHit hitInfo, float maxDistance, int layerMask);
                    Raycast(Ray ray);
                    Raycast(Ray ray, float maxDistance);
                    Raycast(Ray ray, out RaycastHit hitInfo, [Internal.DefaultValue("Mathf.Infinity")] float maxDistance, [Internal.DefaultValue("DefaultRaycastLayers")] int layerMask, [Internal.DefaultValue("QueryTriggerInteraction.UseGlobal")] QueryTriggerInteraction queryTriggerInteraction);
                    Raycast(Ray ray, [Internal.DefaultValue("Mathf.Infinity")] float maxDistance, [Internal.DefaultValue("DefaultRaycastLayers")] int layerMask, [Internal.DefaultValue("QueryTriggerInteraction.UseGlobal")] QueryTriggerInteraction queryTriggerInteraction);
                    Raycast(Vector3 origin, Vector3 direction, out RaycastHit hitInfo);
                    Raycast(Vector3 origin, Vector3 direction, out RaycastHit hitInfo, float maxDistance);
                    Raycast(Vector3 origin, Vector3 direction, out RaycastHit hitInfo, float maxDistance, int layerMask);
                    Raycast(Vector3 origin, Vector3 direction, out RaycastHit hitInfo, float maxDistance, int layerMask, QueryTriggerInteraction queryTriggerInteraction);
                    Raycast(Vector3 origin, Vector3 direction);
                    Raycast(Vector3 origin, Vector3 direction, float maxDistance);
                    Raycast(Ray ray, float maxDistance, int layerMask);
            */
        }
    }
}
