using UnityEngine;
using Unity.Mathematics;

using EduLabs.Utility;

namespace EduLabs.Controllers
{
  public class InputController
  {

    private static bool _cameraLocked;
    public static bool cameraLocked { get { return _cameraLocked; } }

    private static float _sensitivity;
    public static float sensitivity { get { return _sensitivity; } }

    private static Input.InputData _state = new Input.InputData
    {
      player = new Input.PlayerInputData
      {
        playerMovement = float2.zero,
        cameraMovement = float2.zero
      },
      analog = new Input.AnalogInputData
      {
        direction = Input.AnalogType.NONE,
        holdingUp = false,
        holdingDown = false,
        holdingLeft = false,
        holdingRight = false
      },
      tools = new Input.ToolsInputData
      {
        grabMode = false,
        selectMode = false
      }
    };
    public static Input.InputData state { get { return _state; } }


    // Eventos Principales
    public static void InitController(Input.InputSettings input)
    {
      _sensitivity = input.player.sensitivity;
      BindControls(input);
      LockCamera();
    }

    public static void DisableController()
    {
      UnlockCamera();
    }

    private static void BindControls(Input.InputSettings input)
    {
      input.player.moveStick.performed +=
        (ctx) => _state.player.playerMovement
          = EduTools.ToFloat2(ctx.ReadValue<Vector2>());
      input.player.moveStick.canceled +=
        (ctx) => _state.player.playerMovement = float2.zero;

      input.player.cameraPitch.performed +=
        (ctx) => _state.player.cameraMovement.x = ctx.ReadValue<float>();
      input.player.cameraPitch.canceled +=
        (ctx) => _state.player.cameraMovement.x = 0;

      input.player.cameraYaw.performed +=
        (ctx) => _state.player.cameraMovement.y = ctx.ReadValue<float>();
      input.player.cameraYaw.canceled +=
        (ctx) => _state.player.cameraMovement.y = 0;

      input.player.exitAction.performed += (ctx) => ToggleCameraLock();
    }


    // Funciones
    public static void LockCamera()
    {
      CameraLock(true);
    }

    public static void UnlockCamera()
    {
      CameraLock(false);
    }

    public static void ToggleCameraLock()
    {
      CameraLock(!_cameraLocked);
    }


    private static void CameraLock(bool doLock)
    {
      Cursor.lockState = doLock ? CursorLockMode.Locked : CursorLockMode.None;
      Cursor.visible = !doLock;
      _cameraLocked = doLock;
    }
  }
}