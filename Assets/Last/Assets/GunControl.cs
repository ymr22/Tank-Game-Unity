using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunControl : MonoBehaviour
{
    // Start is called before the first frame update
    [Tooltip("If rotationSpeed == 0.5, then it takes 2 seconds to spin 180 degrees")]
    [SerializeField] [Range(0, 10)] float rotationSpeed = 0.5f;
 
    [Tooltip("If isInstant == true, then rotationalSpeed == Infinity")]  
    [SerializeField] bool isInstant = false;
 
    Camera _camera = null;  // cached because Camera.main is slow, so we only call it once.
 
    void Start()
    {
        _camera = Camera.main;
    }
 
    void Update()
    {
        Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
        // Copy the ray's direction
        Vector3 mouseDirection = ray.direction;
        // Constraint it to stay in the X/Z plane
        
        mouseDirection.y += 0.5f;
        
        //Look for the constraint direction
        Quaternion targetRotation = Quaternion.LookRotation(mouseDirection);
 
        if (isInstant)
        {
            transform.rotation = targetRotation;
        }
        else
        {
            Quaternion currentRotation = transform.rotation;
            float angularDifference = Quaternion.Angle(currentRotation, targetRotation);
 
            // will always be positive (or zero)
            if (angularDifference > 0) transform.rotation = Quaternion.Slerp(
                currentRotation,
                targetRotation,
                (rotationSpeed * 180 * Time.deltaTime) / (angularDifference)
            );
            else transform.rotation = targetRotation;
        }
    }
}
