using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {
    //ゲームState
    enum State
    {
        Ready,
        Play,
        GameOver
    }

    State state;
    int score;

    public SealController seal;
    public GameObject blocks;
    public Text scoreLabel;
    public Text stateLabel;
    public Ranking ranking;

    void Start()
    {
        //スタート時にReadyState転換
        Ready();
    }

    void LateUpdate()
    {
        //State監視
        switch (state)
        {
            case State.Ready:
                //タップでゲームスタート
                if (Input.GetButtonDown("Fire1")) GameStart();
                break;
            case State.Play:
                //死亡時はゲームオーバー
                if (seal.IsDead()) GameOver();
                break;
            case State.GameOver:
                //タップでシーンリロード
                //if (Input.GetButtonDown("Fire1"))
                //Reload();
                break;
        }
    }

    void Ready()
    {
        state = State.Ready;

        //各オブジェクトの無効化
        seal.SetSteerActive(false);
        blocks.SetActive(false);

        //スコアラベルアップデート
        scoreLabel.text = "Score : " + 0;

        stateLabel.gameObject.SetActive(true);
        stateLabel.text = "Ready";

        GameObject.Find("BackGround").GetComponent<AudioSource>().Play();
    }

    void GameStart()
    {
        state = State.Play;

        //各オブジェクトの有効化
        seal.SetSteerActive(true);
        blocks.SetActive(true);

        //最初入力時のみゲームコントローラーから渡す
        seal.Flap();

        //スコアラベルアップデート
        stateLabel.gameObject.SetActive(false);
        stateLabel.text = "";

    }

    void GameOver()
    {
        state = State.GameOver;

        //シーン内のすべてのScrollObjectコンポーネント
        ScrollObject[] scrollObjects = GameObject.FindObjectsOfType<ScrollObject>();

        //ScrollObjectのスクロール無効化
        foreach (ScrollObject so in scrollObjects) so.enabled = false;

        //スコアラベルアップデート
        stateLabel.gameObject.SetActive(true);
        stateLabel.text = "GameOver";
        
        GameObject.Find("GameOver").GetComponent<AudioSource>().Play();

        ranking.SaveScore(score);
    }

    void Reload()
    {
        //ロード中のシーンリロード
        //Application.LoadLevel(Application.loadedLevel);  //5.2
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);   //5.3
    }

    public void IncreaseScore()
    {
        score++;
        scoreLabel.text = "Score : " + score;
    }

    public void Update()
    {
        //終了
        if (state == State.GameOver && Input.GetKey(KeyCode.Escape))
        {
            Application.Quit();
        }
        //リロード
        else if (state == State.GameOver && Input.GetButtonDown("Fire1"))
        {
            Reload();
        }
    }
}
