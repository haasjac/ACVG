using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using Facebook.Unity;
using Global;

public class FBscript : MonoBehaviour {

    public Text DialogUsername;
    public Image DialogProfilePic;
    public Text logButtonText;
    public Text swipeItScore;
    public Text chickenRoadScore;

    void Start() {
        swipeItScore.text = swipeIt.singleHighScore.ToString();
        chickenRoadScore.text = chickenRoad.highestLevelBeaten.ToString();
        DialogUsername.text = "Welcome!";
        DialogProfilePic.sprite = null;

        if (FB.IsInitialized) {
            SetInit();
        } else {
            FB.Init(SetInit, OnHideUnity);
        }
        
    }

    void SetInit() {

        if (FB.IsLoggedIn) {
            Debug.Log("FB is logged in");
        } else {
            Debug.Log("FB is not logged in");
            
        }

        DealWithFBMenus(FB.IsLoggedIn);

    }

    void OnHideUnity(bool isGameShown) {

        if (!isGameShown) {
            Time.timeScale = 0;
        } else {
            Time.timeScale = 1;
        }

    }

    public void toggleLog() {
        if (FB.IsLoggedIn) {
            FBlogout();
        } else {
            FBlogin();
        }
    }

    void FBlogin() {

        List<string> permissions = new List<string>();
        permissions.Add("public_profile");

        Time.timeScale = 0;
        FB.LogInWithReadPermissions(permissions, AuthCallBack);
    }

    void FBlogout() {
        FB.LogOut();
        DealWithFBMenus(FB.IsLoggedIn);
    }

    void AuthCallBack(IResult result) {

        Time.timeScale = 1;
        if (result.Error != null) {
            Debug.Log(result.Error);
        } else {
            if (FB.IsLoggedIn) {
                Debug.Log("FB is logged in");
                print("userID: " + AccessToken.CurrentAccessToken.UserId);
                facebook.ID = AccessToken.CurrentAccessToken.UserId;
                StartCoroutine(user());
            } else {
                Debug.Log("FB is not logged in");
            }

            DealWithFBMenus(FB.IsLoggedIn);
        }

    }

    void DealWithFBMenus(bool isLoggedIn) {

        if (isLoggedIn) {

            FB.API("/me?fields=first_name", HttpMethod.GET, DisplayUsername);
            FB.API("/me?fields=picture", HttpMethod.GET, DisplayProfilePic);
            logButtonText.text = "Log Out";
            ///picture?type=square&height=128&width=128

        } else {
            DialogUsername.text = "Welcome!";
            DialogProfilePic.sprite = null;
            logButtonText.text = "Log In With Facebook";
        }

    }

    IEnumerator user() {
        yield return StartCoroutine(myApi.S.makeUser());
        swipeIt.singleHighScore = int.Parse(myApi.swipeItInfo["score"]);
        chickenRoad.highestLevelBeaten = int.Parse(myApi.chickenRoadInfo["score"]);
        swipeItScore.text = myApi.swipeItInfo["score"];
        chickenRoadScore.text = myApi.chickenRoadInfo["score"];
        functions.save();
    }

    void DisplayUsername(IResult result) {

        if (result.Error == null) {

            DialogUsername.text = "Welcome, " + result.ResultDictionary["first_name"] + "!";

            facebook.name = result.ResultDictionary["first_name"].ToString();

        } else {
            Debug.Log(result.Error);
        }

    }

    void DisplayProfilePic(IGraphResult result) {

        if (result.Texture != null) {
            DialogProfilePic.sprite = Sprite.Create(result.Texture, new Rect(0, 0, 128, 128), new Vector2());

        }

    }

}