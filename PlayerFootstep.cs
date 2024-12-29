using UnityEngine;
using UnityEngine.UI;

public class PlayerFootstep : MonoBehaviour
{
	public AudioSource _as;

	public AudioClip[] footsteps;

	public AudioClip[] decays;

	public AudioClip stoneDrag;

	private Text _coordinates;

	public bool justFootsteps = true;

	private void Start()
	{
		if (!justFootsteps && (Object)(object)_coordinates == (Object)null)
		{
			_coordinates = GameObject.Find("Coordinates").GetComponent<Text>();
		}
	}

	public void PlayFootstep()
	{
		if (!_as.isPlaying)
		{
			_as.volume = 1f;
			_as.clip = footsteps[Random.Range(0, footsteps.Length)];
			_as.Play();
		}
	}

	public void PlayDecay()
	{
		if (!_as.isPlaying)
		{
			_as.volume = 1f;
			_as.clip = decays[Random.Range(0, decays.Length)];
			_as.Play();
		}
	}

	public void PlayStoneDrag()
	{
		if (!_as.isPlaying)
		{
			_as.volume = 0.3f;
			_as.clip = stoneDrag;
			_as.Play();
		}
	}

	public void StopStoneDrag()
	{
		if (_as.isPlaying)
		{
			_as.Stop();
		}
	}

	private void Update()
	{
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		if (!justFootsteps)
		{
			if ((Object)(object)_coordinates == (Object)null)
			{
				_coordinates = GameObject.Find("Coordinates").GetComponent<Text>();
				return;
			}
			_coordinates.text = "X: " + (int)((Component)this).transform.position.x + " Z: " + (int)((Component)this).transform.position.z;
		}
	}
}
