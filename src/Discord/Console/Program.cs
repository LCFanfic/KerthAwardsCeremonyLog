// SPDX-License-Identifier: MIT
// SPDX-FileCopyrightText: COPYRIGHT Lois & Clark Fanfiction Tooling

using System;
using System.Xml;
using LCFanfic.KerthAwardsCeremonyLog.Discord.LogProcessor;

namespace LCFanfic.KerthAwardsCeremonyLog.Discord.Console;

class Program
{
  static int Main (string[] args)
  {
    if (args.Length != 3)
    {
      System.Console.WriteLine("Usage: program.exe stage.json chat.json output.html");
      return -1;
    }

    var stageJsonPath = args[0];
    var chatJsonPath = args[1];
    var outputPath = args[2];

    if (!File.Exists(stageJsonPath))
    {
      System.Console.WriteLine("Stage file '{0}' does not exist.", stageJsonPath);
      return -1;
    }

    if (!File.Exists(chatJsonPath))
    {
      System.Console.WriteLine("Chat file '{0}' does not exist.", chatJsonPath);
      return -1;
    }

    try
    {
      var stageLog = ReadJsonFile(stageJsonPath);
      var chatLog = ReadJsonFile(chatJsonPath);

      var aggregatedLog = AggregateLogs(stageLog, chatLog);

      GenerateOutput(outputPath, aggregatedLog);

      return 0;
    }
    catch (Exception ex)
    {
      System.Console.WriteLine(ex);
      return -2;
    }
  }

  private static ChatMessage[] ReadJsonFile (string stageJsonPath)
  {
    var logReader = new LogReader();
    using (var stageStream = File.OpenRead(stageJsonPath))
    {
      return logReader.ReadMessages(stageStream).ToArray();
    }
  }

  private static IEnumerable<StageMessageWithChat> AggregateLogs (IReadOnlyList<ChatMessage> stageLog, IReadOnlyList<ChatMessage> chatLog)
  {
    var messageAggregator = new MessageAggregator();
    return messageAggregator.AggregateChats(stageLog, chatLog);
  }

  private static void GenerateOutput (string outputPath, IEnumerable<StageMessageWithChat> aggregatedLog)
  {
    var htmlFormatter = new HtmlFormatter();
    using var outputStream = XmlTextWriter.Create(
        outputPath,
        new XmlWriterSettings() { ConformanceLevel = ConformanceLevel.Fragment, Indent = true, NewLineOnAttributes = true });
    htmlFormatter.FormatMessages(outputStream, aggregatedLog);
  }
}
