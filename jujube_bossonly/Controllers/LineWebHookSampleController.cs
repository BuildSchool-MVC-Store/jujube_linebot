using OSLibrary.ADO.NET.Repositories;
using OSLibrary.Models;
using OSLibrary.Sevices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace jujube_bossonly.Controllers
{
    public class LineBotWebHookController : isRock.LineBot.LineWebHookControllerBase
    {
        const string channelAccessToken = "m8WL6gHe9OsEiBAfLgXwqjHVpIEGAoKobcVO17W2ZRCnWoEuMuSh+/QERpqspXNMgRrL2zo6uRMRT64N5xIH1IUHdGlx2/vxX4n/xL13wE20mcXNpi15EX3g07Qwo72cqeLaf7PoBEMTwfTv4p5oLwdB04t89/1O/w1cDnyilFU=";
        const string AdminUserId= "Ua3819bb7440344f1ed8c21ba3ee72d13";

        [Route("api/LineWebHookSample")]
        [HttpPost]
        public IHttpActionResult POST()
        {
            try
            {
                //設定ChannelAccessToken(或抓取Web.Config)
                this.ChannelAccessToken = channelAccessToken;
                //取得Line Event(範例，只取第一個)
                var LineEvent = this.ReceivedMessage.events.FirstOrDefault(); //JSON轉成物件
                isRock.LineBot.Bot bot = new isRock.LineBot.Bot("0A/dwcLNYoarvmtMAuIiXl745SBEYWOq1FZtg0feMY5e+bUIBVG5SOh8V7oUjlE8xA3etOVliIFz7NZNF1ZXZMt/2dn/MU6+p+YmrOboCtW0JkoSmiCVZebxOc3dyKljZaVV9XWfHvcBOq4wSki8OAdB04t89/1O/w1cDnyilFU=");
                //配合Line verify 
                if (LineEvent.replyToken == "00000000000000000000000000000000") return Ok();
                //回覆訊息

                string lineID = ReceivedMessage.events.FirstOrDefault().source.userId;
                var userid = bot.GetUserInfo(lineID).displayName;
                var model = new Products();
                var service = new OrdersService();
                var repository = new OrdersRepository();

                if (LineEvent.type == "message")
                {
                    if (LineEvent.message.type == "text") //收到文字
                    {
                        if (LineEvent.message.text.ToLower() == "銷售額多少")
                        {
                            //建立actions, 作為ButtonTemplate的用戶回覆行為
                            var actions = new List<isRock.LineBot.TemplateActionBase>();
                            actions.Add(new isRock.LineBot.MessageAction() { label = "年", text = "年" });
                            actions.Add(new isRock.LineBot.MessageAction() { label = "半年", text = "半年" });
                            actions.Add(new isRock.LineBot.MessageAction() { label = "月", text = "月" });
                            actions.Add(new isRock.LineBot.MessageAction() { label = "日", text = "日" });
                            var ButtonTempalteMsg = new isRock.LineBot.ButtonsTemplate()
                            {
                                title = "請選擇",
                                text = "以下選項",
                                altText = "請在手機上檢視",
                                thumbnailImageUrl = new Uri("https://i.imgur.com/waYRM42.jpg"),
                                actions = actions
                            };
                            this.ReplyMessage(LineEvent.replyToken, ButtonTempalteMsg);
                        }
                        if (LineEvent.message.text.ToLower() == "年")
                        {
                            DateTime from = new DateTime(2018, 1, 1);
                            DateTime to = new DateTime(2018, 12, 31);
                            var year = repository.GetByOrder_Date(from, to);
                            this.ReplyMessage(LineEvent.replyToken, year.Sum(x => x.Total).ToString().ToLower() + "元");
                        }
                        if (LineEvent.message.text.ToLower() == "半年")
                        {
                            DateTime from = new DateTime(2018, 1, 1);
                            DateTime to = new DateTime(2018, 6, 30);
                            var year = repository.GetByOrder_Date(from, to);
                            this.ReplyMessage(LineEvent.replyToken, year.Sum(x => x.Total) + "元");
                        }
                        if (LineEvent.message.text.ToLower() == "月")
                        {
                            DateTime from = new DateTime(2018, 6, 1);
                            DateTime to = new DateTime(2018, 6, 30);
                            var year = repository.GetByOrder_Date(from, to);
                            this.ReplyMessage(LineEvent.replyToken, year.Sum(x => x.Total).ToString() + "元");
                        }
                        if (LineEvent.message.text.ToLower() == "日")
                        {
                            DateTime from = new DateTime(2018, 6, 20, 0, 0, 0);
                            DateTime to = new DateTime(2018, 6, 20, 23, 59, 59);
                            var year = repository.GetByOrder_Date(from, to);
                            this.ReplyMessage(LineEvent.replyToken, year.Sum(x => x.Total).ToString() + "元");
                        }
                        if (LineEvent.message.text.ToLower() == "商品")
                        {
                            //建立actions, 作為ConfirmTemplate的用戶回覆行為
                            var actions = new List<isRock.LineBot.TemplateActionBase>();
                            actions.Add(new isRock.LineBot.MessageAction() { label = "最熱銷", text = "最熱銷" });
                            actions.Add(new isRock.LineBot.MessageAction() { label = "最滯銷", text = "最滯銷" });
                            var ConfirmTemplate = new isRock.LineBot.ConfirmTemplate()
                            {
                                text = "請選擇",
                                altText = "請在手機上檢視",
                                actions = actions
                            };
                            this.ReplyMessage(LineEvent.replyToken, ConfirmTemplate);
                        }
                        if (LineEvent.message.text == "最熱銷" || LineEvent.message.text == "最滯銷")
                        {
                            var a3 = "";
                            var b = "";
                            var c = "";
                        //    var product = repository.GetProductTop9();
                            int a9;
                            if (LineEvent.message.text == "最熱銷")
                            {
                                b = "最熱銷";
                                c = "DESC";
                            }
                            else
                            {
                                b = "最滯銷";
                                c = "ASC";
                               
                            }
                            var product = repository.GetProductTop9(c);
                            if (LineEvent.message.text == "最熱銷")
                            {
                                a9 = product.Max(x => x.count);
                            }
                            else
                            {
                                a9 = product.Min(x => x.count);
                            }

                            foreach (var i in product)
                            {
                                if (i.count == a9)
                                { 
                                    a3 += i.Product_Name.ToString() + " \n";
                                }
                            }

                            this.ReplyMessage(LineEvent.replyToken, $"{b}  :  \n{a3}");
                        }
                        if (LineEvent.message.text == "庫存少於的商品有哪些")
                        {
                            //建立actions, 作為ButtonTemplate的用戶回覆行為
                            var actions = new List<isRock.LineBot.TemplateActionBase>();
                            actions.Add(new isRock.LineBot.MessageAction() { label = "5項", text = "5" });
                            actions.Add(new isRock.LineBot.MessageAction() { label = "10項", text = "10" });
                            actions.Add(new isRock.LineBot.MessageAction() { label = "15項", text = "15" });
                            var ButtonTempalteMsg = new isRock.LineBot.ButtonsTemplate()
                            {
                                title = "請選擇",
                                text = "以下選項",
                                altText = "請在手機上檢視",
                                thumbnailImageUrl = new Uri("https://i.imgur.com/waYRM42.jpg"),
                                actions = actions
                            };
                            this.ReplyMessage(LineEvent.replyToken, ButtonTempalteMsg);
                        }
                        //   if (LineEvent.message.text == "5" || LineEvent.message.text == "10" || LineEvent.message.text == "15")
                        int s;
                        if (int.TryParse(LineEvent.message.text, out s))
                        {
                            // var s = int.Parse(LineEvent.message.text);
                            var product = repository.GetStock(s);
                            var a1 = "";
                            foreach (var i in product)
                            {
                                a1 += $"商品編號 : {i.Product_ID}  商品名稱 :  {i.Product_Name} 尺寸 : {i.Size}  顏色 : {i.Color} 數量 : {i.Quantity}  , \n";
                            }
                            try
                            {
                                this.ReplyMessage(LineEvent.replyToken, $"商品庫存  :  \n{a1}");
                            }
                            catch
                            {
                                this.ReplyMessage(LineEvent.replyToken, $"商品庫存  :  只能在0~15內");
                            }
                        }
                    }
                    if (LineEvent.message.type == "sticker")
                    {
                        this.ReplyMessage(LineEvent.replyToken, 1, 2);
                    }
                    if (LineEvent.message.type == "location")
                    {
                        this.ReplyMessage(LineEvent.replyToken, $"你的位置在\n{LineEvent.message.latitude}, {LineEvent.message.longitude}");
                    }
                    if (LineEvent.message.type == "image")
                    {
                        //取得圖片Bytes
                        var bytes = this.GetUserUploadedContent(LineEvent.message.id);

                        var guid = Guid.NewGuid().ToString();
                        var filename = $"{guid}.png";
                        var path = System.Web.Hosting.HostingEnvironment.MapPath("~/temp/");
                        System.IO.File.WriteAllBytes(path + filename, bytes);
                        //取得base URL
                        var baseUrl = Request.RequestUri.GetLeftPart(UriPartial.Authority);
                        //組出外部可以讀取的檔名
                        var url = $"{baseUrl}/temp/{filename}";
                        this.ReplyMessage(LineEvent.replyToken, $"你的圖片位於\n {url}");
                    }
                }
                if (LineEvent.type == "postback")
                {
                    var data = LineEvent.postback.data;
                    var date = LineEvent.postback.Params.date;
                    this.ReplyMessage(LineEvent.replyToken, $"你的postback資料為:{data}\n選擇結果:{date}");
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
