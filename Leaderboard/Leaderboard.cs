using System.Collections;
using System.Collections.Generic;
using System.IO;
public class Leaderboard{
    public string userId;
    public string gameId;
    public float score;
    public Dictionary<string,string> ToJSON(){
        var data=new Dictionary<string,string>();
        data.Add("gameId",gameId);
        data.Add("userId",userId);
        data.Add("score",score.ToString());
        return data;
    }
    public static Leaderboard Parse(Dictionary<string,string> dict){
        if(dict.ContainsKey("userId")&&dict.ContainsKey("gameId")&&dict.ContainsKey("score")){
            if(float.TryParse(dict["score"],out float s)){
                var l=new Leaderboard();
                l.userId=dict["userId"];
                l.gameId=dict["gameId"];
                l.score=s;
                return l;
            }else{
                return null;
            }
        }else{
            return null;
        }
    }
}