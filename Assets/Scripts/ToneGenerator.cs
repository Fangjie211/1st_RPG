using UnityEngine;

public class ToneGenerator : MonoBehaviour
{
    public enum WaveType { Sine, Square, Triangle, Sawtooth }
    public WaveType waveType = WaveType.Sine; // 默认波形

    public float frequency = 440f; // 默认频率为 A4
    public float sampleRate = 44100f; // 采样率
    public float amplitude = 0.5f; // 音量
    public float fadeOutTime = 0.1f; // 淡出时间（秒）

    private AudioSource audioSource;
    private float phase = 0f;
    private bool isPlaying = false;
    private bool isFadingOut = false;
    private float fadeOutCounter = 0f;

    void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.spatialBlend = 0; // 2D 音效
        audioSource.loop = true; // 循环播放
        audioSource.Stop(); // 确保开始时没有播放
    }

    void OnAudioFilterRead(float[] data, int channels)
    {
        if (!isPlaying) return; // 如果没有播放，直接返回

        float increment = frequency * 2f * Mathf.PI / sampleRate;

        for (int i = 0; i < data.Length; i += channels)
        {
            phase += increment;

            // 根据波形生成声音
            float sample = 0f;
            switch (waveType)
            {
                case WaveType.Sine:
                    sample = Mathf.Sin(phase);
                    break;
                case WaveType.Square:
                    sample = Mathf.Sign(Mathf.Sin(phase));
                    break;
                case WaveType.Triangle:
                    sample = Mathf.PingPong(phase, 1f);
                    break;
                case WaveType.Sawtooth:
                    sample = (phase % (2f * Mathf.PI) / (2f * Mathf.PI) - 1f);
                    break;
            }

            // 应用淡出效果
            if (isFadingOut)
            {
                fadeOutCounter += (1f / sampleRate) * data.Length / channels;
                float fadeOutProgress = Mathf.Clamp01(fadeOutCounter / fadeOutTime);
                sample *= (1f - fadeOutProgress); // 逐渐降低音量

                // 淡出完成后停止播放
                if (fadeOutProgress >= 1f)
                {
                    isPlaying = false;
                    isFadingOut = false;
                    fadeOutCounter = 0f;
                    audioSource.Stop();
                }
            }

            // 应用音量
            data[i] = amplitude * sample;

            // 如果是立体声，复制到右声道
            if (channels == 2)
            {
                data[i + 1] = data[i];
            }

            // 防止相位溢出
            if (phase > 2f * Mathf.PI)
            {
                phase -= 2f * Mathf.PI;
            }
        }
    }

    public void PlayTone(float freq)
    {
        frequency = freq;
        isPlaying = true;
        isFadingOut = false; // 停止淡出
        fadeOutCounter = 0f;
        if (!audioSource.isPlaying)
        {
            audioSource.Play();
        }
    }

    public void StopTone()
    {
        if (isPlaying && !isFadingOut)
        {
            isFadingOut = true; // 开始淡出
        }
    }

    public void SetWaveType(WaveType type)
    {
        waveType = type;
    }
}