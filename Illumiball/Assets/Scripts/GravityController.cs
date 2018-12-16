using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityController : MonoBehaviour
{

    //重力加速度
    const float Gravity = 9.81f;

    //重力の適用状態
    public float gravityScale = 1.0f;

	void Update ()
    {

        Vector3 vector = new Vector3();

        //エディターと実機での処理を分離
        if(Application.isEditor)
        { 
            vector.x = Input.GetAxis("Horizontal");
            vector.y = Input.GetAxis("Vertical");

            //高さの判定
            if (Input.GetKey("z"))
            {
                vector.y = 1.0f;
            } else
            {
                vector.y = -1.0f;
            }
        }
        else
        {
            //加速度センサーの入力をUnity空間の軸にマッピング
            vector.x = Input.acceleration.x;
            vector.z = Input.acceleration.y;
            vector.y = Input.acceleration.z;
        }


        //Sceneの重力とベクターを一致させる
        Physics.gravity = Gravity * vector.normalized * gravityScale;
    }
}
