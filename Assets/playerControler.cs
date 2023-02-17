using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//controls both of the players actors at the same time.
public class playerControler : MonoBehaviour
{
    [SerializeField]
    private CharacterController _controler1;
    [SerializeField]
    private CharacterController _controler2;
    private PlayerInput _input;

    private Vector3 _movementInput;
    [SerializeField]
    private float _speed;
    [SerializeField]
    private float _jumpForce;
    // Start is called before the first frame update
    void Start()
    {
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
            if (_controler1.isGrounded)
            {
                _movementInput.y += _jumpForce;
            }
        };
        _input.Player.Enable();
           
    }

    // Update is called once per frame
    void Update()
    {
        _controler1.Move(_movementInput);
        _controler2.Move(_movementInput);
    }
}
