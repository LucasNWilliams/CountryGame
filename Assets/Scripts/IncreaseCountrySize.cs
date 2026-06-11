using UnityEngine;
using UnityEngine.InputSystem;

public class IncreaseCountrySize : MonoBehaviour
{
    public GameObject childGameObject;
    public GameObject newChildGameObject;
    private InputAction _expandAction;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        childGameObject = this.transform.GetChild(0).gameObject;
        _expandAction = InputSystem.actions.FindAction("Jump");
        
    }

    // Update is called once per frame
    void Update()
    {
        if(_expandAction.WasPressedThisFrame())
        {
            ExpandCountry();
        };
    }

    private void ExpandCountry()
    {
        newChildGameObject = Instantiate(childGameObject, this.transform);
        newChildGameObject.gameObject.name = "Plane" + this.transform.childCount;
        newChildGameObject.transform.position = new Vector3(newChildGameObject.transform.position.x + 10, newChildGameObject.transform.position.y, newChildGameObject.transform.position.z);
        // newChildGameObject.transform.position = this.transform.position.z + 1;
        childGameObject = this.transform.GetChild(this.transform.childCount - 1).gameObject;
    }
}
