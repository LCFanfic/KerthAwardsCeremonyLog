// SPDX-License-Identifier: MIT
// SPDX-FileCopyrightText: COPYRIGHT Lois & Clark Fanfiction Tooling

namespace LCFanfic.KerthAwardsCeremonyLog.Discord.LogProcessor;

public record StageMessageWithChat
{
  public required ChatMessage Stage { get; init; }
  public required ChatMessage[] Chat { get; init; }
}
