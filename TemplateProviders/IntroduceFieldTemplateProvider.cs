﻿using JetBrains.Annotations;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Tree;
#if RESHARPER7
using System.Linq;
#endif

namespace JetBrains.ReSharper.ControlFlow.PostfixCompletion.TemplateProviders
{
  [PostfixTemplateProvider("field", "Introduces field for expression")]
  public class IntroduceFieldTemplateProvider : IntroduceMemberTemplateProviderBase
  {
    protected override IntroduceMemberLookupItem CreateLookupItem(
      PrefixExpressionContext expression, IType expressionType, bool isStatic)
    {
      return new IntroduceFieldLookupItem(expression, expressionType, isStatic);
    }

    private sealed class IntroduceFieldLookupItem : IntroduceMemberLookupItem
    {
      public IntroduceFieldLookupItem(
        [NotNull] PrefixExpressionContext context,
        [NotNull] IType expressionType, bool isStatic)
        : base("field", context, expressionType, isStatic) { }

      protected override IClassMemberDeclaration CreateMemberDeclaration(CSharpElementFactory factory)
      {
        var declaration = factory.CreateFieldDeclaration(ExpressionType, "__");
        declaration.SetStatic(IsStatic);

        return declaration;
      }

      protected override ICSharpTypeMemberDeclaration GetAnchorMember(
        TreeNodeCollection<ICSharpTypeMemberDeclaration> members)
      {
        return members.LastOrDefault(
          m => m.DeclaredElement is IField && m.IsStatic == IsStatic);
      }
    }
  }
}