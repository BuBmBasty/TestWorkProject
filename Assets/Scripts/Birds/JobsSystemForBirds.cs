using Unity.Burst;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Jobs;

namespace Birds
{
    [BurstCompile]
    public struct JobsSystemForBirds : IJobParallelForTransform
    {
        public NativeArray<Vector3> Positions;
        public NativeArray<Vector3> Velocities;
        public NativeArray<Vector3> Accelerations;
        public float DeltaTime, VelocityLimit;

        private Vector3 _velocityVector, _direction;
    
        public void Execute(int index, TransformAccess transform)
        {
            _velocityVector = Velocities[index] + Accelerations[index]*DeltaTime;
            _direction = _velocityVector.normalized;

            _velocityVector = _direction * Mathf.Clamp(_velocityVector.magnitude, 1,VelocityLimit);

            transform.position += _velocityVector * DeltaTime;
            transform.rotation = Quaternion.LookRotation(_direction);

            Positions[index] = transform.position;
            Velocities[index] = _velocityVector;
            Accelerations[index] = Vector3.zero;
        }
    }
}

