using Godot;
using System.Collections.Generic;
using System.Diagnostics;

public partial class AudioManager : Node
{
    public static AudioManager? Instance { get; private set; }

    public bool DebugWriteAudioPlayback { get; set; } = false;

    private const int InitialAudioPlayerCount = 20;

    public enum Bus
    {
        Master = 0,
        Music = 1,
        SFX = 2,
        UI = 3
    }

    public enum Sound
    {
        UISelect
    }

    private readonly Dictionary<Sound, AudioStream> _soundToStream = new()
    {
        {Sound.UISelect, ResourceLoader.Load<AudioStream>("res://Audio/select.wav")}
    };

    private Queue<AudioStreamPlayer> _audioPlayerQueue = new();
    private int _availableAudioPlayers = 0;


    private readonly Dictionary<Bus, StringName> _busStringNames = new();

    public static int GetBusLinearEnergyPercentage(Bus bus)
        => Mathf.RoundToInt(100 * GetBusLinearEnergy(bus));
    public static float GetBusLinearEnergy(Bus bus)
        => Mathf.DbToLinear(AudioServer.GetBusVolumeDb((int)bus));

    public static void UpdateBusDbLevelFromLinear(Bus bus, int linearEnergyPercentage)
        => AudioServer.SetBusVolumeDb((int)bus, Mathf.LinearToDb(linearEnergyPercentage / 100f));
    public static void UpdateBusDbLevelFromLinear(Bus bus, float linearEnergy)
        => AudioServer.SetBusVolumeDb((int)bus, Mathf.LinearToDb(linearEnergy));

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

    public void Play(object sender, Sound sound, Bus bus = Bus.Master, float volumeDbOffset = 0, float pitchScale = 1)
    {
        if (DebugWriteAudioPlayback)
        {
            Debug.WriteLine($"AudioManager playing sound \"{sound}\", {bus} bus\n" +
                $"Sender: {sender} - {Time.GetTicksMsec()}ms\n");
        }

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
