/* This is taken from this blog post:
 * http://loyc-etc.blogspot.ca/2014/05/2d-convex-hull-in-c-45-lines-of-code.html
 *
 * All I have done is renamed "DList" to "CircularList" and then wrote a wrapper for the generic C# list.
 * The structure that is supposed to be used is *much* more efficient, but this works for my purposes.
 *
 * This can be dropped right into your Unity project and will work without any adjustments.
 * Editted and adjusted by tetryds
 */
using System.Collections.Generic;
using UnityEngine;

namespace PhysicsVisualizer
{

    public static class HullComputer
    {
        public static IList<Vector2> ComputeConvexHull(List<Vector2> points, bool sortInPlace = false)
        {
            if (!sortInPlace)
                points = new List<Vector2>(points);

            static int Compare(Vector2 a, Vector2 b)
            {
                return a.x == b.x ? a.y.CompareTo(b.y) : (a.x > b.x ? 1 : -1);
            }

            points.Sort(Compare);

            var hull = new HullList<Vector2>();
            int lower = 0;
            int upper = 0;

            // Builds a hull such that the output polygon starts at the leftmost Vector2.
            for (int i = points.Count - 1; i >= 0; i--)
            {
                Vector2 point = points[i];

                Vector2 current;

                // build lower hull (at end of output list)
                while (lower >= 2 && (current = hull.Last).Sub(hull[hull.Count - 2]).Cross(point.Sub(current)) >= 0)
                {
                    hull.PopLast();
                    lower--;
                }
                hull.PushLast(point);
                lower++;

                // build upper hull (at beginning of output list)
                while (upper >= 2 && (current = hull.First).Sub(hull[1]).Cross(point.Sub(current)) <= 0)
                {
                    hull.PopFirst();
                    upper--;
                }

                // when upper=0, share the Vector2 added above
                if (upper != 0)
                    hull.PushFirst(point);

                upper++;
                Debug.Assert(upper + lower == hull.Count + 1);
            }
            hull.PopLast();
            return hull;
        }

        private static Vector2 Sub(this Vector2 a, Vector2 b)
        {
            return a - b;
        }

        private static float Cross(this Vector2 a, Vector2 b)
        {
            return a.x * b.y - a.y * b.x;
        }

        private class HullList<T> : List<T>
        {
            public T Last
            {
                get => this[Count - 1];
                set => this[Count - 1] = value;
            }

            public T First
            {
                get => this[0];
                set => this[0] = value;
            }

            public void PushLast(T obj) => Add(obj);

            public T PopLast()
            {
                T retVal = this[Count - 1];
                RemoveAt(Count - 1);
                return retVal;
            }

            public void PushFirst(T obj) => Insert(0, obj);

            public T PopFirst()
            {
                T retVal = this[0];
                RemoveAt(0);
                return retVal;
            }
        }
    }
}
