using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

namespace Birds
{
    [BurstCompile]
    public struct AreaJob : IJobParallelFor
    {
        [ReadOnly] public NativeArray<Vector3> Positions;

        public NativeArray<Vector3> Accelerations;
        public Vector3 AreaSize;

        public void Execute(int index)
        {
            var pos = Positions[index];
            var size = AreaSize * 0.5f;

            Accelerations[index] += Compensate(-size.x - pos.x, Vector3.right)
                                    + Compensate(size.x - pos.x, Vector3.left)
                                    + Compensate(-size.y - pos.y, Vector3.up)
                                    + Compensate(size.y - pos.y, Vector3.down)
                                    + Compensate(-size.z - pos.z, Vector3.forward)
                                    + Compensate(size.z - pos.z, Vector3.back);
        }

        private Vector3 Compensate(float delta, Vector3 direction)
        {
            const float threshold = 5f;
            const float multipler = 100f;
            delta = Mathf.Abs(delta);
            if (delta > threshold)
            {
                return Vector3.zero;
            }

            return direction * (1 - delta / threshold) * multipler;
        }
    }
}
