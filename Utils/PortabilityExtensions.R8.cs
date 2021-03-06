﻿using System;
using JetBrains.Annotations;
using JetBrains.DocumentModel;
using JetBrains.ReSharper.Feature.Services.LiveTemplates.Hotspots;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp.Util;
using JetBrains.ReSharper.Psi.Tree;
using EternalLifetime = JetBrains.DataFlow.EternalLifetime;

namespace JetBrains.ReSharper.ControlFlow.PostfixCompletion
{
  public static class PortabilityExtensions
  {
    public static PredefinedType GetPredefinedType([NotNull] this ITreeNode node)
    {
      return node.GetPsiModule().GetPredefinedType(node.GetResolveContext());
    }

    public static void CommitAllDocuments([NotNull] this IPsiServices services)
    {
      services.Files.CommitAllDocuments();
    }

    public static void DoTransaction(
      [NotNull] this IPsiServices services, [NotNull] string commandName, [NotNull] Action action)
    {
      services.Transactions.Execute(commandName, action);
    }

    public static DocumentRange GetHotspotRange(this DocumentRange documentRange)
    {
      return documentRange;
    }

    public static bool IsForeachEnumeratorPatternType([NotNull] this ITypeElement typeElement)
    {
      return CSharpDeclaredElementUtil.IsForeachEnumeratorPatternType(typeElement);
    }

    public static void AdviceFinished(
      [NotNull] this HotspotSession session, [NotNull] Action<HotspotSession, TerminationType> action)
    {
      session.Closed.Advise(EternalLifetime.Instance, (args) =>
      {
        action(session, args.TerminationType);
      });
    }
  }
}