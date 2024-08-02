using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEditor.PlayerSettings;
using UnityEngine.UIElements;

namespace PhysicsVisualizer
{
    public static class ExtraGizmos
    {
        public static Vector3 Up;

        public static float ArrowTipScale = 0.1f;
        public static int Steps = 4;

        public static int SafeSteps => Mathf.Clamp(Steps, 1, int.MaxValue);

        public static void DrawCone(Vector3 pos, Vector3 dir, float length, float angle, ConePart parts = ConePart.All)
        {
            float radius = Mathf.Tan(Mathf.Deg2Rad * angle) * length;
            dir = dir.normalized;

            // Draw circles
            if (parts.HasFlag(ConePart.Circles))
            {
                int steps = Mathf.RoundToInt(SafeSteps * length / radius / 6);
                float heightStep = length / steps;
                for (int i = 1; i < steps + 1; i++)
                {
                    float circleRadius = Mathf.Tan(Mathf.Deg2Rad * angle) * heightStep * i;
                    DrawCircle(pos + heightStep * i * dir, dir, circleRadius);
                }
            }

            if (!parts.HasFlag(ConePart.Cap) && !parts.HasFlag(ConePart.Shell)) return;

            Vector3 bottomPos = pos + dir * length;

            if (parts.HasFlag(ConePart.Cap))
                DrawCircleCap(bottomPos, dir, radius);

            if (parts.HasFlag(ConePart.Shell))
            {
                int stepCount = SafeSteps;
                float stepAngle = 360f / stepCount;
                var step = Quaternion.AngleAxis(stepAngle, dir);

                Vector3 previous = Quaternion.LookRotation(dir, Up) * Vector3.up * radius;
                for (int i = 0; i <= stepCount; i++)
                {
                    Vector3 next = step * previous;
                    Gizmos.DrawLine(pos, next + bottomPos);
                    previous = next;
                }
            }
        }

        public static void DrawCone2(Vector3 pos, Vector3 dir, float length, float radius, ConePart parts = ConePart.All)
        {
            float angle = Mathf.Atan2(radius, length) * Mathf.Rad2Deg;
            DrawCone(pos, dir, length, angle, parts);
        }

        public static void DrawCircle(Vector3 pos, Vector3 normal, float radius)
        {
            normal = normal == Vector3.zero ? Vector3.forward : normal;

            int stepCount = 64;
            float stepAngle = 360f / stepCount;
            var step = Quaternion.AngleAxis(stepAngle, normal);

            Vector3 previous = Quaternion.LookRotation(normal, Up) * Vector3.up * radius;

            for (int i = 0; i <= stepCount; i++)
            {
                Vector3 next = step * previous;
                Gizmos.DrawLine(previous + pos, next + pos);
                previous = next;
            }
        }

        public static void DrawArcHalf(Vector3 pos, Vector3 dir, Vector3 normal, float radius, float angle)
        {
            dir = dir == Vector3.zero ? Vector3.forward : dir;
            var dirRot = Quaternion.LookRotation(dir, normal);
            var sideRight = Quaternion.AngleAxis(angle, normal);

            int stepCount = 64;
            float step = 1f / stepCount;

            var startPoint = pos + dirRot * Vector3.forward * radius;

            var currentPoint = startPoint;

            for (int i = 0; i < stepCount + 1; i++)
            {
                var sideStep = Quaternion.Slerp(Quaternion.identity, sideRight, step * i);

                var point = pos + sideStep * dirRot * Vector3.forward * radius;
                Gizmos.DrawLine(currentPoint, point);
                currentPoint = point;
            }
        }

        public static void DrawArc(Vector3 pos, Vector3 dir, Vector3 normal, float radius, float angle)
        {
            dir = dir == Vector3.zero ? Vector3.forward : dir;
            var dirRot = Quaternion.LookRotation(dir, normal);
            var sideRight = Quaternion.AngleAxis(angle, normal);
            var sideLeft = Quaternion.AngleAxis(-angle, normal);

            int stepCount = 64;
            float step = 1f / stepCount;

            var startPoint = pos + dirRot * Vector3.forward * radius;

            var currentPoint = startPoint;

            for (int i = 0; i < stepCount + 1; i++)
            {
                var sideStep = Quaternion.Slerp(Quaternion.identity, sideRight, step * i);

                var point = pos + sideStep * dirRot * Vector3.forward * radius;
                Gizmos.DrawLine(currentPoint, point);
                currentPoint = point;
            }

            currentPoint = startPoint;

            for (int i = 0; i < stepCount + 1; i++)
            {
                var sideStep = Quaternion.Slerp(Quaternion.identity, sideLeft, step * i);

                var point = pos + sideStep * dirRot * Vector3.forward * radius;
                Gizmos.DrawLine(currentPoint, point);
                currentPoint = point;
            }
        }

