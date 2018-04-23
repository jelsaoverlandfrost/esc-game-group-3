using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorRotator : MonoBehaviour {

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(new Vector3(30, 0, 0) * Time.deltaTime);
    }
}
