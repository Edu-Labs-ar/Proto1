using UnityEngine;
using Unity.Entities;

namespace EduLabs.Input
{
  [GenerateAuthoringComponent]
  public struct PlayerInputData : IComponentData
  {

    public Vector2 playerMovement;

    public Vector2 cameraMovement;

    public bool exitPressed;
  }

}
