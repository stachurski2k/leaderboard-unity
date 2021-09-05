using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class SimpleJSONParser: IParser
{
   public override Dictionary<string,string> Parse(string json){
        json=json.Replace("\"","");
        json=json.Replace("{","");
        json=json.Replace("}","");
        string[] vals=json.Split(',');
        var resultData=new Dictionary<string,string> ();
        for (int i = 0; i < vals.Length; i++)
        {
            string[] str=vals[i].Split(':');
            if(str.Length>=2){
                resultData.Add(str[0],str[1]);
            }
        }
        return resultData;
   }
}
