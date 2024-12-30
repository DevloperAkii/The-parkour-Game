using UnityEngine;

public class ParkourGameManager : MonoBehaviour
{

    [SerializeField]
    private Trigger finishTrigger;
    [SerializeField]
    private CameraMovement cameraMovement;

    [SerializeField]
    private GameObject WiningBackGround;

    private void Start()
    {
        finishTrigger.OnEnter += FinishTrigger_OnEnter;

        WiningBackGround.SetActive(false);
    }

    private void FinishTrigger_OnEnter(object sender, Trigger.OnEnterEventArgs e)
    {
        if (e.HitGameObject == PlayerMovement.instance.gameObject)
        {
            WiningBackGround.SetActive(true);
            cameraMovement.SetCursourMode(CursorLockMode.None, true);
            cameraMovement.enabled = false;
        }
    }
}
