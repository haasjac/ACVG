using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Facebook.Unity;
using System.IO;

namespace Global {

    public static class accessibility {
        static bool _accessible;

        public static void setAccessibility(bool b) {
            _accessible = b;
        }

        public static bool getAccessibility() {
            return _accessible;
        }
    }

    public static class facebook {
        public static string ID;
        public static string name;
    }

    public static class swipeIt {
        public static int currentScore;
        public enum gameMode { single, multi, tutorial };
        public static gameMode mode = gameMode.single;


        public static int singleHighScore;
        public static int multiHighScore;
        public static List <KeyValuePair<string, int>> leaderboardScores;
        public static Dictionary< string, string > leaderboardNames;
        public static Dictionary<string, Texture2D> leaderboardPics;

        public static int getHighScore() {
            if (mode == gameMode.single || mode == gameMode.tutorial) {
                return singleHighScore;
            } else if (mode == gameMode.multi) {
                return multiHighScore;
            } else {
                return 0;
            }
        }

        public static bool checkHighScore() {
            if ((mode == gameMode.single || mode == gameMode.tutorial) && currentScore > singleHighScore) {
                singleHighScore = currentScore;
                functions.save();
                if (FB.IsLoggedIn) {
                    myApi.S.StartCoroutine(myApi.S.postScore(singleHighScore));
                }
                return true;
            } else if (mode == gameMode.multi && currentScore > multiHighScore) {
                multiHighScore = currentScore;
                functions.save();
                return true;
            } else {
                return false;
            }
        }

        static void getName(string id) {
            FB.API("/" + id + "?fields=id,first_name", HttpMethod.GET, callBack);
        }

        static void you(IGraphResult result) {
            MonoBehaviour.print(result.Texture.name);
            
        }

        static void callBack(IResult result) {
            if (result.Error == null) {
                leaderboardNames[result.ResultDictionary["id"].ToString()] = result.ResultDictionary["first_name"].ToString();                
            } else {
                Debug.Log(result.Error);
            }
        }

        public static IEnumerator updateLeaderboard() {
            yield return myApi.S.StartCoroutine(myApi.S.leaderboard());
            leaderboardScores = new List<KeyValuePair<string, int>>();
            leaderboardNames = new Dictionary<string, string>();
            leaderboardPics = new Dictionary<string, Texture2D>();
            for (int i = 0; i < myApi.highScores.Count; i++) {
                int x = 0;
                if (int.TryParse(myApi.highScores[i]["score"], out x)) {
                    leaderboardScores.Add(new KeyValuePair<string, int>(myApi.highScores[i]["user_id"], x));
                } else {
                    leaderboardScores.Add(new KeyValuePair<string, int>(myApi.highScores[i]["user_id"], -1));
                }
                getName(myApi.highScores[i]["user_id"]);
            }
            //sort leaderboard
            leaderboardScores.Sort((z, y) => y.Value.CompareTo(z.Value));
        }
    }

    static class chickenRoad {
        public static int highestLevelBeaten;
        public static bool easyTutorialPlayed;
        public static bool mediumTutorialPlayed;
        public static bool hardTutorialPlayed;


        public static bool hacked;
        public static string tutorials;
        public static string difficulty;
        public static string levelTitle;
        public static string levelObstacles;
        public static bool crHowToPlay;
        public static int accuracy;
        public static int score;
    }

    public static class functions {

