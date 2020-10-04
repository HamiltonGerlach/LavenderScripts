using System;
using System.Threading;
using System.Collections.Concurrent;
using System.Diagnostics.Tracing;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#nullable enable

namespace Lavender.Movement
{
    public class AutoLevel
    {
        readonly int _numRays;
        readonly Ray[] _rays;
        public float BaseAperture { get; set; }
        public float ScanAperture { get; set; }
        
        
        
        
        public AutoLevel(int NumRays, float baseAperture, float scanAperture)
        {
            _numRays = NumRays;
            _rays = Enumerable.Range(0, _numRays + 1).Select(i => new Ray()).ToArray(); // Create array of rays
            
            BaseAperture = baseAperture;
            ScanAperture = scanAperture;
        }
        
        
        public void SetScanCone(Vector3 Pos, Vector3 LocalX, Vector3 LocalY, Vector3 LocalZ)
        {
            foreach (int i in Enumerable.Range(0, _numRays))
            {
                float angle = 2 * Mathf.PI * i / _numRays;
                
                _rays[i].origin = Pos + Mathf.Cos(angle) * BaseAperture * LocalX
                                      + Mathf.Sin(angle) * BaseAperture * LocalZ;
                                      
                _rays[i].direction = -LocalY + Mathf.Cos(angle) * (BaseAperture + ScanAperture) * LocalX
                                             + Mathf.Sin(angle) * (BaseAperture + ScanAperture) * LocalZ;
            }
            
            _rays[_numRays].direction = -LocalY;
            _rays[_numRays].origin = Pos;
        }
        
        
        public Vector3? ScanCollider(Collider ScanObject, float MaxDistance,
                                     out int numHits, out bool centerHit)
        {
            centerHit = false;
            
            var hitNormals = new List<Vector3>(_numRays);
            
            foreach (Ray r in _rays)
            {
                if (ScanObject.Raycast(r, out RaycastHit hitInfo, MaxDistance))
                {
                    hitNormals.Add(hitInfo.normal);
                    
                    if (r.Equals(_rays.Last()))
                        centerHit = true;
                }
            }
            
            numHits = hitNormals.Count;
            
            if (hitNormals.Count > 0)
            {
                Vector3 CumulativeNormal = Vector3.zero;
                
                foreach (Vector3 n in hitNormals)
                {
                    CumulativeNormal += n;
                }
                
                CumulativeNormal.Normalize();
                
                return CumulativeNormal;
            }
            else
            {
                return null;
            }
        }
    }
}