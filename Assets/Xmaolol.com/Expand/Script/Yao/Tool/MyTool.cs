using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

namespace xmaolol.com
{
    public static class MyTool
    {
        /// <summary>
        /// 返回十字型的ListVector2不包括中心点
        /// </summary>
        /// <returns>十字型的ListVector2</returns>
        /// <param name="centerPoint">中心点</param>
        public static List<Vector2> GetCrossTypeVector2List(Vector2 centerPoint)
        {
            List<Vector2> CrossTypeVector2List = new List<Vector2>();
            CrossTypeVector2List.Add(new Vector2(centerPoint.x + 1, centerPoint.y));
            CrossTypeVector2List.Add(new Vector2(centerPoint.x - 1, centerPoint.y));
            CrossTypeVector2List.Add(new Vector2(centerPoint.x, centerPoint.y + 1));
            CrossTypeVector2List.Add(new Vector2(centerPoint.x, centerPoint.y - 1));
            return CrossTypeVector2List;
        }

        /// <summary>
        /// Gets the fork type vector2 list.
        /// </summary>
        /// <returns>The fork type vector2 list.</returns>
        /// <param name="centerPoint">Center point.</param>
        public static List<Vector2> GetForkTypeVector2List(Vector2 centerPoint)
        {
            List<Vector2> ForkTypeVector2List = new List<Vector2>();
            ForkTypeVector2List.Add(new Vector2(centerPoint.x + 1, centerPoint.y + 1));
            ForkTypeVector2List.Add(new Vector2(centerPoint.x - 1, centerPoint.y - 1));
            ForkTypeVector2List.Add(new Vector2(centerPoint.x + 1, centerPoint.y - 1));
            ForkTypeVector2List.Add(new Vector2(centerPoint.x - 1, centerPoint.y + 1));
            return ForkTypeVector2List;
        }

        /// <summary>
        /// Gets the unix start to now time total minutes.
        /// </summary>
        /// <returns>The unix start to now time total minutes.</returns>
        public static int GetUnixStartToNowTimeTotalMinutes()
        {
            TimeSpan ts = DateTime.Now - new DateTime(1970, 1, 1, 0, 0, 0);
            int UnixStartToNowTimeTotalMinutes = Convert.ToInt32(ts.TotalMinutes);
            return UnixStartToNowTimeTotalMinutes;
        }

        /// <summary>
        /// 可以生出来吗
        /// </summary>
        /// <returns><c>true</c>, if birth was caned, <c>false</c> otherwise.</returns>
        /// <param name="birthProbability">Birth probability.</param>
        public static bool CanBirth(float birthProbability)
        {
            int random = UnityEngine.Random.Range(1, 1001);
            int birthValue = (int)(birthProbability * 1000);
            if (random <= birthValue)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 得到随机的1或者-1
        /// </summary>
        /// <returns>The random one or negative one.</returns>
        public static int GetRandomOneOrNegativeOne()
        {
            bool mybool = CanBirth(0.5f);
            if (mybool)
            {
                return 1;
            }
            else
            {
                return -1;
            }
        }

        /// <summary>
        /// Gets the number by string.
        /// </summary>
        /// <returns>The number by string.</returns>
        /// <param name="str">String.</param>
        public static int GetNumberByString(string str)
        {
            if (string.IsNullOrEmpty(str)) return -1;
            else return int.Parse(Regex.Replace(str, @"[^0-9]+", ""));
        }

        public static void ClearMemory()
        {
            GC.Collect();
            Resources.UnloadUnusedAssets();
        }

        public static void OpenLoadSceneHelper()
        {
            GameObject canvas = GameObject.Find("Canvas");
            if (canvas != null)
            {
                GameObject maskPanel = canvas.transform.Find("MaskPanel").gameObject;
                if (maskPanel.activeInHierarchy == false)
                {
                    maskPanel.SetActive(true);
                }
            }
        }

    }
}


