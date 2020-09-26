using UnityEngine;

namespace EduLabs.Input
{

  [RequireComponent(typeof(MainInputSystem))]
  public class InputManager : MonoBehaviour
  {
    private static InputManager _instance;
    public static InputManager instance => _instance;


    public PlayerInputSettings player;

    public AnalogInputSettings analog;

    public ToolsInputSettings tools;


    private void Awake()
    {
      _instance = this;

      // player.moveStick.Enable();
      // player.moveX.Enable();
      // player.moveY.Enable();
      // player.cameraStick.Enable();
      // player.cameraYaw.Enable();
      // player.cameraPitch.Enable();
      // player.exitAction.Enable();

      // analog.up.Enable();
      // analog.down.Enable();
      // analog.left.Enable();
      // analog.right.Enable();

      // tools.grabTool.Enable();
      // tools.selectTool.Enable();
    }


    private void OnDestroy()
    {
      _instance = null;

      // player.moveStick.Disable();
      // player.moveX.Disable();
      // player.moveY.Disable();
      // player.cameraStick.Disable();
      // player.cameraYaw.Disable();
      // player.cameraPitch.Disable();
      // player.exitAction.Disable();

      // analog.up.Disable();
      // analog.down.Disable();
      // analog.left.Disable();
      // analog.right.Disable();

      // tools.grabTool.Disable();
      // tools.selectTool.Disable();
    }
  }

}

