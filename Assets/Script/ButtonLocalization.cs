using UnityEngine;
using UnityEngine.UI;

public class ButtonLocalization : MonoBehaviour {

    //按钮正常状态下图片的名字
    public string normalSpriteName;

    //按钮按下状态下图片的名字
    public string pressedSpriteName;

    //自身的Image组件
    Image selfImage;

    //临时的图片状态
    SpriteState tempSpriteState;

	// Use this for initialization
	void Start () {

        //获得自身的Image组件
        selfImage = this.GetComponent<Image>();

        //获得自身Button组件的图片状态
        tempSpriteState = this.GetComponent<Button>().spriteState;

        //如果为中文版本
        if (MyClass.localizationLanguageIndex == 0)
        {
            //显示正常状态下的中文图片
            selfImage.sprite = Resources.Load<Sprite>("PictureChinese/" + normalSpriteName);

            //显示按下状态下的中文图片
            tempSpriteState.pressedSprite = Resources.Load<Sprite>("PictureChinese/" + pressedSpriteName);
        }

        //否则
        else
        {
            //显示正常状态下的英文图片
            selfImage.sprite = Resources.Load<Sprite>("PictureEnglish/" + normalSpriteName);

            //显示按下状态下的英文图片
            tempSpriteState.pressedSprite = Resources.Load<Sprite>("PictureEnglish/" + pressedSpriteName);
        }

        //自动调整图片大小
        selfImage.SetNativeSize();

        //更新自身Button组件的图片状态
        GetComponent<Button>().spriteState = tempSpriteState;	
	}
}
