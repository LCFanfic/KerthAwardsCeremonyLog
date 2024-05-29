// SPDX-License-Identifier: MIT
// SPDX-FileCopyrightText: COPYRIGHT Lois & Clark Fanfiction Tooling

using System.Text.Json;

namespace LCFanfic.KerthAwardsCeremonyLog.Discord.LogProcessor;

public class LogReader
{
  public IEnumerable<ChatMessage> ReadMessages (Stream jsonStream)
  {
    var options = new JsonSerializerOptions
                  {
                      PropertyNameCaseInsensitive = true
                  };

    var logJson = (LogJson?)JsonSerializer.Deserialize(jsonStream, typeof(LogJson), new LogContext(options));

    var jsonMessages = logJson?.Messages ?? Array.Empty<MessageJson>();

    return jsonMessages.Select(FlattenMessage);
  }

  private ChatMessage FlattenMessage (MessageJson jsonMessage)
  {
    return new ChatMessage
           {
               Timestamp = jsonMessage.Timestamp,
               Content = jsonMessage.Content,
               Attachments = jsonMessage.Attachments.Select(a =>  new Uri(a.Url)).ToArray(),
               AuthorName = jsonMessage.Author.NickName,
               Reactions = jsonMessage.Reactions.Select(
                   r => new ChatMessageReaction()
                        {
                            Emoji = new Uri(r.Emoji.ImageUrl),
                            AlternateText = r.Emoji.Code,
                            UserNames = r.Users.Select(u => u.NickName).ToArray()
                        }
                   ).ToArray()
           };
  }
}
