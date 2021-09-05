using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net.Http;

public class Test : MonoBehaviour
{
    private void Start()
    {
        Add();
    }
    async void Add(){
        
        //creates new score if it is greater than previus one, takes userId, gameId and score
        int statusCode=await LeaderboardManager.instance.AddScore(new Leaderboard(){
            userId="user1",
            gameId=LeaderboardManager.instance.gameId,
            score=100,
        });
        print(statusCode);
        
        //get leaderboard based on gameId and userId
        LeaderboardResponse res=await LeaderboardManager.instance.GetById(LeaderboardManager.instance.gameId,"user1");
        print(res.statusCode);
        print(res.leaderboard.score);
        print(res.place);
        //gets top scores, takes gamedId and max number of scores to get
        LeaderboardResponseArray top =await LeaderboardManager.instance.GetTop(LeaderboardManager.instance.gameId,5);
        print(top.statusCode);
        print(top.leaderboards.Count);

        //deletes score, takes gameId and userId
        LeaderboardResponse r=await LeaderboardManager.instance.DeleteId(LeaderboardManager.instance.gameId,"user1");
        print(r.statusCode);
        print(r.leaderboard.score);

        //deletes all scores asociated with gameId
        //int s=await LeaderboardManager.instance.DeleteAll(LeaderboardManager.instance.gameId);
        //print(s);
    }
}
