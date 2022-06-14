using DOTS_Unity.Components;
using Unity.Entities;
using Unity.Jobs;

namespace DOTS_Unity.Systems
{
    public partial class LifetimeSystem : SystemBase
    {
        private EndSimulationEntityCommandBufferSystem bufferSystem;

        protected override void OnCreate()
        {
            bufferSystem = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
        }

        protected override void OnUpdate()
        {
            float deltaTime = Time.DeltaTime;
            var buffer = bufferSystem.CreateCommandBuffer().AsParallelWriter();

            Entities.ForEach((Entity entity, int entityInQueryIndex, ref Lifetime lifetime) =>
            {
                lifetime.value -= deltaTime;

                if (lifetime.value <= 0)
                {
                    buffer.DestroyEntity(entityInQueryIndex, entity);
                }

            }).ScheduleParallel();

            bufferSystem.AddJobHandleForProducer(Dependency);
        }
    }
}