// SPDX-License-Identifier: MIT
// SPDX-FileCopyrightText: COPYRIGHT Lois & Clark Fanfiction Tooling

namespace LCFanfic.KerthAwardsCeremonyLog.Discord.LogProcessor;

public record ChatMessage
{
  public required DateTime Timestamp { get; init; }
  public required string Content { get; init; }
  public required Uri[] Attachments { get; init; }
  public required string AuthorName { get; init; }
  public required ChatMessageReaction[] Reactions { get; init; }
}

public record ChatMessageReaction
{
  public required Uri Emoji { get; init; }
  public required string AlternateText { get; init; }
  public required string[] UserNames { get; init; }
}
