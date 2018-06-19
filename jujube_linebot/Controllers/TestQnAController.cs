using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace StudyHostExampleLinebot.Controllers
{
    public class TestQnAController : isRock.LineBot.LineWebHookControllerBase
    {
        const string channelAccessToken = "OBSqJrJv637VJ2mEAyheqT/WmiJzfb9PVG7PXeJc8C1TOWv5TjAiRt3lFmpUa9gfN+q9sj9RV1whB1hNOdj3UdPfNyHQXSx7QB3kFAQNR4UXRjahIla5rOMQPn/vigc713nWd2oSs9KMs9GaoqtQugdB04t89/1O/w1cDnyilFU=";
        const string AdminUserId = "U275c68b802e11bb599413ef87dcea051";
        const string QnAKey = "177149db-d05a-419e-a905-9fbddba9053d";
        const string Endpoint = "https://jujube2018.azurewebsites.net/qnamaker/knowledgebases/0b6fec5d-c0b2-4f9d-9081-d6f6b7543bbd/generateAnswer"; //ex.westus
        const string UnknowAnswer = "不好意思，您可以換個方式問嗎? 我不太明白您的意思...";

        [Route("api/TestQnA")]
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
                isRock.LineBot.Bot bot = new isRock.LineBot.Bot(ChannelAccessToken);
                string Lineid = ReceivedMessage.events.FirstOrDefault().source.userId;
                var Userinfo = bot.GetUserInfo(Lineid);
                if (LineEvent.type == "follow")
                    this.ReplyMessage(LineEvent.replyToken, $"{Userinfo.displayName} 您好,\n謝謝您加我為好友!! 我可以回覆您任何問題!!");
                if (LineEvent.type == "message")
                {
                    if (LineEvent.message.type == "text") //收到文字
                    {
                        //建立 MsQnAMaker Client
                        var helper = new isRock.MsQnAMaker.Client(
                            new Uri(Endpoint), QnAKey);
                        var QnAResponse = helper.GetResponse(LineEvent.message.text.Trim());
                        var ret = (from c in QnAResponse.answers
                                   orderby c.score descending
                                   select c
                                ).Take(1);

                        var responseText = UnknowAnswer;
                        if (ret.FirstOrDefault().score > 0)
                            responseText = ret.FirstOrDefault().answer;
                        //回覆
                        this.ReplyMessage(LineEvent.replyToken, responseText);
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
