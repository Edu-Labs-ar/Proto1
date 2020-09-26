using Unity.Entities;

namespace EduLabs.Input
{
  [GenerateAuthoringComponent]
  public struct ToolsInputData : IComponentData
  {

    public bool grabMode;

    public bool selectMode;
  }

}
