using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class UnitTestProject
{
    [Test]
    public void UnitTest()
    {
        Hover hover = null;
        Sprite sprite = null;
        hover.Activate(sprite);
        Debug.Log(hover);
        Assert.AreEqual(true, hover.spriteRenderer.enabled);

    }
}
