using UnityEngine;
using System.Collections;
using SimpleJSON;
using Facebook.Unity;

public class myApi : MonoBehaviour {

    string url = "http://acvg-uno-flask-env.xnuwix2zea.us-west-2.elasticbeanstalk.com";

    IEnumerator GETlevels() {
        url += "/levels";
        WWW www = new WWW(url);
        yield return www;
        if (www.error == null) {
            var j = JSON.Parse(www.text);
            print(j.ToString());

            //Example Output
            /*{
               "<level_id>":{
                  "difficulty":"easy",
                  "objects":"chicken-goose-chicken"
               }
               "<level_id>":{
                  "difficulty":"medium",
                  "objects":"chicken-goose-chicken-goose"
               }
               "<level_id>":{
                  "difficulty":"hard",
                  "objects":"chicken-goose-chicken-goose-car"
               }
            }*/

        } else {
            print(www.error);
        }
    }

    IEnumerator GETlevelsId() {
        url += "/levels?id=" + AccessToken.CurrentAccessToken.UserId;
        WWW www = new WWW(url);
        yield return www;
        if (www.error == null) {
            var j = JSON.Parse(www.text);
            print(j.ToString());

            // Example Output
            /*{
                "<level_id>":{
                    "difficulty":"easy",
                    "objects":"...",
                    "beat_level":true
                }
                "<level_id>":{
                    "difficulty":"medium",
                    "objects":"chicken-goose-chicken-goose"
      	            "beat_level":false
                }
                "<level_id>":{
                    "difficulty":"hard",
                    "objects":"chicken-goose-chicken-goose-car"
                    "beat_level":false
                }
              }*/
        } else {
            print(www.error);
        }
    }

    IEnumerator GEThighScoreSwipeIt() {
        url += "/high_score/swipe_it";
        WWW www = new WWW(url);
        yield return www;
        if (www.error == null) {
            var j = JSON.Parse(www.text);
            print(j.ToString());

            // Example Output
            /*
            {
               "<user_id>":{
                  "score":104,
                  "high_score_time":"2016-11-09 09:59:40"
               }
               "<user_id>":{
                  "score":103,
                  "high_score_time":"2016-11-09 09:59:40"
               }
               "<user_id>":{
                  "score":101,
                  "high_score_time":"2016-11-09 09:59:39"
               }
               "<user_id>":{
                  "score":101,
                  "high_score_time":"2016-11-09 09:59:40"
               }
               "<user_id>":{
                  "score":100,
                  "high_score_time":"2016-11-09 09:59:40"
               }
            }
            */
        } else {
            print(www.error);
        }
    }

    IEnumerator POSThighScoreSwipeIt(int score) {
        url += "/high_score/swipe_it";
        WWWForm form = new WWWForm();
        form.AddField("id", AccessToken.CurrentAccessToken.UserId);
        form.AddField("score", score);
        WWW www = new WWW(url, form);
        yield return www;
        if (www.error == null) {
            var j = JSON.Parse(www.text);
            print(j.ToString());
        } else {
            print(www.error);
        }
    }

    IEnumerator POSThighScoreChickenRoad(int level_id, bool beat_level) {
        url += "/high_score/chicken_road";
        WWWForm form = new WWWForm();
        form.AddField("id", AccessToken.CurrentAccessToken.UserId);
        form.AddField("level_id", level_id);
        form.AddField("beat_level", (beat_level ? 1 : 0));
        WWW www = new WWW(url, form);
        yield return www;
        if (www.error == null) {
            var j = JSON.Parse(www.text);
            print(j.ToString());
        } else {
            print(www.error);
        }
    }

    IEnumerator POSTuser() {
        url += "/user";
        WWWForm form = new WWWForm();
        form.AddField("id", AccessToken.CurrentAccessToken.UserId);
        WWW www = new WWW(url, form);
        yield return www;
        if (www.error == null) {
            var j = JSON.Parse(www.text);
            print(j.ToString());
        } else {
            print(www.error);
        }
    }

    IEnumerator GETuserId() {
        url += "/user?=" + AccessToken.CurrentAccessToken.UserId;
        WWW www = new WWW(url);
        yield return www;
        if (www.error == null) {
            var j = JSON.Parse(www.text);
            print(j.ToString());

            // Example Output
            /*
            {
               "swipe it":{
                  "score":102,
                  "high_score_time": "2016-11-12 12:13:10"
               },
               "chicken road":{
                  "level_id":1,
                  "beat_level_time": "2016-11-12 12:13:10"
               }
            }
            */
        } else {
            print(www.error);
        }
    }
}
