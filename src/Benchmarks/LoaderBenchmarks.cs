/* AngelCode bitmap font parsing using C#
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

    private readonly string _textFileName;

    private readonly V1TextLoader _v1TextLoader;

    private readonly V1XmlLoader _v1XmlLoader;

    private readonly string _xmlFileName;

    #endregion

    #region Constructors

    public LoaderBenchmarks()
    {
      string path;

      path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "data");
      _textFileName = Path.Combine(path, "arial-32bi.fnt");
      _xmlFileName = Path.Combine(path, "arial-32bi.xml.fnt");
      _v1TextLoader = new V1TextLoader();
      _v1XmlLoader = new V1XmlLoader();
    }

    #endregion

    #region Methods

    [Benchmark]
    public BitmapFont LoadAuto()
    {
      return BitmapFontLoader.LoadFontFromFile(_xmlFileName);
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

    [Benchmark]
    public BitmapFont V1LoadText()
    {
      return _v1TextLoader.Load(_textFileName);
    }

    [Benchmark]
    public BitmapFont V1LoadXml()
    {
      return _v1XmlLoader.Load(_xmlFileName);
    }

    #endregion
  }
}
