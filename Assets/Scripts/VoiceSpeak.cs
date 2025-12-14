using System.Collections;
using System.Diagnostics;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

public class PiperTTS : MonoBehaviour
{
    public AudioSource audioSource;

    string piperPath;
    string modelPath;
    string outputWav;

    void Awake()
    {
        piperPath = Path.Combine(Application.streamingAssetsPath, "piper.exe");
        modelPath = Path.Combine(Application.streamingAssetsPath, "en_US-arctic-medium.onnx");
        outputWav = Path.Combine(Application.persistentDataPath, "player_name.wav");

        if (!File.Exists(piperPath))
        {
            UnityEngine.Debug.LogError("Piper executable not found");

        }
        if (!File.Exists(modelPath))
        {
            UnityEngine.Debug.LogError("Voice model not found");
        }
    }

    public void SpeakName(string playerName)
    {
        StartCoroutine(GenerateAndPlay(playerName));
        UnityEngine.Debug.Log("Logged name: " + playerName);
    }

    IEnumerator GenerateAndPlay(string text)
    {
        // Delete old file if exists
        if (File.Exists(outputWav))
        {
            File.Delete(outputWav);
        }

        var process = new Process();
        process.StartInfo.FileName = piperPath;
        process.StartInfo.Arguments = $"-m \"{modelPath}\" -f \"{outputWav}\"";
        process.StartInfo.RedirectStandardInput = true;
        process.StartInfo.RedirectStandardError = true;
        process.StartInfo.UseShellExecute = false;
        process.StartInfo.CreateNoWindow = true;

        process.Start();

        process.StandardInput.WriteLine(text);
        process.StandardInput.Close();

        // Wait for process to finish
        while (!process.HasExited)
        {
            yield return null;
        }

        // Check for errors
        string err = process.StandardError.ReadToEnd();
        if (!string.IsNullOrEmpty(err))
        {
            UnityEngine.Debug.LogError("Piper error: " + err);
        }

        // Wait for the file to exist
        float timeout = 1f;
        while (!File.Exists(outputWav) && timeout > 0)
        {
            yield return null;
            timeout -= Time.deltaTime;
        }

        if (!File.Exists(outputWav))
        {
            UnityEngine.Debug.LogError("Output WAV not created");
            yield break;
        }

        // Load audio
        using var www = UnityWebRequestMultimedia.GetAudioClip("file://" + outputWav, AudioType.WAV);
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            UnityEngine.Debug.LogError("Error loading WAV: " + www.error);
            yield break;
        }

        audioSource.clip = DownloadHandlerAudioClip.GetContent(www);
        audioSource.Play();
    }
}
