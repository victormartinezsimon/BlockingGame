using UnityEngine;
using System.Collections;
using UnityEngine.Advertisements;

public class AdsManager : MonoBehaviour {

	public static AdsManager instance;
	public delegate void CallbackAdd(bool result);
	public CallbackAdd myCallback;

	// Use this for initialization
	void Start () {
	
		if(instance != null) { 
			Destroy(this.gameObject);
			return;
		}
		instance = this;
		DontDestroyOnLoad(this.gameObject);

		#if !UNITY_STANDALONE
		if (Advertisement.isSupported) {
			Advertisement.Initialize ("1011522", false);
		} else {
			Debug.Log("Platform not supported");
		}
		#endif
	}

	public static AdsManager getInstance() {
		return instance;
	}
	
	public static void showVideoAdd(CallbackAdd myCallback) {
		AdsManager.getInstance().myCallback = myCallback;
		AdsManager.getInstance().StartCoroutine(AdsManager.getInstance().ShowAdWhenReady());
	}
	public IEnumerator ShowAdWhenReady() {
		//#if UNITY_EDITOR
		//yield return 0;
		//AdCallbackhandler(ShowResult.Finished);
		//#else
		while (!Advertisement.isReady())
			yield return null;
		Debug.Log("video ready");
		ShowOptions options = new ShowOptions ();
		options.resultCallback = AdCallbackhandler;
		Debug.Log("Try to show video");
		Advertisement.Show (null, options);
		//#endif
	}
	void AdCallbackhandler (ShowResult result)
	{
		Debug.Log("class called");
		switch(result)
		{
		case ShowResult.Finished:
			Debug.Log ("Ad Finished. Rewarding player...");
			AdsManager.getInstance().myCallback(true);
			break;
		case ShowResult.Skipped:
			Debug.Log ("Ad skipped. Son, I am dissapointed in you");
			AdsManager.getInstance().myCallback(false);
			break;
		case ShowResult.Failed:
			Debug.Log("I swear this has never happened to me before");
			AdsManager.getInstance().myCallback(false);
			break;
		}
	}
}
