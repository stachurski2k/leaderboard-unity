using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net.Http;
using System.Threading.Tasks;
using System.Net;
using System.IO;
public struct LeaderboardResponse{
    public Leaderboard leaderboard;
    public int place;
    public int statusCode;
}
public struct LeaderboardResponseArray{
    public List<Leaderboard> leaderboards;
    public int statusCode;
}
public struct GameKeyResponse{
   public int statusCode;
   public GameKey gameKey;
}
public class LeaderboardManager : MonoBehaviour
{
   public static LeaderboardManager instance;
   public string serverIp;
   public string gameId;
   public string apiKey;
   public string gameKey;
   public string originalGameKey;
   HttpClient client=new HttpClient();
   public IParser parser;
   private void Awake()
   {
       if(instance==null){
           instance=this;
       }else{
           Destroy(this);
       }
   }
#region Helpers
    async Task<GameKey> ParseGameKey(HttpResponseMessage response){
            var body=await response.Content.ReadAsStringAsync();
            var obj=parser.Parse(body);
            return GameKey.Parse(obj);
   }
   async Task<Leaderboard> ParseLeaderboard(HttpResponseMessage response){
            string body=await response.Content.ReadAsStringAsync();
            var resultData=parser.Parse(body);
            return Leaderboard.Parse(resultData);
   }
   async Task<HttpResponseMessage> GetResponse(string path,Dictionary<string,string> json){
       var response=await client.PostAsync(serverIp+path,new FormUrlEncodedContent(json));
       return response;
   }
#endregion
   public async void GenerateThis(){
        var result=await GenerateKey(gameId);
        if(result.statusCode==202){
            this.gameKey=result.gameKey.gameKey;
            originalGameKey=this.gameKey;
        }else{
            print("This game was already generated!");
        }
   }
   public void GetThis(){
       this.gameKey=originalGameKey;
        // var result=await GetKey(gameId,apiKey);
        // if(result.statusCode==200){
        //     this.gameKey=result.gameKey.gameKey;
        // }else{
        //     print("Getting game key failed with "+result.statusCode);
        // }
   }
   public async Task<GameKeyResponse> GenerateKey(string gameId){
       var dict=new Dictionary<string,string>();
       dict.Add("gameId",gameId);
       dict.Add("key",apiKey);

       var response=await GetResponse("/games/generateKey",dict);
       int statusCode=(int)response.StatusCode;
       GameKey GK=new GameKey();
       if(statusCode==202){
           GK=await ParseGameKey(response);
       }
       return new GameKeyResponse(){
           statusCode=statusCode,
           gameKey=GK
       };
   }
   public async Task<GameKeyResponse> GetKey(string gameId,string apiKey){
        var dict=new Dictionary<string,string>();
       dict.Add("gameId",gameId);
       dict.Add("key",apiKey);

       var response=await GetResponse("/games/getKey",dict);
       int statusCode=(int)response.StatusCode;
       GameKey GK=new GameKey();
       if(statusCode==200){
            GK=await ParseGameKey(response);
       }
       return new GameKeyResponse(){
           statusCode=statusCode,
           gameKey=GK,
       };
   }
   public async Task<int> AddScore(Leaderboard l){
        var dict=l.ToJSON();
        dict.Add("gameKey",gameKey);
        var response=await GetResponse("/api/add",dict);
        return (int)response.StatusCode;
   }
   public async Task<LeaderboardResponse> GetById(string gameId,string userId){
        var dict=new Dictionary<string,string>();
        dict.Add("gameKey",gameKey);
        dict.Add("gameId",gameId);
        dict.Add("userId",userId);

        var response=await GetResponse("/api/getId",dict);

        int statusCode=(int)response.StatusCode;
        Leaderboard l=null;
        int place=-1;
        if(statusCode==200){
            string body=await response.Content.ReadAsStringAsync();
            var resultData=parser.Parse(body);
            l= Leaderboard.Parse(resultData);
            if(resultData.ContainsKey("place")){
                if(int.TryParse(resultData["place"],out int v)){
                    place=v;
                }
            }
        }
        return new LeaderboardResponse(){
            leaderboard=l,
            statusCode=statusCode,
            place=place,
        };
   }
   public async Task<LeaderboardResponseArray> GetTop(string gameId,int num){
        var request=new Dictionary<string,string>();
        request.Add("gameId",gameId);
        request.Add("num",num.ToString());
        request.Add("gameKey",gameKey);

        var response=await GetResponse("/api/gettop",request);

        int statusCode=(int)response.StatusCode;

        List<Leaderboard> leaderboards=new List<Leaderboard>();
        if(statusCode==200){
            string body=await response.Content.ReadAsStringAsync();
            string json=body.Replace("[","");
            json=json.Replace("]","");
            json=json.Replace("},{","}^{");
            string[] objects=json.Split('^');
            for (int i = 0; i < objects.Length; i++)
            {
                var lData=parser.Parse(objects[i]);
                var l=Leaderboard.Parse(lData);
                if(l!=null){
                    leaderboards.Add(l);
                }
            }
        }
        return new LeaderboardResponseArray(){
            statusCode=statusCode,
            leaderboards=leaderboards
        };
   }   
   public async Task<LeaderboardResponse> DeleteId(string gameId,string userId){
        var request=new Dictionary<string,string>();
        request.Add("gameId",gameId);
        request.Add("gameKey",gameKey);
        request.Add("userId",userId);
        var response=await GetResponse("/api/deleteId",request);

        int statusCode=(int)response.StatusCode;
        Leaderboard l=null;
        if(statusCode==200){
            l=await ParseLeaderboard(response);
        }
        return new LeaderboardResponse(){
            statusCode=statusCode,
            leaderboard=l
        };
   }
   public async Task<int> DeleteAll(string gamedId){
       var request=new Dictionary<string,string>();
        request.Add("gameId",gameId);
        request.Add("gameKey",gameKey);
        var response=await GetResponse("/api/deleteAll",request);
        return (int)response.StatusCode;
   }
}
