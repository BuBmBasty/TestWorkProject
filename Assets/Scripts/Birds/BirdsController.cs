using System;
using System.Collections;
using System.Collections.Generic;
using Birds;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;
using UnityEngine.Jobs;
using Random = UnityEngine.Random;

public class BirdsController : MonoBehaviour
{
    [SerializeField] private BirdData _birdPrefab;
    [SerializeField] private int _numberOfBirds;
    [SerializeField] private float _destinationThresold, _maxSpeed;
    [SerializeField] private Vector3 _areaSize, _weightSize;

    private NativeArray<Vector3> _positions;
    private NativeArray<Vector3> _velocities;
    private NativeArray<Vector3> _accelerations;
    
    private TransformAccessArray _transformAccess;
    void Start()
    {
        _positions = new NativeArray<Vector3>(_numberOfBirds, Allocator.Persistent);
        _velocities = new NativeArray<Vector3>(_numberOfBirds, Allocator.Persistent);
        _accelerations = new NativeArray<Vector3>(_numberOfBirds, Allocator.Persistent);

        var transforms = new Transform[_numberOfBirds];
        
        for (int i = _numberOfBirds; i > 0; i--)
        {
            var bird = Instantiate(_birdPrefab);
            bird.SetAnimation((int)Random.Range(20,50),(float)Random.Range(0.3f,1));
            transforms[i - 1] = bird.transform;
            _velocities[i - 1] = Random.insideUnitSphere;
        }

        _transformAccess = new TransformAccessArray(transforms);
    }

  
    void Update()
    {
        var boundsJob = new AreaJob()
        {
            Accelerations = _accelerations,
            Positions = _positions,
            AreaSize = _areaSize
        };
        var accelerationJob = new AccelerationJob()
        {
            Positions = _positions,
            Velocities = _velocities,
            Accelerations = _accelerations,
            Weights = _weightSize,
            DestinationThreshold = _destinationThresold
        };
        
        var moveJob = new JobsSystemForBirds()
        {
            Positions = _positions,
            Velocities = _velocities,
            Accelerations = _accelerations,
            VelocityLimit = _maxSpeed,
            DeltaTime = Time.deltaTime
        };
        var areaSizeHandle = boundsJob.Schedule(_numberOfBirds, 0);
        var aceelerationHandle = accelerationJob.Schedule(_numberOfBirds, 0,areaSizeHandle);
        var moveHandle = moveJob.Schedule(_transformAccess,aceelerationHandle);
        moveHandle.Complete();
    }

    private void OnDestroy()
    {
        _positions.Dispose();
        _velocities.Dispose();
        _accelerations.Dispose();
        _transformAccess.Dispose();
    }
}
