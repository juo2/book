using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class ClipUtility
{
    public static AudioClip FilterAudioClip(AudioClip bgm, float loopTime, AudioClip[] clips)
    {
        int sampleRate = bgm.frequency;
        int channelCount = bgm.channels;
        int frameCount = Mathf.RoundToInt(bgm.length * sampleRate);

        float[] bgmData = new float[frameCount * channelCount];
        bgm.GetData(bgmData, 0);

        int totalFrameCount = Mathf.RoundToInt(loopTime * sampleRate);
        float[] resultData = new float[totalFrameCount * channelCount];

        // ���Ʊ������ֵ���Ƶ���ݵ� resultData ��
        for (int i = 0; i < frameCount * channelCount; i++)
        {
            resultData[i] = bgmData[i];
        }

        // ������Ҫ¼�Ƶ� n �� AudioClip���������ǵ���Ƶ�����ۼӵ� resultData ��
        int offset = frameCount;
        for (int i = 0; i < clips.Length; i++)
        {
            AudioClip clip = clips[i];
            int clipFrameCount = Mathf.RoundToInt(clip.length * sampleRate);
            float[] clipData = new float[clipFrameCount * channelCount];
            clip.GetData(clipData, 0);

            for (int j = 0; j < clipFrameCount * channelCount; j++)
            {
                resultData[offset + j] += clipData[j];
            }

            offset += clipFrameCount;
        }

        // ����һ���µ� AudioClip������¼�Ƶ���Ƶ���ݸ��Ƶ�������Ƶ������
        AudioClip resultClip = AudioClip.Create("Recorded Audio", totalFrameCount, channelCount, sampleRate, false);
        resultClip.SetData(resultData, 0);

        return resultClip;
    }

    public static void AudioClipToWav(AudioClip clip, string filePath)
    {
        // ͨ�� GetOutputData ��ȡ��Ƶ����
        float[] samples = new float[clip.samples];
        clip.GetData(samples, 0);

        // ����Ƶ����תΪ 16 λ PCM ��ʽ
        Int16[] intData = new Int16[samples.Length];
        const float rescaleFactor = 32767;
        for (int i = 0; i < samples.Length; i++)
        {
            intData[i] = (short)(samples[i] * rescaleFactor);
        }

        // ������д�� WAV �ļ�
        byte[] byteData = new byte[intData.Length * 2];
        Buffer.BlockCopy(intData, 0, byteData, 0, byteData.Length);
        using (FileStream fileStream = new FileStream(filePath, FileMode.Create))
        {
            using (BinaryWriter writer = new BinaryWriter(fileStream))
            {
                // д�� RIFF ��ʶ�����ļ���С
                writer.Write(new char[] { 'R', 'I', 'F', 'F' });
                writer.Write(36 + byteData.Length);

                // д���ʽ�͸�ʽ���С
                writer.Write(new char[] { 'W', 'A', 'V', 'E' });
                writer.Write(new char[] { 'f', 'm', 't', ' ' });
                writer.Write(16);

                // д���ʽ��Ϣ
                writer.Write((ushort)1);
                writer.Write((ushort)clip.channels);
                writer.Write(clip.frequency);
                writer.Write(clip.frequency * clip.channels * 2);
                writer.Write((ushort)(clip.channels * 2));
                writer.Write((ushort)16);

                // д�����ݿ��ʶ�����ļ���С
                writer.Write(new char[] { 'd', 'a', 't', 'a' });
                writer.Write(byteData.Length);

                // д����Ƶ����
                writer.Write(byteData);
            }
        }
    }
}