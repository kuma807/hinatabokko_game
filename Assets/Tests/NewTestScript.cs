using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

/*
 * �e�X�g�̎g����
 * 1. �e�X�g�N���X���쐬�A�e�X�g�R�[�h������
 * 2. Unity �� Test Runner (Window->General->Test Runner) �Ŏ��s�A���ʂ��m�F
 * 
 * VS Code ��Ŏ��s�ł���悤�ɂ������������ǂ悭������Ȃ�������
 * �ȉ��̓T���v��
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

    // ���؂���ꍇ�� Assert ���g��
    [Test]
    public void SampleScriptEditTest_Add1()
    {
        Assert.That(100 + 200, Is.EqualTo(300));
    }
}
