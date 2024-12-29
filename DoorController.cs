using System.Collections;
using ControlFreak2;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class DoorController : MonoBehaviour
{
	private Door lastDoor;

	private Gate lastGate;

	private Elevator lastElev;

	private SCP1162 last1162;

	private SCP106ControlPanel scp106;

	private AlphaWarhead lastAlpha;

	private GameObject uiButton;

	private GameObject needKeycard;

	private WaitForSeconds delay = new WaitForSeconds(1.5f);

	private void Start()
	{
		if (((Component)this).gameObject.GetComponent<PhotonView>().IsMine)
		{
			uiButton = GameObject.Find("DoorButton");
			needKeycard = GameObject.Find("NeedKeycard");
			if ((Object)(object)uiButton != (Object)null)
			{
				uiButton.SetActive(false);
			}
			if ((Object)(object)needKeycard != (Object)null)
			{
				needKeycard.SetActive(false);
			}
		}
	}

	private void Update()
	{
		if (((Component)this).gameObject.GetComponent<PhotonView>().IsMine)
		{
			if ((Object)(object)lastDoor != (Object)null && CF2Input.GetButtonDown("Interact"))
			{
				lastDoor.ChangeState();
				uiButton.SetActive(false);
			}
			if ((Object)(object)lastGate != (Object)null && CF2Input.GetButtonDown("Interact"))
			{
				lastGate.ChangeState();
				uiButton.SetActive(false);
			}
			if ((Object)(object)scp106 != (Object)null && CF2Input.GetButtonDown("Interact"))
			{
				scp106.Contain();
				uiButton.SetActive(false);
			}
			if (Object.op_Implicit((Object)(object)lastElev) && CF2Input.GetButtonDown("Interact"))
			{
				((Component)lastElev).GetComponent<PhotonView>().RPC("Use", (RpcTarget)0, new object[0]);
				uiButton.SetActive(false);
			}
			if (Object.op_Implicit((Object)(object)last1162) && CF2Input.GetButtonDown("Interact"))
			{
				((Component)last1162).GetComponent<PhotonView>().RPC("GetRandomItem", (RpcTarget)0, new object[0]);
				uiButton.SetActive(false);
			}
			if (Object.op_Implicit((Object)(object)lastAlpha) && CF2Input.GetButtonDown("Interact"))
			{
				((Component)lastAlpha).GetComponent<PhotonView>().RPC("Detonate", (RpcTarget)0, new object[0]);
				uiButton.SetActive(false);
			}
		}
	}

	public void Entered(Door _lastDoor, Gate _lastGate, SCP106ControlPanel _scp106, Elevator _elev, SCP1162 _1162, AlphaWarhead _alpha)
	{
		if (!((Component)this).gameObject.GetComponent<PhotonView>().IsMine)
		{
			return;
		}
		if ((Object)(object)uiButton == (Object)null)
		{
			uiButton = GameObject.Find("DoorButton");
		}
		if ((Object)(object)_lastDoor != (Object)null)
		{
			lastDoor = _lastDoor;
			if ((Object)(object)uiButton != (Object)null)
			{
				uiButton.SetActive(true);
			}
			return;
		}
		if ((Object)(object)_lastGate != (Object)null)
		{
			lastGate = _lastGate;
			if ((Object)(object)uiButton != (Object)null)
			{
				uiButton.SetActive(true);
			}
			return;
		}
		if ((Object)(object)_scp106 != (Object)null)
		{
			scp106 = _scp106;
			if ((Object)(object)uiButton != (Object)null)
			{
				uiButton.SetActive(true);
			}
			return;
		}
		if ((Object)(object)_elev != (Object)null)
		{
			lastElev = _elev;
			if ((Object)(object)uiButton != (Object)null)
			{
				uiButton.SetActive(true);
			}
			return;
		}
		if ((Object)(object)_1162 != (Object)null)
		{
			last1162 = _1162;
			if ((Object)(object)uiButton != (Object)null)
			{
				uiButton.SetActive(true);
			}
			return;
		}
		if ((Object)(object)_alpha != (Object)null)
		{
			lastAlpha = _alpha;
			if ((Object)(object)uiButton != (Object)null)
			{
				uiButton.SetActive(true);
			}
			return;
		}
		lastDoor = null;
		lastGate = null;
		scp106 = null;
		lastElev = null;
		last1162 = null;
		lastAlpha = null;
		if ((Object)(object)uiButton != (Object)null)
		{
			uiButton.SetActive(false);
		}
	}

	public void NeedKeycard(int id)
	{
		if (((Component)this).gameObject.GetComponent<PhotonView>().IsMine)
		{
			((MonoBehaviour)this).StartCoroutine(DisplayNeedKeycard(id));
		}
	}

	private IEnumerator DisplayNeedKeycard(int id)
	{
		needKeycard.SetActive(true);
		needKeycard.GetComponent<Text>().text = "Need keycard level " + id;
		yield return delay;
		needKeycard.GetComponent<Text>().text = string.Empty;
		needKeycard.SetActive(false);
	}
}
