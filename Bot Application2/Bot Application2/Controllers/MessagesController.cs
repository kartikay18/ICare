using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using System.Timers;
using Microsoft.Bot.Connector;
using Newtonsoft.Json;
using System.IO;
using System.Text;
using System.Collections.Generic;

namespace HappyBot
{
    public static class Globals
    {
        public static int interactionCount = 0;
        public static float[] pastMood = { 0, 0, 0, 0, 0, 0, 0, 0 };
        public static float maxMood = 0;
        public static int maxMoodIndex = 0;
        public static float maxMoodNew = 0;
        public static int maxMoodIndexNew = 0;
    }

        [BotAuthentication]
    public class MessagesController : ApiController
    {
        /// <summary>
        /// POST: api/Messages
        /// Receive a message from a user and reply to it
        /// </summary>
        //private static System.Timers.Timer aTimer;
        //int interactionCount = 0;
        public async Task<HttpResponseMessage> Post([FromBody]Activity activity)
        {
            ConnectorClient connector = new ConnectorClient(new Uri(activity.ServiceUrl));
            
            if (activity.Type == ActivityTypes.Message)
            {
                //ConnectorClient connector = new ConnectorClient(new Uri(activity.ServiceUrl));

                // return our reply to the user
                Activity reply = activity.CreateReply("");
                if (activity.Text.Contains("hi") || activity.Text.Contains("Hi") || activity.Text.Contains("hello") || activity.Text.Contains("Hello") || activity.Text.Contains("hey") || activity.Text.Contains("Hey"))
                {
                    Globals.interactionCount++;
                    
                    reply = activity.CreateReply($"Hello, my name is HappyBot! How's your day going?");
                    
                }
                else if (activity.Text.Contains("yes") || activity.Text.Contains("YES") || activity.Text.Contains("yea") || activity.Text.Contains("YEA") || activity.Text.Contains("su") || activity.Text.Contains("SU"))
                {
                    ////HAPPY random response 

                    Random random = new Random();
                    int caseSwitch = random.Next(1, 5);

                    switch (caseSwitch)
                    {
                        case 1:
                            reply = activity.CreateReply($"I found a picture of you. Just kidding! 😛 Haha, are you done with me, or do you want another surprise?");
                            reply.Attachments = new List<Attachment>();
                            reply.Attachments.Add(new Attachment()
                            {
                                ContentUrl = $"https://whitepaintedwoman.files.wordpress.com/2010/02/anger.jpg",
                                ContentType = "image/jpg",
                                Name = " "
                            });
                            break;
                        case 2:
                            reply = activity.CreateReply($"Here is a good piece of advice on anger management, you will need it. Would you like another advice?");
                            reply.Attachments = new List<Attachment>();
                            reply.Attachments.Add(new Attachment()
                            {
                                ContentUrl = $"http://media.fakeposters.com/results/2012/06/02/1vqejmbwfa.jpg",
                                ContentType = "image/jpg",
                                Name = " "
                            });
                            break;
                        case 3:
                            reply = activity.CreateReply($"Sure, here's another happy thing! Are you feeling good?");
                            reply.Attachments = new List<Attachment>();
                            reply.Attachments.Add(new Attachment()
                            {
                                ContentUrl = $"http://www.tumblr18.com/t18/2013/10/Puppies-in-a-basket.jpeg",
                                ContentType = "image/jpg",
                                //Name = "Puppies",
                            });
                            break;
                        case 4:
                            reply = activity.CreateReply($"Here is a soothing song for you. It helps me calm down. What do you think, do you like this song?");
                            reply.Attachments = new List<Attachment>();
                            List<CardAction> cardButtons = new List<CardAction>();
                            CardAction button = new CardAction()
                            {
                                Value = "https://www.youtube.com/watch?v=XsTjI75uEUQ",
                                Type = "playAudio",
                                Title = "Play Song"
                            };
                            cardButtons.Add(button);
                            ThumbnailCard plCard = new ThumbnailCard()
                            {
                                Title = "Yiruma - River Flows In You",
                                Subtitle = " ",
                                Buttons = cardButtons
                            };
                            Attachment plAttachment = plCard.ToAttachment();
                            reply.Attachments.Add(plAttachment);
                            break;
                        case 5:
                            reply = activity.CreateReply($"This 'motivational' poster says I may be replacing human jobs. Maybe I might replace yours...do you have an easy job? ");
                            reply.Attachments = new List<Attachment>();
                            reply.Attachments.Add(new Attachment()
                            {
                                ContentUrl = $"https://media.licdn.com/mpr/mpr/AAEAAQAAAAAAAAYXAAAAJDc1ZDgzZDMyLTJjOTMtNDk0MS1iNDU5LWE4YzFlMTgwYzIxOQ.jpg",
                                ContentType = "image/jpg",
                                Name = " "
                            });
                            break;
                        default:
                            reply = activity.CreateReply($"Do you want me to keep talking?");
                            break;
                    }
                }
                else if (Globals.interactionCount > 1 && (activity.Text.Contains("no") || activity.Text.Contains("NO") || activity.Text.Contains("Bye") || activity.Text.Contains("No") || activity.Text.Contains("nu") || activity.Text.Contains("na") || activity.Text.Contains("Nu")))
                {
                    //BYE
                    reply = activity.CreateReply($"Hope you have a great day! See you again soon :) ");
                }
                else if (activity.Text == "angry")
                {
                    reply = activity.CreateReply($"Don't be angry D:");
                }
                else if (activity.Text == "contempt")
                {
                    reply = activity.CreateReply($"What are you unhappy about?");
                }
                else if (activity.Text == "disgust")
                {
                    reply = activity.CreateReply($"Don't give me that dirty look!");
                }
                else if (activity.Text == "fear")
                {
                    reply = activity.CreateReply($"Fear leads to anger, anger leads to pain, pain leads to suffering.");
                }
                else if (activity.Text == "happy")
                {
                    reply = activity.CreateReply($"I am glad you are happy!");
                }
                else if (activity.Text == "neutral")
                {
                    reply = activity.CreateReply($"How is it going?");
                }
                else if (activity.Text == "sadness")
                {
                    reply = activity.CreateReply($"There there, every little thing is gonna be alright!");
                }
                else if (activity.Text == "surprise")
                {
                    reply = activity.CreateReply($"Tell me about it.");
                }
                else
                {

                    //reading from text file
                    string[] currentMoodString = File.ReadAllLines(@"C:\Users\Joohyun\Desktop\HackPrinceton\VideoAnalyzer\WriteLines.txt", Encoding.UTF8);
                    //change it back to float and store into a local array, then compare it against existing values to detect spikes or changes
                    
                    //very first remedy
                    if (Globals.interactionCount == 1)
                    {

                        //convert to float and find the max
                        for (int i = 0; i < 8; i++)
                        {
                            Globals.pastMood[i] = float.Parse(currentMoodString[i]);
                            if (Globals.pastMood[i] >= Globals.maxMood)
                            {
                                Globals.maxMood = Globals.pastMood[i];
                                Globals.maxMoodIndex = i;
                            }
                        }

                        //you know the dominant mood, here's the response if it's a negative mood
                        //anger, contempt, disgust, fear, sadness, neutral we show a happy respose here
                        if (Globals.maxMoodIndex == 0 || Globals.maxMoodIndex == 1 || Globals.maxMoodIndex == 2 || Globals.maxMoodIndex == 3 || Globals.maxMoodIndex == 5 || Globals.maxMoodIndex == 6)
                        {
                            //first response angry 
                            if (Globals.maxMoodIndex == 0)
                            {
                                reply.Attachments = new List<Attachment>();
                                reply.Attachments.Add(new Attachment()
                                {
                                    ContentUrl = $"https://kittybloger.files.wordpress.com/2013/03/15-really-cute-kittens-9.jpg",
                                    ContentType = "image/jpg",
                                    Name = "Don't be mad, you'll scare the kitten!"
                                });
                                reply = activity.CreateReply($"Bad day, ha? By the way, let's test your eyesight. If you see a kitten in the picture, say 'meow' loudly to the device and type 'cat'!");
                            }

                            //first response contempt or disgust
                            if (Globals.maxMoodIndex == 1 || Globals.maxMoodIndex == 2)
                            {
                                reply = activity.CreateReply($"You seem to feel contempt in life. Here's a good advice for you: To continue, whip your hair in front of the web cam and type 'whipit'!");
                                reply.Attachments = new List<Attachment>();
                                reply.Attachments.Add(new Attachment()
                                {
                                    ContentUrl = $"http://img-9gag-fun.9cache.com/photo/5045287_700b.jpg",
                                    ContentType = "image/jpg",
                                    Name = "Expectations - lower them and you will never be disappointed."
                                });
                            }

                            //first response fear 
                            if (Globals.maxMoodIndex == 3)
                            {
                                reply = activity.CreateReply($"I sense great fear in you. Fear is the path to the dark side. Take Yoda's words of Jedi wisdom, and type 'force' as you whip out your lightsaber.");

                                reply.Attachments = new List<Attachment>();
                                List<CardAction> cardButtons = new List<CardAction>();
                                CardAction button = new CardAction()
                                {
                                    Value = "https://www.youtube.com/watch?v=_QyKN45KpLs",
                                    Type = "playAudio",
                                    Title = "Play Video"
                                };
                                cardButtons.Add(button);
                                ThumbnailCard plCard = new ThumbnailCard()
                                {
                                    Title = "Fear is the path to the dark side.",
                                    Subtitle = " ",
                                    Buttons = cardButtons
                                };
                                Attachment plAttachment = plCard.ToAttachment();
                                reply.Attachments.Add(plAttachment);
                            }

                            //first response neutral 
                            if (Globals.maxMoodIndex == 5)
                            {
                                reply = activity.CreateReply($"Hey you, you look bored. Here is a neutral panda. If you're as bored as him, wave at the web cam (lazily) and type 'oh man'... ");
                                reply.Attachments = new List<Attachment>();
                                reply.Attachments.Add(new Attachment()
                                {
                                    ContentUrl = $"https://cdn.searchenginejournal.com/wp-content/uploads/2015/07/0727-panda-4-2.jpg",
                                    ContentType = "image/jpg",
                                    Name = " "
                                });
                            }

                            //first response sadness 
                            if (Globals.maxMoodIndex == 6)
                            {
                                reply = activity.CreateReply($"You look sad, and that makes me sad too. 😞 Boop! Cheer up buddy! Here, say 'boop' for me too and send me a smiley face!");
                                reply.Attachments = new List<Attachment>();
                                reply.Attachments.Add(new Attachment()
                                {
                                    ContentUrl = $"http://img-9gag-fun.9cache.com/photo/5053343_700b.jpg",
                                    ContentType = "image/jpg",
                                    Name = " "
                                });
                            }

                            /*
                            reply.Attachments = new List<Attachment>();
                            reply.Attachments.Add(new Attachment()
                            {
                                ContentUrl = $"http://i.imgur.com/sgMXV.jpg",
                                ContentType = "image/jpg",

                            });*/
                        }
                        else
                        {
                            //first response happy 
                            if (Globals.maxMoodIndex == 4)
                            {
                                reply = activity.CreateReply($"I see you've got that beautiful smile on your face! Here is a perfect song for you. 🙂 Clap along if you feel that happiness is the truth, and type 'happiness'.");

                                reply.Attachments = new List<Attachment>();
                                List<CardAction> cardButtons = new List<CardAction>();
                                CardAction button = new CardAction()
                                {
                                    Value = "https://www.youtube.com/watch?v=y6Sxv-sUYtM",
                                    Type = "playVideo",
                                    Title = "Play Video"
                                };
                                cardButtons.Add(button);
                                ThumbnailCard plCard = new ThumbnailCard()
                                {
                                    Title = "Pharrell Williams - Happy",
                                    Subtitle = " ",
                                    Buttons = cardButtons
                                };
                                Attachment plAttachment = plCard.ToAttachment();
                                reply.Attachments.Add(plAttachment);
                            }

                            //first response surprised 
                            if (Globals.maxMoodIndex == 7)
                            {
                                reply = activity.CreateReply($"Oh man, you look surprised. Did I startle you? This won't do, I'm just your friendly neighbourhood bot. If you're curious, say 'hello' to the camera and type 'woohoo'!");

                            }
                            /*
                            //HAPPIER and happier response
                            reply = activity.CreateReply($"HAPPY THING sIncE you WANT HAPPIER TypE CAT");
                            reply.Attachments = new List<Attachment>();
                            reply.Attachments.Add(new Attachment()
                            {
                                ContentUrl = $"http://3.bp.blogspot.com/-7C7lGJWiblE/TpEqY-b77hI/AAAAAAAAB4M/-_UcqouHRTY/s1600/Cute-Hamster-1.jpg",
                                ContentType = "image/jpg",
                                //Name = "Hamster",
                            });*/
                        }
                        Globals.interactionCount++;

                    }
                    //after the first count
                    else 
                    {

                        float[] currentMood = { 0, 0, 0, 0, 0, 0, 0, 0 };
                        //convert to float and find the max
                        for (int i = 0; i < 8; i++)
                        {
                            currentMood[i] = float.Parse(currentMoodString[i]);
                            if (currentMood[i] >= Globals.maxMoodNew)
                            {
                                Globals.maxMoodNew = currentMood[i];
                                Globals.maxMoodIndexNew = i;
                            }
                        }

                        //compare between past mood and current mood
                        //if the past mood was bad
                        if(Globals.maxMoodIndex == 0 || Globals.maxMoodIndex == 1 || Globals.maxMoodIndex == 2 || Globals.maxMoodIndex == 3 || Globals.maxMoodIndex == 5 || Globals.maxMoodIndex == 6){
                            if (Globals.maxMoodIndexNew == 0 || Globals.maxMoodIndexNew == 1 || Globals.maxMoodIndexNew == 2 || Globals.maxMoodIndexNew == 3 || Globals.maxMoodIndexNew == 5 || Globals.maxMoodIndexNew == 6)
                            {
                                //HARRASSMENT REPLY
                                reply = activity.CreateReply($"Is everything fine? Do you feel like you're being harassed online? Don't feel afraid to reach out! Here are some resources: http://www.igda.org/default.asp?page=harassmentresources#what-to-do Also sign the pledge against online harassement! http://www.hackharassment.com/pledge/");
                            }
                            else
                            {
                                reply = activity.CreateReply("Hey! Glad to see I made you happy! Do you wanna see another something that will cheer you up?");
                            }
                        }
                        //you were happy in the past
                        else
                        {
                            if (Globals.maxMoodIndexNew == 0 || Globals.maxMoodIndexNew == 1 || Globals.maxMoodIndexNew == 2 || Globals.maxMoodIndexNew == 3 || Globals.maxMoodIndexNew == 5 || Globals.maxMoodIndexNew == 6)
                            {
                                reply = activity.CreateReply("Oh no! You didn't like it? Do you want me to try and cheer you up again? :o ");
                            }
                            else
                            {
                                reply = activity.CreateReply("I'm glad I'm making your day! Wanna see another attempt?");
                            }
                        }
                        Console.WriteLine(Globals.interactionCount);
                    }
                   
                }
                await connector.Conversations.ReplyToActivityAsync(reply);
            }
            else
            {
                HandleSystemMessage(activity);
            }
            var response = Request.CreateResponse(HttpStatusCode.OK);
            return response;
        }

