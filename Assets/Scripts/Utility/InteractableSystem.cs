using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Transforms;
using Unity.Mathematics;

namespace EduLabs.Utility
{

  public class InteractableSystem : JobComponentSystem
  {

    EntityQuery playerQuery;
    EntityQuery interactablesQuery;

    protected override void OnCreate()
    {
      playerQuery = GetEntityQuery(
        ComponentType.ReadOnly<Player.PlayerTag>(),
        ComponentType.ReadOnly<Rotation>());

      interactablesQuery = GetEntityQuery(
        ComponentType.ReadOnly<InteractableTag>(),
        ComponentType.ReadOnly<Translation>());
    }

    struct DistanceEntry
    {
      public float distance;
      public Entity entity;
    }


    struct ComputeApparentDistance : IJobChunk
    {
      [ReadOnly] public float3 cameraPos;
      [ReadOnly] public float3 cameraFwd;
      public NativeArray<DistanceEntry> apparentDistances;

      [ReadOnly] public ArchetypeChunkEntityType EntityType;
      [ReadOnly] public ArchetypeChunkComponentType<Translation> TranslationType;

      public void Execute(ArchetypeChunk chunk, int chunkIndex, int firstEntityIndex)
      {
        NativeArray<Entity> chunkEntities = chunk.GetNativeArray(EntityType);
        NativeArray<Translation> chunkTranslations = chunk.GetNativeArray(TranslationType);
        for (int idx = 0; idx < chunk.Count; ++idx)
        {
          float3 objDir = chunkTranslations[idx].Value - cameraPos;
          apparentDistances[firstEntityIndex + idx] = new DistanceEntry
          {
            distance = math.dot(cameraFwd, objDir),
            entity = chunkEntities[idx]
          };
        }
      }
    }


    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
      NativeArray<Entity> players = playerQuery.ToEntityArray(Allocator.TempJob);
      Entity camera = players[0];
      Translation cameraPos = EntityManager.GetComponentData<Translation>(camera);
      Rotation cameraRot = EntityManager.GetComponentData<Rotation>(camera);

      float3 cameraFwd = math.mul(cameraRot.Value, new float3(0, 0, 1));

      NativeArray<DistanceEntry> apparentDistances = new NativeArray<DistanceEntry>(
        interactablesQuery.CalculateEntityCount(), Allocator.TempJob);

      JobHandle apparentDistanceHandle = new ComputeApparentDistance
      {
        EntityType = GetArchetypeChunkEntityType(),
        TranslationType = GetArchetypeChunkComponentType<Translation>(true),
        cameraPos = cameraPos.Value,
        cameraFwd = cameraFwd,
      }.ScheduleParallel(interactablesQuery, inputDeps);

      JobHandle sortHandle = Job.WithCode(() =>
      {
        apparentDistances.Sort(Comparer<DistanceEntry>.Create(
          (DistanceEntry entryA, DistanceEntry entryB) => math.abs(entryA.distance).CompareTo(math.abs(entryB.distance))));

        // apparentDistances[0] Es el elemento más cercano para interactuar,
        // y el resto queda a mano para cuando se implemente lo de
        // seleccionar antes de interactuar
      })
      .WithDeallocateOnJobCompletion(apparentDistances)
      .Schedule(apparentDistanceHandle);

      return sortHandle;
    }

  }

}