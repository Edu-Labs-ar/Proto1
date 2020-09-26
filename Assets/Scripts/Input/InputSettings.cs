using UnityEngine;

namespace EduLabs.Input
{

  [CreateAssetMenu(fileName = "InputSettings", menuName="EduLabs/Settings/Input", order = 1)]
  public class InputSettings : ScriptableObject
  {

    public PlayerInputSettings player;

    public AnalogInputSettings analog;

    public ToolsInputSettings tools;


    private void OnEnable()
    {
      player.moveStick.Enable();
      player.cameraYaw.Enable();
      player.cameraPitch.Enable();
      player.exitAction.Enable();

      analog.up.Enable();
      analog.down.Enable();
      analog.left.Enable();
      analog.right.Enable();

      tools.grabTool.Enable();
      tools.selectTool.Enable();
    }


    private void OnDisable()
    {
      player.moveStick.Disable();
      player.cameraYaw.Disable();
      player.cameraPitch.Disable();
      player.exitAction.Disable();

      analog.up.Disable();
      analog.down.Disable();
      analog.left.Disable();
      analog.right.Disable();

      tools.grabTool.Disable();
      tools.selectTool.Disable();
    }
  }

}

