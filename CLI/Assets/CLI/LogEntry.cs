using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MFYG.CLI
{
    public class LogEntry : MonoBehaviour //TODO: Add option to make closeable (eg. pressing an 'x' in the top right to close - infinite displayTime)
    {
        [SerializeField] private Text text;
        [SerializeField] private Image image;
        [SerializeField] private CanvasGroup canvasGroup;
        //TODO: Priority?


        #region private variables
        private float origDisplayTime;
        private float displayTimeLeft;
        private float fadeTimeLeft;
        private IEnumerator fadeRoutine;
        #endregion

        public void SetMessage(string message, MessageType messageType, float displayTime)
        {
            text.text = message;
            image.sprite = Resources.Load<Sprite>(messageType.ToString() + "Icon");

            origDisplayTime = displayTime;
        }

        public void StartDisplay()
        {
            if (fadeRoutine != null)
            {
                StopCoroutine(fadeRoutine);
            }

            fadeRoutine = DisplayAndFade();
            StartCoroutine(fadeRoutine);
        }

        private IEnumerator DisplayAndFade()
        {
            displayTimeLeft = origDisplayTime;
            fadeTimeLeft = CLIPreferences.LOG_DEFAULT_FADE_TIME;

            float appearTime = CLIPreferences.LOG_DEFAULT_APPEAR_TIME;
            while (appearTime > 0f)
            {
                appearTime -= Time.deltaTime;
                transform.localScale = new Vector2(1f, 1 - (appearTime / CLIPreferences.LOG_DEFAULT_APPEAR_TIME));
                yield return null;
            }
            transform.localScale = Vector2.one;

            while (displayTimeLeft > 0f && fadeTimeLeft > 0f)
            {
                while (displayTimeLeft > 0f)
                {
                    displayTimeLeft -= Time.deltaTime;
                    yield return null;
                }
                displayTimeLeft = 0f;

                while (fadeTimeLeft > 0f)
                {
                    float alpha = fadeTimeLeft / CLIPreferences.LOG_DEFAULT_FADE_TIME;
                    canvasGroup.alpha = alpha;

                    fadeTimeLeft -= Time.deltaTime;
                    yield return null;
                }
                fadeTimeLeft = 0f;

                yield return null;
            }

            Destroy(gameObject);
        }
    }
}
