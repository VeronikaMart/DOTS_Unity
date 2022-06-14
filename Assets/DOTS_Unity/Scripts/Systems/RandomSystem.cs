using Unity.Collections;
using Unity.Entities;
using Unity.Jobs.LowLevel.Unsafe;

namespace DOTS_Unity.Systems
{
    [UpdateInGroup(typeof(InitializationSystemGroup))] // To make sure it will be in Initialization group
    public partial class RandomSystem : SystemBase
    {
        // Exposes a buffer of native memory, which allows to share data between managed and native without marshalling costs
        public NativeArray<Unity.Mathematics.Random> RandomArray { get; private set; }

        // Called once when system is created 
        protected override void OnCreate()
        {
            var maxJobThreadCount = JobsUtility.MaxJobThreadCount;
            var randomArray = new Unity.Mathematics.Random[maxJobThreadCount]; // How many threads we will use 
            var seed = new System.Random();

            for (int i = 0; i < maxJobThreadCount; i++)
            {
                randomArray[i] = new Unity.Mathematics.Random((uint)seed.Next()); // Generate random number 
            }

            // Allocator is how memory is going to be deal with 
            RandomArray = new NativeArray<Unity.Mathematics.Random>(randomArray, Allocator.Persistent); // Until program stop running 
        }

        protected override void OnDestroy()
        {
            RandomArray.Dispose(); // Get rid of memory, when we stop running the system
        }

        protected override void OnUpdate() { }
    }
}