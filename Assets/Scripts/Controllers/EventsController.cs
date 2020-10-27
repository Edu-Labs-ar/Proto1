using UnityEngine;
using EduLabs.Input;

namespace EduLabs.Controllers
{
  [AddComponentMenu("EduLabs/EventsController")]
  public class EventsController : MonoBehaviour
  {

    void Start()
    {
      InputSettings input = InputSettings.Instance;
      input.player.exitAction.performed += (ctx) => OnEscape();

      InputController.InitController(input);
    }

    void OnDisable() {
      InputController.DisableController();
    }


    private static void OnEscape()
    {
      UIController.ToggleInGameMenu();
    }
  }
}