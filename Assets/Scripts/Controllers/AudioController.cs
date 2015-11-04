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
	public AudioClip HardestLevelWonMusic;

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

	public void StopCurrentClip () {
		allAudioSources[currentChannel].Stop();
	}

	public void ToggleLoopCurrentClip (bool active) {
		allAudioSources[currentChannel].loop = active;
	}

	public void ToggleMuteCurrentClip (bool muted) {
		allAudioSources[currentChannel].mute = muted;
	}

	public void SetClipVolume (float volume = 1.0f) {
		allAudioSources[currentChannel].volume = volume;
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
		} else if (state == GameState.Start) {
			SetClip(MainMenuMusic);
		}
		    
		PlayCurrentClip();

	}

	private void toggleMuteMusic (bool muted) {
		SetChannel(Channel.Music);
		ToggleMuteCurrentClip(muted);
	}


	private void unmuteMusic () {
		toggleMuteMusic(false);
	}

	private AudioClip currentClip () {
		return allAudioSources[currentChannel].clip;
	}

	private void toggleSlidingSound (bool active) {
		SetChannel(Channel.SFX2);
		ToggleLoopCurrentClip(active);

		if (active) {
			SetClip(SlideSFX);
			PlayCurrentClip();
		} else if (currentClip() == SlideSFX) {
			SetClip(null);
			StopCurrentClip();
		}

	}

	private void playLevelCompleteMusic (bool hardestLevel) {
		SetChannel(Channel.SFX2);
		SetClip(hardestLevel?HardestLevelWonMusic:PrisonerFoundSFX);
		ToggleLoopCurrentClip(false);
		PlayCurrentClip();
		toggleMuteMusic(true);
		Invoke("unmuteMusic", HardestLevelWonMusic.length);
	}

	private void playDestroyWallSound () {
		SetChannel(Channel.SFX1);
		SetClip(ExplosionSFX);
		SetClipVolume(0.5f);
		ToggleLoopCurrentClip(false);
		PlayCurrentClip();
	}

	private void playPageTurnSound () {
		SetClipVolume();
		SetChannel(Channel.SFX1);
		SetClip(PageTurnSFX);
		ToggleLoopCurrentClip(false);
		PlayCurrentClip();
	}

	void SubscribeEvents () {
		CharacterMover.OnCharacterMove += toggleSlidingSound;
		MazePieceController.OnDestroyWall += playDestroyWallSound;
		GameController.OnInstructionPageTurn += playPageTurnSound;
		GameController.OnMazeComplete += playLevelCompleteMusic;
	}

	void UnsubscribeEvents () {
		CharacterMover.OnCharacterMove -= toggleSlidingSound;
		MazePieceController.OnDestroyWall -= playDestroyWallSound;
		GameController.OnInstructionPageTurn -= playPageTurnSound;
		GameController.OnMazeComplete -= playLevelCompleteMusic;
	}

}
