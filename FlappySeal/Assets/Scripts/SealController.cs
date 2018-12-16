using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SealController : MonoBehaviour {

    Rigidbody2D rb2d;
    Animator animator;
    float angle;
    bool isDead;

    public float maxHeight;
    public float flapVelocity;
    public float relativeVelocityX;
    public GameObject sprite;

    public bool IsDead()
    {
        return isDead;
    }

    void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
        animator = sprite.GetComponent<Animator>();
    }

    void Update()
    {
        //画面の高さを超えない範囲でタップを入力する
        if (Input.GetButtonDown("Fire1") && transform.position.y < maxHeight)
        {
            Flap();
        }

        //角度反映
        ApplyAngle();

        //angleが水平の場合、flapフラグをtrueにする
        animator.SetBool("flap", angle >= 0.0f);
    }

    public void Flap()
    {
        //ゲームオーバーの場合、flap操作なし
        if (isDead) return;

        //重力がない場合、操作なし
        if (rb2d.isKinematic) return;

        GameObject.Find("Jump").GetComponent<AudioSource>().Play();

        //Velocityを利用し上に加速
        rb2d.velocity = new Vector2(0.0f, flapVelocity);
    }

    void ApplyAngle()
    {
        //現在速度、相對速度から進行角度を得る
        float targetAngle;

        //死亡時に下向き
        if (isDead)
        {
            targetAngle = -90.0f;
        }
        else
        {
            targetAngle =
                Mathf.Atan2(rb2d.velocity.y, relativeVelocityX) * Mathf.Rad2Deg;
        }

        //回転アニメーション
        angle = Mathf.Lerp(angle, targetAngle, Time.deltaTime * 10.0f);

        // Rotation反映
        sprite.transform.localRotation = Quaternion.Euler(0.0f, 0.0f, angle);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (isDead) return;

        //衝突効果
        Camera.main.SendMessage("Clash");

        //死亡フラグ
        isDead = true;
    }

    public void SetSteerActive(bool active)
    {
        // RigidbodyのOn/Off
        rb2d.isKinematic = !active;
    }
}
