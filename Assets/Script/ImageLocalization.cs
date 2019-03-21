using UnityEngine;
using UnityEngine.UI;

public class ImageLocalization : MonoBehaviour {

    //图片的名字
    public string spriteName;

    //自身的Image组件
    Image selfImage;

	// Use this for initialization
	void Start () {

        //获得自身的Image组件
        selfImage = GetComponent<Image>();

        //如果为中文版本
        if (MyClass.localizationLanguageIndex == 0)
        {
            //显示正常状态下的中文图片
            selfImage.sprite = Resources.Load<Sprite>("PictureChinese/" + spriteName);
        }

        //否则
        else
        {
            //显示正常状态下的英文图片
            selfImage.sprite = Resources.Load<Sprite>("PictureEnglish/" + spriteName);
        }

        //自动调整图片大小
        selfImage.SetNativeSize();	
	}
}
