﻿using JetBrains.ReSharper.Feature.Services.Tests.CSharp.FeatureServices.CodeCompletion;
using JetBrains.ReSharper.TestFramework;
using NUnit.Framework;

namespace JetBrains.ReSharper.ControlFlow.PostfixCompletion
{
  [TestNetFramework4]
  public class PostfixCompletionListTest : CodeCompletionTestBase
  {
    protected override bool ExecuteAction { get { return false; } }
    protected override bool CheckAutomaticCompletionDefault() { return true; }

    protected override string RelativeTestDataPath
    {
      get { return PostfixCompletionTestsAssembly.TestDataPath + @"\Completion\List"; }
    }

    [Test] public void TestUnresolved01() { DoNamedTest(); }
    [Test] public void TestUnresolved02() { DoNamedTest(); }
    [Test] public void TestUnresolved03() { DoNamedTest(); }
    [Test] public void TestUnresolved04() { DoNamedTest(); }

    [Test] public void TestType01() { DoNamedTest(); }
    [Test] public void TestType02() { DoNamedTest(); }
    [Test] public void TestType03() { DoNamedTest(); }
    [Test] public void TestType04() { DoNamedTest(); }

    [Test] public void TestVar01() { DoNamedTest(); }

    [Test] public void TestSwitch01() { DoNamedTest(); }
  }
}