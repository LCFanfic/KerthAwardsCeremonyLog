// SPDX-License-Identifier: MIT
// SPDX-FileCopyrightText: COPYRIGHT Lois & Clark Fanfiction Tooling

namespace LCFanfic.KerthAwardsCeremonyLog.Discord.LogProcessor.Tests;

public class MessageAggregatorTest
{
  [Test]
  public void AggregateChats_WithEmptyStage_AndEmptyChat_ReturnsEmptyResult ()
  {
    var messageAggregator = new MessageAggregator();

    var stageMessages = new ChatMessage[0];
    var chatMessages = new ChatMessage[0];
    var result = messageAggregator.AggregateChats(stageMessages, chatMessages).ToArray();

    Assert.That(result, Is.Empty);
  }


  [Test]
  public void AggregateChats_WithNonEmptyStage_AndEmptyChat_ReturnsStageChats ()
  {
    var messageAggregator = new MessageAggregator();

    var stageMessages = new[] { CreateStageMessage("stage 0"), CreateStageMessage("stage 1") };
    var chatMessages = new ChatMessage[0];
    var result = messageAggregator.AggregateChats(stageMessages, chatMessages).ToArray();

    Assert.That(result.Length, Is.EqualTo(2));
    Assert.That(result[0].Stage.Content, Is.EqualTo("stage 0"));
    Assert.That(result[0].Chat, Is.Empty);

    Assert.That(result[1].Stage.Content, Is.EqualTo("stage 1"));
    Assert.That(result[1].Chat, Is.Empty);
  }


  [Test]
  public void AggregateChats_WithNonEmptyStage_AndNonEmptyChat_ReturnsStageChatsWithMatchingChats ()
  {
    var messageAggregator = new MessageAggregator();
    var baseTimestamp = DateTime.Now;
    var stageMessages = new[] { CreateStageMessage("stage 0", baseTimestamp), CreateStageMessage("stage 1", baseTimestamp.AddSeconds(60)) };
    var chatMessages = new[]
                       {
                           CreateStageMessage("chat 0-0", baseTimestamp.AddSeconds(0)),
                           CreateStageMessage("chat 0-1", baseTimestamp.AddSeconds(1)),
                           CreateStageMessage("chat 0-2", baseTimestamp.AddSeconds(59)),
                           CreateStageMessage("chat 1-0", baseTimestamp.AddSeconds(60)),
                           CreateStageMessage("chat 1-1", baseTimestamp.AddSeconds(61))
                       };
    var result = messageAggregator.AggregateChats(stageMessages, chatMessages).ToArray();

    Assert.That(result.Length, Is.EqualTo(2));
    Assert.That(result[0].Stage.Content, Is.EqualTo("stage 0"));
    Assert.That(result[0].Chat.Select(m => m.Content), Is.EqualTo(new[] { "chat 0-0", "chat 0-1", "chat 0-2" }));

    Assert.That(result[1].Stage.Content, Is.EqualTo("stage 1"));
    Assert.That(result[1].Chat.Select(m => m.Content), Is.EqualTo(new[] { "chat 1-0", "chat 1-1" }));
  }


  private ChatMessage CreateStageMessage (string content, DateTime? timestamp = null)
  {
    return new ChatMessage()
           {
               Timestamp = timestamp ?? DateTime.Now,
               Content = content,
               AuthorName = "TheAuthor",
               Attachments = new Uri[0],
               Reactions = new ChatMessageReaction[0]
           };
  }
}
