using System;
using System.Collections.Generic;
using log4net.Util;
using System.Text.RegularExpressions;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.XR;
using static Codice.Client.Commands.WkTree.WorkspaceTreeNode;

namespace PhysicsVisualizer
{
    public static class PhysicsVisualizer
    {
        public static float CapDistance = 1_000f;

        // Objects
        public static bool UseSourceObject = true;

        // Dir arrow
        public static bool UseDirArrow = true;
        public static float DirArrowLength = 0.1f;

        // Hit
        public static bool UseArrowForNormals = true;
        public static float HitNormalLength = 0.1f;
        public static bool UseHitSphere = true;
        public static float HitSphereRadius = 0.025f;

        // Colors
        public static Color HitPointColor = Color.green;
        public static Color HitCastColor = Color.green;
        public static Color NoHitCastColor = Color.red;
        public static Color AllCastColor = Color.cyan;
        public static Color SourceObjectColor = Color.magenta;

        public static void Raycast(Ray ray, RaycastHit hit, float maxDistance)
        {
            maxDistance = GetCappedDistance(maxDistance);
            if (hit.collider)
            {
                GizmosScheduler.Instance.ScheduleDraw(() =>
                {
                    DrawHit(hit);
                    Gizmos.color = HitCastColor;
                    Gizmos.DrawRay(ray.origin, ray.direction * hit.distance);
                    DrawDirArrow(ray);
                });
            }
            else
            {
                GizmosScheduler.Instance.ScheduleDraw(() =>
                {
                    Gizmos.color = NoHitCastColor;
                    Gizmos.DrawRay(ray.origin, ray.direction * maxDistance);
                    DrawDirArrow(ray);
                });
            }
        }

        public static void RaycastAll(Ray ray, RaycastHit[] hits, int count, float maxDistance)
        {
            maxDistance = GetCappedDistance(maxDistance);
            GizmosScheduler.Instance.ScheduleDraw(() =>
            {
                Gizmos.color = HitCastColor;
                for (int i = 0; i < count; i++)
                {
                    DrawHit(hits[i]);
                }

                Gizmos.color = AllCastColor;
                Gizmos.DrawRay(ray.origin, ray.direction * maxDistance);
                DrawDirArrow(ray);
            });
        }

        public static void SphereCast(Ray ray, float radius, RaycastHit hit, float maxDistance)
        {
            maxDistance = GetCappedDistance(maxDistance);
            if (hit.collider)
            {
                GizmosScheduler.Instance.ScheduleDraw(() =>
                {
                    DrawHit(hit);
                    Gizmos.color = HitCastColor;
                    ExtraGizmos.DrawCapsule(ray.origin, ray.direction, radius, hit.distance);
                    DrawDirArrow(ray);
                    if (UseSourceObject)
                    {
                        Gizmos.color = SourceObjectColor;
                        ExtraGizmos.DrawSphere(ray.origin, ray.direction, radius);
                    }
                });
            }
            else
            {
                GizmosScheduler.Instance.ScheduleDraw(() =>
                {
                    Gizmos.color = NoHitCastColor;
                    ExtraGizmos.DrawCapsule(ray.origin, ray.direction, radius, maxDistance);
                    DrawDirArrow(ray);
                    if (UseSourceObject)
                    {
                        Gizmos.color = SourceObjectColor;
                        ExtraGizmos.DrawSphere(ray.origin, ray.direction, radius);
                    }
                });
            }
        }

        public static void SphereCastAll(Ray ray, float radius, RaycastHit[] hits, int count, float maxDistance)
        {
            maxDistance = GetCappedDistance(maxDistance);
            GizmosScheduler.Instance.ScheduleDraw(() =>
            {
                for (int i = 0; i < count; i++)
                {
                    var hit = hits[i];
                    DrawHit(hit);
                    Gizmos.color = HitCastColor;
                    ExtraGizmos.DrawSphereCap(ray.origin + ray.direction * hit.distance, ray.direction, radius, 90f, ExtraGizmos.SphereCapPart.Arches | ExtraGizmos.SphereCapPart.Circles);
                }

                Gizmos.color = AllCastColor;
                ExtraGizmos.DrawCapsule(ray.origin, ray.direction, radius, maxDistance);
                DrawDirArrow(ray);
                if (UseSourceObject)
                {
                    Gizmos.color = SourceObjectColor;
                    ExtraGizmos.DrawSphere(ray.origin, ray.direction, radius);
                }
            });
        }

