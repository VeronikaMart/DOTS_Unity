using Unity.Entities;
using Unity.Mathematics;

namespace DOTS_Unity.Components
{
    [GenerateAuthoringComponent] // Version use as MonoBehaviour
    public struct Destination : IComponentData
    {
        public float3 value; // Alternative Vector3
    }
}