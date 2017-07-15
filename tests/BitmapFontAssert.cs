﻿/* AngelCode bitmap font parsing using C#
 * http://www.cyotek.com/blog/angelcode-bitmap-font-parsing-using-csharp
 *
 * Copyright © 2012-2017 Cyotek Ltd.
 *
 * Licensed under the MIT License. See LICENSE.txt for the full text.
 */

using System.Collections.Generic;
using NUnit.Framework;

namespace Cyotek.Drawing.BitmapFont.Tests
{
  internal static class BitmapFontAssert
  {
    #region Static Methods

    public static void AreEqual(BitmapFont expected, BitmapFont actual)
    {
      if (expected == null && actual != null)
      {
        Assert.Fail("Expected null but received value.");
      }
      else if (expected != null && actual == null)
      {
        Assert.Fail("Expected value but received null.");
      }
      else if (expected != null)
      {
        Assert.AreEqual(expected.AlphaChannel, actual.AlphaChannel, nameof(BitmapFont.AlphaChannel) + " does not match.");
        Assert.AreEqual(expected.BlueChannel, actual.BlueChannel, nameof(BitmapFont.BlueChannel) + " does not match.");
        Assert.AreEqual(expected.RedChannel, actual.RedChannel, nameof(BitmapFont.RedChannel) + " does not match.");
        Assert.AreEqual(expected.GreenChannel, actual.GreenChannel, nameof(BitmapFont.GreenChannel) + " does not match.");
        Assert.AreEqual(expected.BaseHeight, actual.BaseHeight, nameof(BitmapFont.BaseHeight) + " does not match.");
        Assert.AreEqual(expected.Bold, actual.Bold, nameof(BitmapFont.Bold) + " does not match.");
        Assert.AreEqual(expected.Italic, actual.Italic, nameof(BitmapFont.Italic) + " does not match.");
        Assert.AreEqual(expected.Charset, actual.Charset, nameof(BitmapFont.Charset) + " does not match.");
        Assert.AreEqual(expected.FamilyName, actual.FamilyName, nameof(BitmapFont.FamilyName) + " does not match.");
        Assert.AreEqual(expected.FontSize, actual.FontSize, nameof(BitmapFont.FontSize) + " does not match.");
        Assert.AreEqual(expected.LineHeight, actual.LineHeight, nameof(BitmapFont.LineHeight) + " does not match.");
        Assert.AreEqual(expected.OutlineSize, actual.OutlineSize, nameof(BitmapFont.OutlineSize) + " does not match.");
        Assert.AreEqual(expected.Packed, actual.Packed, nameof(BitmapFont.Packed) + " does not match.");
        Assert.AreEqual(expected.Padding, actual.Padding, nameof(BitmapFont.Padding) + " does not match.");
        Assert.AreEqual(expected.Smoothed, actual.Smoothed, nameof(BitmapFont.Smoothed) + " does not match.");
        Assert.AreEqual(expected.Spacing, actual.Spacing, nameof(BitmapFont.Spacing) + " does not match.");
        Assert.AreEqual(expected.StretchedHeight, actual.StretchedHeight, nameof(BitmapFont.StretchedHeight) + " does not match.");
        Assert.AreEqual(expected.SuperSampling, actual.SuperSampling, nameof(BitmapFont.SuperSampling) + " does not match.");
        Assert.AreEqual(expected.TextureSize, actual.TextureSize, nameof(BitmapFont.TextureSize) + " does not match.");
        Assert.AreEqual(expected.Unicode, actual.Unicode, nameof(BitmapFont.Unicode) + " does not match.");

        AreEqual(expected.Pages, actual.Pages);
        AreEqual(expected.Characters, actual.Characters);
        AreEqual(expected.Kernings, actual.Kernings);
      }
    }

    private static void AreEqual(IDictionary<Kerning, int> expected, IDictionary<Kerning, int> actual)
    {
      Assert.AreEqual(expected.Count, actual.Count, nameof(BitmapFont.Kernings) + " count does not match.");

      foreach (KeyValuePair<Kerning, int> expectedPair in expected)
      {
        int actualKerning;
        int expectedKerning;

        expectedKerning = expectedPair.Value;

        if (!actual.TryGetValue(expectedPair.Key, out actualKerning))
        {
          Assert.Fail("Kerning for pair '" + expectedPair.Key.FirstCharacter + " ','" + expectedPair.Key.SecondCharacter + "' not found.");
        }
        else
        {
          Assert.AreEqual(expectedKerning, actualKerning, "Kerning for pair '" + expectedPair.Key.FirstCharacter + " ','" + expectedPair.Key.SecondCharacter + "' does not match.");
        }
      }
    }

    private static void AreEqual(Page[] expected, Page[] actual)
    {
      Assert.AreEqual(expected.Length, actual.Length, nameof(BitmapFont.Pages) + " count does not match.");

      for (int i = 0; i < expected.Length; i++)
      {
        AreEqual(expected[i], actual[i]);
      }
    }

    private static void AreEqual(Page expected, Page actual)
    {
      Assert.AreEqual(expected.Id, actual.Id, nameof(Page.Id) + " does not match.");
      Assert.AreEqual(expected.FileName, actual.FileName, nameof(Page.FileName) + " does not match.");
    }

    private static void AreEqual(IDictionary<char, Character> expected, IDictionary<char, Character> actual)
    {
      Assert.AreEqual(expected.Count, actual.Count, nameof(BitmapFont.Characters) + " count does not match.");

      foreach (KeyValuePair<char, Character> expectedPair in expected)
      {
        Character actualCharacter;
        Character expectedCharacter;

        expectedCharacter = expectedPair.Value;

        if (!actual.TryGetValue(expectedPair.Key, out actualCharacter))
        {
          Assert.Fail("Character '" + expectedPair.Key + "' not found.");
        }
        else
        {
          AreEqual(expectedCharacter, actualCharacter);
        }
      }
    }

    private static void AreEqual(Character expected, Character actual)
    {
      Assert.AreEqual(expected.Char, actual.Char, nameof(Character.Char) + " does not match.");
      Assert.AreEqual(expected.Channel, actual.Channel, nameof(Character.Channel) + " does not match.");
      Assert.AreEqual(expected.TexturePage, actual.TexturePage, nameof(Character.TexturePage) + " does not match.");
      Assert.AreEqual(expected.XAdvance, actual.XAdvance, nameof(Character.XAdvance) + " does not match.");
      Assert.AreEqual(expected.Offset, actual.Offset, nameof(Character.Offset) + " does not match.");
      Assert.AreEqual(expected.Bounds, actual.Bounds, nameof(Character.Bounds) + " does not match.");
    }

    #endregion
  }
}
