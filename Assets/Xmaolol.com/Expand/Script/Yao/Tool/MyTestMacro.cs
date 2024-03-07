using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QFramework;

namespace xmaolol.com
{
    //测试宏
    public static class MyTestMacro
    {
        public static bool IsTest = false;

        /// <summary>
        /// 是编辑器下面的测试模式吗
        /// </summary>
        /// <returns><c>true</c>, if test mode was ised, <c>false</c> otherwise.</returns>
        public static bool IsTestMode()
        {
            if (Platform.IsEditor)
            {
                return IsTest;
            }
            else
            {
                return false;
            }
        }
    }
}
