using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerActor : MonoBehaviour, IEnergyHolder
{
    public float speed;
    private playerControler _playerControler;
    private PlayerActor _otherActor;
    [SerializeField]
    private GameObject _otherActorVisuals;
    [SerializeField]
    private LayerMask objectMask;

    private CharacterController _controller;
    [SerializeField]
    private Transform _cameraPoint;
    [SerializeField]
    private GameObject _camera;
    [SerializeField]
    private bool _active;
    public bool Active { get { return _active; } }
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
    private Vector3 velocity;
    [SerializeField]
    private GameObject _leftImage;
    [SerializeField]
    private GameObject _rightImage;
    [SerializeField]
    private GameObject _SwapImage;


    [SerializeField]
    private int _energyCharges;
    private int _maxEnergyCharges = 3;
    [SerializeField]
    private GameObject _movingEnergyParticle;
    [SerializeField]
    private GameObject[] _enegryChargeVisuals;



    public bool canJump;
    private bool _canShoot = true;
    public int enegry { get { return _energyCharges; } set { _energyCharges = Mathf.Clamp(value, 0, _maxEnergyCharges); }  }
    public int maxEnegry { get { return _maxEnergyCharges; } set { } }

    public GameObject LastHoverObject;
    private AudioSource _source;

    [SerializeField]
    private AudioClip _fireClip;
    [SerializeField]
    private AudioClip _TakeClip;
    



    // Start is called before the first frame update
    void Start()
    {
        _source = GetComponent<AudioSource>();
        _controller = GetComponent<CharacterController>();
        _camera.SetActive(_active);


        gravity = -(2 * maxJumpHeight) / Mathf.Pow(timeToJumpApex, 2);
        maxJumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;
        minJumpVelocity = Mathf.Sqrt(2 * Mathf.Abs(gravity) * minJumpHeight);
        RedefineEnergyVisuals();
    }

    // Update is called once per frame
    void Update()
    {
        HoverUpdate();
    }

    void HoverUpdate()
    {
        if(!_active) { return; }
        GameObject gameObject = GetObjectInfront();
        if(gameObject != LastHoverObject && LastHoverObject != null) { LastHoverObject.SendMessage("OnHoverEnd", SendMessageOptions.DontRequireReceiver); }
        LastHoverObject = gameObject;
        LastHoverObject.SendMessage("OnHover", enegry, SendMessageOptions.DontRequireReceiver);
    }

    public void ShowOtherSideView(bool b)
    {
        _otherActorVisuals.SetActive(b);
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
        if(!_canShoot) { return; }
        if(enegry <= 0) { return; }
        GameObject gameObject = GetObjectInfront();
        if (gameObject == null) { return; }
        if (gameObject == _otherActor.gameObject)
        {
            if (!_active) { return; }
        }
        IEnergyHolder energyHolder = gameObject.GetComponent<IEnergyHolder>();
        if(energyHolder == null) { return; }
        if(energyHolder.enegry >= energyHolder.maxEnegry) { return; }
        energyHolder.AddEnergy();
        TakeEnergy();
        SpawnEnergyTransferParticle(gameObject.transform, transform.position);
        _source.clip = _fireClip;
        _source.Play();
    }

    public void ActionTakeEnegry()
    {
        if(!_canShoot) { return; }
        if(enegry == maxEnegry) { return; }
        GameObject gameObject = GetObjectInfront();
        if(gameObject == null) { return; }
        if (gameObject == _otherActor.gameObject)
        {
            if (!_active) { return; }
        }
        IEnergyHolder energyHolder = gameObject.GetComponent<IEnergyHolder>();
        if (energyHolder == null) { return; }
        if(energyHolder.enegry == 0) { return; }
        energyHolder.TakeEnergy();
        AddEnergy();
        SpawnEnergyTransferParticle(transform, gameObject.transform.position);
        _source.clip = _TakeClip;
        _source.Play();
    }

    private void SpawnEnergyTransferParticle(Transform parent, Vector3 startingpoint)
    {
        GameObject tempObject = Instantiate(_movingEnergyParticle, parent);
        tempObject.transform.position = startingpoint;
    }

    void OnHover(int charges)
    {
        if (_active) { return; }
        if (charges > 0 && _energyCharges < maxEnegry)
        {
            _leftImage.SetActive(true);
        }
        else
        {
            _leftImage.SetActive(false);
        }

        if (charges < 3 && _energyCharges != 0)
        {
            _rightImage.SetActive(true);
        }
        else
        {
            _rightImage.SetActive(false);
        }


        _SwapImage.SetActive(true);
    }

    void OnHoverEnd()
    {
        _leftImage.SetActive(false);
        _rightImage.SetActive(false);
        _SwapImage.SetActive(false);
    }

    public bool Interact()
    {
        if(!_active) { return false; }
        GameObject gameObject = GetObjectInfront();
        if (gameObject == null) { return false; }
        if (gameObject == _otherActor.gameObject) {  PlayerInteract(); return true; }
        return false;
       // PlayerInteract();
    }

    private void PlayerInteract()
    {
        _playerControler.SwapActive();
    }

    public void Rotate(float rotationX = 0.0f,float rotationY = 0.0f)
    {
        _rotationX += rotationX;
        _rotationY += _active?rotationY:-rotationY;
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
        if (_controller.isGrounded && canJump)
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
        vector.x = _active ? vector.x : -vector.x;
        _controller.Move(transform.TransformDirection(vector) * Time.deltaTime * speed );

        _controller.Move(velocity * Time.deltaTime);
        if (!_controller.isGrounded)
        {
            velocity.y += gravity * Time.deltaTime;
            velocity.y = Mathf.Clamp(velocity.y, gravity, Mathf.Infinity);
        }
    }

    public void SwapActive()
    {
        _active = !_active;
        _camera.SetActive(_active);
    }


    public void SetOtherActor(PlayerActor actor)
    {
        _otherActor = actor;
    }
    public void SetControlerScript(playerControler controler)
    {
        _playerControler = controler;
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


    public void SetCanShoot(bool b)
    {
        _canShoot = b;
    }
}
