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
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _territoryGameObject = Instantiate(territoryPrefab, this.transform);
        _attackAction = InputSystem.actions.FindAction("Attack");
        _clearAction = InputSystem.actions.FindAction("Clear");
        
    }

    // Update is called once per frame
    void Update()
    {
        if(_attackAction.WasPressedThisFrame())
        {
            Vector3 mousePos = Mouse.current.position.ReadValue();
            // mousePos.z = 10;
            // Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(mousePos);
            // Vector3 mouseWorldPos = Mouse.current.position.ReadValue();
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

    private void ExpandCountry(Vector3 targetPosition)
    {


        Ray ray = Camera.main.ScreenPointToRay(targetPosition);
        
        Plane horizontalPlane = new Plane(Vector3.up, new Vector3(0, 1, 0));

        // 3. Intersect the ray with the plane
        if (horizontalPlane.Raycast(ray, out float enter))
        {
            // Get the exact Vector3 point where the ray crossed the plane
            // return ray.GetPoint(enter);
            targetPosition = ray.GetPoint(enter);
        }

        Transform closestChild = _territoryGameObject.transform;
        
        foreach (Transform child in this.transform)
        {
            if (closestChild)
            {
                float childX = targetPosition.x - child.position.x;
                float childZ = targetPosition.z - child.position.z;
                Vector3 childDistance = targetPosition - child.position;
                
                Vector3 closestChildDistance = targetPosition - closestChild.position;

                if (childDistance.sqrMagnitude < closestChildDistance.sqrMagnitude)
                {
                    closestChild = child;
                }
            }
            else
            {
                closestChild = child;
            }
            
            
        }
        
        
        _newTerritoryGameObject = Instantiate(closestChild.gameObject, this.transform);
        _newTerritoryGameObject.gameObject.name = "Territory" + this.transform.childCount;
        
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
        
        

        _newTerritoryGameObject.transform.position = new Vector3(newXPosition, _newTerritoryGameObject.transform.position.y, newZPosition);
        // newTerritoryGameObject.transform.position = new Vector3(newTerritoryGameObject.transform.position.x + 10, newTerritoryGameObject.transform.position.y, newTerritoryGameObject.transform.position.z);
        // territoryGameObject = this.transform.GetChild(this.transform.childCount - 1).gameObject;
    }
}
