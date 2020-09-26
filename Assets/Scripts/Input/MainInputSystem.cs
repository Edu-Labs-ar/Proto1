using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Entities;
using Unity.Jobs;

namespace EduLabs.Input
{

  [AlwaysSynchronizeSystem]
  [RequiresEntityConversion]
  [RequireComponent(typeof(PlayerInputData), typeof(AnalogInputData), typeof(ToolsInputData))]
  public class MainInputSystem : JobComponentSystem
  {

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
    }

    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
      // InputSystem.Update();

      if (InputManager.instance == null) return default;
      InputManager input = InputManager.instance;
      PlayerInputSettings playerSettings = input.player;
      AnalogInputSettings analogSettings = input.analog;
      ToolsInputSettings toolsSettings = input.tools;
      
      JobHandle playerHandle = Entities.ForEach(
        (ref PlayerInputData data) =>
      {

        // data.playerMovement = playerSettings.moveStick.ReadValue<Vector2>();
        // data.playerMovement.x += playerSettings.moveX.ReadValue<float>();
        // data.playerMovement.y += playerSettings.moveY.ReadValue<float>();

        // data.cameraMovement = playerSettings.cameraStick.ReadValue<Vector2>();
        // data.cameraMovement.x += playerSettings.cameraYaw.ReadValue<float>();
        // data.cameraMovement.y += playerSettings.cameraPitch.ReadValue<float>();

        // data.exitPressed = playerSettings.exitAction.ReadValue<bool>();
      }).Schedule(inputDeps);

      JobHandle analogHandle = Entities.ForEach(
        (ref AnalogInputData data) =>
      {

        bool wasHoldingUp = data.holdingUp;
        bool wasHoldingDown = data.holdingDown;
        bool wasHoldingLeft = data.holdingLeft;
        bool wasHoldingRight = data.holdingRight;

        // data.holdingUp = analogSettings.up.ReadValue<bool>();
        // data.holdingDown = analogSettings.down.ReadValue<bool>();
        // data.holdingLeft = analogSettings.left.ReadValue<bool>();
        // data.holdingRight = analogSettings.right.ReadValue<bool>();

        if (!(data.holdingUp || data.holdingDown || data.holdingLeft || data.holdingRight))
          data.direction = AnalogType.NONE;
        else if (!wasHoldingUp && data.holdingUp)
          data.direction = AnalogType.UP;
        else if (!wasHoldingDown && data.holdingDown)
          data.direction = AnalogType.DOWN;
        else if (!wasHoldingLeft && data.holdingLeft)
          data.direction = AnalogType.LEFT;
        else if (!wasHoldingRight && data.holdingRight)
          data.direction = AnalogType.RIGHT;
      }).Schedule(inputDeps);

      JobHandle toolsHandle = Entities.ForEach(
        (ref ToolsInputData data) =>
      {

        // data.grabMode = toolsSettings.grabTool.ReadValue<bool>();
        // data.selectMode = toolsSettings.selectTool.ReadValue<bool>();
      }).Schedule(inputDeps);

      return JobHandle.CombineDependencies(playerHandle, analogHandle, toolsHandle);
    }
  }

}
