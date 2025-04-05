using Godot;
using System.Collections.Generic;

public partial class AudioManager : Node
{
    public static AudioManager? Instance { get; private set; }

    private const int InitialAudioPlayerCount = 1;

    private Queue<AudioStreamPlayer> _audioPlayerQueue = new();
    private int _availableAudioPlayers = 0;

    public enum Bus
    {
        Master,
        SFX,
        UI
    }

    public enum Sound
    {
        UISelect
    }

    private readonly Dictionary<Sound, AudioStream> _soundToStream = new()
    {
        {Sound.UISelect, ResourceLoader.Load<AudioStream>("res://Audio/select.wav")}
    };

    private readonly Dictionary<Bus, StringName> _busStringNames = new();

    private void AddAudioPlayers(int count)
    {
        for (int i = 0; i < count; i++)
        {
            var audioPlayer = new AudioStreamPlayer();
            AddChild(audioPlayer);
            _audioPlayerQueue.Enqueue(audioPlayer);
            _availableAudioPlayers++;

            audioPlayer.Finished += () =>
            {
                _audioPlayerQueue.Enqueue(audioPlayer);
                _availableAudioPlayers++;
            };
        }
    }

    public override void _Ready()
    {
        Instance = this;
        AddAudioPlayers(InitialAudioPlayerCount);
    }

    public void Play(Sound sound, Bus bus = Bus.Master, float volumeDbOffset = 0, float pitchScale = 1)
    {
        if (_availableAudioPlayers == 0)
        {
            AddAudioPlayers(1);
        }

        var audioPlayer = _audioPlayerQueue.Dequeue();
        _availableAudioPlayers--;

        if (!_busStringNames.TryGetValue(bus, out var busName))
        {
            busName = bus.ToString();
        }

        audioPlayer.Bus = busName;
        audioPlayer.Stream = _soundToStream[sound];
        audioPlayer.VolumeDb = volumeDbOffset;
        audioPlayer.PitchScale = pitchScale;
        audioPlayer.Play();
    }
}
