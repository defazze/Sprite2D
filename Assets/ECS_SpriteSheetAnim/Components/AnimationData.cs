using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public struct AnimationData : IComponentData
{
    public int currentFrame;
    public float frameTimer;
    public float frameTimerMax;

    public Vector4 uv;
    public Matrix4x4 matrix;

    public int atlasKey;
}
