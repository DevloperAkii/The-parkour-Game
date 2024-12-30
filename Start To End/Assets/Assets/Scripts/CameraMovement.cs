using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    //Camera Behaviour Controlling variables
    [Header("Camera Stats")]
    [SerializeField]
    private float senstivity = 40.0f;
    [SerializeField]
    private float minimumYClamp = -85, maximumYClamp = 85;
    [SerializeField]
    private bool invertX = false,invertY = false;

    //Creating Two Floats To Feeding Axis Inputs
    private float xAxisInput,yAxisInput;

    private void Awake()
    {
        SetCursourMode(CursorLockMode.Locked,false);
    }
    //Setiing Up Inputs
    private void SettingInputs()
    {
        //Feeding Axis Inputs In Desire Variables 
        xAxisInput += Input.GetAxisRaw("Mouse X") * senstivity * Time.deltaTime;
        yAxisInput -= Input.GetAxisRaw("Mouse Y") * senstivity * Time.deltaTime;

        //clamping Y Axis Value To Pervent Uncertain Behaviours
        yAxisInput = Mathf.Clamp(yAxisInput, minimumYClamp, maximumYClamp);
    }
    private void Rotation()
    {
        if(invertX)
            xAxisInput = -xAxisInput;
        if (invertY)
            yAxisInput = -yAxisInput;
        //Setting Up Camera Rotaion According To Desire Axis
        Quaternion cameraRotation = Quaternion.Euler(yAxisInput, xAxisInput, 0);
        transform.rotation = cameraRotation;
    }
    private void Update()
    {
        SettingInputs();
        Rotation();
    }

    public void SetCursourMode(CursorLockMode cursorMode , bool visible)
    {
        Cursor.lockState = cursorMode;
        Cursor.visible = visible;
    }
}
