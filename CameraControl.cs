using System.Linq;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.Debug;
using TMPro;


using Lavender.Movement;
using Lavender.Stream;


public class CameraControl : MonoBehaviour, PlayerControl.IPlayerActions
{    
    //! Constants
    const float TIME_SCALE = 100.0f, AUTOLEVEL_MAXDIST = 10.0f, FIRE_PRIMARY_TIME_THRESHOLD = 0.25f, WEAPON_DISTANCE = 1.0f;
    const float THRUST_MAGNITUDE = 33.0f, BANK_MAGNITUDE = 1.25f, SLIDE_SPEED = 35.0f;
    const float ROTATION_SPEED = 5.0f, TURN_SPEED = 0.35f, LOOKSPEED_CLAMP = 10.0f;
    readonly Vector2 TURN_FACTOR = new Vector2(1.0f, 0.725f);
    //?
    
        
    //! Coordinate system & movement
    Vector3 _localX, _localY, _localZ, _localPos, _autoLevelNormal, _lastAutoLevelNormal;
    Quaternion _localQ;
    Vector2 _turnDelta = Vector2.zero, _slideVector = Vector2.zero;
    float _thrustDirection = 0.0f, _bankDirection = 0.0f;
    //?
    
    
    
    // Fire timekeeping
    float _firePrimaryTimeElapsed = 0.0f;
    bool _primaryEngaged = false, _secondaryEngaged = false;
    
    
    
    //! Unity objects
    PlayerControl _input;
    Rigidbody _rb;
    Collider _colliderMine;
    GameObject _test;
    AutoLevel _autoLevel;
    readonly SegmentStreamer segmentStreamer = SegmentStreamer.Instance;
    //?
    
    
    
    
    // Public fields
    public GameObject TestProjectile;
    public TMP_Text TMPtest;
    
    public List<WorldSegment> CurrentSegments;
     
    
        
    // Start is called before the first frame update
    void Awake()
    {
        // Cache rigidbody and other components.
        _rb = GetComponent<Rigidbody>();
        
        _test = GameObject.Find("MineMerged");
        _colliderMine = _test.GetComponent<Collider>();
        
        _lastAutoLevelNormal = transform.up;
        _localPos = transform.position;
    }
    
    void Start()
    {
        
    }
    
    public void OnEnable()
    {
        _input ??= new PlayerControl();
        _input.Player.SetCallbacks(this);
        _input.Player.Enable();
        
        _autoLevel = new AutoLevel(8, 0.1f, 0.001f);
        
        segmentStreamer.Subscribe(this.gameObject, OnWorldSegmentEvent, 
                                  SubscriptionMode.Selective, new List<string>() {"Bird"});
    }
    public void OnDisable()
    {
        _input.Player.Disable();
        
        segmentStreamer.Unsubscribe(this.gameObject, OnWorldSegmentEvent, true);
    }
    
    public void OnLook(InputAction.CallbackContext obj)
    {
        if (obj.time < 0.0f) return;
        
        _turnDelta = obj.ReadValue<Vector2>();
    }
    public void OnThrust(InputAction.CallbackContext obj)
    {
        if (obj.time < 0.0f) return;
        
        _thrustDirection = obj.ReadValue<float>();
    }
    public void OnSlide(InputAction.CallbackContext obj)
    {
        if (obj.time < 0.0f) return;
        
        _slideVector = obj.ReadValue<Vector2>();
    }
    public void OnRoll(InputAction.CallbackContext obj)
    {
        if (obj.time < 0.0f) return;
        
        _bankDirection = obj.ReadValue<float>(); 
    }
    public void OnActionPrimary(InputAction.CallbackContext obj)
    {
        // Weird workaround to disregard events on/before startup that get stuck
        if (obj.time < 0.0f) return;
        
        switch (obj.phase)
        {
            case InputActionPhase.Started:
                _primaryEngaged = true;
                break;
            case InputActionPhase.Canceled:
                _primaryEngaged = false;
                break;
        }
    }
    public void OnActionSecondary(InputAction.CallbackContext obj)
    {
        if (obj.time < 0.0f) return;
        
        switch (obj.phase)
        {
            case InputActionPhase.Started:
                _secondaryEngaged = true;
                break;
            case InputActionPhase.Canceled:
                _secondaryEngaged = false;
                break;
        } 
    }
    
    
    
    public void OnWorldSegmentEvent(SegmentEvent Event, GameObject Reporter, WorldSegment Segment)
    {
        if (Event == SegmentEvent.TriggerEnter)
            OnWorldSegmentEnter(Segment, Reporter);
        else if (Event == SegmentEvent.TriggerExit)
            OnWorldSegmentExit(Segment, Reporter);
    }
    public void OnWorldSegmentEnter(WorldSegment Segment, GameObject Reporter)
    {
        Log(Reporter.name + " entered " + Segment.name);
    }
    public void OnWorldSegmentExit(WorldSegment Segment, GameObject Reporter)
    {
        Log(Reporter.name + " exited " + Segment.name);
    }

