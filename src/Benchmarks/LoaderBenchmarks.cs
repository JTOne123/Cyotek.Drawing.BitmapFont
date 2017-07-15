﻿/* AngelCode bitmap font parsing using C#
 * http://www.cyotek.com/blog/angelcode-bitmap-font-parsing-using-csharp
 *
 * Copyright © 2012-2017 Cyotek Ltd.
 *
 * Licensed under the MIT License. See LICENSE.txt for the full text.
 */

using System;
using System.IO;
using BenchmarkDotNet.Attributes;
using Cyotek.Drawing.BitmapFont;

namespace Benchmarks
{
  [MemoryDiagnoser]
  //[InliningDiagnoser]
  //[HardwareCounters(HardwareCounter.BranchMispredictions, HardwareCounter.BranchInstructions)]
  public class LoaderBenchmarks
  {
    #region Constants

    private readonly string _binaryFileName;

    private readonly string _textFileName;

    private readonly string _xmlFileName;

    #endregion

    #region Constructors

    public LoaderBenchmarks()
    {
      string path;

      path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "data");
      _textFileName = Path.Combine(path, "text.fnt");
      _xmlFileName = Path.Combine(path, "xml.fnt");
      _binaryFileName = Path.Combine(path, "binary.fnt");
    }

    #endregion

    #region Methods

    [Benchmark]
    public BitmapFont LoadAutoBinary()
    {
      return BitmapFontLoader.LoadFontFromFile(_binaryFileName);
    }

    [Benchmark]
    public BitmapFont LoadAutoText()
    {
      return BitmapFontLoader.LoadFontFromFile(_textFileName);
    }

    [Benchmark]
    public BitmapFont LoadAutoXml()
    {
      return BitmapFontLoader.LoadFontFromFile(_xmlFileName);
    }

    [Benchmark]
    public BitmapFont LoadBinary()
    {
      return BitmapFontLoader.LoadFontFromBinaryFile(_binaryFileName);
    }

    [Benchmark]
    public BitmapFont LoadText()
    {
      return BitmapFontLoader.LoadFontFromTextFile(_textFileName);
    }

    [Benchmark]
    public BitmapFont LoadXml()
    {
      return BitmapFontLoader.LoadFontFromXmlFile(_xmlFileName);
    }

    #endregion
  }
}
