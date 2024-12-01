using System.Linq;
using UnityEngine;

public class FirstPersonAudio : MonoBehaviour
{
    [SerializeField] private FirstPersonMovement _character;
    [SerializeField] private GroundCheck _groundCheck;

    [Header("Step")]
    [SerializeField] private AudioSource _stepAudio;
    [SerializeField] private AudioSource _runningAudio;
    [Tooltip("Minimum velocity for moving audio to play")]
    /// <summary> "Minimum velocity for moving audio to play" </summary>
    [SerializeField] private float _velocityThreshold = .01f;
    private Vector2 _lastCharacterPosition;
    private Vector2 CurrentCharacterPosition => new(_character.transform.position.x, _character.transform.position.z);

    [Header("Landing")]
    [SerializeField] private AudioSource _landingAudio;
    [SerializeField] private AudioClip[] _landingSFX;

    [Header("Jump")]
    [SerializeField] private Jump _jump;
    [SerializeField] private AudioSource _jumpAudio;
    [SerializeField] private AudioClip[] _jumpSFX;

    [Header("Crouch")]
    [SerializeField] private Crouch _crouch;
    [SerializeField] private AudioSource _crouchStartAudio, _crouchedAudio, _crouchEndAudio;
    [SerializeField] private AudioClip[] _crouchStartSFX, _crouchEndSFX;

    private AudioSource[] MovingAudios => new AudioSource[] { _stepAudio, _runningAudio, _crouchedAudio };


    void Reset()
    {
        _character = GetComponentInParent<FirstPersonMovement>();
        _groundCheck = (transform.parent ?? transform).GetComponentInChildren<GroundCheck>();
        _stepAudio = GetOrCreateAudioSource("Step Audio");
        _runningAudio = GetOrCreateAudioSource("Running Audio");
        _landingAudio = GetOrCreateAudioSource("Landing Audio");

        _jump = GetComponentInParent<Jump>();
        if (_jump)
            _jumpAudio = GetOrCreateAudioSource("Jump Audio");

        _crouch = GetComponentInParent<Crouch>();
        if (_crouch)
        {
            _crouchStartAudio = GetOrCreateAudioSource("Crouch Start Audio");
            _crouchedAudio = GetOrCreateAudioSource("Crouched Audio");
            _crouchEndAudio = GetOrCreateAudioSource("Crouch End Audio");
        }
    }

    void OnEnable() => SubscribeToEvents();

    void OnDisable() => UnsubscribeToEvents();

    void FixedUpdate()
    {
        // Play moving audio if the character is moving and on the ground.
        float velocity = Vector3.Distance(CurrentCharacterPosition, _lastCharacterPosition);
        if (velocity >= _velocityThreshold && _groundCheck && _groundCheck.IsGrounded)
            if (_crouch && _crouch.IsCrouched)
                SetPlayingMovingAudio(_crouchedAudio);
            else if (_character.IsRunning)
                SetPlayingMovingAudio(_runningAudio);
            else
                SetPlayingMovingAudio(_stepAudio);
        else
            SetPlayingMovingAudio(null);

        // Remember lastCharacterPosition.
        _lastCharacterPosition = CurrentCharacterPosition;
    }


    /// <summary>Pause all MovingAudios and enforce play on audioToPlay.</summary>
    /// <param name="audioToPlay">Audio that should be playing.</param>
    void SetPlayingMovingAudio(AudioSource audioToPlay)
    {
        foreach (var audio in MovingAudios.Where(audio => audio != audioToPlay && audio != null))
            audio.Pause();

        // Play audioToPlay if it was not playing.
        if (audioToPlay && !audioToPlay.isPlaying)
            audioToPlay.Play();
    }

    #region Play instant-related audios.
    void PlayLandingAudio() => PlayRandomClip(_landingAudio, _landingSFX);
    void PlayJumpAudio() => PlayRandomClip(_jumpAudio, _jumpSFX);
    void PlayCrouchStartAudio() => PlayRandomClip(_crouchStartAudio, _crouchStartSFX);
    void PlayCrouchEndAudio() => PlayRandomClip(_crouchEndAudio, _crouchEndSFX);
    #endregion

    #region Subscribe/unsubscribe to events.
    void SubscribeToEvents()
    {
        _groundCheck.Grounded += PlayLandingAudio;

        if (_jump)
            _jump.Jumped += PlayJumpAudio;

        if (_crouch)
        {
            _crouch.CrouchStart += PlayCrouchStartAudio;
            _crouch.CrouchEnd += PlayCrouchEndAudio;
        }
    }

    void UnsubscribeToEvents()
    {
        _groundCheck.Grounded -= PlayLandingAudio;

        if (_jump)
            _jump.Jumped -= PlayJumpAudio;

        if (_crouch)
        {
            _crouch.CrouchStart -= PlayCrouchStartAudio;
            _crouch.CrouchEnd -= PlayCrouchEndAudio;
        }
    }
    #endregion

    #region Utility.
    /// <summary>Get an existing AudioSource from a name or create one if it was not found.</summary>
    /// <param name="name">Name of the AudioSource to search for.</param>
    /// <returns>The created AudioSource.</returns>
    AudioSource GetOrCreateAudioSource(string name)
    {
        AudioSource result = System.Array.Find(GetComponentsInChildren<AudioSource>(), a => a.name == name);
        if (result)
            return result;

        result = new GameObject(name).AddComponent<AudioSource>();
        result.spatialBlend = 1;
        result.playOnAwake = false;
        result.transform.SetParent(transform, false);
        return result;
    }

    static void PlayRandomClip(AudioSource audio, AudioClip[] clips)
    {
        if (!audio || clips.Length <= 0)
            return;

        AudioClip clip = clips[Random.Range(0, clips.Length)];
        if (clips.Length > 1)
            while (clip == audio.clip)
                clip = clips[Random.Range(0, clips.Length)];

        audio.clip = clip;
        audio.Play();
    }
    #endregion 
}
