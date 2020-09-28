using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Physics;

using EduLabs.Input;

namespace EduLabs.Player
{
  public class PlayerMotorSystem : JobComponentSystem
  {

    private EntityQuery movementQuery;

    // El sistema de físicas de ECS
    // no tiene una implementación bonita de constraints
    // Este workaround bloquea la rotación en el plano xz
    public struct LockRotationJob : IJobChunk
    {

      public ArchetypeChunkComponentType<PhysicsMass> massAccessor;

      public void Execute(ArchetypeChunk chunk,
        int chunkIndex,
        int firstEntityIndex)
      {
        NativeArray<PhysicsMass> masses
          = chunk.GetNativeArray<PhysicsMass>(massAccessor);

        for (int idx = 0; idx < masses.Length; ++idx)
        {
          PhysicsMass mass = masses[idx];

          // Cambio un poco los datos para que
          // la entidad tenga masa infinita
          // en los ejes de rotación xz
          mass.InverseInertia[0] = mass.InverseInertia[2] = 0;

          masses[idx] = mass;
        }
      }
    }


    public struct MovementJob : IJobChunk
    {

      public ArchetypeChunkComponentType<PhysicsVelocity> velocityAccessor;

      [ReadOnly]
      public ArchetypeChunkComponentType<PlayerInputData> inputAccessor;

      public void Execute(ArchetypeChunk chunk,
        int chunkIndex,
        int firstEntityIndex)
      {
        NativeArray<PlayerInputData> inputs
          = chunk.GetNativeArray<PlayerInputData>(inputAccessor);
        NativeArray<PhysicsVelocity> velocities
          = chunk.GetNativeArray<PhysicsVelocity>(velocityAccessor);

        for (int idx = 0; idx < velocities.Length; ++idx)
        {
          PhysicsVelocity velocity = velocities[idx];
          velocity.Linear.xz = inputs[idx].playerMovement;

          velocities[idx] = velocity;
        }
      }
    }

    protected override void OnCreate()
    {
      // Busco las entidades que hay que mover
      EntityQueryDesc movableDescriptor = new EntityQueryDesc
      {
        All = new ComponentType[] {
          typeof(PhysicsVelocity),
          ComponentType.ReadOnly<PlayerInputData>()
        }
      };

      movementQuery = GetEntityQuery(movableDescriptor);
    }


    protected override void OnStartRunning()
    {
      NativeArray<Entity> asd = movementQuery.ToEntityArray(Allocator.TempJob);

      // Bloqueo la rotación en el plano xz
      new LockRotationJob
      {
        massAccessor = GetArchetypeChunkComponentType<PhysicsMass>(false)
      }.Run(movementQuery);

      asd.Dispose();
    }

    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
      JobHandle movementHandle = new MovementJob
      {
        velocityAccessor = GetArchetypeChunkComponentType<PhysicsVelocity>(false),
        inputAccessor = GetArchetypeChunkComponentType<PlayerInputData>(true)
      }.Schedule(movementQuery, inputDeps);

      return movementHandle;
    }
  }
}