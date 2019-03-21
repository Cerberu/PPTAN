using UnityEngine;
using UnityEngine.UI;

public class MenuCharactor : MonoBehaviour {

    //该角色所对应的角色索引
    public int playerIndex;

    //自身的Transform组件
    Transform selfTransform;

    //该角色激活时使用
    void OnEnable()
    {
        //临时的颜色变量
        Color tempColor;

        //获得自身的Transform组件
        selfTransform = transform;

        //获得自身图片的颜色变量
        tempColor = GetComponent<Image>().color;

        //如果该角色已经解锁
        if(MyClass.charactorUnlockState[playerIndex] == 1)
        {
            //颜色透明度为1
            tempColor.a = 1;

            //更新图片颜色
            GetComponent<Image>().color = tempColor;
        }

        //如果该角色未解锁
        else
        {
            //颜色透明度为0.3F
            tempColor.a = 0.3F;

            //更新图片颜色
            GetComponent<Image>().color = tempColor;
        }
    }

    // Update is called once per frame
    void Update ()
    {
        //临时比例
        float alpha;

        //临时缩放
        float tempScale;

        //获得该角色实时的X轴坐标值
        float realtimeCoordinateX = selfTransform.position.x;

        //如果x坐标[-1.25,1.25)
        if(realtimeCoordinateX >= -1.25F && realtimeCoordinateX < 1.25F)
        {
            //如果之前处于屏幕中心的玩家不是该玩家
            if (MenuController.Instance.centerPlayerIndex != playerIndex)
            {
                //该玩家成为处于屏幕中心的玩家
                MenuController.Instance.centerPlayerIndex = playerIndex;

                //角色名字的颜色
                MenuController.Instance.nameText.color = MenuController.Instance.nameColor[playerIndex];

                //如果游戏文字为汉语
                if (MyClass.localizationLanguageIndex == 0)
                {
                    //显示当前选中角色的中文名字
                    MenuController.Instance.nameText.text = MyClass.charactorNameChinese[playerIndex];
                }

                //非汉语
                else
                {
                    //显示当前选中角色的英文名字
                    MenuController.Instance.nameText.text = MyClass.charactorNameEnglish[playerIndex];
                }

                //如果该角色已经解锁
                if (MyClass.charactorUnlockState[playerIndex] == 1)
                {
                    //角色确认按钮激活
                    MenuController.Instance.charactorOkButton.SetActive(true);

                    //购买角色按钮禁用
                    MenuController.Instance.buyCharactorButton.SetActive(false);
                }

                //如果该角色未解锁
                else
                {
                    //角色确认按钮禁用
                    MenuController.Instance.charactorOkButton.SetActive(false);

                    //购买角色按钮激活
                    MenuController.Instance.buyCharactorButton.SetActive(true);

                    //更新角色价格
                    MenuController.Instance.charactorCostText.text = MyClass.charactorCost[playerIndex].ToString();
                }
            }
        }

        //如果X坐标（-2.5，0]
        if(realtimeCoordinateX > -2.5F && realtimeCoordinateX <= 0)
        {
            //计算比例
            alpha = (realtimeCoordinateX + 2.5F) / 2.5F;

            //计算缩放
            tempScale = 0.4F * (1 - alpha) + alpha;

            //更新该角色的缩放
            selfTransform.localScale = tempScale * Vector3.one;
        }

        //如果X坐标（0，2.5F)
        else if (realtimeCoordinateX > 0 && realtimeCoordinateX < 2.5F)
        {
            //计算比例
            alpha = realtimeCoordinateX / 2.5F;

            //计算缩放
            tempScale = (1 - alpha) + 0.4F * alpha;

            //更新该角色的缩放
            selfTransform.localScale = tempScale * Vector3.one;
        }

        else
        {
            //该角色的缩放维持0.4F
            selfTransform.localScale = 0.4F * Vector3.one;
        }
    }

    //方法，角色解锁
    public void CharactorUnlock()
    {
        //该角色解锁
        MyClass.charactorUnlockState[playerIndex] = 1;

        //存入玩家偏好
        PlayerPrefs.SetInt("charactorUnlockState" + playerIndex, 1);

        //获得自身图片的颜色变量
        Color tempColor = GetComponent<Image>().color;
       
        //颜色透明度为1
        tempColor.a = 1;

        //更新图片颜色
        GetComponent<Image>().color = tempColor;

        //角色确认按钮激活
        MenuController.Instance.charactorOkButton.SetActive(true);

        //购买角色按钮禁用
        MenuController.Instance.buyCharactorButton.SetActive(false);
    }
}