        public static void DrawCircleCap(Vector3 pos, Vector3 dir, float radius)
        {
            dir = dir.normalized;

            // Draw body
            int stepCount = SafeSteps;
            float stepAngle = 360f / stepCount;
            var step = Quaternion.AngleAxis(stepAngle, dir);
            Vector3 previous = Quaternion.LookRotation(dir, Up) * Vector3.up * radius;

            Vector3 bottomPos = pos;

            for (int i = 0; i <= stepCount; i++)
            {
                Vector3 next = step * previous;
                Gizmos.DrawLine(pos, next + bottomPos);
                Gizmos.DrawLine(bottomPos, next + bottomPos);
                previous = next;
            }
        }

        public static void DrawSphereCap(Vector3 pos, Vector3 dir, float radius, float angle, SphereCapPart parts = SphereCapPart.All)
        {

            if (parts.HasFlag(SphereCapPart.Cone))
            {
                DrawCone(pos, dir, radius * Mathf.Cos(Mathf.Deg2Rad * angle), angle, ConePart.Shell);
            }

            if (parts.HasFlag(SphereCapPart.Circles))
            {
                float maxStepAngle = Mathf.CeilToInt(360f / SafeSteps);
                float currentAngle = Mathf.Abs(angle);

                while (currentAngle > 0)
                {
                    float radAngle = Mathf.Deg2Rad * currentAngle;
                    float distance = radius * Mathf.Cos(radAngle);
                    float capRadius = radius * Mathf.Sin(radAngle);

                    DrawCircle(pos + dir * distance, dir, capRadius);
                    currentAngle -= maxStepAngle;
                }
            }

            if (parts.HasFlag(SphereCapPart.Arches))
            {
                dir = dir == Vector3.zero ? Vector3.forward : dir;
                var lookRot = Quaternion.LookRotation(dir, Up);
                int archSteps = SafeSteps;
                float angleStep = 360f / archSteps;
                for (int i = 0; i < archSteps; i++)
                {
                    var spinRot = Quaternion.AngleAxis(angleStep * i, Vector3.forward);
                    DrawArcHalf(pos, dir, lookRot * spinRot * Vector3.left, radius, angle);
                    //DrawArc(pos, dir, lookRot * spinRot *Vector3.right, radius, angle);
                }
            }
        }

        public static void DrawSphere(Vector3 pos, Vector3 dir, float radius)
        {
            DrawSphereCap(pos, dir, radius, 180f, SphereCapPart.Arches | SphereCapPart.Circles);
        }

        public static void DrawCylinder(Vector3 pos, Vector3 dir, float radius, float length, CylinderPart parts = CylinderPart.All)
        {
            dir = dir == Vector3.zero ? Vector3.forward : dir.normalized;


            // Draw circles
            if (parts.HasFlag(CylinderPart.Circles))
            {
                int steps = Mathf.RoundToInt(SafeSteps * length / radius / 6);
                //int steps = STEPS;
                float heightStep = length / steps;
                for (int i = 0; i < steps + 1; i++)
                {
                    DrawCircle(pos + heightStep * i * dir, dir, radius);
                }
            }

            if (!parts.HasFlag(CylinderPart.Cap) && !parts.HasFlag(CylinderPart.Shell)) return;

            Vector3 bottomPos = pos + dir * length;

            if (parts.HasFlag(CylinderPart.Cap))
            {
                DrawCircleCap(bottomPos, dir, radius);
                DrawCircleCap(pos, dir, radius);
            }

            if (parts.HasFlag(CylinderPart.Shell))
            {
                int stepCount = SafeSteps;
                float stepAngle = 360f / stepCount;
                var step = Quaternion.AngleAxis(stepAngle, dir);

                Vector3 previous = Quaternion.LookRotation(dir, Up) * Vector3.up * radius;
                for (int i = 0; i <= stepCount; i++)
                {
                    Vector3 next = step * previous;
                    Gizmos.DrawLine(next + pos, next + bottomPos);
                    previous = next;
                }
            }
        }

