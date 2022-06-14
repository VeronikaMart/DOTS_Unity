using DOTS_Unity.Components;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;

namespace DOTS_Unity.Systems
{
    public partial class MoveToDestinationSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            float deltaTime = Time.DeltaTime;

            // "Ref" we can read and change transform, "in" only take values
            Entities.ForEach((ref Translation translation, ref Rotation rotation, in Destination destination, in MovementSpeed movementSpeed) =>
            {
                // Check if all x, y, z components are equal
                if (math.all(destination.value == translation.Value))
                {
                    return;
                }

                float3 destinatioTo = destination.value - translation.Value;
                rotation.Value = quaternion.LookRotation(destinatioTo, new float3(0, 1, 0));

                float3 movement = math.normalize(destinatioTo) * movementSpeed.value * deltaTime;

                if (math.length(movement) >= math.length(destinatioTo))
                {
                    translation.Value = destination.value;
                }

                else
                {
                    translation.Value += movement;
                }

            }).ScheduleParallel(); // Schedule() Puts the job into the job queue for execution at the appropriate time
                                   // Run job not in main thread, better performance 
                                   // Run() on main thread
                                   // ScheduleParallel() parallel
        }
    }
}