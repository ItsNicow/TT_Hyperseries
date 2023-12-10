using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class PageSwiper : MonoBehaviour, IDragHandler, IEndDragHandler
{
    Vector3 currentViewLocation;

    public bool canSwipe = true;

    float swipeDistance;
    float swipePercentThreshold = 0.2f;
    float easeTime = 0.2f;

    bool validSwipe;

    void Start()
    {
        currentViewLocation = transform.position;
    }

    public void OnDrag(PointerEventData data)
    {
        if (canSwipe)
        {
            swipeDistance = data.pressPosition.x - data.position.x;

            if ((swipeDistance < 0 && transform.position.x < Screen.width / 2) || (swipeDistance > 0 && transform.position.x > -Screen.width / 2))
            {
                validSwipe = true;
                transform.position = currentViewLocation - new Vector3(swipeDistance, 0, 0);
            }
            else
            {
                validSwipe = false;
            }
        }
    }

    public void OnEndDrag(PointerEventData data)
    {
        if (canSwipe)
        {
            float swipePercent = (data.pressPosition.x - data.position.x) / Screen.width;

            if (validSwipe || (!validSwipe && Mathf.Abs(swipeDistance) >= Screen.width))
            {
                if (Mathf.Abs(swipePercent) >= swipePercentThreshold)
                {
                    Vector3 newLocation = currentViewLocation;

                    if (swipePercent > 0)
                    {
                        newLocation += new Vector3(-Screen.width, 0);
                    }
                    else if (swipePercent < 0)
                    {
                        newLocation += new Vector3(Screen.width, 0);
                    }

                    StartCoroutine(SmoothMove(transform.position, newLocation, easeTime));
                    currentViewLocation = newLocation;
                }
                else
                {
                    StartCoroutine(SmoothMove(transform.position, currentViewLocation, easeTime));
                }
            }
        }
    }

    public void SwipeTo(int page)
    {
        if (canSwipe)
        {
            Vector3 newLocation = currentViewLocation;
            newLocation += new Vector3((page == 0 ? -1 : 1) * Screen.width, 0);

            StartCoroutine(SmoothMove(transform.position, newLocation, easeTime));
            currentViewLocation = newLocation;
        }
    }

    IEnumerator SmoothMove(Vector3 startPosition, Vector3 endPosition, float time)
    {
        float t = 0f;

        while (t <= 1)
        {
            t += Time.deltaTime / time;

            transform.position = Vector3.Lerp(startPosition, endPosition, Mathf.SmoothStep(0, 1, t));

            yield return null;
        }
    }
}
