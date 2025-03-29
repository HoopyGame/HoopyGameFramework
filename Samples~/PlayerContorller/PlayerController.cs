/*
 | | | | ___   ___  _ __  _   _ / ___| __ _ _ __ ___   ___ 
 | |_| |/ _ \ / _ \| '_ \| | | | |  _ / _` | '_ ` _ \ / _ \
 |  _  | (_) | (_) | |_) | |_| | |_| | (_| | | | | | |  __/
 |_| |_|\___/ \___/| .__/ \__, |\____|\__,_|_| |_| |_|\___|
                   |_|    |___/                            
┌──────────────────────────────────────────────┐
│　Copyright(C) 2025 by HoopyGameStudio
│　描   述*：绝简单的第一人称控制器
│　创 建 人*：Hoopy
│　创建时间：2025-01-01 00:00:00
└──────────────────────────────────────────────┘
┌──────────────────────────────────────────────┐
│　修 改 人：
│　修改描述：
└──────────────────────────────────────────────┘
*/
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private float movementSpeed = 7f;
    [SerializeField] private float mouseSensitivity = 2f;
    [SerializeField] private float smoothRotation = 5f;


    private CharacterController _playerCharacterController;
    private Transform _viewCamaer;


    private Vector3 _velocity;
    private float _yRotation;
    private float _xRotation;

    public bool _canMove;
    public bool _canRotate;
    public bool _canJump;
    private void Awake()
    {
        _playerCharacterController = GetComponent<CharacterController>();
        _viewCamaer = transform.FindTransFromChild("Camera");

        _xRotation = transform.rotation.eulerAngles.y;
    }
    private void Update()
    {
        if (_canMove) Move();
        if (_canRotate) Rotate();
    }
    private void Move()
    {
        // 处理h和v
        float horizontalMovement = Input.GetAxis("Horizontal") * movementSpeed;
        float verticalMovement = Input.GetAxis("Vertical") * movementSpeed;
        //Vector3 dir = new Vector3(horizontalMovement, 0, verticalMovement);
        //dir = transform.TransformDirection(dir);
        Vector3 _dir = transform.forward * verticalMovement + transform.right * horizontalMovement + _velocity;
        if (!_playerCharacterController.isGrounded)
        {
            _velocity.y += Physics.gravity.y * Time.deltaTime;
        }
        else
        {
            _velocity.y = -0.2f;
        }
        _playerCharacterController.Move(_dir * Time.deltaTime);

    }
    private void Rotate()
    {
        if (!_canRotate) return;
        if (!Extension.IsClickUIPointer())
        {
            if (Input.GetMouseButton(1))
            {
                if (Input.GetMouseButtonDown(1))
                {
                    Cursor.lockState = CursorLockMode.Locked;
                    Cursor.visible = false;
                }

                float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;
                // 处理水平旋转（鼠标左右移动）
                float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
                _yRotation -= mouseY;
                _xRotation += mouseX;
            }
            else if (Input.GetMouseButtonUp(1))
            {
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
            }
        }
        float tmpSmoothRotation = smoothRotation * Time.deltaTime;
        //旋转X
        Quaternion Rotations = Quaternion.Euler(0, _xRotation, 0);
        transform.rotation = Quaternion.Lerp(transform.rotation, Rotations, tmpSmoothRotation);
        //旋转Y
        _yRotation = Mathf.Clamp(_yRotation, -60, 80);
        Quaternion targetRotation = Quaternion.Euler(_yRotation, 0f, 0f);
        _viewCamaer.localRotation = Quaternion.Lerp(_viewCamaer.localRotation, targetRotation, tmpSmoothRotation);
    }

    public void MoveAndRotateController(bool canMove, bool canRotate)
    {
        _canMove = canMove;
        _canRotate = canRotate;
    }


    public void ChangePosition(Vector3 pos)
    {
        _playerCharacterController.enabled = false;
        transform.position = pos;
        _xRotation = 0;
        _playerCharacterController.enabled = true;
    }
}
