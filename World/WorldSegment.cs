using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Lavender.Stream;

public class WorldSegment : MonoBehaviour
{
    GameObject[] _currentSegments;
    CameraControl _playerObject;
    
    public List<GameObject> LinkedSegments;
    
    
    // Start is called before the first frame update
    void Start()
    {
        _currentSegments = GameObject.FindGameObjectsWithTag("WorldSegment");
        _playerObject = GameObject.Find("PlayerObject").GetComponentInParent<CameraControl>();
        
        foreach (GameObject linkedSeg in LinkedSegments)
        {
            if (_currentSegments.Contains(linkedSeg))
                continue;
            else
            {
                //Instantiate(linkedSeg);
                // load linked segments
            }   
        }
    }

    // Update is called once per frame
    void Update()
    {
        _currentSegments = GameObject.FindGameObjectsWithTag("WorldSegment");
    }
    
    
    void OnCollisionEnter(Collision other) =>
        SegmentStreamer.Broadcast(SegmentEvent.CollisionEnter, other.gameObject, this);
    
    void OnTriggerEnter(Collider other) {
        SegmentStreamer.Broadcast(SegmentEvent.TriggerEnter, other.gameObject, this);
        
        if (!other.name.Equals("PlayerObject")) return;
        
        if (!_playerObject.CurrentSegments.Contains(this))
            _playerObject.CurrentSegments.Add(this);
        
        foreach (GameObject linkedSeg in LinkedSegments)
        {
            if (_currentSegments.Contains(linkedSeg))
                continue;
            else
            {
                //Instantiate(linkedSeg);
                // load linked segments
                linkedSeg.SetActive(true);
                
            }
        }
        
        foreach (GameObject seg in _currentSegments)
        {
            if (seg.Equals(this.gameObject) || LinkedSegments.Contains(seg))
                continue;
            else
            {
                seg.SetActive(false);
            }
        }
        
    }
    
    
    void OnTriggerExit(Collider other) {
        SegmentStreamer.Broadcast(SegmentEvent.TriggerExit, other.gameObject, this);
        
        if (!other.name.Equals("PlayerObject")) return;
        
        if (_playerObject.CurrentSegments.Contains(this))
            _playerObject.CurrentSegments.Remove(this);
        
        foreach (GameObject seg in _currentSegments)
        {
            if (seg.Equals(this.gameObject) || LinkedSegments.Contains(seg))
                continue;
            else
            {
                // seg.SetActive(false);
            }
        }        
    }
    
    /*
    void OnTriggerStay(Collider other) {
        if (!other.name.Equals("PlayerObject")) return;
        
        foreach (GameObject linkedSeg in LinkedSegments)
        {
            if (!linkedSeg.activeSelf)
                linkedSeg.SetActive(true);
        }
    }
    */
    
}
