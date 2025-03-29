/*
 | | | | ___   ___  _ __  _   _ / ___| __ _ _ __ ___   ___ 
 | |_| |/ _ \ / _ \| '_ \| | | | |  _ / _` | '_ ` _ \ / _ \
 |  _  | (_) | (_) | |_) | |_| | |_| | (_| | | | | | |  __/
 |_| |_|\___/ \___/| .__/ \__, |\____|\__,_|_| |_| |_|\___|
                   |_|    |___/                            
┌──────────────────────────────────────────────┐
│　Copyright(C) 2025 by HoopyGameStudio
│　描   述*：存储所有考核题目，从Excel中读取
│　创 建 人*：Hoopy
│　创建时间：2025-01-01 00:00:00
└──────────────────────────────────────────────┘
┌──────────────────────────────────────────────┐
│　修 改 人：
│　修改描述：
└──────────────────────────────────────────────┘
*/
using System;
using System.Collections.Generic;
using UnityEngine;

[ExcelAsset]
public class ExamineQuestion : ScriptableObject
{
    public List<ChoiceQuestion> ChoiceQuestion;
    public List<TFQuestion> TFQuestion;
}
[Serializable]
public struct ChoiceQuestion
{
    public int id;
    public string topic;
    public string answerOne;
    public string answerTwo;
    public string answerThree;
    public string answerFour;
    public string answer;
}
[Serializable]
public struct TFQuestion
{
    public int id;
    public string topic;
    public bool answer;
}
