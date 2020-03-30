using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

    public Vector3 offset;
    public float speed = 10f;
    private Transform target;

    void Start() {
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void LateUpdate() {
        // If there is no target don't follow
        if (!target) return;

        // For now just follow with offset
        Vector3 move = (target.position + offset) - transform.position;
        transform.position += move * Time.deltaTime * speed;

    }
}
