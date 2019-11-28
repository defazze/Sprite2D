using UnityEngine;
using Unity.Entities;
using Unity.Collections;
using Unity.Transforms;
using Unity.Mathematics;
using Random = UnityEngine.Random;

public class GameHandler : MonoBehaviour
{
    private static GameHandler instance;

    public static GameHandler GetInstance()
    {
        return instance;
    }

    public Material knightAtlas;
    public float scaleFactor;

    private NativeHashMap<int, RowData> _atlasData;

    private void Awake()
    {
        instance = this;
        _atlasData = new NativeHashMap<int, RowData>(4, Allocator.Persistent);
        EntityManager entityManager = World.Active.EntityManager;

        EntityArchetype entityArchetype = entityManager.CreateArchetype(
            typeof(Translation),
            typeof(AnimationData),
            typeof(AtlasData)
        );

        NativeArray<Entity> entityArray = new NativeArray<Entity>(1, Allocator.Temp);
        entityManager.CreateEntity(entityArchetype, entityArray);

        var atlasData = new NativeHashMap<int, RowData>(4, Allocator.Persistent);

        atlasData.TryAdd((int)KnightAtlasKeys.Hurt, new RowData(new float2(102, 64), 0, 2));
        atlasData.TryAdd((int)KnightAtlasKeys.Walk, new RowData(new float2(306, 64), 64, 6));
        atlasData.TryAdd((int)KnightAtlasKeys.Idle, new RowData(new float2(270, 64), 128, 6));
        atlasData.TryAdd((int)KnightAtlasKeys.Attack, new RowData(new float2(744, 64), 192, 6));

        var atlasDataComponent = new AtlasData { rows = atlasData, bounds = new float2(744, 256), material = knightAtlas };

        var keys = new[] { 1, 2, 3, 4 };

        foreach (Entity entity in entityArray)
        {
            entityManager.SetComponentData(entity,
                new Translation
                {
                    Value = new float3(Random.Range(-5f, 5f), Random.Range(-2.5f, 2.5f), 0)
                }
            );

            var key = keys[Random.Range(0, 4)];
            var framesCount = atlasData[key].framesCount;

            entityManager.SetComponentData(entity,
                new AnimationData
                {
                    currentFrame = UnityEngine.Random.Range(0, (int)framesCount),
                    frameTimer = UnityEngine.Random.Range(0f, 1f),
                    frameTimerMax = .1f,
                    atlasKey = key
                }
            );

            entityManager.SetSharedComponentData(entity, atlasDataComponent);
        }

        entityArray.Dispose();

    }

    void OnDestroy()
    {
        _atlasData.Dispose();
    }

}
