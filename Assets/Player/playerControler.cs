using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//controls both of the players actors at the same time.
public class playerControler : MonoBehaviour
{
    [SerializeField]
    private PlayerActor _controler1;
    [SerializeField]
    private PlayerActor _controler2;
    private PlayerInput _input;

    private Vector3 _movementInput;
    [SerializeField]
    private float _speed;


    [SerializeField] 
    private float sensitivity = 5.0f;

    private float rotationX = 0.0f;
    private float rotationY = 0.0f;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        _controler1.SetOtherActor(_controler2);
        _controler2.SetOtherActor(_controler1);
        _input = new PlayerInput();
        _input.Player.Move.performed += ctx =>
        {
            Vector2 input = ctx.ReadValue<Vector2>() * _speed;
            _movementInput = new Vector3(input.x, _movementInput.y, input.y) ;
        };
        _input.Player.Move.canceled += ctx =>
        {
            _movementInput = new Vector3(0,_movementInput.y, 0);
        };

        _input.Player.Jump.performed += ctx =>
        {
            _controler1.OnJumpInput();
            _controler2.OnJumpInput();
            
        };
        _input.Player.Jump.canceled += ctx =>
        {
            _controler1.OnJumpInputUp();
            _controler2.OnJumpInputUp();

        };
        _input.Player.Look.performed += ctx =>
        {
            Vector2 lookDelta = ctx.ReadValue<Vector2>();
            rotationX += lookDelta.y * sensitivity;
            rotationY += lookDelta.x * sensitivity;

        };

        _input.Player.Look.canceled += ctx =>
        {
            rotationX = 0;
            rotationY = 0;
        };


        _input.Player.Fire.performed += ctx =>
        {
            _controler1.ActionFireEnergy();
            _controler2.ActionFireEnergy();
        };
        _input.Player.SecondaryFire.performed += ctx =>
        {
            _controler1.ActionTakeEnegry();
            _controler2.ActionTakeEnegry();
        };
        _input.Player.Interact.performed += ctx =>
        {
            _controler1.Interact();
            _controler2.Interact();
        };

        _input.Player.ShowOther.performed += ctx =>
        {
            _controler1.ShowOtherSideView(true);
            _controler2.ShowOtherSideView(true);
        };

        _input.Player.ShowOther.canceled += ctx =>
        {
            _controler1.ShowOtherSideView(false);
            _controler2.ShowOtherSideView(false);
        };

        _input.Player.Enable();
    }

    // Update is called once per frame
    void Update()
    {
        HandelCameraAndRotation();
        _controler1.Move(_movementInput);
        _controler2.Move(_movementInput);
    }

    private void SwapActive()
    {
        _controler1.SwapActive();
        _controler2.SwapActive();
    }

    private void HandelCameraAndRotation()
    {
        _controler1.Rotate(-rotationX, rotationY);
        _controler2.Rotate(-rotationX, rotationY);
    }

    

    
}
