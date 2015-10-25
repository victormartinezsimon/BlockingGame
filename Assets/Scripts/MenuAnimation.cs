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

		Vector3 posEntryPlay = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width*0.5f, -Screen.height * 0.2f, 0)); 
		Vector3 posEntryName = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width*0.5f, Screen.height * 1.2f, 0)); 
		m_play.transform.position = new Vector3(posEntryPlay.x, posEntryPlay.y, 0);
		m_name.transform.position = new Vector3(posEntryName.x, posEntryName.y, 0);

		yield return new WaitForSeconds(timeWait);
		moveMenu();
		yield return new WaitForSeconds(timeWaitBetweenAnimations);
		movePlay();

	}

	void movePlay() {
		Vector3 posEndPlay = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width * 0.5f, Screen.height * 0.2f, 0)); 
		Vector3 startLocal = m_play.transform.localPosition;
		m_play.transform.position = posEndPlay;
		Vector3 endlocal = m_play.transform.localPosition;
		m_play.transform.localPosition = startLocal;
		m_play.SetAnimationPosition(new Vector2(startLocal.x, startLocal.y),
		                            new Vector2(endlocal.x, endlocal.y),
		                            curveAnimation, curveAnimation);

		m_play.ChangeSetState(false);
		m_play.OpenCloseObjectAnimation();
	}
	void moveMenu() {
		Vector3 posEndMenu = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width * 0.5f, Screen.height * 0.8f, 0)); 
		Vector3 startLocal = m_name.transform.localPosition;
		m_name.transform.position = posEndMenu;
		Vector3 endlocal = m_name.transform.localPosition;
		m_name.transform.localPosition = startLocal;
		m_name.SetAnimationPosition(new Vector2(startLocal.x, startLocal.y),
		                            new Vector2(endlocal.x, endlocal.y),
		                            curveAnimation, curveAnimation);
		
		m_name.ChangeSetState(false);
		m_name.OpenCloseObjectAnimation();
	}

}
