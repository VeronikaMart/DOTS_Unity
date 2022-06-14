using Unity.Entities;

namespace DOTS_Unity.Components
{
    [GenerateAuthoringComponent]
    public struct MovementSpeed : IComponentData
    {
        public float value;
    }
}