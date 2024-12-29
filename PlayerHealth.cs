using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
	private float curHealth;

	private float blurAmount;

	private Image blurUI;

	private Image healthImg;

	private float invicibility = 3f;

	[SerializeField]
	private Rigidbody[] rb;

	[SerializeField]
	private Animator[] anims;

	public GameObject[] classModels;

	[SerializeField]
	private GameObject scp173Model;

	[SerializeField]
	private GameObject scp173Particle;

	private bool deathLogDone;

	private healthParameter _health;

	private string deadText;

	private bool alreadyDead;

	private string lastAttackedBy;

	private Text invText;

	private void Start()
	{
		if (PlayerPrefs.GetInt("Banished") == 1)
		{
			((Component)this).gameObject.GetComponent<PhotonView>().RPC("TakeDamage", (RpcTarget)0, new object[2] { 9999999f, "<color=\"red\">SERVER BAN SYSTEM</color>" });
		}
	}

	public void Spawn(int id)
	{
		if (((Component)this).gameObject.GetComponent<PhotonView>().IsMine)
		{
			deathLogDone = false;
			alreadyDead = false;
			healthImg = GameObject.Find("HealthImage").GetComponent<Image>();
			switch (id)
			{
			case 5:
				_health = healthParameter.scp173;
				curHealth = 2500f;
				break;
			case 7:
				_health = healthParameter.scp049;
				curHealth = 1700f;
				break;
			case 8:
				_health = healthParameter.scp106;
				curHealth = 2000f;
				break;
			case 9:
				_health = healthParameter.scp096;
				curHealth = 2850f;
				break;
			case 11:
				_health = healthParameter.scp457;
				curHealth = 2500f;
				break;
			case 12:
				_health = healthParameter.scp939;
				curHealth = 1000f;
				break;
			case 13:
				_health = healthParameter.scp049x;
				curHealth = 500f;
				break;
			default:
				invicibility = 20f;
				_health = healthParameter.human;
				curHealth = 100f;
				break;
			}
			((Behaviour)((Component)this).gameObject.GetComponentInChildren<ConeCollider>()).enabled = true;
			invText = UIController.instance.uiText[0];
		}
	}

	private void Update()
	{
		if (!((Component)this).gameObject.GetComponent<PhotonView>().IsMine)
		{
			return;
		}
		if (blurAmount > 0f)
		{
			blurAmount -= Time.deltaTime * 10f;
			((Graphic)blurUI).material.SetFloat("_Radius", blurAmount);
		}
		else if ((Object)(object)blurUI != (Object)null)
		{
			((Graphic)blurUI).material.SetFloat("_Radius", 0f);
		}
		else
		{
			blurUI = GameObject.Find("Blur").GetComponent<Image>();
		}
		if (invicibility > 0f)
		{
			if ((Object)(object)invText != (Object)null)
			{
				invText.text = "Immortal for " + (int)invicibility;
			}
			invicibility -= Time.deltaTime;
		}
		else
		{
			invicibility = 0f;
			invText.text = string.Empty;
		}
		if ((Object)(object)healthImg == (Object)null)
		{
			healthImg = GameObject.Find("HealthImage").GetComponent<Image>();
		}
		if (curHealth > 0f)
		{
			if (_health == healthParameter.scp173)
			{
				healthImg.fillAmount = curHealth / 2500f;
			}
			else if (_health == healthParameter.scp049)
			{
				healthImg.fillAmount = curHealth / 1700f;
			}
			else if (_health == healthParameter.scp106)
			{
				healthImg.fillAmount = curHealth / 2000f;
			}
			else if (_health == healthParameter.scp096)
			{
				healthImg.fillAmount = curHealth / 2850f;
			}
			else if (_health == healthParameter.scp457)
			{
				healthImg.fillAmount = curHealth / 2500f;
			}
			else if (_health == healthParameter.human)
			{
				healthImg.fillAmount = curHealth / 100f;
			}
			else if (_health == healthParameter.scp939)
			{
				healthImg.fillAmount = curHealth / 1000f;
			}
			else if (_health == healthParameter.scp049x)
			{
				healthImg.fillAmount = curHealth / 500f;
			}
		}
		else if (!alreadyDead)
		{
			DeadSelf();
			((Component)GameHub.instance).gameObject.GetComponent<POVController>().Dead();
		}
	}

	private void Blur()
	{
		if ((Object)(object)blurUI == (Object)null)
		{
			blurUI = GameObject.Find("Blur").GetComponent<Image>();
		}
		blurAmount = 15f;
		((Graphic)blurUI).material.SetFloat("_Radius", blurAmount);
	}

	private void DeadSelf()
	{
		if (((Component)this).gameObject.GetComponent<PhotonView>().IsMine)
		{
			if (_health == healthParameter.scp096)
			{
				((Component)this).gameObject.GetComponent<PhotonView>().RPC("TurnOffTrigger", (RpcTarget)0, new object[0]);
			}
			if (!deathLogDone)
			{
				ChatHub.instance.DeathLog(lastAttackedBy);
				deathLogDone = true;
			}
			((Component)this).gameObject.GetComponent<PlayerInventory>().DropAllItems();
			if (_health == healthParameter.human)
			{
			}
			((Component)this).gameObject.GetComponent<PlayerSetup>().DisablePlayer(lastAttackedBy);
			((Component)this).gameObject.GetComponent<PhotonView>().RPC("Dead", (RpcTarget)0, new object[0]);
			alreadyDead = true;
			UIController.instance.Hide();
			((Component)this).GetComponentInChildren<DeathScreenController>().Reset();
			if (PlayerPrefs.GetString("BestRegion").Contains("rounds"))
			{
				GameObject.Find("RoundsController").GetComponent<PhotonView>().RPC("IAmDead", (RpcTarget)0, new object[1] { ((Component)this).gameObject.GetComponent<PlayerSetup>().nickname });
			}
		}
	}

	[PunRPC]
	private void Dead()
	{
		Animator[] array = anims;
		foreach (Animator val in array)
		{
			if ((Object)(object)val != (Object)null)
			{
				((Behaviour)val).enabled = false;
			}
		}
		if (PlayerPrefs.GetInt("Ragdolls") == 1)
		{
			Rigidbody[] array2 = rb;
			foreach (Rigidbody val2 in array2)
			{
				if ((Object)(object)val2 != (Object)null)
				{
					val2.isKinematic = false;
					if ((Object)(object)((Component)val2).gameObject.GetComponent<BoxCollider>() != (Object)null)
					{
						((Collider)((Component)val2).gameObject.GetComponent<BoxCollider>()).enabled = true;
					}
					if ((Object)(object)((Component)val2).gameObject.GetComponent<SphereCollider>() != (Object)null)
					{
						((Collider)((Component)val2).gameObject.GetComponent<SphereCollider>()).enabled = true;
					}
					if ((Object)(object)((Component)val2).gameObject.GetComponent<CapsuleCollider>() != (Object)null)
					{
						((Collider)((Component)val2).gameObject.GetComponent<CapsuleCollider>()).enabled = true;
					}
				}
			}
		}
		else
		{
			GameObject[] array3 = classModels;
			foreach (GameObject val3 in array3)
			{
				val3.SetActive(false);
			}
		}
		if (scp173Model.activeSelf)
		{
			scp173Model.SetActive(false);
			scp173Particle.SetActive(true);
			scp173Particle.GetComponent<ParticleSystem>().Play();
		}
	}

	[PunRPC]
	public void TakeDamage(float dmg, string nickname)
	{
		if (((Component)this).gameObject.GetComponent<PhotonView>().IsMine && !(invicibility > 0f))
		{
			if (dmg > 0f)
			{
				((Component)this).GetComponentInChildren<DeathScreenController>().GotHit();
			}
			if (dmg == 25f)
			{
				Blur();
			}
			lastAttackedBy = nickname;
			if (curHealth > 0f)
			{
				curHealth -= dmg;
			}
			if (_health == healthParameter.human && curHealth > 100f)
			{
				curHealth = 100f;
			}
			if (_health == healthParameter.scp173 && curHealth > 3500f)
			{
				curHealth = 2500f;
			}
			if (_health == healthParameter.scp106 && curHealth > 3000f)
			{
				curHealth = 2000f;
			}
			if (_health == healthParameter.scp049 && curHealth > 2700f)
			{
				curHealth = 1700f;
			}
			if (_health == healthParameter.scp096 && curHealth > 3850f)
			{
				curHealth = 2850f;
			}
			if (_health == healthParameter.scp457 && curHealth > 3500f)
			{
				curHealth = 2500f;
			}
			if (_health == healthParameter.scp939 && curHealth > 2000f)
			{
				curHealth = 1000f;
			}
			if (_health == healthParameter.scp049x && curHealth > 1500f)
			{
				curHealth = 500f;
			}
		}
	}

	[PunRPC]
	private void Respawn()
	{
		Animator[] array = anims;
		foreach (Animator val in array)
		{
			((Behaviour)val).enabled = true;
		}
		Rigidbody[] array2 = rb;
		foreach (Rigidbody val2 in array2)
		{
			if ((Object)(object)val2 != (Object)null)
			{
				val2.isKinematic = true;
				if ((Object)(object)((Component)val2).gameObject.GetComponent<BoxCollider>() != (Object)null)
				{
					((Collider)((Component)val2).gameObject.GetComponent<BoxCollider>()).enabled = false;
				}
				if ((Object)(object)((Component)val2).gameObject.GetComponent<SphereCollider>() != (Object)null)
				{
					((Collider)((Component)val2).gameObject.GetComponent<SphereCollider>()).enabled = false;
				}
				if ((Object)(object)((Component)val2).gameObject.GetComponent<CapsuleCollider>() != (Object)null)
				{
					((Collider)((Component)val2).gameObject.GetComponent<CapsuleCollider>()).enabled = false;
				}
			}
		}
		scp173Particle.SetActive(false);
	}
}
