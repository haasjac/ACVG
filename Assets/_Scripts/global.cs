using UnityEngine;
using System.Collections;

// Class to keep track of Global variables. Utilizes lazy initialization.
public class global {
    // Singleton
    static global _s = null;
    public static global S
    {
        get
        {
            if (_s == null)
                _s = new global();
            return _s;
        }
    }

    // Public
    public bool accessibility;
    public int currentScore;
    public int highScore;

    public void save() {
        PlayerPrefs.SetInt("accessibility", (accessibility ? 1 : 0));
        PlayerPrefs.SetInt("highScore", highScore);
        PlayerPrefs.Save();
    }

    public void load() {
        if (PlayerPrefs.HasKey("accessibility")) {
            accessibility = (PlayerPrefs.GetInt("accessibility") == 1 ? true : false);
        }
        if (PlayerPrefs.HasKey("highScore")) {
            highScore = PlayerPrefs.GetInt("highScore");
        }
    }

    public bool checkHighScore() {
        if (currentScore > highScore) {
            highScore = currentScore;
            save();
            return true;
        }
        else {
            return false;
        }
    }

    // Private
    private global() {
        accessibility = true;
        currentScore = 0;
        highScore = 0;
        load();
    }

}
