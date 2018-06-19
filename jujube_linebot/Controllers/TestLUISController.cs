using OSLibrary.ADO.NET.Repositories;
using OSLibrary.Sevices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace jujube_linebot.Controllers
{
    public class TestLUISController : isRock.LineBot.LineWebHookControllerBase
    {
        const string channelAccessToken = "OBSqJrJv637VJ2mEAyheqT/WmiJzfb9PVG7PXeJc8C1TOWv5TjAiRt3lFmpUa9gfN+q9sj9RV1whB1hNOdj3UdPfNyHQXSx7QB3kFAQNR4UXRjahIla5rOMQPn/vigc713nWd2oSs9KMs9GaoqtQugdB04t89/1O/w1cDnyilFU=";
        const string AdminUserId = "U275c68b802e11bb599413ef87dcea051";
        const string LuisAppId = "827960e6-5891-4075-a5a6-5dce2763c33a";
        const string LuisAppKey = "8a5157b1c0df4c3691921bbdf2a03f81";
        const string Luisdomain = "westus"; //ex.westus

        [Route("api/TestLUIS")]
        [HttpPost]
        public IHttpActionResult POST()
        {
            try
            {
                //設定ChannelAccessToken(或抓取Web.Config)
                this.ChannelAccessToken = channelAccessToken;
                //取得Line Event(範例，只取第一個)
                var LineEvent = this.ReceivedMessage.events.FirstOrDefault();
                //配合Line verify 
                if (LineEvent.replyToken == "00000000000000000000000000000000") return Ok();
                //回覆訊息
                var stockService = new StockService();
                var stockrepository = new StockRepository();
                if (LineEvent.type == "message")
                {
                    var repmsg = "";
                    if (LineEvent.message.type == "text") //收到文字
                    {
                        //建立LuisClient
                        Microsoft.Cognitive.LUIS.LuisClient lc =
                          new Microsoft.Cognitive.LUIS.LuisClient(LuisAppId, LuisAppKey, true, Luisdomain);

                        //Call Luis API 查詢
                        var ret = lc.Predict(LineEvent.message.text).Result;
                        if (ret.Intents.Count() <= 0)
                            repmsg = $"你說了 '{LineEvent.message.text}' ，但我看不懂喔!";
                        else if (ret.TopScoringIntent.Name == "None")
                            repmsg = $"你說了 '{LineEvent.message.text}' ，但不在我的服務範圍內喔!";
                        else
                        {
                            if (ret.TopScoringIntent.Name == "商品查詢")
                            {
                                var stockdetails = stockService.GetByProductName(ret.Entities.FirstOrDefault().Value.FirstOrDefault().Value);
                                repmsg = $"您詢問的商品是{ret.Entities.FirstOrDefault().Value.FirstOrDefault().Value}，顏色有{stockdetails}";
                            }
                            repmsg = $"OK，你想 '{ret.TopScoringIntent.Name}'，";
                            if (ret.Entities.Count > 0)
                                repmsg += $"想要的是 '{ ret.Entities.FirstOrDefault().Value.FirstOrDefault().Value}' ";
                        }
                        //回覆
                        this.ReplyMessage(LineEvent.replyToken, repmsg);
                    }
                    if (LineEvent.message.type == "sticker") //收到貼圖
                        this.ReplyMessage(LineEvent.replyToken, 1, 2);
                }
                //response OK
                return Ok();
            }
            catch (Exception ex)
            {
                //如果發生錯誤，傳訊息給Admin
                this.PushMessage(AdminUserId, "發生錯誤:\n" + ex.Message);
                //response OK
                return Ok();
            }
        }
    }
}
