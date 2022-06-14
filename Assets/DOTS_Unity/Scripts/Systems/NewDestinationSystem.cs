using DOTS_Unity.Components;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;

namespace DOTS_Unity.Systems
{
    public partial class NewDestinationSystem : SystemBase
    {
        private RandomSystem randomSystem;

        protected override void OnCreate()
        {
            // Get random system and store it here 
            // RandomSystem exists before NewDestinationSystem
            randomSystem = World.GetExistingSystem<RandomSystem>();
        }

        protected override void OnUpdate()
        {
            var randomArray = randomSystem.RandomArray;

            Entities
                .WithNativeDisableContainerSafetyRestriction(randomArray) // Turning off safety feature 
                .ForEach((int nativeThreadIndex, ref Destination destination, in Translation translation) =>
                {

                // How far we are from destination 
                float distance = math.abs(math.length(destination.value - translation.Value));

                    if (distance < .1f)
                    {
                        var random = randomArray[nativeThreadIndex];

                        destination.value.x = random.NextFloat(0, 500);
                        destination.value.z = random.NextFloat(0, 500);

                        randomArray[nativeThreadIndex] = random; // Save back to array
                    }

                }).ScheduleParallel();
        }
    }
}