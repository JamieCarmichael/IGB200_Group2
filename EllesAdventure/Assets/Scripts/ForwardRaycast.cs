using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForwardRaycast : MonoBehaviour
{
    RaycastHit hit;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(InputManager.Instance.PlayerInput.InGame.Raycast.ReadValue<float>() > 0f)
        {
            Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit,100);
            Debug.Log(hit.transform.parent.name);
        }
    }
}
