using UnityEngine;

public class CanvasPlayController : MonoBehaviour
{
    /* ==================== Fields ==================== */

    [SerializeField] private Joystick _joystick = null;
    [SerializeField] private GameObject[] _buttons = null;

    public static CanvasPlayController Instance
    {
        get;
        private set;
    }



    /* ==================== Public Methods ==================== */

    public void SetControllerActive(bool active)
    {
        gameObject.SetActive(active);
        _joystick.SetJoystickActive(active);
    }


    public void ButtonClick(int index)
    {
        switch (index)
        {
            case Constants.BUTTON_ATTACK:
                HorizontalPlayerControl.Instance.Attack();
                return;

            case Constants.BUTTON_INTERACT:
                HorizontalPlayerControl.Instance.Interact();
                return;

            case Constants.BUTTON_EXTRA:
                HorizontalPlayerControl.Instance.Extra();
                return;
        }
    }


    public void SetInteractBtnActive(bool active)
    {
        _buttons[Constants.BUTTON_INTERACT].SetActive(active);
    }



    /* ==================== Private Methods ==================== */

    private void Awake()
    {
        Instance = this;
        gameObject.SetActive(false);
    }
}