        public static void DrawCapsule(Vector3 pos, Vector3 dir, float radius, float length)
        {
            DrawSphereCap(pos, -dir, radius, 90f, SphereCapPart.Arches | SphereCapPart.Circles);
            DrawCylinder(pos, dir, radius, length, CylinderPart.Shell);
            DrawSphereCap(pos + dir * length, dir, radius, 90f, SphereCapPart.Arches | SphereCapPart.Circles);
        }

        public static void DrawCapsule2(Vector3 pos1, Vector3 pos2, float radius)
        {
            Vector3 dir = pos2 - pos1;
            DrawCapsule(pos1, dir.normalized, radius, dir.magnitude);
        }

        public static void DrawCapsuleHull(Vector3 pos1, Vector3 pos2, float radius, Vector3 dir, float length, CapsuleHullPart parts = CapsuleHullPart.All)
        {
            dir = dir == Vector3.zero ? Vector3.forward : dir.normalized;

            Vector3 capsuleDir = pos1 - pos2;

            var sideRight = Quaternion.AngleAxis(90f, dir);
            var sideLeft = Quaternion.AngleAxis(-90f, dir);

            // Top
            var rotTop = Quaternion.LookRotation(dir, capsuleDir);
            var topCenter = pos1 + rotTop * Vector3.up * radius;
            var topRight = pos1 + sideRight * rotTop * Vector3.up * radius;
            var topLeft = pos1 + sideLeft * rotTop * Vector3.up * radius;

            // Bottom
            var rotBottom = Quaternion.LookRotation(dir, -capsuleDir);
            var bottomCenter = pos2 + rotBottom * Vector3.up * radius;
            var bottomRight = pos2 + sideRight * rotBottom * Vector3.up * radius;
            var bottomLeft = pos2 + sideLeft * rotBottom * Vector3.up * radius;

            // Draw Hull
            Vector3 bottomDist = dir * length;
            if (parts.HasFlag(CapsuleHullPart.Links))
            {
                Gizmos.DrawRay(topCenter, bottomDist);
                Gizmos.DrawRay(topRight, bottomDist);
                Gizmos.DrawRay(topLeft, bottomDist);
                Gizmos.DrawRay(bottomCenter, bottomDist);
                Gizmos.DrawRay(bottomRight, bottomDist);
                Gizmos.DrawRay(bottomLeft, bottomDist);
            }

            // Draw Profile
            if (parts.HasFlag(CapsuleHullPart.Silhouette))
            {
                DrawArc(pos1, rotTop * Vector3.up, dir, radius, 90f);
                DrawArc(pos1 + bottomDist, rotTop * Vector3.up, dir, radius, 90f);
                DrawArc(pos2, rotBottom * Vector3.up, dir, radius, 90f);
                DrawArc(pos2 + bottomDist, rotBottom * Vector3.up, dir, radius, 90f);
                Gizmos.DrawLine(topRight, bottomLeft);
                Gizmos.DrawLine(topLeft, bottomRight);
                Gizmos.DrawLine(topRight + bottomDist, bottomLeft + bottomDist);
                Gizmos.DrawLine(topLeft + bottomDist, bottomRight + bottomDist);
            }
        }

        public static void DrawArrow(Vector3 pos, Vector3 dir, float length)
        {
            dir = dir.normalized;
            Vector3 tipPos = pos + dir * length;
            Gizmos.DrawLine(pos, tipPos);
            float coneScale = ArrowTipScale * length;
            DrawCone2(tipPos, -dir, 2f * coneScale, coneScale);
        }

        public static void DrawSquare(Vector3 pos, Quaternion rot, Vector2 halfExtents)
        {
            var flippedExtents = new Vector2(halfExtents.x, -halfExtents.y);

            Vector3 bottomLeft = pos + rot * halfExtents;
            Vector3 bottomRight = pos + rot * flippedExtents;
            Vector3 topRight = pos + rot * -halfExtents;
            Vector3 topLeft = pos + rot * -flippedExtents;

            Gizmos.DrawLine(bottomLeft, bottomRight);
            Gizmos.DrawLine(bottomRight, topRight);
            Gizmos.DrawLine(topRight, topLeft);
            Gizmos.DrawLine(topLeft, bottomLeft);
        }

