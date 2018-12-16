using UnityEngine;
using System.Collections;

public class Hole : MonoBehaviour
{

    bool fallIn;

    //ゴールのボール色タグ
    public string activeTag;

    public bool IsFallIn()
    {
        return fallIn;
    }

    //ゴール判定
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == activeTag)
        {
            fallIn = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == activeTag)
        {
            fallIn = false;
        }
    }

    void OnTriggerStay(Collider other)
    {
        //Colliderに接続されるオブジェクトのRigidbodyコンポーネント取得
        Rigidbody r = other.gameObject.GetComponent<Rigidbody>();

        //ボールの方向を計算
        Vector3 direction = transform.position - other.gameObject.transform.position;
        direction.Normalize();

        //タグによりボールの移動速度を調節
        if (other.gameObject.tag == activeTag)
        {
            r.velocity *= 0.9f;

            r.AddForce(direction * r.mass * 80.0f);
        }
        //ボールを止める
        else
        {
            r.AddForce(-direction * r.mass * 150.0f);
        }
    }
}
