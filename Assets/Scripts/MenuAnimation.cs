using UnityEngine;
using System.Collections;

public class MenuAnimation : MonoBehaviour {

	public float timeInitWait = 1.0f;
	public float timeWaitBetweenAnimations = 1.0f;
	public EasyTween m_name;
	public EasyTween m_play;
	public EasyTween m_records;

	// Use this for initialization
	IEnumerator Start () {
		yield return new WaitForSeconds(timeInitWait);
		moveMenu();
		yield return new WaitForSeconds(timeWaitBetweenAnimations);
		movePlay();
		yield return new WaitForSeconds(timeWaitBetweenAnimations);
		moveRecord();
	}

	void movePlay() {
		m_play.OpenCloseObjectAnimation();
	}
	void moveMenu() {
		m_name.OpenCloseObjectAnimation();
	}
	void moveRecord() {
		m_records.OpenCloseObjectAnimation();
	}
}