        public static void DrawBox(Vector3 pos, Quaternion rot, Vector3 halfExtents)
        {
            var edges = BoxEdges.FromBox(pos, rot, halfExtents);

            //Draw Backface
            Gizmos.DrawLine(edges.bottomLeftBack, edges.bottomRightBack);
            Gizmos.DrawLine(edges.bottomRightBack, edges.topRightBack);
            Gizmos.DrawLine(edges.topRightBack, edges.topLeftBack);
            Gizmos.DrawLine(edges.topLeftBack, edges.bottomLeftBack);

            //Draw Frontface
            Gizmos.DrawLine(edges.bottomLeftFront, edges.bottomRightFront);
            Gizmos.DrawLine(edges.bottomRightFront, edges.topRightFront);
            Gizmos.DrawLine(edges.topRightFront, edges.topLeftFront);
            Gizmos.DrawLine(edges.topLeftFront, edges.bottomLeftFront);

            //Draw Sidefaces
            Gizmos.DrawLine(edges.bottomLeftBack, edges.bottomLeftFront);
            Gizmos.DrawLine(edges.bottomRightBack, edges.bottomRightFront);
            Gizmos.DrawLine(edges.topRightBack, edges.topRightFront);
            Gizmos.DrawLine(edges.topLeftBack, edges.topLeftFront);
        }

        public static void DrawBoxLinks((Vector3 pos, Quaternion rot, Vector3 halfExtents)[] boxes, BoxLinksPart parts = BoxLinksPart.All)
        {
            if (boxes.Length < 2) return;

            BoxEdges[] edges = boxes.Select(b => BoxEdges.FromBox(b.pos, b.rot, b.halfExtents)).ToArray();

            for (int i = 1; i < edges.Length; i++)
            {
                var first = edges[i - 1];
                var second = edges[i];

                if (parts.HasFlag(BoxLinksPart.All))
                {
                    Gizmos.DrawLine(first.bottomLeftBack, second.bottomLeftBack);
                    Gizmos.DrawLine(first.bottomRightBack, second.bottomRightBack);
                    Gizmos.DrawLine(first.topRightBack, second.topRightBack);
                    Gizmos.DrawLine(first.topLeftBack, second.topLeftBack);
                    Gizmos.DrawLine(first.bottomLeftFront, second.bottomLeftFront);
                    Gizmos.DrawLine(first.bottomRightFront, second.bottomRightFront);
                    Gizmos.DrawLine(first.topRightFront, second.topRightFront);
                    Gizmos.DrawLine(first.topLeftFront, second.topLeftFront);
                }
                else
                {
                    Vector3 dir = (boxes[i].pos - boxes[i - 1].pos).normalized;
                    if (dir == Vector3.zero)
                        dir = boxes[i].rot * Vector3.forward;

                    var dirRot = Quaternion.LookRotation(dir, Up);
                    var inverseRot = Quaternion.Inverse(dirRot);

                    var projectedPoints = new Vector3[]
                    {
                        inverseRot * first.bottomLeftBack,
                        inverseRot * first.bottomRightBack,
                        inverseRot * first.topRightBack,
                        inverseRot * first.topLeftBack,
                        inverseRot * first.bottomLeftFront,
                        inverseRot * first.bottomRightFront,
                        inverseRot * first.topRightFront,
                        inverseRot * first.topLeftFront
                    };

                    int innerMostIndex = 0;
                    float innerMostDistance = projectedPoints[0].z;
                    int outerMostIndex = 0;
                    float outerMostDistance = projectedPoints[0].z;

                    for (int j = 0; j < projectedPoints.Length; j++)
                    {
                        float distance = projectedPoints[j].z;
                        if (innerMostDistance > distance)
                        {
                            innerMostDistance = distance;
                            innerMostIndex = j;
                        }
                        else if (outerMostDistance < distance)
                        {
                            outerMostDistance = distance;
                            outerMostIndex = j;
                        }
                    }

                    if (parts.HasFlag(BoxLinksPart.HullLinks))
                    {
                        if (innerMostIndex != 0 && outerMostIndex != 0) Gizmos.DrawLine(first.bottomLeftBack, second.bottomLeftBack);
                        if (innerMostIndex != 1 && outerMostIndex != 1) Gizmos.DrawLine(first.bottomRightBack, second.bottomRightBack);
                        if (innerMostIndex != 2 && outerMostIndex != 2) Gizmos.DrawLine(first.topRightBack, second.topRightBack);
                        if (innerMostIndex != 3 && outerMostIndex != 3) Gizmos.DrawLine(first.topLeftBack, second.topLeftBack);
                        if (innerMostIndex != 4 && outerMostIndex != 4) Gizmos.DrawLine(first.bottomLeftFront, second.bottomLeftFront);
                        if (innerMostIndex != 5 && outerMostIndex != 5) Gizmos.DrawLine(first.bottomRightFront, second.bottomRightFront);
                        if (innerMostIndex != 6 && outerMostIndex != 6) Gizmos.DrawLine(first.topRightFront, second.topRightFront);
                        if (innerMostIndex != 7 && outerMostIndex != 7) Gizmos.DrawLine(first.topLeftFront, second.topLeftFront);
                    }
                    else if (parts.HasFlag(BoxLinksPart.InnerLinks))
                    {
                        if (innerMostIndex != 0 && outerMostIndex != 0) Gizmos.DrawLine(first.bottomLeftBack, second.bottomLeftBack);
                        if (innerMostIndex != 1 && outerMostIndex != 1) Gizmos.DrawLine(first.bottomRightBack, second.bottomRightBack);
                        if (innerMostIndex != 2 && outerMostIndex != 2) Gizmos.DrawLine(first.topRightBack, second.topRightBack);
                        if (innerMostIndex != 3 && outerMostIndex != 3) Gizmos.DrawLine(first.topLeftBack, second.topLeftBack);
                        if (innerMostIndex != 4 && outerMostIndex != 4) Gizmos.DrawLine(first.bottomLeftFront, second.bottomLeftFront);
                        if (innerMostIndex != 5 && outerMostIndex != 5) Gizmos.DrawLine(first.bottomRightFront, second.bottomRightFront);
                        if (innerMostIndex != 6 && outerMostIndex != 6) Gizmos.DrawLine(first.topRightFront, second.topRightFront);
                        if (innerMostIndex != 7 && outerMostIndex != 7) Gizmos.DrawLine(first.topLeftFront, second.topLeftFront);
                    }
                }
            }
        }

