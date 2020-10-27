using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace EduLabs.Input
{

  [Serializable]
  public struct AnalogInputSettings
  {
    public InputAction up;

    public InputAction down;

    public InputAction left;

    public InputAction right;

  }


  [Serializable]
  public struct PlayerInputSettings
  {
    [Range(.1f, 3f)]
    public float sensitivity;

    public InputAction moveStick;

    public InputAction cameraYaw;

    public InputAction cameraPitch;

    public InputAction exitAction;
  }


  [Serializable]
  public struct ToolsInputSettings
  {

    public InputAction grabTool;

    public InputAction selectTool;
  }


  [CreateAssetMenu(fileName = "InputSettings", menuName = "EduLabs/Settings/Input", order = 1)]
  public class InputSettings : ScriptableObject
  {

    private static InputSettings _instance;
    public static InputSettings Instance
    {
      get { return _instance; }
    }


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

      _instance = this;
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

