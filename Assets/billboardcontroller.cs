using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class billboardcontroller : MonoBehaviour
{
    private Transform body;
    public Camera theCam;
    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        body.LookAt(theCam.transform);
        body.rotation = Quaternion.Euler(0f, 180f+body.rotation.eulerAngles.y, 0f);
    }
}
