// SPDX-License-Identifier: MIT
// SPDX-FileCopyrightText: COPYRIGHT Lois & Clark Fanfiction Tooling

namespace LCFanfic.KerthAwardsCeremonyLog.Discord.LogProcessor.Tests;

public class LogReaderTest
{
  [Test]
  public void ReadMessages ()
  {
    var logReader = new LogReader();
    using (var stream = GetTestData("chat.json"))
    {
      var messages = logReader.ReadMessages(stream).ToArray();
      Assert.That(messages.Length, Is.EqualTo(2));

      Assert.That(messages[0].Timestamp, Is.EqualTo(DateTime.Parse("2024-04-21T16:27:02.358+00:00")));
      Assert.That(messages[0].Content, Is.EqualTo("*creates the stage*"));
      Assert.That(messages[0].AuthorName, Is.EqualTo("UserA"));
    }
  }

  [Test]
  public void ReadMessages_WithReactions ()
  {
    var logReader = new LogReader();
    using (var stream = GetTestData("thread.json"))
    {
      var messages = logReader.ReadMessages(stream).ToArray();
      Assert.That(messages.Length, Is.EqualTo(2));

      Assert.That(messages[1].Reactions.Length, Is.EqualTo(2));

      Assert.That(messages[1].Reactions[0].Emoji, Is.EqualTo(new Uri("https://cdn.jsdelivr.net/gh/twitter/twemoji@latest/assets/svg/1f44b.svg")));
      Assert.That(messages[1].Reactions[0].AlternateText, Is.EqualTo("wave"));
      Assert.That(messages[1].Reactions[0].UserNames, Is.EqualTo(new[] { "UserC", "UserD" }));

      Assert.That(messages[1].Reactions[1].Emoji, Is.EqualTo(new Uri("https://cdn.jsdelivr.net/gh/twitter/twemoji@latest/assets/svg/2764.svg")));
      Assert.That(messages[1].Reactions[1].AlternateText, Is.EqualTo("heart"));
      Assert.That(messages[1].Reactions[1].UserNames, Is.EqualTo(new[] { "UserE" }));
    }
  }

  [Test]
  public void ReadMessages_WithAttachments ()
  {
    var logReader = new LogReader();
    using (var stream = GetTestData("attachment.json"))
    {
      var messages = logReader.ReadMessages(stream).ToArray();
      Assert.That(messages.Length, Is.EqualTo(1));

      Assert.That(messages[0].Content, Is.EqualTo(""));
      Assert.That(
          messages[0].Attachments,
          Is.EqualTo(
              new[]
              {
                  new Uri(
                      "https://cdn.discordapp.com/attachments/1231642363881848933/1231690721665486939/image.png?ex=6633431f&is=6631f19f&hm=1e266c2616b50c606084aa0fba79ee6bc35144647891b74ba512ddfc865bb66e&")
              }));
    }
  }

  private Stream GetTestData (string name)
  {
    var assembly = typeof(LogReaderTest).Assembly;
    var resourceName = assembly.GetName().Name + ".TestData." + name;

    var stream = assembly.GetManifestResourceStream(resourceName);
    Assert.That(stream, Is.Not.Null, $"Resource '{resourceName}' not found in assembly.");

    return stream;
  }
}