        public static void BoxCast(Ray ray, Vector3 halfExtents, Quaternion rot, RaycastHit hit, float maxDistance)
        {
            maxDistance = GetCappedDistance(maxDistance);
            if (hit.collider)
            {
                GizmosScheduler.Instance.ScheduleDraw(() =>
                {
                    DrawHit(hit);
                    Gizmos.color = HitCastColor;

                    ExtraGizmos.DrawBox(ray.origin + ray.direction * hit.distance, rot, halfExtents);
                    (Vector3, Quaternion, Vector3)[] boxes = new[] {
                        (ray.origin, rot, halfExtents),
                        (ray.origin + ray.direction * hit.distance, rot, halfExtents)
                    };
                    ExtraGizmos.DrawBoxLinks(boxes, ExtraGizmos.BoxLinksPart.HullLinks);
                    DrawDirArrow(ray);
                    if (UseSourceObject)
                    {
                        Gizmos.color = SourceObjectColor;
                        ExtraGizmos.DrawBox(ray.origin, rot, halfExtents);
                    }
                });
            }
            else
            {
                GizmosScheduler.Instance.ScheduleDraw(() =>
                {
                    Gizmos.color = NoHitCastColor;
                    ExtraGizmos.DrawBox(ray.origin, rot, halfExtents);
                    ExtraGizmos.DrawBox(ray.origin + ray.direction * maxDistance, rot, halfExtents);
                    (Vector3, Quaternion, Vector3)[] boxes = new[] {
                        (ray.origin, rot, halfExtents),
                        (ray.origin + ray.direction * maxDistance, rot, halfExtents)
                    };
                    ExtraGizmos.DrawBoxLinks(boxes, ExtraGizmos.BoxLinksPart.HullLinks);
                    DrawDirArrow(ray);
                });
            }
        }

        public static void BoxCastAll(Ray ray, Vector3 halfExtents, Quaternion rot, RaycastHit[] hits, int count, float maxDistance)
        {
            maxDistance = GetCappedDistance(maxDistance);
            GizmosScheduler.Instance.ScheduleDraw(() =>
            {
                for (int i = 0; i < count; i++)
                {
                    var hit = hits[i];
                    DrawHit(hit);
                    Gizmos.color = HitCastColor;
                    ExtraGizmos.DrawBox(ray.origin + ray.direction * hit.distance, rot, halfExtents);
                }

                Gizmos.color = AllCastColor;
                ExtraGizmos.DrawBox(ray.origin + ray.direction * maxDistance, rot, halfExtents);
                (Vector3, Quaternion, Vector3)[] boxes = new[] {
                        (ray.origin, rot, halfExtents),
                        (ray.origin + ray.direction * maxDistance, rot, halfExtents)
                };
                ExtraGizmos.DrawBoxLinks(boxes, ExtraGizmos.BoxLinksPart.HullLinks);
                DrawDirArrow(ray);
                if (UseSourceObject)
                {
                    Gizmos.color = SourceObjectColor;
                    ExtraGizmos.DrawBox(ray.origin, rot, halfExtents);
                }
            });
        }

