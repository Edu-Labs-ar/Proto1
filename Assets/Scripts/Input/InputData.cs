using System;
using Unity.Mathematics;

namespace EduLabs.Input
{

  [Serializable]
  public struct InputData {

    public AnalogInputData analog;

    public PlayerInputData player;

    public ToolsInputData tools;
  }

  [Serializable]
  public struct AnalogInputData
  {
    public AnalogType direction;

    public bool holdingUp;

    public bool holdingDown;

    public bool holdingLeft;

    public bool holdingRight;

  }


  [Serializable]
  public struct PlayerInputData
  {

    public float2 playerMovement;

    public float2 cameraMovement;
  }


  [Serializable]
  public struct ToolsInputData
  {

    public bool grabMode;

    public bool selectMode;
  }

}
