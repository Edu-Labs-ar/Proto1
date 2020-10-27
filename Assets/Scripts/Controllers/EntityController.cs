using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;

using EduLabs.Input;
using EduLabs.Player;

namespace EduLabs.Controllers
{

  public enum EntityType { Player }


  [AddComponentMenu("EduLabs/Entities/EntityController")]
  public class EntityController : MonoBehaviour
  {

    private static EntityController _instance;

    public static EntityController Instance
    {
      get { return _instance; }
    }


    public GameObject playerPrefab;

    [HideInInspector]
    public Entity player;


    private EntityManager entityManager;
    private BlobAssetStore blobAssetStore;
    private GameObjectConversionSettings conversionSettings;

    void Awake()
    {
      if (_instance == null)
      {
        _instance = this;
        DontDestroyOnLoad(this);
      }
      else
      {
        Destroy(this);
      }
    }

    void Start()
    {
      entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
      blobAssetStore = new BlobAssetStore();

      conversionSettings = GameObjectConversionSettings
              .FromWorld(World.DefaultGameObjectInjectionWorld, blobAssetStore);

      CreatePlayer();
    }

    void OnDisable()
    {
      blobAssetStore.Dispose();
    }


    private void CreatePlayer()
    {
      Entity playerEntityPrefab = GameObjectConversionUtility.ConvertGameObjectHierarchy(playerPrefab, conversionSettings);
      entityManager.SetName(playerEntityPrefab, "Player Prefab");
      entityManager.AddComponent(playerEntityPrefab, typeof(PlayerTag));

      // El sistema de físicas de ECS
      // no tiene una implementación bonita de constraints
      // Este workaround bloquea la rotación en los planos correspondientes
      Rigidbody prefabRb = playerPrefab.GetComponent<Rigidbody>();
      if (prefabRb != null)
      {
        RigidbodyConstraints constraints = prefabRb.constraints;
        PhysicsMass mass = entityManager.GetComponentData<PhysicsMass>(playerEntityPrefab);

        // Cambio un poco los datos para que
        // la entidad tenga masa infinita
        // en los ejes de rotación
        if ((constraints & RigidbodyConstraints.FreezeRotationX) != 0)
          mass.InverseInertia[0] = 0;

        if ((constraints & RigidbodyConstraints.FreezeRotationY) != 0)
          mass.InverseInertia[1] = 0;

        if ((constraints & RigidbodyConstraints.FreezeRotationZ) != 0)
          mass.InverseInertia[2] = 0;

        entityManager.SetComponentData(playerEntityPrefab, mass);
      }


      player = entityManager.Instantiate(playerEntityPrefab);
      entityManager.SetName(player, "Player");
    }
  }
}

