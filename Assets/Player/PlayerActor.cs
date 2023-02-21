using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerActor : MonoBehaviour, IEnergyHolder
{
    private PlayerActor _otherActor;
    [SerializeField]
    private LayerMask objectMask;

    private CharacterController _controller;
    [SerializeField]
    private Transform _cameraPoint;
    [SerializeField]
    private GameObject _camera;
    [SerializeField]
    private bool active;
    [SerializeField] 
    private float maxUpAngle = 80.0f;
    [SerializeField] 
    private float maxDownAngle = 80.0f;
    private float _rotationX = 0.0f;
    private float _rotationY = 0.0f;
    public float maxJumpHeight = 4;
    public float minJumpHeight = 1;
    public float timeToJumpApex = .4f;
    private float gravity;
    private float maxJumpVelocity;
    private float minJumpVelocity;
    Vector3 velocity;

    [SerializeField]
    private int _energyCharges;
    private int _maxEnergyCharges = 3;
    [SerializeField]
    private GameObject _movingEnergyParticle;
    [SerializeField]
    private GameObject[] _enegryChargeVisuals;


    public int enegry { get { return _energyCharges; } set { _energyCharges = Mathf.Clamp(value, 0, _maxEnergyCharges); }  }
    public int maxEnegry { get { return _maxEnergyCharges; } set { } }



    // Start is called before the first frame update
    void Start()
    {
        _controller = GetComponent<CharacterController>();
        _camera.SetActive(active);


        gravity = -(2 * maxJumpHeight) / Mathf.Pow(timeToJumpApex, 2);
        maxJumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;
        minJumpVelocity = Mathf.Sqrt(2 * Mathf.Abs(gravity) * minJumpHeight);
        RedefineEnergyVisuals();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public GameObject GetObjectInfront()
    {
        RaycastHit hit;
        Physics.Raycast(_camera.transform.position, _camera.transform.forward, out hit, Mathf.Infinity);
        if(hit.collider == null) { return null; }
        return hit.collider.gameObject;
    }



    public void ActionFireEnergy()
    {
        if(enegry <= 0) { return; }
        GameObject gameObject = GetObjectInfront();
        if (gameObject == null) { return; }
        if (gameObject == _otherActor.gameObject)
        {
            if (!active) { return; }
        }
        IEnergyHolder energyHolder = gameObject.GetComponent<IEnergyHolder>();
        if(energyHolder == null) { return; }
        if(energyHolder.enegry >= energyHolder.maxEnegry) { return; }
        energyHolder.AddEnergy();
        TakeEnergy();
        SpawnEnergyTransferParticle(gameObject.transform, transform.position);
    }

    public void ActionTakeEnegry()
    {
        if(enegry == maxEnegry) { return; }
        GameObject gameObject = GetObjectInfront();
        if(gameObject == null) { return; }
        if (gameObject == _otherActor.gameObject)
        {
            if (!active) { return; }
        }
        IEnergyHolder energyHolder = gameObject.GetComponent<IEnergyHolder>();
        if (energyHolder == null) { return; }
        if(energyHolder.enegry == 0) { return; }
        energyHolder.TakeEnergy();
        AddEnergy();
        SpawnEnergyTransferParticle(transform, gameObject.transform.position);
    }

    private void SpawnEnergyTransferParticle(Transform parent, Vector3 startingpoint)
    {
        GameObject tempObject = Instantiate(_movingEnergyParticle, parent);
        tempObject.transform.position = startingpoint;
    }

    public void Interact()
    {
        GameObject gameObject = GetObjectInfront();
        if (gameObject == null) { return; }
        if (gameObject == _otherActor.gameObject) {  PlayerInteract(); return; }
    }

    private void PlayerInteract()
    {
        SwapActive();
    }

    public void Rotate(float rotationX = 0.0f,float rotationY = 0.0f)
    {
        _rotationX += rotationX;
        _rotationY += active?rotationY:-rotationY;
        transform.localRotation = Quaternion.Euler(0, _rotationY, 0.0f);
        Quaternion cameraRotation = Quaternion.Euler(_rotationX,0,0);
        Quaternion clampedRotation = ClampRotation(cameraRotation);
        // Apply the clamped rotation to the cameraPoint transform
        _cameraPoint.localRotation = clampedRotation;

        _rotationX = _cameraPoint.eulerAngles.x;
        _rotationY = transform.eulerAngles.y;
    }

    Quaternion ClampRotation(Quaternion rotation)
    {
        // Clamp the rotation to limit how far up/down the camera can look
        Vector3 eulerAngles = rotation.eulerAngles;
        eulerAngles.x = ClampAngle(eulerAngles.x, maxDownAngle, maxUpAngle);
        return Quaternion.Euler(eulerAngles);
    }

    float ClampAngle(float angle, float min, float max)
    {
        float minAngle = 360 - min;
        if (angle >= minAngle -10)
        {
            angle = Mathf.Clamp(angle, minAngle, 360);
        }
        else
        {
           angle = Mathf.Clamp(angle, -90, max);
        }
        return angle;
    }


    public void OnJumpInput()
    {
        if (_controller.isGrounded)
        {
            velocity.y = maxJumpVelocity;
        }
    }
    public void OnJumpInputUp()
    {
        if (velocity.y > minJumpVelocity)
        {
            velocity.y = minJumpVelocity;
        }
    }

    public void Move(Vector3 vector)
    {
        vector.x = active ? vector.x : -vector.x;
        _controller.Move(transform.TransformDirection(vector));

        _controller.Move(velocity * Time.deltaTime);
        if (!_controller.isGrounded)
        {
            velocity.y += gravity * Time.deltaTime;
            velocity.y = Mathf.Clamp(velocity.y, gravity, Mathf.Infinity);
        }
    }

    public void SwapActive()
    {
        active = !active;
        _camera.SetActive(active);
    }


    public void SetOtherActor(PlayerActor actor)
    {
        _otherActor = actor;
    }

    public void AddEnergy()
    {
        enegry++;
        RedefineEnergyVisuals();
    }

    public void TakeEnergy()
    {
        enegry--;
        RedefineEnergyVisuals();
    }

    private void RedefineEnergyVisuals()
    {
        for (int i = 0; i < _enegryChargeVisuals.Length; i++)
        {
            if (enegry >= i+1)
            {
                _enegryChargeVisuals[i].SetActive(true);
            }
            else
            {
                _enegryChargeVisuals[i].SetActive(false);
            }
        }
    }
}