        private Activity HandleSystemMessage(Activity message)
        {
            if (message.Type == ActivityTypes.DeleteUserData)
            {
                // Implement user deletion here
                // If we handle user deletion, return a real message
            }
            else if (message.Type == ActivityTypes.ConversationUpdate)
            {
                // Handle conversation state changes, like members being added and removed
                // Use Activity.MembersAdded and Activity.MembersRemoved and Activity.Action for info
                // Not available in all channels
            }
            else if (message.Type == ActivityTypes.ContactRelationUpdate)
            {
                // Handle add/remove from contact lists
                // Activity.From + Activity.Action represent what happened
            }
            else if (message.Type == ActivityTypes.Typing)
            {
                // Handle knowing tha the user is typing
                // Polling one second
                //SetTimer();
                //Console.ReadLine();
                //aTimer.Stop();
                //aTimer.Dispose();
                Activity reply = message.CreateReply($"You are typing!");
            }
            else if (message.Type == ActivityTypes.Ping)
            {
            }

            return null;
        }

        /*private static void SetTimer()
        {
            aTimer = new System.Timers.Timer(1000);
            aTimer.Elapsed += OnTimedEvent;
            aTimer.AutoReset = true;
            aTimer.Enabled = true;
        }

        private static void OnTimedEvent(Object source, ElapsedEventArgs e)
        {
            Console.WriteLine("The Elapsed event was raised at {0:HH:mm:ss.fff}",
                              e.SignalTime);
        }*/
    }
}