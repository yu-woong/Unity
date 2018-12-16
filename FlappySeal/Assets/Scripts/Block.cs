using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour {
    public float minHeight;
    public float maxHeight;
    public GameObject pivot;

    void Start()
    {
        //スタート時に表示される壁の高さ変更
        ChangeHeight();
    }

    void ChangeHeight()
    {
        //高さのランダム設定
        float height = Random.Range(minHeight, maxHeight);
        pivot.transform.localPosition = new Vector3(0.0f, height, 0.0f);
    }

    //ScrollObjectスクリプトからメッセージを受け高さ変更
    void OnScrollEnd()
    {
        ChangeHeight();
    }
}
