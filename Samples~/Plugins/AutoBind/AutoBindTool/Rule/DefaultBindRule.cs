/*
 | | | | ___   ___  _ __  _   _ / ___| __ _ _ __ ___   ___ 
 | |_| |/ _ \ / _ \| '_ \| | | | |  _ / _` | '_ ` _ \ / _ \
 |  _  | (_) | (_) | |_) | |_| | |_| | (_| | | | | | |  __/
 |_| |_|\___/ \___/| .__/ \__, |\____|\__,_|_| |_| |_|\___|
                   |_|    |___/                            
┌──────────────────────────────────────────────┐
│　Copyright(C) 2025 by HoopyGameStudio
│　描   述*：默认匹配规则
│　创 建 人*：Hoopy
│　创建时间：2025-01-01 00:00:00
└──────────────────────────────────────────────┘
┌──────────────────────────────────────────────┐
│　修 改 人：
│　修改描述：
└──────────────────────────────────────────────┘
*/
using System.Collections.Generic;
using UnityEngine;

namespace HoopyGame{
    public class DefaultBindRule : IBindRule
    {
        private Dictionary<string, string> prefixMap = new Dictionary<string, string>()
    {
        {"Trans","Transform" },
        {"RectT","RectTransform" },
        {"CG","CanvasGroup" },

        {"Tog","Toggle" },
        {"Btn","Button" },
        {"Img","Image" },
        {"Sld","Slider" },
        {"Dropd","TMP_Dropdown" },
        {"Txt", "TextMeshProUGUI"},
        {"IF","TMP_InputField" },
    };

        public virtual bool IsValid(Transform target, List<string> filedNames, List<string> componentTypeNames)
        {
            string[] nameArray = target.name.Split('_');
            if (nameArray.Length < 2) return false;

            string filedName = nameArray[0];

            for (int i = 1; i < nameArray.Length; i++)
            {
                string prefix = nameArray[i];
                if (prefixMap.TryGetValue(prefix, out string comName))
                {
                    filedNames.Add(filedName + "_" + prefix);
                    componentTypeNames.Add(comName);
                }
                else
                {
                    return false;
                }
            }

            return true;
        }
    }
}