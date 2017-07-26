using System;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Builder.Luis.Models;
using Microsoft.Bot.Connector;
using System.Collections.Generic;

namespace Bot_Application1.Dialogs
{
    // The LuisModel attribute specifies your LUIS app ID and your LUIS subscription key
    [LuisModel("ca337683-0f82-488d-b9bd-c609e296e465", "2b4909a3ce2842d985d8a3461ff1baed")]
    [Serializable]
    public class RootDialog : LuisDialog<object>
    {

        [LuisIntent("")]
        [LuisIntent("None")]
        public async Task None(IDialogContext context, LuisResult result)
        {
            string message = $"Sorry, I did not understand '{result.Query}'.";

            await context.PostAsync(message);

            context.Wait(this.MessageReceived);
        }

        [LuisIntent("Correct Phone Number")]
        public async Task Search(IDialogContext context, IAwaitable<IMessageActivity> activity, LuisResult result)
        {
            var message = await activity;
            EntityRecommendation phoneEntityRecommendation;
            result.TryFindEntity("builtin.phonenumber", out phoneEntityRecommendation);
            string phonenumber = phoneEntityRecommendation.Entity.Trim();
            await context.PostAsync($"Welcome to the Phone number verifyier! We are analyzing your message: '{message.Text}'...");

            if (phonenumber.Length==10)
            {
                await context.SayAsync($"The phone number: '{phonenumber}' is correct", $"The phone number: '{phonenumber}' is correct");
            }
            else
            {
                await context.PostAsync($"The phone number: '{phonenumber}' is not correct");
            }
          
        }
        [LuisIntent("Learn")]
        public async Task Learn(IDialogContext context, IAwaitable<IMessageActivity> activity, LuisResult result)
        {
            EntityRecommendation skillEntityRecommendation;
            result.TryFindEntity("skill", out skillEntityRecommendation);
            string skill = skillEntityRecommendation.Entity.Trim();

            //call khanacadamy api with the skill passed by the user. The API should return partially completed or recommended tutorial based on customer history.

            var message = context.MakeMessage() as IMessageActivity;
            message.Speak = $"Here are some of the tutorials from Khan Academy that would help improve your {skill} skills";
            message.Summary = $"Here are some of the tutorials from Khan Academy that would help improve your {skill} skills";
            message.Text = "";
            message.Attachments = new List<Attachment>(){
                new ThumbnailCard(
                    "Introduction to Algebra", 
                    "This topic is an overview of the fundamental ideas and tools of algebra.", 
                    "10 of 25 complete", 
                    new List<CardImage>() {
                        new CardImage("https://cdn.kastatic.org/genfiles/topic-icons/icons/algebra_basics.png-276272-128c.png")
                    },
                    new List<CardAction>() {
                        new CardAction(
                            "openUrl",
                            "View Tutorial",
                            null,
                            "https://www.khanacademy.org/math/algebra/introduction-to-algebra"
                        )
                    }
                ).ToAttachment(),
                new ThumbnailCard(
                    "One-variable linear equations", 
                    "Learn how to solve linear equations that contain a single variable. For example, solve 2(x+3)=(4x-1)/2+7.", 
                    "2 of 25 complete",
                    new List<CardImage>() {
                        new CardImage("https://cdn.kastatic.org/genfiles/topic-icons/icons/one_variable_linear_equations.png-06c5c2-128c.png"
                        )
                    },
                    new List<CardAction>() {
                        new CardAction(
                            "openUrl",
                            "View Tutorial", null,
                            "https://www.khanacademy.org/math/algebra/one-variable-linear-equations"
                            )
                         }
                    ).ToAttachment(),
                new ThumbnailCard(
                    "Units of measurement in modeling", 
                    "Modeling is an amazing world, full of challenges. In this topic, we will start to think about some general modeling concerns, before we dive into modeling situations with different kinds of functions and equations throughout the Algebra curriculum.", 
                    "0 of 40 complete",
                     new List<CardImage>() {
                        new CardImage("https://cdn.kastatic.org/genfiles/topic-icons/icons/units_in_modeling.png-64be6d-128c.png")
                    },
                    new List<CardAction>() {
                        new CardAction(
                            "openUrl",
                            "View Tutorial",
                            null,
                            "https://www.khanacademy.org/math/algebra/units-in-modeling"
                        )
                    }
                ).ToAttachment(),
                new ThumbnailCard(
                    "Two-variable linear equations",
                    "Learn about linear equations that contain two variables, and how these can be represented by graphical lines and tables of values.",
                    "0 of 12 complete",
                     new List<CardImage>() {
                        new CardImage("https://cdn.kastatic.org/genfiles/topic-icons/icons/exponent_equations.png-388a0a-128c.png")
                    },
                    new List<CardAction>() {
                        new CardAction(
                            "openUrl",
                            "View Tutorial",
                            null,
                            "https://www.khanacademy.org/math/algebra/two-var-linear-equations"
                        )
                    }
                ).ToAttachment()
            };
            await context.PostAsync(message);
        }

        [LuisIntent("Last Image")]
        public async Task LastImage(IDialogContext context, IAwaitable<IMessageActivity> activity, LuisResult result)
        {
            //call onedrive api to get the last image stored in onedrive
            var message = context.MakeMessage() as IMessageActivity;
            message.Speak = "Here are the last 3 whiteboard pictures from your onedrive";
            message.Summary = "Here are the last 3 whiteboard pictures from your onedrive";
            message.Text = "Here are the last 3 whiteboard pictures from your onedrive";
            message.Attachments = new List<Attachment>(){
            new HeroCard("Whiteboard Snapshot 1","", "7/18/2017 4:50pm", new List<CardImage>()
            {
                new CardImage() { Url = "http://okaymountain.com/wp-content/uploads/2015/02/whiteboard.jpg" }
            }, new List<CardAction>()
            {
                new CardAction("openUrl", "View Image", null, "https://1drv.ms/i/s!AgOrUfnsJ2_fiNkLiKNtH8cZrWASCg")
            }
            ).ToAttachment(),
            new HeroCard("Whiteboard Snapshot 2","", "7/18/2017 2:30pm", new List<CardImage>()
            {
                new CardImage() { Url = "http://eimagine.com/wp-content/uploads/2015/08/WhiteBoard_Camera.jpg" }
            }, new List<CardAction>()
            {
                new CardAction("openUrl", "View Image", null, "https://1drv.ms/i/s!AgOrUfnsJ2_fiNkLiKNtH8cZrWASCg")
            }
            ).ToAttachment(),
            new HeroCard("Whiteboard Snapshot 3","", "7/18/2017 10:30pm", new List<CardImage>()
            {
                new CardImage() { Url = "http://i1.wp.com/abud.me/wp/wp-content/uploads/2011/08/BCA-Example-Whiteboard-2.jpg" }
            }, new List<CardAction>()
            {
                new CardAction("openUrl", "View Image", null, "https://1drv.ms/i/s!AgOrUfnsJ2_fiNkLiKNtH8cZrWASCg")
            }
            ).ToAttachment()
          };
            await context.PostAsync(message);
        }
    }
}