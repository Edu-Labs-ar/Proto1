using UnityEngine;
using Unity.Entities;
using Unity.Transforms;

using EduLabs.Controllers;

namespace EduLabs.Utility
{
  [AddComponentMenu("EduLabs/Entities/EntityFollower")]
  public class EntityFollower : MonoBehaviour
  {

    public delegate Entity EntityDelegate(EntityController ec);

    public enum UpdateMode { Normal, Late }


    public bool followRotation = false;

    public float offsetDistance = 0f;

    public EntityType entityType;

    public UpdateMode updateMode = UpdateMode.Normal;


    private Entity entity;

    private EntityManager entityManager;


    void Start()
    {

      entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;

      switch (entityType)
      {
        case EntityType.Player:
          entity = EntityController.Instance.player;
          break;
      }
    }


    void Update()
    {
      if (updateMode != UpdateMode.Normal) return;
      UpdatePosition();
    }

    void LateUpdate()
    {
      if (updateMode != UpdateMode.Late) return;
      UpdatePosition();
    }

    private void UpdatePosition()
    {
      Translation entityPos = entityManager.GetComponentData<Translation>(entity);

      if (offsetDistance == 0)
      {
        transform.position = EduTools.ToVector3(entityPos.Value);
        if (followRotation) {
          Rotation entityRot = entityManager.GetComponentData<Rotation>(entity);
          transform.rotation = EduTools.ToQuaternion(entityRot.Value.value);
        }
      }
      else if (followRotation)
      {
        Rotation entityRot = entityManager.GetComponentData<Rotation>(entity);
        Quaternion targetRotation = EduTools.ToQuaternion(entityRot.Value.value);
        transform.position = targetRotation * Vector3.back * offsetDistance;
        transform.rotation = targetRotation;
      }
      else
      {
        Vector3 translation = EduTools.ToVector3(entityPos.Value) - transform.position;
        translation *= 1 - offsetDistance / translation.magnitude;

        transform.position += translation;
      }
    }


  }
}