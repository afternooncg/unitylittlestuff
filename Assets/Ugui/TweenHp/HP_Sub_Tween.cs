using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// 脚本功能：双层血条, 缓动掉血效果
/// 挂载位置：挂在Slider游戏体上比较好
/// </summary>


// 为了节省性能, 采用调用式, 而不是用update去监控血量变化
public class HP_Sub_Tween : MonoBehaviour
{
    // [实际血量]-瞬间掉血,  Slider组件里自带的那个Fill
    public RectTransform fill_rect_trans;
    // [缓动血量]-缓慢掉血,  自己复制出来的Fill_1
    public RectTransform tween_rect_trans;

    private float tween_speed = 4.0f;
    private bool tween_flag = false;
    private float last_max_x = 0;
    private float start_x = 0;     // 缓慢掉血的血条-缓动起点
    private float end_x = 0;       // 缓慢掉血的血条-缓动终点
    private float now_x = 0;
    private float tm_t = 0;


    // Start is called before the first frame update
    void Start()
    {
        // 确保[实际血量]显示在最上面(对应在Hierarchy同级的最下面)
        fill_rect_trans.SetAsLastSibling();
        // 初始的时候让[实际血量]和[缓动血量]一致
        tween_rect_trans.anchorMax = fill_rect_trans.anchorMax;
        last_max_x = fill_rect_trans.anchorMax.x;
    }


    // Update is called once per frame
    void Update()
    {
        // 启动过渡动画
        if (tween_flag)
        {
            tm_t += tween_speed * Time.deltaTime;
            Debug.Log(tm_t);
            if (tm_t >= 1)
            {
                tm_t = 1;
                tween_flag = false;   // 关闭过渡效果
                last_max_x = end_x;   // 记录缓动停止到哪里
            }
            // 采用Lerp, 暴击的时候, 会显得血掉的快
            now_x = Mathf.Lerp(start_x, end_x, tm_t);
            tween_rect_trans.anchorMax = new Vector2(now_x, fill_rect_trans.anchorMax.y);
        }
    }


    // 启动减血效果(此时Slider的value已经变化过了, [实际血量]已经变化)
    public void Start_Tween()
    {
        start_x = last_max_x;
        end_x = fill_rect_trans.anchorMax.x;
        tween_flag = true;
        tm_t = 0;
    }

}