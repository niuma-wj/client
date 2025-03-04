using System;
using System.IO;
using NAudio.Lame;
using NAudio.Wave.WZT;
using UnityEngine;

/// <summary>
/// 音频剪辑转换器
/// </summary>
public static class AudioClipConverter
{
    /// <summary>
    /// AudioClip转Wav
    /// </summary>
    /// <param name="clip"></param>
    /// <returns></returns>
    public static byte[] AudioClipToWav(AudioClip clip, int position)
    {
        int size = position;
        if (size < 0 || size > clip.samples)
            size = clip.samples;
        float[] samples = new float[size];
        clip.GetData(samples, 0);

        // 通道数量
        short numChannels = (short)clip.channels;
        // 采样率
        int sampleRate = clip.frequency;
        // 1:PCM
        short audioFormat = 1;
        // 每次采样用多少比特存储
        short bitsPerSample = 16;
        // 字节率
        int byteRate = sampleRate * numChannels * bitsPerSample / 8;
        // 块对齐
        short blockAlign = (short)(numChannels * bitsPerSample / 8);
        int dataSize = samples.Length * 2;

        MemoryStream memoryStream = new MemoryStream();
        BinaryWriter writer = new BinaryWriter(memoryStream);
 
        // RIFF chunk
        writer.Write("RIFF".ToCharArray());
        writer.Write(dataSize + 36); // File size
        writer.Write("WAVE".ToCharArray());
 
        // Format chunk
        writer.Write("fmt ".ToCharArray());
        writer.Write(16); // Size
        writer.Write(audioFormat); // Compression code
        writer.Write(numChannels); // Num channels
        writer.Write(sampleRate); // Sample rate
        writer.Write(byteRate); // Byte rate
        writer.Write(blockAlign); // Block align
        writer.Write(bitsPerSample); // Bits per sample
 
        // Data chunk
        writer.Write("data".ToCharArray());
        writer.Write(dataSize);
 
        for (int i = 0; i < samples.Length; i++)
        {
            // 将采样值映射到[0, short.MaxValue]范围
            var sample = (int)(samples[i] * short.MaxValue);
            // 以小端方式存储
            writer.Write((byte)(sample & 0xff));
            writer.Write((byte)((sample >> 8) & 0xff));
        }
 
        memoryStream.Position = 0;
        byte[] wavData = memoryStream.ToArray();
 
        writer.Close();
        memoryStream.Close();
 
        return wavData;
    }
 
    /// <summary>
    /// 将AudioClip保存为wav文件
    /// </summary>
    /// <param name="clip">要保存的AudioClip</param>
    /// <param name="path">保存路径</param>
    public static void SaveWav(AudioClip clip, int position, string path)
    {
        FileStream fileStream = new FileStream(path, FileMode.Create, FileAccess.Write);
        SaveWav(clip, position, fileStream);
        fileStream.Flush();
        fileStream.Close();
    }

    public static void SaveWav(AudioClip clip, int position, Stream outStream)
    {
        byte[] bytes = AudioClipToWav(clip, position);
        outStream.Write(bytes, 0, bytes.Length);
    }

    /// <summary>
    /// 将AudioClip保存为MP3
    /// </summary>
    /// <param name="clip"></param>
    /// <param name="path"></param>
    public static void SaveMp3(AudioClip clip, int position, string path)
    {
        FileStream fileStream = new FileStream(path, FileMode.Create, FileAccess.Write);
        SaveMp3(clip, position, fileStream);
        fileStream.Flush();
        fileStream.Close();
    }

    public static void SaveMp3(AudioClip clip, int position, Stream outStream)
    {
        int size = position;
        if (size < 0 || size > clip.samples)
            size = clip.samples;
        float[] samples = new float[size * clip.channels];
        clip.GetData(samples, 0);

        // 将音频数据写入内存流中
        MemoryStream ms = new MemoryStream();
        for (int i = 0; i < samples.Length; i++)
        {
            // 将采样值映射到[0, short.MaxValue]范围
            int sample = (int)(samples[i] * short.MaxValue);
            // 以小端方式存储
            ms.WriteByte((byte)(sample & 0xff));
            ms.WriteByte((byte)((sample >> 8) & 0xff));
        }
        ms.Position = 0;

        // 每次采样用多少比特存储
        short bitsPerSample = 16;
        // 通道数量
        short numChannels = (short)clip.channels;
        // 采样率
        int sampleRate = clip.frequency;
        // 将内存流中的数据编码为MP3文件
        using (LameMP3FileWriter writer = new LameMP3FileWriter(outStream, new WaveFormat(sampleRate, bitsPerSample, numChannels), LAMEPreset.ABR_128))
        {
            ms.CopyTo(writer);
        }
        ms.Flush();
        ms.Close();
    }

    /// <summary>
    /// 将mp3文件转换成wav文件
    /// </summary>
    /// <param name="mp3File">mp3文件</param>
    /// <param name="wavFile">wav文件</param>
    public static bool ConvertMp3ToWav(string mp3File, string wavFile)
    {
        try
        {
            NAudio.Wave.Mp3FileReader reader = new NAudio.Wave.Mp3FileReader(mp3File);
            NAudio.Wave.WaveFileWriter.CreateWaveFile(wavFile, reader);
            reader.Close();
            return true;
        }
        catch (Exception ex)
        {
            Debug.LogError(ex.Message);
            return false;
        }
        
    }
}