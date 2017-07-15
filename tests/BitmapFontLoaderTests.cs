﻿/* AngelCode bitmap font parsing using C#
 * http://www.cyotek.com/blog/angelcode-bitmap-font-parsing-using-csharp
 *
 * Copyright © 2012-2017 Cyotek Ltd.
 *
 * Licensed under the MIT License. See LICENSE.txt for the full text.
 */

using NUnit.Framework;

namespace Cyotek.Drawing.BitmapFont.Tests
{
  public class BitmapFontLoaderTests : TestBase
  {
    #region Methods

    [Test]
    public void Binary_is_supported()
    {
      // arrange
      BitmapFont actual;
      BitmapFont expected;
      string fileName;

      fileName = this.SimpleBinaryFileName;

      expected = this.Simple;

      // act
      actual = BitmapFontLoader.LoadFontFromBinaryFile(fileName);

      // assert
      BitmapFontAssert.AreEqual(expected, actual);
    }

    [Test]
    public void File_detection_supports_binary()
    {
      // arrange
      BitmapFont actual;
      BitmapFont expected;
      string fileName;

      fileName = this.SimpleBinaryFileName;

      expected = this.Simple;

      // act
      actual = BitmapFontLoader.LoadFontFromFile(fileName);

      // assert
      BitmapFontAssert.AreEqual(expected, actual);
    }

    [Test]
    public void File_detection_supports_text()
    {
      // arrange
      BitmapFont actual;
      BitmapFont expected;
      string fileName;

      fileName = this.SimpleTextFileName;

      expected = this.Simple;

      // act
      actual = BitmapFontLoader.LoadFontFromFile(fileName);

      // assert
      BitmapFontAssert.AreEqual(expected, actual);
    }

    [Test]
    public void File_detection_supports_xml()
    {
      // arrange
      BitmapFont actual;
      BitmapFont expected;
      string fileName;

      fileName = this.SimpleXmlFileName;

      expected = this.Simple;

      // act
      actual = BitmapFontLoader.LoadFontFromFile(fileName);

      // assert
      BitmapFontAssert.AreEqual(expected, actual);
    }

    [Test]
    public void Text_is_supported()
    {
      // arrange
      BitmapFont actual;
      BitmapFont expected;
      string fileName;

      fileName = this.SimpleTextFileName;

      expected = this.Simple;

      // act
      actual = BitmapFontLoader.LoadFontFromTextFile(fileName);

      // assert
      BitmapFontAssert.AreEqual(expected, actual);
    }

    [Test]
    public void Xml_is_supported()
    {
      // arrange
      BitmapFont actual;
      BitmapFont expected;
      string fileName;

      fileName = this.SimpleXmlFileName;

      expected = this.Simple;

      // act
      actual = BitmapFontLoader.LoadFontFromXmlFile(fileName);

      // assert
      BitmapFontAssert.AreEqual(expected, actual);
    }

    #endregion
  }
}
