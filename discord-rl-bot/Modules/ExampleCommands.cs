﻿using CodyTedrick.DiscordBot.Services;
using Discord.Interactions;

namespace CodyTedrick.DiscordBot.Modules;

public class ExampleCommands : InteractionModuleBase<SocketInteractionContext>
{
    // dependencies can be accessed through Property injection, public properties with public setters will be set by the service provider
    public InteractionService Commands { get; set; }
    private CommandHandler _handler;

    // constructor injection is also a valid way to access the dependecies
    public ExampleCommands (CommandHandler handler)
    {
        _handler = handler;
    }

    // our first /command!
    [SlashCommand("8ball", "find your answer!")]
    public async Task EightBall(string question)
    {
        // create a list of possible replies
        var replies = new List<string>();

        // add our possible replies
        replies.Add("yes");
        replies.Add("no");
        replies.Add("maybe");
        replies.Add("hazzzzy....");

        // get the answer
        var answer = replies[new Random().Next(replies.Count - 1)];

        // reply with the answer
        await RespondAsync($"You asked: [**{question}**], and your answer is: [**{answer}**]");
    }
}