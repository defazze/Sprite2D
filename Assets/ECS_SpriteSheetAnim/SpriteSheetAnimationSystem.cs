using UnityEngine;
using Unity.Entities;
using Unity.Collections;
using Unity.Jobs;
using Unity.Transforms;
using Unity.Mathematics;

public class SpriteSheetAnimation_Animate : JobComponentSystem
{

    //[BurstCompile]
    public struct Job : IJobForEachWithEntity<AnimationData, Translation>
    {

        public float deltaTime;

        public void Execute(Entity e, int what, ref AnimationData animationData, ref Translation translation)
        {
            var em = World.Active.EntityManager;
            var atlasData = em.GetSharedComponentData<AtlasData>(e);

            var rowData = atlasData.rows[animationData.atlasKey];
            var scaleFactor = GameHandler.GetInstance().scaleFactor;

            animationData.frameTimer += deltaTime;
            while (animationData.frameTimer >= animationData.frameTimerMax)
            {
                animationData.frameTimer -= animationData.frameTimerMax;
                animationData.currentFrame = (animationData.currentFrame + 1) % rowData.framesCount;

                float uvWidth = (rowData.bounds.x / atlasData.bounds.x) / rowData.framesCount;
                float uvHeight = (rowData.bounds.y / atlasData.bounds.y);
                float uvOffsetX = uvWidth * animationData.currentFrame;
                float uvOffsetY = (rowData.offsetY / atlasData.bounds.y);
                animationData.uv = new Vector4(uvWidth, uvHeight, uvOffsetX, uvOffsetY);

                float3 position = translation.Value;
                position.z = position.y * .01f;
                var scale = new float3((rowData.bounds.x / rowData.framesCount) / scaleFactor, rowData.bounds.y / scaleFactor, 1f);
                animationData.matrix = Matrix4x4.TRS(position, Quaternion.identity, scale);
            }
        }

    }

    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        Job job = new Job
        {
            deltaTime = Time.deltaTime
        };
        return job.Schedule(this, inputDeps);
    }

}