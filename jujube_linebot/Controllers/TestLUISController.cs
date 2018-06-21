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
                isRock.LineBot.Bot bot = new isRock.LineBot.Bot(ChannelAccessToken);
                string Lineid = ReceivedMessage.events.FirstOrDefault().source.userId;
                var Userinfo = bot.GetUserInfo(Lineid);
                if (LineEvent.type == "follow")
                    this.ReplyMessage(LineEvent.replyToken, $"{Userinfo.displayName} 您好,\n謝謝您加我為好友!! 我可以回覆您任何問題!!");
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
                            if (ret.TopScoringIntent.Name == "會員問題")
                            {
                                var actions = new List<isRock.LineBot.TemplateActionBase>();
                        
                                actions.Add(new isRock.LineBot.PostbackAction()
                                { label = "註冊", data = "歡迎您到官網點選右上角的 Sign up，填完資料點選註冊就完成囉。" });
                                actions.Add(new isRock.LineBot.PostbackAction()
                                { label = "修改資料", data = "麻煩您到官網登入後，點選右上角的頭像標誌即可編輯資料。" });
                                actions.Add(new isRock.LineBot.PostbackAction()
                                { label = "FB登入", data = "不好意思，目前尚未提供FB登入服務，麻煩您到官網完成註冊手續。" });


                                var actions2 = new List<isRock.LineBot.TemplateActionBase>();
                                actions2.Add(new isRock.LineBot.PostbackAction()
                                { label = "忘記密碼", data = "非常抱歉我們目前不提供密碼提示，麻煩您再申請一個帳號，謝謝您。" });
                                actions2.Add(new isRock.LineBot.PostbackAction()
                                { label = "折扣", data = "不好意思，我們目前沒有折扣的活動。" });
                                actions2.Add(new isRock.LineBot.PostbackAction()
                                { label = "聯絡方式", data = "可以於周一 ~ 周五 09:00 ~ 18:00撥打客服專線03-512-3456或是寄 email 到 jujube2018@gmail.com，將有專人為您服務。" });                        

                                var CarouselTemplateMessage = new isRock.LineBot.CarouselTemplate();

                                var column1 = new isRock.LineBot.Column()
                                {
                                    text = "這些是常見的會員問題，希望可以解決您的問題:)",
                                    title = "會員問題",
                                    //設定圖片
                                    thumbnailImageUrl = new Uri("https://images.unsplash.com/photo-1511822487717-d127fa0aa63d?ixlib=rb-0.3.5&ixid=eyJhcHBfaWQiOjEyMDd9&s=a262d451dcc65e94722d66513b70100d&auto=format&fit=crop&w=1189&q=80"),
                                    actions = actions //設定回覆動作
                                };
                                var column2 = new isRock.LineBot.Column()
                                {
                                    text = "這些是常見的會員問題，希望可以解決您的問題:)",
                                    title = "會員問題",
                                    //設定圖片
                                    thumbnailImageUrl = new Uri("https://images.unsplash.com/photo-1511822487717-d127fa0aa63d?ixlib=rb-0.3.5&ixid=eyJhcHBfaWQiOjEyMDd9&s=a262d451dcc65e94722d66513b70100d&auto=format&fit=crop&w=1189&q=80"),
                                    actions = actions2 //設定回覆動作
                                };
                                
                                CarouselTemplateMessage.columns.Add(column1);
                                CarouselTemplateMessage.columns.Add(column2);                              
                               
                                //發送
                                this.ReplyMessage(LineEvent.replyToken, CarouselTemplateMessage);
                            }
                            
                            if (ret.TopScoringIntent.Name == "商品問題")
                            {
                                foreach(var item in ret.Entities)
                                {
                                    if(item.Value.FirstOrDefault().Value == "上衣")
                                    {
                                        repmsg = $"這邊是我們上衣的頁面，歡迎參觀選購! http://jujube.azurewebsites.net/Products?CategoryName=TOP&Gender=MEN 謝謝您 :)";
                                    }
                                    if (item.Value.FirstOrDefault().Value == "下著")
                                    {
                                        repmsg = $"這邊是我們下著的頁面，歡迎參觀選購! http://jujube.azurewebsites.net/Products?CategoryName=BOTTOM&Gender=MEN 謝謝您 :)";
                                    }
                                    if (item.Value.FirstOrDefault().Value == "連身")
                                    {
                                        repmsg = $"這邊是我們連身的頁面，歡迎參觀選購! http://jujube.azurewebsites.net/Products?CategoryName=JUMPSUIT&Gender=MEN 謝謝您 :)";
                                    }
                                    if (item.Value.FirstOrDefault().Value == "內褲")
                                    {
                                        repmsg = $"不好意思，內褲還沒開賣，敬請期待，謝謝您 :)";
                                    }
                                }
                                this.ReplyMessage(LineEvent.replyToken, repmsg);
                            }

                            if (ret.TopScoringIntent.Name == "客訴行為")
                            {
                                foreach(var item in ret.Entities)
                                {
                                    if(item.Value.FirstOrDefault().Value == "")
                                }

                                //發送
                                this.ReplyMessage(LineEvent.replyToken, CarouselTemplateMessage);
                            }
                            //repmsg = $"OK，你想 '{ret.TopScoringIntent.Name}'，";
                            //if (ret.Entities.Count > 0)
                            //    repmsg += $"想要的是 '{ ret.Entities.FirstOrDefault().Value.FirstOrDefault().Value}' ";

                        }

                        //this.ReplyMessage(LineEvent.replyToken, repmsg);
                    }
                    if (LineEvent.message.type == "sticker") //收到貼圖
                        this.ReplyMessage(LineEvent.replyToken, 1, 2);
                    
                }
                if (LineEvent.type == "postback")
                {
                    var data = LineEvent.postback.data;
                    this.ReplyMessage(LineEvent.replyToken, data);
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