        public static void save() {
            PlayerPrefs.SetInt("accessibility", (accessibility.getAccessibility() ? 1 : 0));
            PlayerPrefs.SetInt("singleHighScore", swipeIt.singleHighScore);
            PlayerPrefs.SetInt("multiHighScore", swipeIt.multiHighScore);
            PlayerPrefs.SetString("facebookID", facebook.ID);
            PlayerPrefs.SetInt("highestLevelBeaten", chickenRoad.highestLevelBeaten);
            PlayerPrefs.SetInt("easyTutorialPlayed", (chickenRoad.easyTutorialPlayed ? 1 : 0));
            PlayerPrefs.SetInt("mediumTutorialPlayed", (chickenRoad.mediumTutorialPlayed ? 1 : 0));
            PlayerPrefs.SetInt("hardTutorialPlayed", (chickenRoad.hardTutorialPlayed ? 1 : 0));
            try {
                for (int i = 0; i < swipeIt.leaderboardScores.Count; i++) {
                    MonoBehaviour.print("saved");
                    PlayerPrefs.SetString("leaderboardID" + i, swipeIt.leaderboardScores[i].Key);
                    PlayerPrefs.SetInt("leaderboardScores" + i, swipeIt.leaderboardScores[i].Value);
                    PlayerPrefs.SetString("leaderboardNames" + i, swipeIt.leaderboardNames[swipeIt.leaderboardScores[i].Key]);
                }
            } catch (System.InvalidCastException e) {
                MonoBehaviour.print(e);
            }
            PlayerPrefs.Save();
        }

        public static void load() {
            //defaults
            accessibility.setAccessibility(true);
            swipeIt.singleHighScore = 0;
            swipeIt.multiHighScore = 0;
            swipeIt.leaderboardScores = new List<KeyValuePair<string, int>>();
            swipeIt.leaderboardNames = new Dictionary<string, string>();
            facebook.ID = "";
            facebook.name = "";
            chickenRoad.highestLevelBeaten = 0;
            chickenRoad.easyTutorialPlayed = false;
            chickenRoad.mediumTutorialPlayed = false;
            chickenRoad.hardTutorialPlayed = false;


            if (PlayerPrefs.HasKey("accessibility")) {
                accessibility.setAccessibility((PlayerPrefs.GetInt("accessibility") == 1 ? true : false));
            }
            if (PlayerPrefs.HasKey("singleHighScore")) {
                swipeIt.singleHighScore = PlayerPrefs.GetInt("singleHighScore");
            }
            if (PlayerPrefs.HasKey("multiHighScore")) {
                swipeIt.multiHighScore = PlayerPrefs.GetInt("multiHighScore");
            }

            if (PlayerPrefs.HasKey("highestLevelBeaten")) {
                chickenRoad.highestLevelBeaten = PlayerPrefs.GetInt("highestLevelBeaten");
            }
            if (PlayerPrefs.HasKey("easyTutorialPlayed")) {
                chickenRoad.easyTutorialPlayed = (PlayerPrefs.GetInt("easyTutorialPlayed") == 1 ? true : false);
            }
            if (PlayerPrefs.HasKey("mediumTutorialPlayed")) {
                chickenRoad.easyTutorialPlayed = (PlayerPrefs.GetInt("mediumTutorialPlayed") == 1 ? true : false);
            }
            if (PlayerPrefs.HasKey("hardTutorialPlayed")) {
                chickenRoad.easyTutorialPlayed = (PlayerPrefs.GetInt("hardTutorialPlayed") == 1 ? true : false);
            }

            if (PlayerPrefs.HasKey("facebookID")) {
                facebook.ID = PlayerPrefs.GetString("facebookID");
            }
            if (PlayerPrefs.HasKey("facebookName")) {
                facebook.name = PlayerPrefs.GetString("facebookName");
            }

            for (int i = 0; i < 5; i++) {
                if (PlayerPrefs.HasKey("leaderboardID" + i) && PlayerPrefs.HasKey("leaderboardScores" + i) && PlayerPrefs.HasKey("leaderboardNames" + i)) {
                    swipeIt.leaderboardScores.Add (new KeyValuePair<string, int> (PlayerPrefs.GetString("leaderboardID" + i), PlayerPrefs.GetInt("leaderboardScores" + i)));
                    swipeIt.leaderboardNames.Add(PlayerPrefs.GetString("leaderboardID" + i), PlayerPrefs.GetString("leaderboardNames" + i));
                }
            }
        }
    }
}
