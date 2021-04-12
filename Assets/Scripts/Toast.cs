using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Toast : MonoBehaviour
{

    private Subscription<ToastRequestEvent> toast_subscription;

    // The two places the toast UI panel alternates between.
    Vector3 hidden_pos;
    Vector3 visible_pos;

    public RectTransform toast_panel;
    public TextMeshProUGUI toast_text;

    // These inspector-accessible variables control how the toast UI panel moves between the hidden and visible positions.
    public AnimationCurve ease;
    public AnimationCurve ease_out;

    // Duration controls.
    public float ease_duration = 0.5f;
    public float show_duration = 2.0f;

    bool toasting = false;

    // We don't want to discard toast requests that come in while we are already toasting. What if the message is critical?
    // The queue keeps a rolling data store of work we still need to do.
    Queue<ToastRequestEvent> requests = new Queue<ToastRequestEvent>();

    // Use this for initialization
    void Awake()
    {
        // Init positions
        hidden_pos = new Vector3(0, 125, 0);
        visible_pos = new Vector3(0, 245, 0);
        toast_subscription = EventBus.Subscribe<ToastRequestEvent>(_OnToast);

    }

    // note that it does not actually launch a toast operation-- it just throws it on the queue for later execution.
    public void _OnToast(ToastRequestEvent msg)
    {
        requests.Enqueue(msg);
    }

    // The Update function is responsible for monitoring the queue and executing requests
    void Update()
    {
        // If a request exists on the queue, and we're not busy servicing an earlier request, we service the next one on the queue.
        if (!toasting && requests.Count > 0)
        {
            ToastRequestEvent new_request = requests.Dequeue();
            toasting = true;

            toast_text.text = new_request.message;
            StartCoroutine(DoToast(ease_duration, show_duration));
        }
    }

    IEnumerator DoToast(float duration_ease_sec, float duration_show_sec)
    {
        // Ease In the UI panel
        float initial_time = Time.time;
        float progress = (Time.time - initial_time) / duration_ease_sec;

        while (progress < 1.0f)
        {
            progress = (Time.time - initial_time) / duration_ease_sec;
            float eased_progress = ease.Evaluate(progress);
            toast_panel.anchoredPosition = Vector3.LerpUnclamped(hidden_pos, visible_pos, eased_progress);

            yield return null;
        }

        // Show the UI Panel for "duration_show_sec" seconds.
        yield return new WaitForSeconds(duration_show_sec);

        // Ease Out the UI panel
        initial_time = Time.time;
        progress = 0.0f;
        while (progress < 1.0f)
        {
            progress = (Time.time - initial_time) / duration_ease_sec;
            float eased_progress = ease_out.Evaluate(progress);
            toast_panel.anchoredPosition = Vector3.LerpUnclamped(hidden_pos, visible_pos, 1.0f - eased_progress);

            yield return null;
        }

        // When we're done toasting, we tell the "Update" function that we're ready for more requests.
        toasting = false;
    }

}