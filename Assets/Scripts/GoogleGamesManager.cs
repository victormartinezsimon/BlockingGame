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
		initialize();
	}
	private void initialize() {
		PlayGamesPlatform.InitializeInstance(GooglePlayGames.BasicApi.PlayGamesClientConfiguration.DefaultConfiguration);
		GooglePlayGames.PlayGamesPlatform.Activate();
		#if !UNITY_STANDALONE && !UNITY_EDITOR
		Social.localUser.Authenticate((bool success) => {
			Debug.Log("Player authenticated => " + success);
			Debug.Log(Social.localUser.userName);
			play20Times();
		});
		#endif
	}
	public static void saveScore(int score) {
#if !UNITY_STANDALONE && !UNITY_EDITOR
		Social.ReportScore(score, GPGSIds.leaderboard_blocking_high_score, (bool success) => {
			Debug.Log("Score saved => " + success);
		});
#endif
	}

	public static void viewLeaderBoard() {
#if !UNITY_STANDALONE && !UNITY_EDITOR
		Social.ShowLeaderboardUI();
#endif
	}

	public static void play20Times(){
#if !UNITY_STANDALONE && !UNITY_EDITOR
		PlayGamesPlatform.Instance.IncrementAchievement(
			Blocking.GPGSIds.achievement_play_20_times, 1, (bool success) => {
			// handle success or failure
			Debug.Log("Play 20 times => " + success);
		});
#endif
	}
	public static void continue10Times(){
#if !UNITY_STANDALONE && !UNITY_EDITOR
		PlayGamesPlatform.Instance.IncrementAchievement(
			Blocking.GPGSIds.achievement_continue_game_10_times, 1, (bool success) => {
			// handle success or failure
			Debug.Log("Continued 10 => " + success);
		});
#endif
	}
	public static void scoreOf10(){
#if !UNITY_STANDALONE && !UNITY_EDITOR
		Social.ReportProgress(Blocking.GPGSIds.achievement_score_of_10, 100.0f, (bool success) => {
			// handle success or failure
			Debug.Log("Score of 10 revealed => " + success);
		});
#endif
	}
	public static void scoreOf30(){
#if !UNITY_STANDALONE && !UNITY_EDITOR
		Social.ReportProgress(Blocking.GPGSIds.achievement_score_of_30, 100.0f, (bool success) => {
			// handle success or failure
			Debug.Log("Score of 30 revealed => " + success);
		});
#endif
	}
	public static void noContinue5(){
#if !UNITY_STANDALONE && !UNITY_EDITOR
		PlayGamesPlatform.Instance.IncrementAchievement(
			Blocking.GPGSIds.achievement_not_continue_5_times, 1, (bool success) => {
			// handle success or failure
			Debug.Log("Not continued 5 => " + success);
		});
#endif
	}
}
