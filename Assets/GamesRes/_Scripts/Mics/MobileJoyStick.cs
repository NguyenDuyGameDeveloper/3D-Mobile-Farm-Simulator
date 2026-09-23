using System;
using UnityEngine;

public class MobileJoyStick : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private RectTransform joyStickOutline;
    [SerializeField] private RectTransform joyStickKnob;

    [Header("Settings")]
    [SerializeField] private float moveFactor;
    private Vector3 move;
    private Vector3 clickedPosition;
    private bool canControl;

    private void Start()
    {
        Hide();
    }
    private void Update()
    {
        if (canControl)
            ControlJoyStick();
    }
    private void ControlJoyStick()
    {
        Vector3 currentPosition = Input.mousePosition;
        Vector3 direction = currentPosition - clickedPosition;

        float moveMagnitude = direction.magnitude * moveFactor / Screen.width;
        moveMagnitude = Mathf.Min(moveMagnitude, joyStickOutline.rect.width / 2);

        move = direction.normalized * moveMagnitude;

        Vector3 targetPosition = clickedPosition + move;

        joyStickKnob.position = targetPosition;

        if (Input.GetMouseButtonUp(0))
            Hide();
    }
    public void ClickedOnJoyStickZoneCallBack()
    {
        clickedPosition = Input.mousePosition;
        joyStickOutline.position = clickedPosition;

        Show();
    }
    private void Show()
    {
        joyStickOutline.gameObject.SetActive(true);
        canControl = true;
    }
    private void Hide()
    {
        joyStickOutline.gameObject.SetActive(false);
        canControl = false;

        move = Vector3.zero;
    }
    public Vector3 GetMoveVector() => move;
}
