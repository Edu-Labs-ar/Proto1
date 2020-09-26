using Unity.Entities;

namespace EduLabs.Input
{
  public struct AnalogInputData : IComponentData
  {
    public AnalogType direction;

    public bool holdingUp;

    public bool holdingDown;

    public bool holdingLeft;

    public bool holdingRight;

  }

}
