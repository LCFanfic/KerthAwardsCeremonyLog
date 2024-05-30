// SPDX-License-Identifier: MIT
// SPDX-FileCopyrightText: COPYRIGHT Lois & Clark Fanfiction Tooling

using System.Xml;

namespace LCFanfic.KerthAwardsCeremonyLog.Discord.LogProcessor;

public class HtmlFormatter
{
  private static readonly char s_nonBreakingWhiteSpace = '\u00A0';
  private static readonly char s_zeroWidthWhiteSpace = '\u200B';

  public void FormatMessages (XmlWriter output, IEnumerable<StageMessageWithChat> messages)
  {
    output.WriteStartElement("table");
    output.WriteAttributeString("class", "sidebysidetranscript");
    RenderHeader(output);
    foreach (var message in messages)
    {
      var stage = message.Stage;

      output.WriteStartElement("tbody");

      output.WriteStartElement("tr");
      RenderStageMessage(output, stage, message.Chat.Length);
      var firstChatMessage = message.Chat.FirstOrDefault();
      if (firstChatMessage == null)
        RenderEmptyChatMessage(output);
      else
        RenderChatMessage(output, firstChatMessage);
      output.WriteEndElement(); // tr

      foreach (var chatMessage in message.Chat.Skip(1))
      {
        output.WriteStartElement("tr");
        RenderChatMessage(output, chatMessage);
        output.WriteEndElement(); // tr
      }

      output.WriteEndElement(); // tbody
    }

    output.WriteEndElement(); // table
  }


  private void RenderHeader (XmlWriter output)
  {
    output.WriteStartElement("colgroup");
    output.WriteStartElement("col");
    output.WriteEndElement(); // col
    output.WriteStartElement("col");
    output.WriteEndElement(); // col
    output.WriteStartElement("col");
    output.WriteAttributeString("style", "width: 40%");
    output.WriteEndElement(); // col
    output.WriteStartElement("col");
    output.WriteEndElement(); // col
    output.WriteStartElement("col");
    output.WriteEndElement(); // col
    output.WriteStartElement("col");
    output.WriteAttributeString("style", "width: 40%");
    output.WriteEndElement(); // col
    output.WriteEndElement(); // colgroup

    output.WriteStartElement("thead");

    output.WriteStartElement("tr");

    output.WriteStartElement("th");
    output.WriteAttributeString("colspan", "3");
    output.WriteValue("#KerthAwards");
    output.WriteEndElement(); // th

    output.WriteStartElement("th");
    output.WriteAttributeString("colspan", "3");
    output.WriteValue("#KerthChat");
    output.WriteEndElement(); // th

    output.WriteEndElement(); // tr

    output.WriteStartElement("tr");

    output.WriteStartElement("td");
    output.WriteAttributeString("colspan", "3");
    output.WriteValue("the “onstage” portion of our show");
    output.WriteEndElement(); // td

    output.WriteStartElement("td");
    output.WriteAttributeString("colspan", "3");
    output.WriteValue("the “kicking back with friends and throwing popcorn at the stage” part of the evening");
    output.WriteEndElement(); // td

    output.WriteEndElement(); // tr

    output.WriteEndElement(); // thead
  }


  private static void RenderStageMessage (XmlWriter output, ChatMessage stage, int rowSpan)
  {
    output.WriteStartElement("td");
    if (rowSpan > 1)
      output.WriteAttributeString("rowspan", rowSpan.ToString());
    output.WriteValue(stage.Timestamp.ToString("HH:mm:ss"));
    output.WriteEndElement();

    output.WriteStartElement("td");
    if (rowSpan > 1)
      output.WriteAttributeString("rowspan", rowSpan.ToString());
    output.WriteValue(stage.AuthorName.Replace(' ', s_nonBreakingWhiteSpace));
    output.WriteEndElement();

    output.WriteStartElement("td");
    if (rowSpan > 1)
      output.WriteAttributeString("rowspan", rowSpan.ToString());
    FormatContent(output, stage);
    output.WriteEndElement();
  }


  private static void RenderEmptyChatMessage (XmlWriter output)
  {
    output.WriteStartElement("td");
    output.WriteValue("");
    output.WriteEndElement();

    output.WriteStartElement("td");
    output.WriteValue("");
    output.WriteEndElement();

    output.WriteStartElement("td");
    output.WriteValue("");
    output.WriteEndElement();
  }


  private static void RenderChatMessage (XmlWriter output, ChatMessage chat)
  {
    output.WriteStartElement("td");
    output.WriteValue(chat.Timestamp.ToString("HH:mm:ss"));
    output.WriteEndElement();

    output.WriteStartElement("td");
    output.WriteValue(chat.AuthorName.Replace(' ', s_nonBreakingWhiteSpace));
    output.WriteEndElement();

    output.WriteStartElement("td");
    FormatContent(output, chat);
    output.WriteEndElement();
  }


  private static void FormatContent (XmlWriter output, ChatMessage message)
  {
    var isFirstLine = true;
    foreach (var line in message.Content.Split(new[] { '\n' }))
    {
      if (!isFirstLine)
      {
        output.WriteStartElement("br");
        output.WriteEndElement();
        output.WriteValue("\n");
      }

      output.WriteValue(line.Replace("/", "/" + s_zeroWidthWhiteSpace));
      isFirstLine = false;
    }
  }
}
