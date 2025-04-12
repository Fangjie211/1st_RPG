using UnityEngine;

public class InstrumentController : MonoBehaviour
{
    public ToneGenerator toneGenerator;

    // 音符频率表 (C4 到 B4)
    private float[] frequencies = new float[]
    {
        261.63f, // C4
        293.66f, // D4
        329.63f, // E4
        349.23f, // F4
        392.00f, // G4
        440.00f, // A4
        493.88f  // B4
    };

    // 绑定的键盘按键
    private KeyCode[] keyCodes = new KeyCode[]
    {
        KeyCode.A, // C4
        KeyCode.S, // D4
        KeyCode.D, // E4
        KeyCode.F, // F4
        KeyCode.G, // G4
        KeyCode.H, // A4
        KeyCode.J  // B4
    };

    void Update()
    {
        // 监听键盘输入
        for (int i = 0; i < keyCodes.Length; i++)
        {
            if (Input.GetKeyDown(keyCodes[i]))
            {
                toneGenerator.PlayTone(frequencies[i]);
            }

            if (Input.GetKeyUp(keyCodes[i]))
            {
                toneGenerator.StopTone();
            }
        }

        // 切换波形
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            toneGenerator.SetWaveType(ToneGenerator.WaveType.Sine);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            toneGenerator.SetWaveType(ToneGenerator.WaveType.Square);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            toneGenerator.SetWaveType(ToneGenerator.WaveType.Triangle);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            toneGenerator.SetWaveType(ToneGenerator.WaveType.Sawtooth);
        }
    }
}