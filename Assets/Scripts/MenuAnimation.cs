using UnityEngine;
using System.Collections;

public class MenuAnimation : MonoBehaviour {

	public float timeWait = 1.0f;
	public float timeWaitBetweenAnimations = 1.0f;
	public EasyTween m_name;
	public EasyTween m_play;

	// Use this for initialization
	IEnumerator Start () {
		yield return new WaitForSeconds(timeWait);
		m_name.OpenCloseObjectAnimation();
		yield return new WaitForSeconds(timeWaitBetweenAnimations);
		m_play.OpenCloseObjectAnimation();
	}
	
	
}
