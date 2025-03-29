using System;
using System.Collections.Generic;
using UnityEngine;

[ExcelAsset]
public class GitHubPackagesList : ScriptableObject
{
	public List<Infomation> PackageList; 
	public List<Infomation> Documents;
	public List<Infomation> Samples;
}
[Serializable]
public class Infomation
{
	public string name;
	public string infomation;
	public string URL;
}
