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
        const string LuisAppId = "6d920831-0f35-4fc6-98da-489c9085697b";
        const string LuisAppKey = "8c5db4d4a17b4d25a99ee6502ada350f";
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
                    this.ReplyMessage(LineEvent.replyToken, $"{Userinfo.displayName} 您好,\n謝謝您加我為好友!! 我可以回覆您任何問題!!\n下方'MORE'選單提供您更便利的購物流程!!");
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
                            repmsg = $"你說了 '{LineEvent.message.text}' ，但不在我的服務範圍內喔! 期待您下次提問!!";
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
                                foreach (var item in ret.Entities)
                                {
                                    if (item.Value.FirstOrDefault().Name == "上衣")
                                        repmsg = $"這邊是我們上衣的頁面，歡迎參觀選購! http://jujube.azurewebsites.net/Products?CategoryName=TOP&Gender=MEN ，目前都是現貨供應中喲!!! 謝謝您 :)";
                                    if (item.Value.FirstOrDefault().Name == "下著")
                                        repmsg = $"這邊是我們下著的頁面，歡迎參觀選購! http://jujube.azurewebsites.net/Products?CategoryName=BOTTOM&Gender=MEN ，目前都是現貨供應中喲!!!謝謝您 :)";
                                    if (item.Value.FirstOrDefault().Name == "連身")
                                        repmsg = $"這邊是我們連身的頁面，歡迎參觀選購! http://jujube.azurewebsites.net/Products?CategoryName=JUMPSUIT&Gender=MEN ，目前都是現貨供應中喲!!!謝謝您 :)";
                                    if (item.Value.FirstOrDefault().Name == "內褲")
                                        repmsg = $"不好意思，內褲還沒開賣，敬請期待，謝謝您 :)";
                                    if (item.Value.FirstOrDefault().Name == "特價")
                                        repmsg = $"官網上顯示折扣的都是特價商品唷!!";
                                    if (item.Value.FirstOrDefault().Name == "新品")
                                        repmsg = $"我們官網顯示在最上面的商品都是新貨唷!!";
                                }
                                this.ReplyMessage(LineEvent.replyToken, repmsg);
                            }

                            if (ret.TopScoringIntent.Name == "客訴行為")
                            {
                                foreach (var item in ret.Entities)
                                {
                                    if (item.Value.FirstOrDefault().Name == "嫌棄")
                                        repmsg = $"不好意思，如果您不滿意的話，在七天鑑賞期之內都有提供退貨服務。謝謝您。";
                                    if (item.Value.FirstOrDefault().Name == "貨品延遲")
                                        repmsg = $"不好意思，讓您等那麼久才收到商品，我們會向運輸公司反應。";
                                    if (item.Value.FirstOrDefault().Name == "純客訴")
                                        repmsg = $"不好意思，要麻煩您於周一 ~ 周五 09:00 ~ 18:00 撥打客服專線 03-512-3456，或是寄 email 到 jujube2018@gmail.com，將會有專人為您服務，謝謝您。";

                                }
                                //發送
                                this.ReplyMessage(LineEvent.replyToken, repmsg);
                            }

                            if (ret.TopScoringIntent.Name == "購買問題")
                            {
                                foreach (var item in ret.Entities)
                                {
                                    if (item.Value.FirstOrDefault().Name == "訂單狀態")
                                        repmsg = $"訂單一旦成立將無法更改，如果需要修改訂單，麻煩您先取消原訂單，再重新下單。";
                                    if (item.Value.FirstOrDefault().Name == "銷貨退回")
                                    {
                                        repmsg = $"先告知您，若商品以取貨超過七天鑑賞期、已下水清洗過、受損、無吊牌等情形，則無法申請退貨。\n" +
                                                 $"\n麻煩您先寄信到jujube2018@gmail.com或於周一~周五 09:00 ~ 18:00撥打客服專線03-512-3456，將有專人為您申辦退貨核准。\n" +
                                                 $"\n退貨流程：\n" +
                                                 $"登入會員 -> 訂單查詢 -> 點選該筆訂單的退貨按鈕 -> 填寫個人帳戶資料 -> 按下確認退貨按鈕，跳出完成退貨對話框即完成退貨手續。" +
                                                 $"宅配業者將會於3~5天內前往取貨(不需要另外收取費用)。\n" +
                                                 $"\n購物金：本店退貨後的款項將直接匯入您的個人帳戶。\n" +
                                                 $"期待您下次購買，謝謝您:)";
                                    }

                                    if (item.Value.FirstOrDefault().Name == "運送時間")
                                        repmsg = $"現貨商品3~5天內會到貨；預購商品需要等7~15天不含假日喲！謝謝您。";
                                    if (item.Value.FirstOrDefault().Name == "預定購買")
                                        repmsg = $"預購商品需要等7~15天不含假日喲。";
                                    if (item.Value.FirstOrDefault().Name == "運送費用")
                                        repmsg = $"運費基本上一律以新台幣60元計算。";
                                    if (item.Value.FirstOrDefault().Name == "購買證明")
                                        repmsg = $"不好意思，本店小本經營尚未提供發票及統編服務。";

                                }
                                //發送
                                this.ReplyMessage(LineEvent.replyToken, repmsg);
                            }
                            //repmsg = $"OK，你想 '{ret.TopScoringIntent.Name}'，";
                            //if (ret.Entities.Count > 0)
                            //    repmsg += $"想要的是 '{ ret.Entities.FirstOrDefault().Value.FirstOrDefault().Value}' ";

                        }

                        this.ReplyMessage(LineEvent.replyToken, repmsg);
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
