using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MouseInputProvider : MonoBehaviour
{
    public GameObject BloodRed;

    private bool didClickObject = false;
    private void OnMouseDown()
    {
        Instantiate(BloodRed, transform.position, Quaternion.identity);
        Destroy(gameObject);
        didClickObject = true;
    }

    public bool GetDidClickObject()
    {
        return didClickObject;
    }

    public void SetDidClickObject(bool didClickObject)
    {
        this.didClickObject = didClickObject;
    }
    
    
    
}
