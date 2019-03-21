using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HomeScrollArea : MonoBehaviour
{
    //鼠标按下时目标滑动栏的值
    float scrollBarValueDown = 0;

    //鼠标按下时的游戏时间
    float gameTimeDown = 0;

    //鼠标松开时目标滑动栏的值
    float scrollBarValueUp = 0;

    //鼠标松开时的游戏时间
    float gameTimeUp = 0;

    //鼠标拖动期间，滑动栏的值变化的速度
    float scrollBarChangeSpeed = 0;

    //鼠标松开之后，滑动栏的值调整的速度
    float scrollBarAdjustSpeed = 0;

    //bool值，指示是否处于调整状态
    bool isAdjusting;

    //调整的目标值
    float adjustTargetValue = 0;
    
    //该滑动区域对应的目标滑动栏
    public Scrollbar targetScrollBar;

    //两个页面标记
    public PageTag firstPageTag;
    public PageTag secondPageTag;

	// Use this for initialization
	void Start () {

        //初始时，显示主菜单
        targetScrollBar.value = 1.0F / 3.0F;	

        //初始时，位于第一页
        MenuController.Instance.currentMenuPage = 1;

        //更新页面标记
        firstPageTag.RefreshPageTag();
        secondPageTag.RefreshPageTag();

        //不处于调整状态
        isAdjusting = false;
	}
	
	// Update is called once per frame
	void Update () {

        if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
        {
            //记录目标滑动栏的值
            scrollBarValueDown = targetScrollBar.value;

            //记录当前游戏时间
            gameTimeDown = Time.time;
        }

        if (Input.GetMouseButtonUp(0) || Input.GetMouseButtonUp(1))
        {
            //记录目标滑动栏的值
            scrollBarValueUp = targetScrollBar.value;

            //记录当前游戏时间
            gameTimeUp = Time.time;

            //计算鼠标拖动期间滑动栏的值的变化速度
            scrollBarChangeSpeed = (scrollBarValueUp - scrollBarValueDown) / (gameTimeUp - gameTimeDown);

            //如果鼠标低速拖动
            if (Mathf.Abs(scrollBarChangeSpeed) <= 0.2F)
            {
                //调整速度为0.6
                scrollBarAdjustSpeed = 0.6F;
            }

            //如果鼠标高速拖动
            else
            {
                //调整速度为1.2
                scrollBarAdjustSpeed = 1.2F;
            }

            //如果鼠标松开时的值小于0.33333
            if (scrollBarValueUp < (1.0F / 3.0F))
            {
                //调整的目标值
                adjustTargetValue = 1.0F / 3.0F;

                //开始调整
                isAdjusting = true;               
            }

            //如果鼠标松开时的值在0.33和0.66之间
            if (scrollBarValueUp > (1.0F / 3.0F) && scrollBarValueUp < (2.0F / 3.0F))
            {
                //如果高速左移
                if (scrollBarChangeSpeed > 0.2F)
                {
                    //如果值大于0.34
                    if (scrollBarValueUp > 0.34F)
                    {
                        //调整的目标值
                        adjustTargetValue = 2.0F / 3.0F;                   
                    }
                    //否则
                    else
                    {
                        //调整的目标值
                        adjustTargetValue = 1.0F / 3.0F;
                    }
                }

                //如果高速右移
                if (scrollBarChangeSpeed < -0.2F)
                {
                    //如果值大于0.66
                    if (scrollBarValueUp > 0.66F)
                    {
                        //调整的目标值
                        adjustTargetValue = 2.0F / 3.0F;
                    }
                    //否则
                    else
                    {
                        //调整的目标值
                        adjustTargetValue = 1.0F / 3.0F;
                    }
                }

                //如果低速左移
                if (scrollBarChangeSpeed > 0 && scrollBarChangeSpeed <= 0.2F)
                {
                    //如果值大于0.36
                    if (scrollBarValueUp > 0.36F)
                    {
                        //调整的目标值
                        adjustTargetValue = 2.0F / 3.0F;
                    }
                    //否则
                    else
                    {
                        //调整的目标值
                        adjustTargetValue = 1.0F / 3.0F;
                    }
                }

                //如果低速右移
                if (scrollBarChangeSpeed < 0 && scrollBarChangeSpeed >= -0.2F)
                {
                    //如果值大于0.64
                    if (scrollBarValueUp > 0.64F)
                    {
                        //调整的目标值
                        adjustTargetValue = 2.0F / 3.0F;
                    }
                    //否则
                    else
                    {
                        //调整的目标值
                        adjustTargetValue = 1.0F / 3.0F;
                    }
                }

                //开始调整
                isAdjusting = true;
            }

            //如果鼠标松开时的值大于0.66
            if (scrollBarValueUp > (2.0F / 3.0F))
            {
                //调整的目标值
                adjustTargetValue = 2.0F / 3.0F;

                //开始调整
                isAdjusting = true;
            }
        }

        //如果处于调整过程中
        if (isAdjusting)
        {
            //进行调整
            targetScrollBar.value = Mathf.MoveTowards(targetScrollBar.value, adjustTargetValue, scrollBarAdjustSpeed * Time.deltaTime);

            //如果已经调整到目标位置
            if (targetScrollBar.value == adjustTargetValue)
            {
                //如果目标值为0.33
                if (adjustTargetValue == (1.0F / 3.0F))
                {
                    //处于第1页
                    MenuController.Instance.currentMenuPage = 1;
                }

                //如果目标值为0.66
                if (adjustTargetValue == (2.0F / 3.0F))
                {
                    //处于第2页
                    MenuController.Instance.currentMenuPage = 2;
                }
               
                isAdjusting = false;

                //更新页面标记
                firstPageTag.RefreshPageTag();
                secondPageTag.RefreshPageTag();
            }
        }
	}
}
