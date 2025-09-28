using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NotificationManager : MonoBehaviour
{
    public static NotificationManager Instance
    {
        get
        {
            if(instance != null)
            {
                return instance;
            }

            instance = FindObjectOfType<NotificationManager>();

            if(instance != null)
            {
                return instance;
            }

            CreateNewInstance();
            return instance;
        }
    }

    private static NotificationManager CreateNewInstance()
    {
        NotificationManager notificationManagerPrefab = Resources.Load<NotificationManager>("UI");
        instance = Instantiate(notificationManagerPrefab);
        return instance;
    }

    private static NotificationManager instance;

    private void Awake()
    {
        Debug.Log("NotificationManager: I'm awake");
        if(Instance != this)
        {
            Destroy(gameObject);
        }
    }

    [SerializeField] private TextMeshProUGUI notificationText;
    [SerializeField] private float fadeTime;

    private IEnumerator notificationCoroutine;

    public void SetNewNotification(string message)
    {
        Debug.Log("NotificationManager: Setting notification to \"" + message + "\"");
        if(notificationCoroutine != null)
        {
            StopCoroutine(notificationCoroutine);
        }
        notificationCoroutine = FadeOutNotification(message);
        StartCoroutine(notificationCoroutine);
    }

    private IEnumerator FadeOutNotification(string message)
    {
        Debug.Log("NotificationManager: Fading out notification");
        notificationText.text = message;
        float time = 0f;
        while(time < fadeTime)
        {
            time += Time.unscaledDeltaTime;
            notificationText.color = new Color(notificationText.color.r, notificationText.color.g, notificationText.color.b, Mathf.Lerp(1f, 0f, time / fadeTime));
            yield return null;
        }
    }
}