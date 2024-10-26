//using System.Collections;
//using System.Collections.Generic;
//using Assets.EditModeTests;
//using Assets.Scripts.Parts;
//using NUnit.Framework;
//using UnityEngine;
//using UnityEngine.TestTools;

//public class PieceMoveTests
//{
//    private readonly Board _board;
//    public PieceMoveTests()
//    {
//        _board = TestUtilities.GetSetupBoard();
//    }

//    [Test]
//    public void d4()
//    {
//        _board.SelectLocation( 2, 4 );
//        _board.SelectLocation( 4, 4 );
//        Assert.True( _board.IsFree( 2, 4 ) );
//        Assert.False( _board.IsFree( 4, 4 ) );
//    }

//    [Test]
//    public void Nc3()
//    {
//        _board.SelectLocation( 1, 2 );
//        _board.SelectLocation( 3, 3 );
//        Assert.True( _board.IsFree( 1, 2 ) );
//        Assert.False( _board.IsFree( 3, 3 ) );
//    }


//}
