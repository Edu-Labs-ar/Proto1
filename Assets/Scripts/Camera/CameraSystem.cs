using Unity.Collections;
using Unity.Transforms;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using EduLabs.Player;
using EduLabs.Controllers;

namespace EduLabs.Camera
{
  public class CameraSystem : JobComponentSystem
  {

    public EntityQuery cameraQuery;

    protected override void OnCreate()
    {
      cameraQuery = GetEntityQuery(
          ComponentType.ReadOnly<PlayerTag>(),
          typeof(Rotation));
    }

    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
      NativeArray<Entity> entities = cameraQuery.ToEntityArray(Allocator.TempJob);

      if (entities.Length == 0)
      {
        // Esto seria cuando no hay cambios
        entities.Dispose();
        return default;
      }

      Entity player = entities[0];

      if (InputController.cameraLocked)
      {
        float2 rotSpeed = InputController.state.player.cameraMovement * Time.DeltaTime * InputController.sensitivity;
        Rotation rotation = EntityManager.GetComponentData<Rotation>(player);
        rotation.Value = quaternion.LookRotation(
          math.mul(rotation.Value, new float3(rotSpeed.yx, 1f)),
          new float3(0, 1 ,0));

        EntityManager.SetComponentData(player, rotation);
      }

      entities.Dispose();
      return default;
    }

  }

}
