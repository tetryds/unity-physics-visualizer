using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using static Codice.Client.Commands.WkTree.WorkspaceTreeNode;

namespace PhysicsVisualizer
{
    public static class ExtraGizmos
    {
        public static int STEPS = 4;

        public static void DrawCone(Vector3 pos, Vector3 dir, float height, float angle, ConePart parts = ConePart.All)
        {
            dir = dir.normalized;

            // Draw circles
            if (parts.HasFlag(ConePart.Circles))
            {
                int steps = STEPS;
                float heightStep = height / steps;
                for (int i = 1; i < steps + 1; i++)
                {
                    float circleRadius = Mathf.Tan(Mathf.Deg2Rad * angle) * heightStep * i;
                    DrawCircle(pos + heightStep * i * dir, dir, circleRadius);
                }
            }

            if (!parts.HasFlag(ConePart.Cap) && !parts.HasFlag(ConePart.Shell)) return;

            float bottomRadius = Mathf.Tan(Mathf.Deg2Rad * angle) * height;
            Vector3 bottomPos = pos + dir * height;

            if (parts.HasFlag(ConePart.Cap))
                DrawCircleCap(bottomPos, dir, bottomRadius);

            if (parts.HasFlag(ConePart.Shell))
            {
                int stepCount = STEPS * 2;
                float stepAngle = 360f / stepCount;
                var step = Quaternion.AngleAxis(stepAngle, dir);

                Vector3 previous = Quaternion.LookRotation(dir) * Vector3.up * bottomRadius;
                for (int i = 0; i <= stepCount; i++)
                {
                    Vector3 next = step * previous;
                    Gizmos.DrawLine(pos, next + bottomPos);
                    previous = next;
                }
            }
        }

        public static void DrawCircle(Vector3 pos, Vector3 normal, float radius)
        {
            int stepCount = 64;
            float stepAngle = 360f / stepCount;
            var step = Quaternion.AngleAxis(stepAngle, normal);

            Vector3 previous = Quaternion.LookRotation(normal) * Vector3.up * radius;

            for (int i = 0; i <= stepCount; i++)
            {
                Vector3 next = step * previous;
                Gizmos.DrawLine(previous + pos, next + pos);
                previous = next;
            }
        }

        public static void DrawArc(Vector3 pos, Vector3 dir, Vector3 normal, float radius, float angle)
        {
            var dirRot = Quaternion.LookRotation(dir, normal);
            var sideRight = Quaternion.AngleAxis(angle, normal);
            var sideLeft = Quaternion.AngleAxis(-angle, normal);

            //int stepCount = Mathf.CeilToInt(angle / 5.625f);
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
            if (dir == Vector3.zero) throw new ArgumentException("Cannot draw a cone with no direction");

            dir = dir.normalized;

            // Draw body
            int stepCount = STEPS;
            float stepAngle = 360f / stepCount;
            var step = Quaternion.AngleAxis(stepAngle, dir);
            Vector3 previous = Quaternion.LookRotation(dir) * Vector3.up * radius;

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
                float maxStepAngle = Mathf.CeilToInt(360f / STEPS);
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
                var lookRot = Quaternion.LookRotation(dir);
                int archSteps = STEPS / 2;
                float angleStep = 180f / archSteps;
                for (int i = 0; i < archSteps; i++)
                {
                    var spinRot = Quaternion.AngleAxis(angleStep * i, Vector3.forward);
                    DrawArc(pos, dir, lookRot * spinRot * Vector3.up, radius, angle);
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
            dir = dir.normalized;


            // Draw circles
            if (parts.HasFlag(CylinderPart.Circles))
            {
                int steps = Mathf.RoundToInt(length / radius);
                float heightStep = length / steps;
                for (int i = 0; i < steps; i++)
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
                int stepCount = STEPS;
                float stepAngle = 360f / stepCount;
                var step = Quaternion.AngleAxis(stepAngle, dir);

                Vector3 previous = Quaternion.LookRotation(dir) * Vector3.up * radius;
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
            DrawCylinder(pos, dir, radius, length, CylinderPart.Circles | CylinderPart.Shell);
            DrawSphereCap(pos + dir * length, dir, radius, 90f, SphereCapPart.Arches | SphereCapPart.Circles);
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
    }
}