    public void ShootPrimary()
    {        
        Vector3 projPos1 = _localPos + (WEAPON_DISTANCE * _localZ) + (WEAPON_DISTANCE * _localX) - (0.5f * WEAPON_DISTANCE * _localY);
        Vector3 projPos2 = _localPos + (WEAPON_DISTANCE * _localZ) - (WEAPON_DISTANCE * _localX) - (0.5f * WEAPON_DISTANCE * _localY);
        
        var Projectile1 = Instantiate(TestProjectile, projPos1, _localQ);
        var Projectile2 = Instantiate(TestProjectile, projPos2, _localQ);
        
        var rbProjectile1 = Projectile1.GetComponent<Rigidbody>();
        var rbProjectile2 = Projectile2.GetComponent<Rigidbody>();
        
        rbProjectile1.AddRelativeForce(new Vector3(0.0f, 0.0f, .5f), ForceMode.Impulse);
        rbProjectile2.AddRelativeForce(new Vector3(0.0f, 0.0f, .5f), ForceMode.Impulse);
        
        _firePrimaryTimeElapsed = 0.0f;
    }
    
    // Update is called once per frame
    void Update()
    {        
        //! Time updates
        float dt = Time.deltaTime, TimeFactor = dt * TIME_SCALE;
        _firePrimaryTimeElapsed += dt;
        //?
        
        Vector3 lastPos = _localPos;
        
        //! Update local coordinates
        _localX = transform.right;
        _localY = transform.up;
        _localZ = transform.forward;
        _localQ = transform.rotation;
        
        _localPos = transform.position;
        //?
        
        
        //! Compute auto-level normal
        _autoLevel.SetScanCone(lastPos, _localX, _localY, _localZ);
        _autoLevelNormal = _autoLevel.ScanCollider(_colliderMine, AUTOLEVEL_MAXDIST,
                                    out int nh, out bool ch) ?? _lastAutoLevelNormal;
        _autoLevelNormal += _lastAutoLevelNormal * 6.0f;
        _autoLevelNormal.Normalize();
        
        _lastAutoLevelNormal = _autoLevelNormal; // cache normal
        //?
        
        
        //! Movement calculation
        var LookDelta = new Vector2(-_turnDelta.y * TURN_FACTOR.y, _turnDelta.x * TURN_FACTOR.x);
        LookDelta.x = Mathf.Clamp(LookDelta.x, -LOOKSPEED_CLAMP, LOOKSPEED_CLAMP);
        LookDelta.y = Mathf.Clamp(LookDelta.y, -LOOKSPEED_CLAMP, LOOKSPEED_CLAMP);
        LookDelta *= TURN_SPEED * TimeFactor;
        
        var Bank = -BANK_MAGNITUDE * _bankDirection * TimeFactor;
        var Rotation = new Vector3(LookDelta.x, LookDelta.y, Bank);
        
        if (_slideVector.magnitude > float.Epsilon)
        {
            _rb.AddRelativeForce(SLIDE_SPEED * _slideVector * TimeFactor, ForceMode.Acceleration);
        }
        if (THRUST_MAGNITUDE > float.Epsilon)
        {
            var Thrust = THRUST_MAGNITUDE * _thrustDirection * TimeFactor;
            _rb.AddRelativeForce(new Vector3(0.0f, 0.0f, Thrust), ForceMode.Acceleration);
        }        
        if (Rotation.magnitude > float.Epsilon) 
        {
            _rb.AddRelativeTorque(Rotation * ROTATION_SPEED, ForceMode.Acceleration);
        }
        //?
        
        
        string segTmp = "";
        GameObject[] currentSegments = GameObject.FindGameObjectsWithTag("WorldSegment");
        foreach (GameObject cs in currentSegments)
        {
            segTmp += cs.name;
        }
        
        string lsegTmp = "";
        foreach (WorldSegment ls in CurrentSegments)
        {
            lsegTmp += ls.name;
        }
        
        
        //! Calculate auto-level torque
        Vector3 projectedNormal = Vector3.ProjectOnPlane(_autoLevelNormal, _localZ);
        float angle = Vector3.SignedAngle(_localY, projectedNormal, _localZ);
        angle = Mathf.Abs(angle) > 0.3f ? angle : 0.0f;
               
        if (Mathf.Abs(angle) > float.Epsilon)
            _rb.AddRelativeTorque(new Vector3(0.0f, 0.0f, 1.0f)
                                * Mathf.Clamp(angle, -20.0f, 20.0f) / 20.0f
                                * Time.fixedDeltaTime,
                                ForceMode.Impulse);
        
        var vec = _localZ * Mathf.Clamp(angle, -20.0f, 20.0f) / 20.0f;
        TMPtest.text = "AutoLevel Torque: " + vec.ToString("F3")
                     + ", Magnitude: " + vec.magnitude.ToString("F3") + "\r\n"
                     + _localPos.ToString("F3") + "\r\n"
                     + segTmp + "\r\n"
                     + lsegTmp;
        //?
        
                        
        // Laser shoot prototype code
        if (_primaryEngaged && (_firePrimaryTimeElapsed > FIRE_PRIMARY_TIME_THRESHOLD))
        {
            ShootPrimary();
        }
            
    }
    
}
