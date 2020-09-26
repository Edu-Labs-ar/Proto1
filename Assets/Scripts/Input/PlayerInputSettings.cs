using System;
using UnityEngine.InputSystem;

namespace EduLabs.Input
{

  [Serializable]
  public struct PlayerInputSettings
  {
    public InputAction moveStick;

    public InputAction cameraYaw;

    public InputAction cameraPitch;

    public InputAction exitAction;
  }

}