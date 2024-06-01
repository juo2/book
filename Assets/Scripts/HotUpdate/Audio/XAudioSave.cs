using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class XAudioSave : MonoBehaviour
{
    public AudioClip clip1; // ��Ҫ¼�Ƶ� AudioClip ����\
    public AudioClip clip2; // ��Ҫ¼�Ƶ� AudioClip ����

    //public float loopTime = 1f; // ѭ��ʱ�䣬��λΪ��
    //public AudioSource bgm; // �������ֵ� AudioSource ���

    private AudioClip recordedAudio; // ¼�Ƶ���Ƶ����
    //private AudioClip filteredBGM; // ���˺�ı�������
    //private float currentTime; // ��ǰ¼�Ƶ�ʱ��
    //private bool isRecording; // �Ƿ�����¼��

    void Start()
    {
        //recordedAudio = AudioClip.Create("Recorded Audio", 0, 2, 44100, false); // ����¼�Ƶ���Ƶ��������
        //filteredBGM = AudioClip.Create("Filtered BGM", 0, 2, 44100, false); // �������˺�ı�����������
        StartRecording();
    }

    void Update()
    {

    }

    public void StartRecording()
    {
        recordedAudio = AudioClip.Create("Recorded Audio", clip1.samples, clip1.channels, clip1.frequency, false); // ����¼�Ƶ���Ƶ��������



        int length1 = Mathf.FloorToInt(recordedAudio.frequency * 3.5f);

        float[] data1 = new float[length1];
        clip1.GetData(data1, 0);


        int length2 = Mathf.FloorToInt(recordedAudio.frequency * 3.5f);

        int length3 = recordedAudio.frequency * 1;

        float[] data2 = new float[length2];
        clip2.GetData(data2, 0);

        recordedAudio.SetData(data1, 0);

        recordedAudio.SetData(data2, length1+ length3);

        StopRecording();
    }

    public void StopRecording()
    {

        // �� recordedAudio �е���Ƶ���ݵ���Ϊ WAV �ļ�
        string path = Path.Combine(Application.dataPath, "recorded_audio.wav");

        ClipUtility.AudioClipToWav(recordedAudio, path);

        Debug.Log("�����ַ��" + path);

        //File.WriteAllBytes(path, wavData);
    }
}
