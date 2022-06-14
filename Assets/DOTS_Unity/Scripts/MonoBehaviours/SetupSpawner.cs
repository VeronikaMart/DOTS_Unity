using DOTS_Unity.Components;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace DOTS_Unity.MonoBehaviours
{
    public class SetupSpawner : MonoBehaviour
    {
        [SerializeField] private GameObject minionPrefab;
        [SerializeField] private int gridSize;
        [SerializeField] private int spread = 1; // Distance between entities
        [Space]
        [SerializeField] private Vector2 speedRange = new Vector2(2, 8);
        [SerializeField] private Vector2 lifetimeRange = new Vector2(10, 60);

        private BlobAssetStore blob;

        private void Start()
        {
            // 1. Store memory
            blob = new BlobAssetStore(); // Allocate memory we need 

            // 2. Converting gameObject to entity
            // Pass into conversions uses this memory and handle it
            var settings = GameObjectConversionSettings.FromWorld(World.DefaultGameObjectInjectionWorld, blob);

            // 3. Take our GO use 2 settings and convert 
            var entity = GameObjectConversionUtility.ConvertGameObjectHierarchy(minionPrefab, settings);

            // 3. Grab our entity manager which handles spawning 
            var entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;

            for (int x = 0; x < gridSize; x++)
            {
                for (int z = 0; z < gridSize; z++)
                {
                    // 4. Call entity manager and instantiate entity
                    var instance = entityManager.Instantiate(entity); // Same Instantiate(personPrefab)

                    float3 position = new float3(x * spread, 0, z * spread);

                    entityManager.SetComponentData(instance, new Translation { Value = position }); // Allow to specify each entity 
                    entityManager.SetComponentData(instance, new Destination { value = position });

                    var speed = UnityEngine.Random.Range(speedRange.x, speedRange.y); // Set random speed for each entity
                    entityManager.SetComponentData(instance, new MovementSpeed { value = speed });

                    var time = UnityEngine.Random.Range(lifetimeRange.x, lifetimeRange.y); // Set random lifetime for each entity
                    entityManager.SetComponentData(instance, new Lifetime { value = time });
                }
            }
        }

        private void OnDestroy()
        {
            blob.Dispose(); // Dispose memory
        }
    }
}