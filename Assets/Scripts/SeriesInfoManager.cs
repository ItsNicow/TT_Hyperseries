using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeriesInfoManager : MonoBehaviour
{
    public RectTransform arrow;
    public RectTransform unfoldableArea;
    public RectTransform episodesListViewport;

    Vector2 originalUnfolableAreaPosition;
    Vector2 originalEpisodesListViewportPosition;

    float easeTime = 0.5f;

    public bool seriesInfoVisible = false;

    void Start()
    {
        originalUnfolableAreaPosition = unfoldableArea.rect.position;    
        originalEpisodesListViewportPosition = episodesListViewport.rect.position;
    }

    public void SeriesInfo()
    {
        float textHeight = VideoPlayerManager.instance.currentEpisodeName.gameObject.GetComponent<RectTransform>().rect.size.y;

        if (seriesInfoVisible)
        {
            //StartCoroutine(SmoothMove(unfoldableArea, new Vector2(0, unfoldableArea.rect.y - Screen.height * 5 / 48), new Vector2(0, unfoldableArea.rect.y), easeTime));
            LeanTween.moveY(unfoldableArea, originalUnfolableAreaPosition.y + (textHeight - 82.04f), easeTime).setEaseInOutExpo();
            LeanTween.moveY(episodesListViewport, originalEpisodesListViewportPosition.y, easeTime).setEaseInOutExpo();
            LeanTween.rotateZ(arrow.gameObject, 0, 0.5f).setEaseInOutExpo();
            seriesInfoVisible = false;
        }
        else
        {
            //StartCoroutine(SmoothMove(unfoldableArea, new Vector2(0, unfoldableArea.rect.y), new Vector2(0, unfoldableArea.rect.y - Screen.height * 5 / 48), easeTime));
            LeanTween.moveY(unfoldableArea, originalUnfolableAreaPosition.y - (118 + textHeight), easeTime).setEaseInOutExpo();
            LeanTween.moveY(episodesListViewport, originalEpisodesListViewportPosition.y - (118 + textHeight), easeTime).setEaseInOutExpo();
            LeanTween.rotateZ(arrow.gameObject, -90, 0.5f).setEaseInOutExpo();
            seriesInfoVisible = true;
        }
    }

    public IEnumerator AdjustSeriesInfo()
    {
        yield return new WaitForSeconds(0.001f);

        float textHeight = VideoPlayerManager.instance.currentEpisodeName.gameObject.GetComponent<RectTransform>().rect.size.y;

        LeanTween.moveY(unfoldableArea, originalUnfolableAreaPosition.y - (118 + textHeight), 0f);
        LeanTween.moveY(episodesListViewport, originalEpisodesListViewportPosition.y - (118 + textHeight), 0f);
    }
}
