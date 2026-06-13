using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class IncreaseCountrySize : MonoBehaviour
{
    public GameObject territoryPrefab;
    private GameObject _territoryGameObject;
    private GameObject _newTerritoryGameObject;
    private InputAction _attackAction;
    private InputAction _clearAction;
    private Plane _horizontalPlane;
    private Ray _ray;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _territoryGameObject = Instantiate(territoryPrefab, this.transform);
        _attackAction = InputSystem.actions.FindAction("Attack");
        _clearAction = InputSystem.actions.FindAction("Clear");
        _horizontalPlane = new Plane(Vector3.up, new Vector3(0, 1, 0));
        
    }

    // Update is called once per frame
    void Update()
    {
        if(_attackAction.WasPressedThisFrame())
        {
            Vector3 mousePos = Mouse.current.position.ReadValue();
            
            _ray = Camera.main.ScreenPointToRay(mousePos);
            ExpandCountry(mousePos);
        };
        
        if (_clearAction.WasPressedThisFrame())
        {

            foreach (Transform child in this.transform)
            {
                Destroy(child.gameObject);
            }
            
            _territoryGameObject = Instantiate(territoryPrefab, this.transform);
            
        }
    }

    private Transform FindNearestTerritory(Vector3 targetPosition, Transform parent)
    {
        // Transform closestChild = _territoryGameObject.transform;
        //
        // foreach (Transform child in parent)
        // {
        //     if (closestChild)
        //     {
        //         Vector3 childDistance = targetPosition - child.position;
        //         
        //         Vector3 closestChildDistance = targetPosition - closestChild.position;
        //
        //         if (childDistance.sqrMagnitude < closestChildDistance.sqrMagnitude)
        //         {
        //             closestChild = child;
        //         }
        //     }
        //     else
        //     {
        //         closestChild = child;
        //     }
        //     
        // }
        // return closestChild;
        
        float radius = 10;
        
        Collider closestCollider = new Collider();
        
        Collider[] hitColliders = new Collider[10];
        
        int test = Physics.OverlapSphereNonAlloc(targetPosition, radius, hitColliders);

        if (test == 0)
        {
            radius += 10;
            test = Physics.OverlapSphereNonAlloc(targetPosition, radius, hitColliders);
        }
        else
        {
            foreach (Collider hitCollider in hitColliders)
            {
                // TODO Returns nullReferenceException
                Vector3 testVal = hitCollider.ClosestPointOnBounds(targetPosition);
                if (closestCollider)
                {
                    if (testVal.sqrMagnitude < closestCollider.ClosestPointOnBounds(targetPosition).sqrMagnitude)
                    {
                        closestCollider = hitCollider;
                    }
                }
                else
                {
                    closestCollider = hitCollider;
                }
            }
        }

        return closestCollider.transform;
    }
    
    private void ExpandCountry(Vector3 targetPosition)
    {
        
        
        // 3. Intersect the ray with the plane
        if (_horizontalPlane.Raycast(_ray, out float enter))
        {
            // Get the exact Vector3 point where the ray crossed the plane
            targetPosition = _ray.GetPoint(enter);
        }
        
        Transform closestChild = FindNearestTerritory(targetPosition, transform);
        
        if (Physics.Raycast(_ray, out RaycastHit hit))
        {
            if (hit.transform.position == closestChild.position)
            {
                return;
            }
        }
        
        float newXPosition = closestChild.position.x;
        float newZPosition = closestChild.position.z;
        
        float xDistance = targetPosition.x - closestChild.position.x;
        float zDistance = targetPosition.z - closestChild.position.z;
        
        float absXDistance = Mathf.Abs(xDistance);
        float absZDistance = Mathf.Abs(zDistance);

        if (absXDistance > absZDistance)
        {
            if (xDistance > 0)
            {
                newXPosition = closestChild.position.x + 10;
            }
            else
            {
                newXPosition = closestChild.position.x - 10;
            }
        } else
        {
            if (zDistance > 0)
            {
                newZPosition = closestChild.position.z + 10;
            }
            else
            {
                newZPosition = closestChild.position.z - 10;
            }
        }
        
        
        _newTerritoryGameObject = Instantiate(territoryPrefab, transform);
        _newTerritoryGameObject.gameObject.name = "Territory" + transform.childCount;

        _newTerritoryGameObject.transform.position = new Vector3(newXPosition, _newTerritoryGameObject.transform.position.y, newZPosition);
    }
}
