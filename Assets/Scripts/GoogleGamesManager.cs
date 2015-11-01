using UnityEngine;
using System.Collections;
using GooglePlayGames;
using UnityEngine.SocialPlatforms;
using Blocking;

public class GoogleGamesManager : MonoBehaviour {

	public static GoogleGamesManager instance;

	// Use this for initialization
	void Start () {
		if(instance != null) { 
			Destroy(this.gameObject);
			return;
		}
		instance = this;
		DontDestroyOnLoad(this.gameObject);

		Social.localUser.Authenticate((bool success) => Debug.Log("Player authenticated => " + success));
	}

	public static void saveScore(int score) {
		PlayGamesPlatform.Instance.IncrementAchievement(
		GPGSIds.leaderboard_blocking_high_score, score, (bool success) => {
			Debug.Log("Score saved => " + success);
		});
	}

	public static void viewLeaderBoard() {
		Social.ShowLeaderboardUI();
	}

	public static void play20Times(){}
	public static void continue10Times(){}
	public static void scoreOf10(){}
	public static void scoreOf30(){}
	public static void noContinue(){}
}
