using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

/*
 * テストの使い方
 * 1. テストクラスを作成、テストコードを書く
 * 2. Unity の Test Runner (Window->General->Test Runner) で実行、結果を確認
 * 
 * VS Code 上で実行できるようにしたかったけどよく分かんなかったよ
 * 以下はサンプル
 */

public class NewTestScript
{
    // A Test behaves as an ordinary method
    [Test]
    public void NewTestScriptSimplePasses()
    {
        // Use the Assert class to test conditions
    }

    // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
    // `yield return null;` to skip a frame.
    [UnityTest]
    public IEnumerator NewTestScriptWithEnumeratorPasses()
    {
        // Use the Assert class to test conditions.
        // Use yield to skip a frame.
        yield return null;
    }

    // 検証する場合は Assert を使う
    [Test]
    public void SampleScriptEditTest_Add1()
    {
        Assert.That(100 + 200, Is.EqualTo(300));
    }
}
