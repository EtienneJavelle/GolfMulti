using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ramp : MonoBehaviour
{
    private void OnTriggerEnter(Collider other) {
        if(other.TryGetComponent<Rigidbody>(out Rigidbody rb)){
            rb.velocity *= 2;
        }
    }
}
