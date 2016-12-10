using UnityEngine;
using System.Collections;
using SimpleJSON;
using System.Collections.Generic;
using UnityEngine.UI;
using Global;

public class myApi : MonoBehaviour {

    public static myApi S;

    void Awake() {
        if (S == null) {
            S = this;
            DontDestroyOnLoad(this);
        } else {
            Destroy(this);
        }
    }

    string baseURL = "http://acvg-uno-flask-env.xnuwix2zea.us-west-2.elasticbeanstalk.com";
    string url;

    public IEnumerator makeUser() {
        yield return StartCoroutine(POSTuser());
        yield return StartCoroutine(POSThighScoreChickenRoad(0, true));
        yield return StartCoroutine(POSThighScoreSwipeIt(0));
        yield return StartCoroutine(GETuserId());
    }

    public IEnumerator postScore(int score) {
        yield return StartCoroutine(POSThighScoreSwipeIt(score));
    }

    public IEnumerator postLevel(int level, bool beaten) {
        yield return StartCoroutine(POSThighScoreChickenRoad(level, beaten));
    }

    public IEnumerator leaderboard() {
        yield return StartCoroutine(GEThighScoreSwipeIt());
    }

    public static Dictionary <string, string> swipeItInfo;
    public static Dictionary <string, string> chickenRoadInfo;
    public static List <Dictionary<string, string> > levels;
    public static List <Dictionary<string, string> > highScores;

    IEnumerator GETlevels() {
        url = baseURL + "/levels";
        WWW www = new WWW(url);
        yield return www;
        if (www.error == null) {
            var j = JSON.Parse(www.text);
            print(j.ToString());

            levels = new List<Dictionary<string, string>>();
            for (int i = 0; i < j.Count; i++) {
                levels.Add(new Dictionary<string, string>());
                levels[i]["difficulty"] = j[i]["difficulty"];
                levels[i]["objects"] = j[i]["objects"];
                levels[i]["beat_level"] = "0";
            }

        } else {
            print(www.error);
        }
    }

    IEnumerator GETlevelsId() {
        url = baseURL + "/levels?id=" + facebook.ID;
        WWW www = new WWW(url);
        yield return www;
        if (www.error == null) {
            var j = JSON.Parse(www.text);
            print(j.ToString());

            levels = new List<Dictionary<string, string>>();
            for (int i = 0; i < j.Count; i++) {
                levels.Add(new Dictionary<string, string>());
                levels[i]["difficulty"] = j[i]["difficulty"];
                levels[i]["objects"] = j[i]["objects"];
                levels[i]["beat_level"] = j[i]["objects"];
            }
        } else {
            print(www.error);
        }
    }

    IEnumerator GEThighScoreSwipeIt() {
        url = baseURL + "/high_score/swipe_it";
        WWW www = new WWW(url);
        yield return www;
        if (www.error == null) {
            var j = JSON.Parse(www.text);
            highScores = new List<Dictionary<string, string>>();
            for (int i = 0; i < j.Count; i++) {
                highScores.Add(new Dictionary<string, string>());
                //print(j[i]["user_id"]);
                highScores[i]["user_id"] = j[i]["user_id"];
                highScores[i]["score"] = j[i]["score"];
                highScores[i]["high_score_time"] = j[i]["high_score_time"];
            }
        } else {
            print(www.error);
        }
    }

    IEnumerator POSThighScoreSwipeIt(int score) {
        url = baseURL + "/high_score/swipe_it";
        WWWForm form = new WWWForm();
        form.AddField("id", facebook.ID);
        form.AddField("score", score);
        WWW www = new WWW(url, form);
        yield return www;
        if (www.error == null) {
            print("POSThighScoreSwipeIt Success");
        } else {
            print(www.error);
        }
    }

    IEnumerator POSThighScoreChickenRoad(int level_id, bool beat_level) {
        url = baseURL + "/high_score/chicken_road";
        WWWForm form = new WWWForm();
        form.AddField("id", facebook.ID);
        form.AddField("level_id", level_id);
        form.AddField("beat_level", (beat_level ? 1 : 0));
        WWW www = new WWW(url, form);
        yield return www;
        if (www.error == null) {
            print("POSThighScoreChickenRoad Success");
        } else {
            print(www.error);
        }
    }

    IEnumerator POSTuser() {
        url = baseURL + "/user";
        WWWForm form = new WWWForm();
        form.AddField("id", facebook.ID);
        WWW www = new WWW(url, form);
        yield return www;
        if (www.error == null) {
            print("POSTuser Success");
        } else {
            print(www.error);
        }
    }

    IEnumerator GETuserId() {
        url = baseURL + "/user?id=" + facebook.ID;
        print(facebook.ID);
        WWW www = new WWW(url);
        yield return www;
        if (www.error == null) {
            var j = JSON.Parse(www.text);
            print(j.ToString());
            swipeItInfo = new Dictionary<string, string>();
            chickenRoadInfo = new Dictionary<string, string>();
            swipeItInfo["score"] = j["swipe it"]["score"];
            swipeItInfo["high_score_time"] = j["swipe it"]["high_score_time"];
            chickenRoadInfo["score"] = j["chicken road"]["level_id"];
            chickenRoadInfo["beat_level_time"] = j["chicken road"]["beat_level_time"];
        } else {
            print(www.error);
        }
    }
}