        [Flags]
        public enum ConePart
        {
            Circles = 1,
            Cap = 2,
            Shell = 4,
            All = 7
        }

        [Flags]
        public enum SphereCapPart
        {
            Circles = 1,
            Cone = 2,
            Arches = 4,
            All = 7
        }

        [Flags]
        public enum CylinderPart
        {
            Circles = 1,
            Cap = 2,
            Shell = 4,
            All = 7
        }

        [Flags]
        public enum CapsulePart
        {
            Circles = 1,
            Cap = 2,
            Shell = 4,
            All = 7
        }

        [Flags]
        public enum BoxLinksPart
        {
            HullLinks = 1,
            InnerLinks = 2,
            All = 3
        }

        [Flags]
        public enum CapsuleHullPart
        {
            Silhouette = 1,
            Links = 2,
            All = 3
        }

        private struct BoxEdges
        {
            //Backfaces
            public Vector3 bottomLeftBack;
            public Vector3 bottomRightBack;
            public Vector3 topRightBack;
            public Vector3 topLeftBack;

            //Frontfaces
            public Vector3 bottomLeftFront;
            public Vector3 bottomRightFront;
            public Vector3 topRightFront;
            public Vector3 topLeftFront;

            public static BoxEdges FromBox(Vector3 pos, Quaternion rot, Vector3 halfExtents)
            {
                Vector3 forward = rot * (Vector3.forward * halfExtents.z);

                Vector2 squareExtents = halfExtents;

                var flippedExtents = new Vector2(squareExtents.x, -squareExtents.y);

                return new BoxEdges
                {
                    //Backfaces
                    bottomLeftBack = pos - forward + rot * squareExtents,
                    bottomRightBack = pos - forward + rot * flippedExtents,
                    topRightBack = pos - forward + rot * -squareExtents,
                    topLeftBack = pos - forward + rot * -flippedExtents,

                    //Frontfaces
                    bottomLeftFront = pos + forward + rot * squareExtents,
                    bottomRightFront = pos + forward + rot * flippedExtents,
                    topRightFront = pos + forward + rot * -squareExtents,
                    topLeftFront = pos + forward + rot * -flippedExtents
                };
            }
        }
    }
}
