using Unity.Entities;

namespace DOTS_Unity.Components
{
    [GenerateAuthoringComponent]
    public struct Lifetime : IComponentData
    {
        public float value;
    }
}