using System;
using System.Collections.Generic;
using UnityEngine;

namespace PhysicsVisualizer
{
    public static class PhysicsVisualizer
    {
        public static void Raycast(Ray ray, RaycastHit hit, float maxDistance)
        {
            if (hit.collider)
            {
                GizmosScheduler.Instance.ScheduleDraw(() =>
                {
                    Gizmos.color = Color.cyan;
                    ExtraGizmos.DrawSphere(hit.point, hit.normal, 0.05f);
                    Gizmos.DrawRay(hit.point, hit.normal * 0.1f);
                    Gizmos.DrawRay(ray.origin, ray.direction * hit.distance);
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
        }

        public static void SphereCast(Ray ray, float radius, RaycastHit hit, float maxDistance)
        {
            if (hit.collider)
            {
                GizmosScheduler.Instance.ScheduleDraw((Action)(() =>
                {
                    Vector3 center = ray.origin + ray.direction * hit.distance;
                    Gizmos.color = Color.green;
                    ExtraGizmos.DrawSphere(hit.point, hit.normal, 0.05f);
                    Gizmos.DrawRay(hit.point, hit.normal * 0.1f);
                    ExtraGizmos.DrawCapsule(ray.origin, ray.direction, radius, hit.distance);
                }));
            }
            else
            {
                GizmosScheduler.Instance.ScheduleDraw(() =>
                {
                    Gizmos.color = Color.red;
                    ExtraGizmos.DrawCapsule(ray.origin, ray.direction, radius, maxDistance);
                });
            }
        }
    }
}
