using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Physics;

using EduLabs.Controllers;

namespace EduLabs.Player
{
  [AlwaysSynchronizeSystem]
  public class PlayerMotorSystem : JobComponentSystem
  {

    private EntityQuery movementQuery;


    protected override void OnCreate()
    {
      // Espero a que el EntityController haya creado las entidades
    }


    protected override void OnStartRunning()
    {
      // Busco al jugador
      movementQuery = GetEntityQuery(
          ComponentType.ReadOnly<PlayerTag>(),
          typeof(PhysicsVelocity));
    }

    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
      // Como hay un único jugador y la accion es sencilla,
      // la dejo en el hilo principal
      NativeArray<Entity> entities
          = movementQuery.ToEntityArray(Allocator.TempJob);

      if (entities.Length == 0)
      {
        // Esto sería cuando no hubo cambios
        entities.Dispose();
        return default;
      }

      Entity player = entities[0];

      PhysicsVelocity velocity = EntityManager.GetComponentData<PhysicsVelocity>(player);

      velocity.Linear.xz = InputController.state.player.playerMovement;

      EntityManager.SetComponentData(player, velocity);

      entities.Dispose();
      return default;
    }
  }
}