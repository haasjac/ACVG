using UnityEngine;
using System.Collections;

public class buttonSetGameMode : MonoBehaviour {

	public void setSingle() {
        swipeIt.S.mode = swipeIt.gameMode.single;
    }

    public void setMulti() {
        swipeIt.S.mode = swipeIt.gameMode.multi;
    }

    public void setTutorial() {
        swipeIt.S.mode = swipeIt.gameMode.tutorial;
    }
}
