using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// Class to keep track of Global variables. Utilizes lazy initialization.
public class swipeIt {
    
    // Singleton
    static swipeIt _s = null;

    // Public Constructor
    public static swipeIt S {
        get {
            if (_s == null)
                _s = new swipeIt();
            return _s;
        }
    }

    // Private Constructor
    private swipeIt() {
        currentScore = 0;
        highScore = 0;
        load();
    }

    // Public Variables
    public enum gameMode { single, multi, tutorial };
    public int currentScore;
    public int highScore;
    public gameMode mode = gameMode.single;
    
        
    // Functions
    public void save() {
        PlayerPrefs.SetInt("swipeIt_highScore", highScore);
        PlayerPrefs.Save();
    }

    public void load() {
        if (PlayerPrefs.HasKey("swipeIt_highScore")) {
            highScore = PlayerPrefs.GetInt("swipeIt_highScore");
        }
    }

    public bool checkHighScore() {
        if (currentScore > highScore) {
            highScore = currentScore;
            save();
            return true;
        } else {
            return false;
        }
    }
}
