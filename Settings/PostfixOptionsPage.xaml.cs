﻿using JetBrains.Annotations;
using JetBrains.DataFlow;
using JetBrains.ReSharper.Feature.Services.Resources;
using JetBrains.ReSharper.Features.Intellisense.Options;
using JetBrains.UI.CrossFramework;
using JetBrains.UI.Options;

namespace JetBrains.ReSharper.ControlFlow.PostfixCompletion.Settings
{
  [OptionsPage(PID, "Postfix completion",
    typeof(ServicesThemedIcons.SurroundTemplate),
    ParentId = IntelliSensePage.PID)]
  public sealed partial class PostfixOptionsPage : IOptionsPage
  {
    public const string PID = "PostfixCompletion2";

    public PostfixOptionsPage([NotNull] Lifetime lifetime,
      [NotNull] OptionsSettingsSmartContext store,
      [NotNull] PostfixTemplatesManager templatesManager)
    {
      InitializeComponent();
      DataContext = new PostfixOptionsViewModel(lifetime, store, templatesManager);
      Control = this;
    }

    public EitherControl Control { get; private set; }
    public string Id { get { return PID; } }
    public bool OnOk() { return true; }
    public bool ValidatePage() { return true; }
  }
}