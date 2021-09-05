using System.IO;
using System.Collections.Generic;

public class GameKey{
    public string gameId;
    public string gameKey;
    public static GameKey Parse(Dictionary<string,string> dict){
        if(dict.ContainsKey("gameId")&&dict.ContainsKey("gameKey")){
            GameKey gk=new GameKey();
            gk.gameId=dict["gameId"];
            gk.gameKey=dict["gameKey"];
            return gk;
        }else{
            return null;
        }
    }
}