// SPDX-License-Identifier: MIT
// SPDX-FileCopyrightText: COPYRIGHT Lois & Clark Fanfiction Tooling

using System;
using System.Text.Json.Serialization;

namespace LCFanfic.KerthAwardsCeremonyLog.Discord.LogProcessor;

[JsonSerializable(typeof(LogJson))]
public partial class LogContext : JsonSerializerContext
{
}

public record LogJson
{
  public required MessageJson[] Messages { get; init; }
}

public record MessageJson
{
  public required DateTime Timestamp { get; init; }
  public required string Content { get; init; }
  public required AttachmentJson[] Attachments { get; init; }
  public required UserJson Author { get; init; }
  public required ReactionJson[] Reactions { get; init; }
}

public record UserJson
{
  public required string NickName { get; init; }
}

public record ReactionJson
{
  public required EmojiJson Emoji { get; init; }
  public required UserJson[] Users { get; init; }
}

public record EmojiJson
{
  public required string ImageUrl { get; init; }
  public required string Code { get; init; }
}

public record AttachmentJson
{
  public required string Url { get; init; }
}

