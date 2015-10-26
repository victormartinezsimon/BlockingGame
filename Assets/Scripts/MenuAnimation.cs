using UnityEngine;
using System.Collections;

public class MenuAnimation : MonoBehaviour {

	public float timeWait = 1.0f;
	public float timeWaitBetweenAnimations = 1.0f;
	public EasyTween m_name;
	public EasyTween m_play;

	public AnimationCurve curveAnimation;

	// Use this for initialization
	IEnumerator Start () {
		yield return new WaitForSeconds(timeWait);
		moveMenu();
		yield return new WaitForSeconds(timeWaitBetweenAnimations);
		movePlay();
	}

	void movePlay() {
		m_play.OpenCloseObjectAnimation();
	}
	void moveMenu() {
		m_name.OpenCloseObjectAnimation();
	}
}
