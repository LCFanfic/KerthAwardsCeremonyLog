// SPDX-License-Identifier: MIT
// SPDX-FileCopyrightText: COPYRIGHT Lois & Clark Fanfiction Tooling

namespace LCFanfic.KerthAwardsCeremonyLog.Discord.LogProcessor;

public class MessageAggregator
{
  public IEnumerable<StageMessageWithChat> AggregateChats (IReadOnlyList<ChatMessage> stageMessages, IReadOnlyList<ChatMessage> chatMessages)
  {
    int chatIndex = 0;
    for (int stageIndex = 0; stageIndex < stageMessages.Count; stageIndex++)
    {
      var stageMessage = stageMessages[stageIndex];
      var nextStageTimestamp = stageIndex + 1 < stageMessages.Count ? stageMessages[stageIndex + 1].Timestamp : DateTime.MaxValue;

      var chatForStageMessage = new List<ChatMessage>();
      for (; chatIndex < chatMessages.Count; chatIndex++)
      {
        var currentChatMessage = chatMessages[chatIndex];
        if (currentChatMessage.Timestamp >= nextStageTimestamp)
          break;

        chatForStageMessage.Add(currentChatMessage);
      }

      yield return new StageMessageWithChat()
                   {
                       Stage = stageMessage,
                       Chat = chatForStageMessage.ToArray()
                   };
    }
  }
}
