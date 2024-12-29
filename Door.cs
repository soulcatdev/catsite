using System.Collections;
using Photon.Pun;
using UnityEngine;

public class Door : MonoBehaviour
{
	[Header("Visuals & Sound Effects")]
	public Animator _anim;

	public AudioSource _as;

	public AudioSource _peepPeep;

	[Header("Parameters")]
	public int keycardLvl;

	public int currentState = 1;

	public float closeTimer;

	public float timer;

	private WaitForSeconds delayClose;

	private void Start()
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Expected O, but got Unknown
		delayClose = new WaitForSeconds(closeTimer);
	}

	private void Update()
	{
		if (timer > 0f)
		{
			timer -= Time.deltaTime;
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		if (((Component)other).tag == "Player" && timer <= 0f)
		{
			if (((Component)other).GetComponent<KeycardController>().GetKeycard() >= keycardLvl)
			{
				((Component)other).GetComponent<DoorController>().Entered(this, null, null, null, null, null);
			}
			else
			{
				((Component)other).GetComponent<DoorController>().NeedKeycard(keycardLvl);
			}
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (((Component)other).tag == "Player" && (Object)(object)((Component)other).GetComponent<DoorController>() != (Object)null)
		{
			((Component)other).GetComponent<DoorController>().Entered(null, null, null, null, null, null);
		}
	}

	public void ChangeState()
	{
		if (!_as.isPlaying)
		{
			if (currentState == 0)
			{
				currentState = 1;
			}
			else
			{
				currentState = 0;
			}
			((Component)this).gameObject.GetComponent<PhotonView>().RPC("RpcChangeState", (RpcTarget)3, new object[1] { currentState });
		}
	}

	[PunRPC]
	public void RpcChangeState(int state)
	{
		if (closeTimer != 0f && state != 1)
		{
			timer = closeTimer;
			((MonoBehaviour)this).StopCoroutine(CloseAfterTimer());
			((MonoBehaviour)this).StartCoroutine(CloseAfterTimer());
		}
		currentState = state;
		_as.Play();
		if (currentState == 0)
		{
			_anim.SetInteger("State", 1);
		}
		else
		{
			_anim.SetInteger("State", 0);
		}
	}

	private IEnumerator CloseAfterTimer()
	{
		if (!_peepPeep.isPlaying)
		{
			timer = closeTimer;
			_peepPeep.Play();
			yield return delayClose;
			((Component)this).gameObject.GetComponent<PhotonView>().RPC("RpcChangeState", (RpcTarget)3, new object[1] { 1 });
		}
	}
}
