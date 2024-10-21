using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Parts;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class PieceMoveTests
{
    // A Test behaves as an ordinary method
    [Test]
    public void PieceMoveTestsSimplePasses()
    {
        // Use the Assert class to test conditions
        var board = new Board( 8, 8, null, new(null) );
        Assert.NotNull(board);
    }

    // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
    // `yield return null;` to skip a frame.
    [UnityTest]
    public IEnumerator PieceMoveTestsWithEnumeratorPasses()
    {
        // Use the Assert class to test conditions.
        // Use yield to skip a frame.
        yield return null;
    }
}
