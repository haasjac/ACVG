using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using Facebook.Unity;
<<<<<<< Updated upstream
using SimpleJSON;
=======
>>>>>>> Stashed changes

public class FBscript : MonoBehaviour {

    public GameObject DialogLoggedIn;
    public GameObject DialogLoggedOut;
    public GameObject DialogUsername;
    public GameObject DialogProfilePic;

<<<<<<< Updated upstream
    [System.Serializable]
    public class json {
        public string test = "t";
        public string difficulty = "nope";
    }

    public string url = "http://acvg-uno-flask-env.xnuwix2zea.us-west-2.elasticbeanstalk.com";

    IEnumerator apiCall() {
        //string userID = AccessToken.CurrentAccessToken.UserId;
        WWWForm form = new WWWForm();
        //form.AddField("id",userID);
        url += "/levels";
        WWW www = new WWW(url);
        yield return www;
        if (www.error == null) {
            var j = JSON.Parse(www.text);
            Debug.Log("WWW Ok!: " + www.text);
            print("json: " + j["1"]["difficulty"]);
        } else {
            Debug.Log("WWW Error: " + www.error);
        }
    }

    public void testAPI() {
        StartCoroutine(apiCall());
    }

=======
>>>>>>> Stashed changes
    void Awake() {
        if (FB.IsLoggedIn) {
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

    public void FBlogin() {

        List<string> permissions = new List<string>();
        permissions.Add("public_profile");

        FB.LogInWithReadPermissions(permissions, AuthCallBack);
    }

    void AuthCallBack(IResult result) {

        if (result.Error != null) {
            Debug.Log(result.Error);
        } else {
            if (FB.IsLoggedIn) {
                Debug.Log("FB is logged in");
<<<<<<< Updated upstream
                print("userID: " + AccessToken.CurrentAccessToken.UserId);
=======
>>>>>>> Stashed changes
            } else {
                Debug.Log("FB is not logged in");
            }

            DealWithFBMenus(FB.IsLoggedIn);
        }

    }

    void DealWithFBMenus(bool isLoggedIn) {

        if (isLoggedIn) {
            DialogLoggedIn.SetActive(true);
            DialogLoggedOut.SetActive(false);

            FB.API("/me?fields=first_name", HttpMethod.GET, DisplayUsername);
            FB.API("/me/picture?type=square&height=128&width=128", HttpMethod.GET, DisplayProfilePic);

        } else {
            DialogLoggedIn.SetActive(false);
            DialogLoggedOut.SetActive(true);
        }

    }

    void DisplayUsername(IResult result) {

        Text UserName = DialogUsername.GetComponent<Text>();

        if (result.Error == null) {

            UserName.text = "Hi there, " + result.ResultDictionary["first_name"];

        } else {
            Debug.Log(result.Error);
        }

    }

    void DisplayProfilePic(IGraphResult result) {

        if (result.Texture != null) {

            Image ProfilePic = DialogProfilePic.GetComponent<Image>();

            ProfilePic.sprite = Sprite.Create(result.Texture, new Rect(0, 0, 128, 128), new Vector2());

        }

    }

}