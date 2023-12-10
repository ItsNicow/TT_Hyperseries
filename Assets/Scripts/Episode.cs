using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class Episode : MonoBehaviour
{
    public string episodeName;
    public int views;

    public Texture thumbnail;
    public VideoClip clip;

    void Start()
    {
        gameObject.name = episodeName;
        GetComponentInChildren<RawImage>().texture = thumbnail;
    }

    public void Select()
    {
        if (VideoPlayerManager.instance.currentEpisode != this)
        {
            VideoPlayerManager.instance.currentEpisode?.Unselect();

            VideoPlayerManager.instance.currentEpisode = this;
            VideoPlayerManager.instance.ResetPlayerInfo(this);

            Color thumbnailColor = GetComponentInChildren<RawImage>().color;
            thumbnailColor.a = 0.2f;

            GetComponentInChildren<RawImage>().color = thumbnailColor;
        }
    }

    public void Unselect()
    {
        Color thumbnailColor = GetComponentInChildren<RawImage>().color;
        thumbnailColor.a = 1;

        GetComponentInChildren<RawImage>().color = thumbnailColor;
    }

    public void Delete()
    {
        EpisodesListManager.instance.episodes.Remove(this);
        Destroy(gameObject);
    }
}
