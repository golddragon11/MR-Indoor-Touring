using UnityEngine;  
using System.Collections;  
public class GGG : MonoBehaviour {  
  
  
    private const float lowPassFilterFactor =2f;

    bool draw = false;  
    bool gyinfo;  
    Gyroscope go;  
    
    Rigidbody RB; 
    

    void Start()  
    {  
        gyinfo = SystemInfo.supportsGyroscope;  
        go = Input.gyro;  
        go.enabled = true;  //打開陀螺儀
        RB = gameObject.GetComponent<Rigidbody>(); 
      // Vector3 deviceGravity = go.gravity;//重力加速度
       //Vector3 rotationVelocity = go.rotationRate;//xyz旋轉速度 (弧度/秒)
       //Vector3 rotationVelocity2= go.rotationRateUnbiased;//更精準的旋轉
        go.updateInterval = 0.1f;//0.1秒更新
       //Vector3 acceleration = go.userAcceleration;//沒有重力加速度的加速度



    }  
    void Update()  
    {  

        if (gyinfo)  
        {

            Vector3 a = go.attitude.eulerAngles;  
            a = new Vector3(-a.x, -a.y, a.z); 
            this.transform.eulerAngles = a;  
            this.transform.Rotate(Vector3.right * 90, Space.World);          
            draw = false;  

        }  
        else  
        {  
            draw = true;   
        }  
    }  

    void FixedUpdate()
{
    Vector3 movement = Vector3.zero;

    //Mobile Devices
    movement = new Vector3(-Input.acceleration.y, 0.0f, Input.acceleration.x);

    RB.AddForce(movement * 20);
}
  
    void OnGUI()  
    {  
        if (draw)  
        {  
            GUI.Label(new Rect(100, 100, 100, 30), "启动失败");  
        }  
        GUI.Label(new Rect(100, 100, 500, 400), "Label : x" + go.attitude.x + "    y" + go.attitude.y + "    z" + go.attitude.z + "    G" +go.gravity+"         R" +go.rotationRateUnbiased );


    }  
     
}  