using UnityEngine;
using Unity.Cinemachine;
using System.Collections;

public class CameraShake : MonoBehaviour
{
    public float shakeDuration = 0.2f;
    public float shakeStrength = 2f;

    private CinemachineBasicMultiChannelPerlin noise;
    private Coroutine shakeCoroutine;

    void Awake()
    {
        noise = GetComponent<CinemachineBasicMultiChannelPerlin>();

        if (noise != null)
        {
            noise.AmplitudeGain = 0f;
        }
    }

    public void Shake()
    {
        if (noise == null)
        {
            Debug.LogWarning("Cinemachine Noise bulunamadı!");
            return;
        }

        if (shakeCoroutine != null)
        {
            StopCoroutine(shakeCoroutine);
        }

        shakeCoroutine = StartCoroutine(ShakeRoutine());
    }

    IEnumerator ShakeRoutine()
    {
        noise.AmplitudeGain = shakeStrength;

        yield return new WaitForSeconds(shakeDuration);

        noise.AmplitudeGain = 0f;

        shakeCoroutine = null;
    }
}