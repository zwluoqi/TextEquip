using System.Collections;
using System.Collections.Generic;
using System.Text;
using Script.Game.Grow;
using TMPro;
using UnityEngine;
using XZXD.UI;

public class StoryPage : UIPage
{
    public RectTransform contentRect;
    public TMP_Text content;
    public TMP_Text title;
    
    public float duration = 0.3f;
    private float timer = 0;
    private string liveInfo = "";
    private int curLength = 0;
    private bool writing = false;


    protected override void DoOpen()
    {
        content.text = "";
        FreshUI();
    }

    void FreshUI()
    {
        sb.Remove(0, sb.Length);
        int story_id = this.GetIntOptionValue("story_id");
        GrowFun.Instance.growData.storyTips[story_id] = true;
        var model = DictDataManager.Instance.dictSystemStoryTip.GetModel(story_id.ToString());
        sb.Append(model.content);
        StartWriteContent();
        title.text = model.title;
    }

    protected override void DoClose ()
    {
        writing = false;
        content.text = "";

    }
//	生于南山南纪元207年
    //	长于太上老君支架
    //	208年前往太虚们修道
    //	209年晋升太虚们首席大弟子
    //	210年境界突破至金丹期
    //	211年获取绝世神兵五道口
    //	212年结识玄虚
    //	213年结识奕剑
    //	211年结识红颜五道口
    //	至此，修道已205载
    //	此生你结识26名道友，与13名道友相见恨晚，与3名道友亲密无间，与一名道友肝胆相照，你与25名红颜结缘。
    //	你的兵器库拥有神兵13件。
    //	了无遗憾，你决定开启新的修行。。。。。。

    StringBuilder sb = new StringBuilder ();
    void StartWriteContent ()
    {
        liveInfo = sb.ToString ();
        timer = 0;
        curLength = 0;
        writing = true;
        contentRect.sizeDelta = new Vector2 (contentRect.sizeDelta.x, 572);
        contentRect.anchoredPosition = new Vector2 (0, 0);
    }
    
    protected override void OnCurrentPageTick (float deltaTime)
    {
        if (writing) {
            timer += deltaTime;
            if (timer > duration) {
                timer = 0;
                curLength++;
                if (liveInfo [curLength] == '<') {
                    var targetSplitPos = liveInfo.IndexOf ('/', curLength);
                    var targetPos = liveInfo.IndexOf ('>', targetSplitPos);
                    if (targetPos != -1) {
                        curLength = targetPos + 1;
                    }
                }
					
                content.text = liveInfo.Substring (0, curLength);
                if (content.preferredHeight > 572) {
                    contentRect.sizeDelta = new Vector2 (contentRect.sizeDelta.x, content.preferredHeight);
                    contentRect.anchoredPosition = new Vector2 (0, content.preferredHeight - 572);
                }
                if (curLength >= liveInfo.Length - 1) {
                    writing = false;
                }
            }
        }

    }
}
