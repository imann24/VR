using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AudioController : MonoBehaviour {
	public static AudioController Instance;

	public AudioSource Music;
	public AudioSource SFX1;
	public AudioSource SFX2;

	public AudioClip GameMusic;
	public AudioClip MainMenuMusic;

	public AudioClip PrisonerFoundSFX;
	public AudioClip ShatterSFX;
	public AudioClip ExplosionSFX;
	public AudioClip SlideSFX;
	public AudioClip PageTurnSFX;
	public AudioClip StartButtonSFX;

	public enum Channel {Music, SFX1, SFX2};
	private Channel currentChannel;

	private Dictionary<Channel, AudioSource> allAudioSources;

	void Awake () {
		// Singleton implementation
		Util.SingletonImplementation(ref Instance, this, gameObject);
	}

	// Use this for initialization
	void Start () {
		allAudioSources = CreateAudioSourceDictionary();
		SubscribeEvents();
	}

	void OnDestroy () {
		UnsubscribeEvents();
	}

	public void PlayCurrentClip () {
		allAudioSources[currentChannel].Play();
	}

	public void SetClip (AudioClip clip) {
		allAudioSources[currentChannel].clip = clip;
	}

	// Sets the current audio source being modified
	public void SetChannel (Channel currentChannel) {
		this.currentChannel = currentChannel;
	}

	
	// The three audiosources are controller by an enum
	Dictionary<Channel, AudioSource> CreateAudioSourceDictionary () {
		Dictionary<Channel, AudioSource> allAudioSources = new Dictionary<Channel, AudioSource>();
		allAudioSources.Add(Channel.Music, Music);
		allAudioSources.Add(Channel.SFX1, SFX1);
		allAudioSources.Add(Channel.SFX2, SFX2);
		return allAudioSources;

	}

	public void SetMusic (GameState state) {
		SetChannel(Channel.Music);

		if (state == GameState.Game) {
			SetClip(GameMusic);
		}
		    
		PlayCurrentClip();

	}

	private void ToggleSlidingSound (bool active) {
		SetChannel(Channel.SFX2);
		allAudioSources[currentChannel].loop = active;

		if (active) {
			SetClip(SlideSFX);
		} else {
			SetClip(null);
			allAudioSources[currentChannel].Stop();
		}

	}

	void SubscribeEvents () {
		CharacterMover.OnCharacterMove += ToggleSlidingSound;
	}

	void UnsubscribeEvents () {
		CharacterMover.OnCharacterMove -= ToggleSlidingSound;
	}
}
