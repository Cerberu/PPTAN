using UnityEngine;
using UnityEngine.UI;

public class PageTag : MonoBehaviour {

    //该标记自身所对应的页面索引
    public int tagPageIndex;

    //激活和未激活时的图片
    public Sprite ActivedTag;
    public Sprite UnActivedTag;

	//方法，刷新页面标记
    public void RefreshPageTag()
    {
        //根据当前所处的菜单页面，确定应该使用哪张图片
        GetComponent<Image>().sprite = (tagPageIndex == MenuController.Instance.currentMenuPage) ? ActivedTag : UnActivedTag;
    }
}
