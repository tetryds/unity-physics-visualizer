using UnityEngine;

namespace PhysicsVisualizer.Demo
{
    [ExecuteAlways]
    public class ExtraGizmosDemo : MonoBehaviour
    {
        [Min(1)]
        [SerializeField] int steps;
        [SerializeField] float length;
        [SerializeField] float radius;
        [SerializeField] Color color;
        [SerializeField] Vector3 boxDimensions;

        [SerializeField] bool cylinder;
        [SerializeField] bool cone;
        [SerializeField] bool circleCap;
        [SerializeField] bool sphereCap;
        [SerializeField] bool arrow;
        [SerializeField] bool box;
        [SerializeField] bool square;
        [SerializeField] bool capsule;

        private void OnDrawGizmos()
        {
            Gizmos.color = color;
            Vector3 initUp = ExtraGizmos.Up;
            ExtraGizmos.Up = transform.up;
            ExtraGizmos.Steps = steps;
            if (cylinder) ExtraGizmos.DrawCylinder(transform.position, transform.forward, radius, length);
            if (cone) ExtraGizmos.DrawCone2(transform.position, transform.forward, length, radius);
            if (circleCap) ExtraGizmos.DrawCircleCap(transform.position, transform.forward, radius);
            if (sphereCap) ExtraGizmos.DrawSphereCap(transform.position, transform.forward, radius, 90f);
            if (arrow) ExtraGizmos.DrawArrow(transform.position, transform.forward, length);
            if (square) ExtraGizmos.DrawSquare(transform.position, transform.rotation, boxDimensions);
            if (box) ExtraGizmos.DrawBox(transform.position, transform.rotation, boxDimensions);
            if (capsule) ExtraGizmos.DrawCapsule(transform.position, transform.forward, radius, length);
            ExtraGizmos.Up = initUp;
        }
    }
}
