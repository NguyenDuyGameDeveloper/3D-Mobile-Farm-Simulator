using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(PlayerAnimator))]
public class Player : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private MobileJoyStick joyStick;
    private PlayerAnimator playerAnimator;
    private CharacterController characterController;

    [Header("Settings")]
    [SerializeField] private float moveSpeed;

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
        playerAnimator = GetComponent<PlayerAnimator>();
    }
    private void Update()
    {
        ManageMoveMent();
    }
    private void ManageMoveMent()
    {
        Vector3 moveVector = joyStick.GetMoveVector() * moveSpeed * Time.deltaTime / Screen.width;

        moveVector.z = moveVector.y;
        moveVector.y = 0;

        if (moveVector.y != 0)
            moveVector.y = 0;
        
        characterController.Move(moveVector);

        playerAnimator.ManageAnimation(moveVector);
    }
}
