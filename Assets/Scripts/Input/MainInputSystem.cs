using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.InputSystem;
using Unity.Entities;
using Unity.Jobs;

namespace EduLabs.Input
{

  [AlwaysSynchronizeSystem]
  [RequiresEntityConversion]
  public class MainInputSystem : JobComponentSystem
  {

    private InputSettings input;

    protected override void OnCreate()
    {
      Entities.ForEach(
        (ref PlayerInputData playerData) =>
        {
          playerData.cameraMovement = Vector2.zero;
          playerData.playerMovement = Vector2.zero;
          playerData.exitPressed = false;
        }).Run();

      Entities.ForEach((ref AnalogInputData analogData) =>
      {
        analogData.direction = AnalogType.NONE;
        analogData.holdingUp = false;
        analogData.holdingDown = false;
        analogData.holdingLeft = false;
        analogData.holdingRight = false;
      }).Run();

      Entities.ForEach((ref ToolsInputData toolsData) =>
      {
        toolsData.grabMode = false;
        toolsData.selectMode = false;
      }).Run();

      // Cargo la configuración de controles
      Addressables.LoadAssetAsync<InputSettings>("Assets/Settings/InputSettings.asset").Completed +=
        handle => input = handle.Result;
    }

    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
      InputSystem.Update();
      if (input == null) return default;

      // Player Input
      PlayerInputSettings playerSettings = input.player;
      Vector2 playerMovement = playerSettings.moveStick.ReadValue<Vector2>();
      Vector2 cameraMovement = new Vector2(
        playerSettings.cameraYaw.ReadValue<float>(),
        playerSettings.cameraPitch.ReadValue<float>()).normalized;
      bool exitPressed = playerSettings.exitAction.ReadValue<float>() != 0;

      JobHandle playerHandle = Entities.ForEach(
        (ref PlayerInputData playerData) =>
      {
        playerData.playerMovement = playerMovement;
        playerData.cameraMovement = cameraMovement;
        playerData.exitPressed = exitPressed;
      }).Schedule(inputDeps);


      // Analog Input
      AnalogInputSettings analogSettings = input.analog;
      bool analogUp = analogSettings.up.ReadValue<int>() != 0;
      bool analogDown = analogSettings.down.ReadValue<int>() != 0;
      bool analogLeft = analogSettings.left.ReadValue<int>() != 0;
      bool analogRight = analogSettings.right.ReadValue<int>() != 0;

      JobHandle analogHandle = Entities.ForEach(
        (ref AnalogInputData data) =>
      {
        if (!(analogUp || analogDown || analogLeft || analogRight))
          data.direction = AnalogType.NONE;
        else if (!data.holdingUp && analogUp)
          data.direction = AnalogType.UP;
        else if (!data.holdingDown && analogDown)
          data.direction = AnalogType.DOWN;
        else if (!data.holdingLeft && analogLeft)
          data.direction = AnalogType.LEFT;
        else if (!data.holdingRight && analogRight)
          data.direction = AnalogType.RIGHT;
      }).Schedule(inputDeps);


      // Tools Input
      ToolsInputSettings toolsSettings = input.tools;
      bool grabMode = toolsSettings.grabTool.ReadValue<int>() != 0;
      bool selectMode = toolsSettings.selectTool.ReadValue<int>() != 0;

      JobHandle toolsHandle = Entities.ForEach(
        (ref ToolsInputData data) =>
      {
        data.grabMode = grabMode;
        data.selectMode = selectMode;
      }).Schedule(inputDeps);

      return JobHandle.CombineDependencies(playerHandle, analogHandle, toolsHandle);
    }
  }

}
