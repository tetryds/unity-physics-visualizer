using System;
using System.Collections.Generic;
using UnityEngine;

namespace PhysicsVisualizer
{
    public class GizmosScheduler : AutoSingletonBehaviour<GizmosScheduler>
    {
        readonly Queue<Action> drawQueue = new();

        public void ScheduleDraw(Action drawAction)
        {
            drawQueue.Enqueue(drawAction);
        }

        private void OnDrawGizmos()
        {
            while (drawQueue.TryDequeue(out var drawAction))
                drawAction();
        }
    }
}
