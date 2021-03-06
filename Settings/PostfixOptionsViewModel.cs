﻿using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using JetBrains.Annotations;
using JetBrains.Application.Settings;
using JetBrains.DataFlow;
using JetBrains.UI.Avalon.TreeListView;
using JetBrains.UI.Extensions.Commands;
using JetBrains.UI.Options;

namespace JetBrains.ReSharper.ControlFlow.PostfixCompletion.Settings
{
  public sealed class PostfixOptionsViewModel : ObservableObject
  {
    [NotNull] private readonly OptionsSettingsSmartContext myStore;
    [NotNull] private readonly PostfixTemplatesManager myTemplatesManager;

    public PostfixOptionsViewModel(
      [NotNull] Lifetime lifetime,
      [NotNull] OptionsSettingsSmartContext store,
      [NotNull] PostfixTemplatesManager templatesManager)
    {
      myStore = store;
      myTemplatesManager = templatesManager;
      Templates = new ObservableCollection<PostfixTemplateViewModel>();
      UseBracesForEmbeddedStatements = new Property<bool>(lifetime, "UseBracesForEmbeddedStatements");
      ShowStaticMembersInCodeCompletion = new Property<bool>(lifetime, "ShowStaticMethodsInCodeCompletion");
      ShowEnumHelpersInCodeCompletion = new Property<bool>(lifetime, "ShowEnumHelpersInCodeCompletion");
      Reset = new DelegateCommand(ResetExecute);

      store.SetBinding(lifetime, PostfixSettingsAccessor.UseBracesForEmbeddedStatements, UseBracesForEmbeddedStatements);
      store.SetBinding(lifetime, PostfixSettingsAccessor.ShowStaticMethodsInCodeCompletion, ShowStaticMembersInCodeCompletion);
      store.SetBinding(lifetime, PostfixSettingsAccessor.ShowEnumHelpersInCodeCompletion, ShowEnumHelpersInCodeCompletion);

      FillTemplates();
    }

    [NotNull] public ObservableCollection<PostfixTemplateViewModel> Templates { get; private set; }
    [NotNull] public IProperty<bool> UseBracesForEmbeddedStatements { get; private set; }
    [NotNull] public IProperty<bool> ShowStaticMembersInCodeCompletion { get; private set; }
    [NotNull] public Property<bool> ShowEnumHelpersInCodeCompletion { get; private set; }
    [NotNull] public ICommand Reset { get; private set; }

    private void FillTemplates()
    {
      var settings = myStore.GetKey<PostfixCompletionSettings>(SettingsOptimization.OptimizeDefault);
      settings.DisabledProviders.SnapshotAndFreeze();

      PropertyChangedEventHandler handler = (sender, args) =>
      {
        if (args.PropertyName == "IsChecked")
        {
          var viewModel = (PostfixTemplateViewModel) sender;
          myStore.SetIndexedValue(
            PostfixSettingsAccessor.DisabledProviders,
            viewModel.SettingsKey, viewModel.IsChecked);
        }
      };

      foreach (var providerInfo in myTemplatesManager.TemplateProvidersInfos
        .OrderBy(providerInfo => providerInfo.Metadata.TemplateName))
      {
        var metadata = providerInfo.Metadata;
        bool isEnabled = (!settings.DisabledProviders.TryGet(providerInfo.SettingsKey, out isEnabled)
                          && !metadata.DisabledByDefault) || isEnabled;

        var itemViewModel = new PostfixTemplateViewModel(
          name: metadata.TemplateName,
          description: metadata.Description,
          example: metadata.Example,
          settingsKey: providerInfo.SettingsKey,
          isChecked: isEnabled);

        itemViewModel.PropertyChanged += handler;
        Templates.Add(itemViewModel);
      }
    }

    private void ResetExecute()
    {
      var settings = myStore.GetKey<PostfixCompletionSettings>(SettingsOptimization.OptimizeDefault);
      settings.DisabledProviders.SnapshotAndFreeze();

      foreach (var provider in settings.DisabledProviders.EnumIndexedValues())
        myStore.RemoveIndexedValue(PostfixSettingsAccessor.DisabledProviders, provider.Key);

      Templates.Clear();
      FillTemplates();
    }
  }
}