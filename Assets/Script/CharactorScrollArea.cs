using UnityEngine;
using UnityEngine.UI;

public class CharactorScrollArea : MonoBehaviour {

    //角色组
    public GameObject charactorGroup;

    //bool值，指示是否允许滑动检测
    bool scrollDetectEnable = false;

    //bool值，指示是否允许位置调整
    bool scrollAdjustEnable = false;

    //角色组的标准x坐标
    float[] charactorGroupStandardX = new float[] { 1125, 875, 625, 375, 125, -125, -375, -625, -875, -1125 };

    //当前角色组的x坐标
    float currentCharactorGroupX;

    //角色位置调整的目标x值
    float charactorAdjustTargetX;

    //角色位置调整的速度
    float charactorAdjustSpeed = 500;

    //自身的ScrollRect组件
    ScrollRect selfScrollRect;

    //该组件激活时调用
    void OnEnable()
    {
        //获得自身的ScrollRect组件
        selfScrollRect = GetComponent<ScrollRect>();

        //初始时，不允许滑动检测
        scrollDetectEnable = false;

        //初始时，不允许位置调整
        scrollAdjustEnable = false;

        //根据当前选中的角色，确定角色组初始位置
        charactorGroup.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(charactorGroupStandardX[MyClass.selectedPlayerIndex], 0, 0);

        //初始时，处于屏幕正中间的角色即为当前选中的角色
        MenuController.Instance.centerPlayerIndex = MyClass.selectedPlayerIndex;

        //角色名字的颜色
        MenuController.Instance.nameText.color = MenuController.Instance.nameColor[MyClass.selectedPlayerIndex];

        //如果游戏文字为汉语
        if (MyClass.localizationLanguageIndex == 0)
        {
            //显示当前选中角色的中文名字
            MenuController.Instance.nameText.text = MyClass.charactorNameChinese[MyClass.selectedPlayerIndex];
        }

        //非汉语
        else
        {
            //显示当前选中角色的英文名字
            MenuController.Instance.nameText.text = MyClass.charactorNameEnglish[MyClass.selectedPlayerIndex];
        }

        //如果该角色已经解锁
        if (MyClass.charactorUnlockState[MyClass.selectedPlayerIndex] == 1)
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
            MenuController.Instance.charactorCostText.text = MyClass.charactorCost[MyClass.selectedPlayerIndex].ToString();
        }
    }

    // Update is called once per frame
    void Update()
    {
        //如果处于位置调整状态
        if (scrollAdjustEnable)
        {
            //进行调整
            currentCharactorGroupX = Mathf.MoveTowards(currentCharactorGroupX, charactorAdjustTargetX, charactorAdjustSpeed * Time.deltaTime);

            //更新角色组的位置
            charactorGroup.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(currentCharactorGroupX, 0, 0);

            //如果已经调整到目标位置
            if(currentCharactorGroupX == charactorAdjustTargetX)
            {
                //停止滑动
                selfScrollRect.StopMovement();
            }
        }
    }

    //方法，该组件开始滑动时的操作
    public void BeginDrag()
    {
        //不允许滑动检测
        scrollDetectEnable = false;

        //不允许位置调整
        scrollAdjustEnable = false;
    }

    //方法，该组件结束滑动时的操作
    public void EndDrag()
    {
        //开始滑动检测
        scrollDetectEnable = true;
    }

    //获得检测
    public void CharactorScrollDetect()
    {
        //内部计数器
        int i = 0;

        //如果允许滑动检测
        if (scrollDetectEnable)
        {
            //如果向左低速滑动
            if (selfScrollRect.velocity.x > -charactorAdjustSpeed && selfScrollRect.velocity.x < 0)
            {
                //不再允许滑动检测
                scrollDetectEnable = false;

                //记录当前角色组的x坐标
                currentCharactorGroupX = charactorGroup.GetComponent<RectTransform>().anchoredPosition3D.x;

                //遍历所有标准位置
                for (i = 0; i < charactorGroupStandardX.Length; i++)
                {
                    //寻找等于当前角色组x坐标的标准位置
                    if (charactorGroupStandardX[i] == currentCharactorGroupX)
                    {
                        //停止滑动
                        selfScrollRect.StopMovement();

                        //跳出循环
                        break;
                    }

                    //寻找第一个小于当前角色组x坐标的标准位置
                    if (charactorGroupStandardX[i] < currentCharactorGroupX)
                    {
                        //角色位置调整的目标x值
                        charactorAdjustTargetX = charactorGroupStandardX[i];

                        //进入位置调整状态
                        scrollAdjustEnable = true;

                        //跳出循环
                        break;
                    }
                }
            }

            //如果向右低速滑动
            else if (selfScrollRect.velocity.x > 0 && selfScrollRect.velocity.x < charactorAdjustSpeed)
            {
                //不再允许滑动检测
                scrollDetectEnable = false;

                //记录当前角色组的x坐标
                currentCharactorGroupX = charactorGroup.GetComponent<RectTransform>().anchoredPosition3D.x;

                //遍历所有标准位置
                for (i = (charactorGroupStandardX.Length - 1); i >= 0; i--)
                {
                    //寻找等于当前角色组x坐标的标准位置
                    if (charactorGroupStandardX[i] == currentCharactorGroupX)
                    {
                        //停止滑动
                        selfScrollRect.StopMovement();

                        //跳出循环
                        break;
                    }

                    //寻找第一个大于当前角色组x坐标的标准位置
                    if (charactorGroupStandardX[i] > currentCharactorGroupX)
                    {
                        //角色位置调整的目标x值
                        charactorAdjustTargetX = charactorGroupStandardX[i];

                        //进入位置调整状态
                        scrollAdjustEnable = true;

                        //跳出循环
                        break;
                    }
                }
            }
        }
    }
}
