using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoreTweet.Streaming;
using System.Text.RegularExpressions;
namespace TaisyaBot {

    class Tokens {
        public string consumerkey = "*****";
        public string consumersecret = "*****"
        public string accesstoken = "*****"
        public string accesstokensecret = "*****"
    }
    class Go_tweet : Tokens {
        public void tweet(int iResult_go) {
            var tokens = CoreTweet.Tokens.Create(consumerkey, consumersecret, accesstoken, accesstokensecret);
            if (iResult_go >= 0 && iResult_go <= 1) {
                tokens.Statuses.Update(new { status = "颯爽出社！" });
                Console.WriteLine("颯爽出社！");
            }
            else {
                tokens.Statuses.Update(new { status = "出社！" });
                Console.WriteLine("出社！");
            }
        }
    }
    class Leave_tweet : Tokens {
        public void tweet(int iResult_leave) {
            var tokens = CoreTweet.Tokens.Create(consumerkey, consumersecret, accesstoken, accesstokensecret);
            if (iResult_leave >= 0 && iResult_leave <= 1) {
                tokens.Statuses.Update(new { status = "颯爽退社！" });
                Console.WriteLine("颯爽退社！");
            }
            else if (iResult_leave >= 2 && iResult_leave <= 7) {
                tokens.Statuses.Update(new { status = "退社！" });
                Console.WriteLine("退社！");
            }
            else {
                tokens.Statuses.Update(new { status = "残業確定！！！" });
                Console.WriteLine("残業確定！！！");
            }
        }
    }
    class Reply_tweet : Tokens {
        public void tweet() {
            var tokens = CoreTweet.Tokens.Create(consumerkey, consumersecret, accesstoken, accesstokensecret);
            var stream = tokens.Streaming.StartStream(StreamingType.User, new StreamingParameters(replies => "all"));
            foreach (var message in stream) {
                if (message is StatusMessage) {
                    var status = (message as StatusMessage).Status;

                    //デバッグ用
                    Console.WriteLine(string.Format("{0}:{1}", status.User.ScreenName, status.Text));
                    //@taikin_botへのリプライかつ指定のリプライが来た際にアドバイスをリプライする。
                    if (Regex.IsMatch(status.Text, "@taikin_bot\\s")) {
                        if (Regex.IsMatch(status.Text, ".*辞めたい.*")) {
                            var rep1 = "@" + status.User.ScreenName + " 早く辞めよう";
                            tokens.Statuses.Update(new { status = rep1, in_reply_to_status_id = status.Id });
                        } else if (Regex.IsMatch(status.Text, ".*死にたい.*")) {
                            var rep2 = "@" + status.User.ScreenName + " 生きねば笑";
                            tokens.Statuses.Update(new { status = rep2, in_reply_to_status_id = status.Id });
                        } else if (Regex.IsMatch(status.Text, ".*帰りたい.*")) {
                            var rep3 = "@" + status.User.ScreenName + " 帰ればいいと思うよ。";
                            tokens.Statuses.Update(new { status = rep3, in_reply_to_status_id = status.Id });
                        } else if (Regex.IsMatch(status.Text, ".*転職したい.*")) {
                            var rep4 = "@" + status.User.ScreenName + " とりあえず今の仕事を辞めてから考えよう！";
                            tokens.Statuses.Update(new { status = rep4, in_reply_to_status_id = status.Id });
                        } else {
                            Console.WriteLine("がんばるぞい");
                        }
                    }
                }
                else if (message is EventMessage) {
                    var ev = message as EventMessage;
                    Console.WriteLine(string.Format("{0}:{1}->{2}",
                    ev.Event, ev.Source.ScreenName, ev.Target.ScreenName));
                }
            }
        }
    }
    class BotMain {
        static void Main(string[] args) {

            //Randomクラスのインスタンス生成
            Random cRandom = new Random();

            //出社時用・退社時ツイート用のランダム値
            int iResult_go = cRandom.Next(9);
            int iResult_leave = cRandom.Next(9);
            //出社退社ツイート
            try {
                if (args[0] == "go") {
                    Go_tweet go = new Go_tweet();
                    go.tweet(iResult_go);
                } else if (args[0] == "leave") {
                    Leave_tweet leave = new Leave_tweet();
                    leave.tweet(iResult_leave);
                } else if (args[0] == "reply") {
                    Reply_tweet reply = new Reply_tweet();
                    reply.tweet();
                }
                else {
                    Console.WriteLine("no tweet");
                }
            } catch (CoreTweet.TwitterException) {
                Console.WriteLine("TwitterAPI制限によりツイートできませんでした。");
            }
        }
    }
}
