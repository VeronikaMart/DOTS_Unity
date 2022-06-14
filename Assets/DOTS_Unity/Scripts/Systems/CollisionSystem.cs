using DOTS_Unity.Components;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Physics.Systems;
using Unity.Rendering;

namespace DOTS_Unity.Systems
{
    public partial class CollisionSystem : SystemBase
    {
        private StepPhysicsWorld stepPhysicsWorld; // Handle gravity, collisions and etc

        protected override void OnCreate()
        {
            stepPhysicsWorld = World.GetOrCreateSystem<StepPhysicsWorld>();
        }

        struct CollisionJob : ITriggerEventsJob
        {
            [ReadOnly] public ComponentDataFromEntity<ObjectTag> objectGroup;
            public ComponentDataFromEntity<URPMaterialPropertyBaseColor> colorGroup; // Ref to material
            public float seed;

            public void Execute(TriggerEvent triggerEvent)
            {
                bool isEntityAObjectWithTag = objectGroup.HasComponent(triggerEvent.EntityA);
                bool isEntityBObjectWithTag = objectGroup.HasComponent(triggerEvent.EntityB);

                if (!isEntityAObjectWithTag || !isEntityBObjectWithTag)
                {
                    return;
                }

                var random = new Random((uint)((1 + seed) + triggerEvent.BodyIndexA * triggerEvent.BodyIndexB)); // (1 + seed) not to be null

                // To make sure that won't be same color when two entities collides
                random = ChangeMaterialColor(random, triggerEvent.EntityA); 
                ChangeMaterialColor(random, triggerEvent.EntityB);
            }

            private Random ChangeMaterialColor(Random random, Entity entity)
            {
                if (colorGroup.HasComponent(entity))
                {
                    var colorComponent = colorGroup[entity];

                    // RGB
                    colorComponent.Value.x = random.NextFloat(0, 1);
                    colorComponent.Value.y = random.NextFloat(0, 1);
                    colorComponent.Value.z = random.NextFloat(0, 1);

                    colorGroup[entity] = colorComponent; // Save back the result 
                }

                return random;
            }
        }

        protected override void OnUpdate()
        {
            Dependency = new CollisionJob
            {
                objectGroup = GetComponentDataFromEntity<ObjectTag>(true),
                colorGroup = GetComponentDataFromEntity<URPMaterialPropertyBaseColor>(),
                seed = System.DateTimeOffset.Now.Millisecond

            }.Schedule(stepPhysicsWorld.Simulation, Dependency);
        }
    }
}