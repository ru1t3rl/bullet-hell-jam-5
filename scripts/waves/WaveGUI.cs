using Godot;

namespace BulletHellJam5.scripts.waves;

public partial class WaveGUI : Label
{
    [Export]
    private string _prefix = "Wave";

    private void OnWaveChanged(int wave, int maxWaves)
    {
        Text = $"{_prefix} {wave}/{maxWaves}";
    }
}