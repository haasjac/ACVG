using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum game { menu, swipeIt, chickenRoad};

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
    public Dictionary<game, float> highScore;
    public game currentGame;
    public bool multiplayer;

    public void save() {
        PlayerPrefs.SetInt("accessibility", (accessibility ? 1 : 0));
        PlayerPrefs.SetFloat("highScore" + currentGame.ToString(), highScore[currentGame]);
        PlayerPrefs.Save();
    }

    public void load() {
        if (PlayerPrefs.HasKey("accessibility")) {
            accessibility = (PlayerPrefs.GetInt("accessibility") == 1 ? true : false);
        }
        if (PlayerPrefs.HasKey("highScore" + currentGame.ToString())) {
            highScore[currentGame] = PlayerPrefs.GetFloat("highScore" + currentGame.ToString());
        }
    }

    public bool checkHighScore() {
        if (currentScore > highScore[currentGame]) {
            highScore[currentGame] = currentScore;
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
        highScore = new Dictionary<game, float> ();
        game[] t = (game[])System.Enum.GetValues(typeof(game));
        for (int i = 0; i < t.Length; i++) {
            highScore[t[i]] = 0;
        }
        currentGame = game.menu;
        multiplayer = false;
        load();
    }

}
