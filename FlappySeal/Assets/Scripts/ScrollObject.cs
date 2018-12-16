using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollObject : MonoBehaviour {

    public float speed;
    public float startPosition;
    public float endPosition;
    
    void Start() {
        speed = 2.0f;
    }
    void Update()
    {
        // フレームのXポジション移動
        transform.Translate(-1 * speed * Time.deltaTime, 0, 0);

        //スクロール更新とともに加速
        speed += 0.005f;

        // スクロールが目標時点まで到着するかをチェック
        if (transform.position.x <= endPosition) ScrollEnd();
    }

    void ScrollEnd()
    {
        // スクロールと共にポジション移動
        transform.Translate(-1 * (endPosition - startPosition), 0, 0);

        // ゲームオブジェクトにつながっているコンポーネントメッセージ送信
        SendMessage("OnScrollEnd", SendMessageOptions.DontRequireReceiver);
    }
}
