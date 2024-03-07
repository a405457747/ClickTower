
using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace xmaolol.com
{
    public class MyRealMachineDebugLog : Singleton<MyRealMachineDebugLog>
    {
        Text DebugLogText;
        string tempTextName = "DebugLogText";

        MyRealMachineDebugLog()
        {
            DebugLogText = GameObject.Find(tempTextName).GetComponent<Text>();
        }

        public void Log(string printString)
        {
            DebugLogText.text = printString;
        }
    }
}