        public static void CapsuleCast(Vector3 pos1, Vector3 pos2, Vector3 dir, float radius, RaycastHit hit, float maxDistance)
        {
            maxDistance = GetCappedDistance(maxDistance);
            if (hit.collider)
            {
                GizmosScheduler.Instance.ScheduleDraw(() =>
                {

                    DrawHit(hit);
                    Gizmos.color = HitCastColor;
                    Vector3 translation = dir * hit.distance;


                    Vector3 initUp = ExtraGizmos.Up;
                    ExtraGizmos.Up = dir;
                    ExtraGizmos.DrawCapsule2(pos1 + translation, pos2 + translation, radius);
                    ExtraGizmos.DrawCapsuleHull(pos1, pos2, radius, dir, hit.distance, ExtraGizmos.CapsuleHullPart.Links);

                    Vector3 center = Vector3.Lerp(pos1, pos2, 0.5f);
                    DrawDirArrow(new Ray(center, dir));
                    if (UseSourceObject)
                    {
                        Gizmos.color = SourceObjectColor;
                        ExtraGizmos.DrawCapsule2(pos1, pos2, radius);
                    }
                    ExtraGizmos.Up = initUp;
                });
            }
            else
            {
                GizmosScheduler.Instance.ScheduleDraw(() =>
                {
                    Gizmos.color = NoHitCastColor;
                    Vector3 translation = dir * maxDistance;

                    Vector3 initUp = ExtraGizmos.Up;
                    ExtraGizmos.Up = dir;
                    ExtraGizmos.DrawCapsule2(pos1 + translation, pos2 + translation, radius);
                    ExtraGizmos.DrawCapsuleHull(pos1, pos2, radius, dir, maxDistance, ExtraGizmos.CapsuleHullPart.Links);

                    Vector3 center = Vector3.Lerp(pos1, pos2, 0.5f);
                    DrawDirArrow(new Ray(center, dir));
                    if (UseSourceObject)
                    {
                        Gizmos.color = SourceObjectColor;
                        ExtraGizmos.DrawCapsule2(pos1, pos2, radius);
                    }
                    ExtraGizmos.Up = initUp;
                });
            }
        }

        public static void CapsuleCastAll(Vector3 pos1, Vector3 pos2, Vector3 dir, float radius, RaycastHit[] hits, int count, float maxDistance)
        {
            maxDistance = GetCappedDistance(maxDistance);
            GizmosScheduler.Instance.ScheduleDraw(() =>
            {
                Vector3 initUp = ExtraGizmos.Up;
                ExtraGizmos.Up = dir;

                Gizmos.color = HitCastColor;
                for (int i = 0; i < count; i++)
                {
                    DrawHit(hits[i]);
                    Vector3 hitTranslation = dir * hits[i].distance;
                    ExtraGizmos.DrawCapsule2(pos1 + hitTranslation, pos2 + hitTranslation, radius);
                }

                Gizmos.color = AllCastColor;
                ExtraGizmos.DrawCapsuleHull(pos1, pos2, radius, dir, maxDistance, ExtraGizmos.CapsuleHullPart.Links);

                Vector3 translation = dir * maxDistance;

                ExtraGizmos.DrawCapsule2(pos1 + translation, pos2 + translation, radius);

                Vector3 center = Vector3.Lerp(pos1, pos2, 0.5f);
                DrawDirArrow(new Ray(center, dir));
                if (UseSourceObject)
                {
                    Gizmos.color = SourceObjectColor;
                    ExtraGizmos.DrawCapsule2(pos1, pos2, radius);
                }
                ExtraGizmos.Up = initUp;
            });
        }

        private static void DrawDirArrow(Ray ray)
        {
            if (!UseDirArrow) return;

            ExtraGizmos.DrawArrow(ray.origin, ray.direction, DirArrowLength);
        }

        private static void DrawHit(RaycastHit hit)
        {
            Gizmos.color = HitPointColor;
            if (UseHitSphere)
                ExtraGizmos.DrawSphere(hit.point, hit.normal, HitSphereRadius);

            if (UseArrowForNormals)
                ExtraGizmos.DrawArrow(hit.point, hit.normal, HitNormalLength);
            else
                Gizmos.DrawRay(hit.point, hit.normal * HitNormalLength);
        }

        private static float GetCappedDistance(float distance)
        {
            return Mathf.Clamp(distance, -CapDistance, CapDistance);
        }
    }
}
