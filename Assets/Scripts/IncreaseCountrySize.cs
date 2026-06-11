using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class IncreaseCountrySize : MonoBehaviour
{
    public GameObject territoryGameObject;
    public GameObject newTerritoryGameObject;
    private InputAction _attackAction;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        territoryGameObject = this.transform.GetChild(0).gameObject;
        _attackAction = InputSystem.actions.FindAction("Attack");
        
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
        
        
        
        
        newTerritoryGameObject = Instantiate(territoryGameObject, this.transform);
        newTerritoryGameObject.gameObject.name = "Territory" + this.transform.childCount;
        
        float newXPosition = newTerritoryGameObject.transform.position.x;
        
        
        float newZPosition = newTerritoryGameObject.transform.position.z;
        
        float xDistance = targetPosition.x - newTerritoryGameObject.transform.position.x;
        float zDistance = targetPosition.z - newTerritoryGameObject.transform.position.z;
        
        float absXDistance = Mathf.Abs(xDistance);
        float absZDistance = Mathf.Abs(zDistance);

        if (absXDistance > absZDistance)
        {
            if (xDistance > 0)
            {
                newXPosition = newTerritoryGameObject.transform.position.x + 10;
            }
            else
            {
                newXPosition = newTerritoryGameObject.transform.position.x - 10;
            }
        } else
        {
            if (zDistance > 0)
            {
                newZPosition = newTerritoryGameObject.transform.position.z + 10;
            }
            else
            {
                newZPosition = newTerritoryGameObject.transform.position.z - 10;
            }
        }
        
        

        newTerritoryGameObject.transform.position = new Vector3(newXPosition, newTerritoryGameObject.transform.position.y, newZPosition);
        // newTerritoryGameObject.transform.position = new Vector3(newTerritoryGameObject.transform.position.x + 10, newTerritoryGameObject.transform.position.y, newTerritoryGameObject.transform.position.z);
        territoryGameObject = this.transform.GetChild(this.transform.childCount - 1).gameObject;
    }
}
