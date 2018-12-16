using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Ranking : MonoBehaviour
{

    private Dictionary<string, int> scores;
    private int score;

    private void Awake()
    {
        scores = new Dictionary<string, int>();
    }

    public void SaveScore(int score)
    {
        this.score = score;
        StartCoroutine(SubmitHighScore());
    }

    private IEnumerator SubmitHighScore()
    {
        string name = "player1";
        string hash_origin = name + "_" + score + "_hash";
        string hash = Md5Sum(hash_origin);
        WWW webRequest = new WWW("http://localhost/insertScore.php?name=" + name + "&score=" + score + "&hash=" + hash);
        yield return webRequest;
        yield return StartCoroutine(RecieveRanking());
    }

    private IEnumerator RecieveRanking()
    {
        WWW webRequest = new WWW("http://localhost/loadRanking.php");
        yield return webRequest;

        string[] stringSeparators = new string[] { "\n" };
        string[] lines = webRequest.text.Split(stringSeparators, System.StringSplitOptions.RemoveEmptyEntries);

        scores.Clear();
        for (int i = 0; i < lines.Length; i++)
        {
            string[] parts = lines[i].Split(',');
            string _name = parts[0];
            int _score = int.Parse(parts[1]);
            scores.Add(_name, _score);
        }
    }

    //暗号学的ハッシュ関数
    public string Md5Sum(string strToEncrypt)
    {
        System.Text.UTF8Encoding ue = new System.Text.UTF8Encoding();
        byte[] bytes = ue.GetBytes(strToEncrypt);

        // encrypt bytes
        System.Security.Cryptography.MD5CryptoServiceProvider md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
        byte[] hashBytes = md5.ComputeHash(bytes);

        // Convert the encrypted bytes back to a string (base 16)
        string hashString = "";

        for (int i = 0; i < hashBytes.Length; i++)
        {
            hashString += System.Convert.ToString(hashBytes[i], 16).PadLeft(2, '0');
        }

        return hashString.PadLeft(32, '0');
    }
}