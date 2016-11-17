using UnityEngine;
using System.Collections;
using Global;

public class buttonSetGameMode : MonoBehaviour {

	public void setSingle() {
        swipeIt.mode = swipeIt.gameMode.single;
    }

    public void setMulti() {
        swipeIt.mode = swipeIt.gameMode.multi;
    }

    public void setTutorial() {
        swipeIt.mode = swipeIt.gameMode.tutorial;
    }
}
