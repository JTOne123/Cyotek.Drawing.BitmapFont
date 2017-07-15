﻿/* AngelCode bitmap font parsing using C#
 * http://www.cyotek.com/blog/angelcode-bitmap-font-parsing-using-csharp
 *
 * Copyright © 2012-2015 Cyotek Ltd.
 *
 * Licensed under the MIT License. See license.txt for the full text.
 */

// Some documentation derived from the BMFont file format specification
// http://www.angelcode.com/products/bmfont/doc/file_format.html

using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using System.Xml;

namespace Cyotek.Drawing.BitmapFont
{
  /// <summary>
  /// A bitmap font.
  /// </summary>
  /// <seealso cref="T:System.Collections.Generic.IEnumerable{Cyotek.Drawing.BitmapFont.Character}"/>
  public class BitmapFont : IEnumerable<Character>
  {
    #region Constants

    /// <summary>
    /// When used with <see cref="MeasureFont(string,double)"/>, specifies that no wrapping should occur.
    /// </summary>
    public const int NoMaxWidth = -1;

    #endregion

    #region Properties

    /// <summary>
    /// Gets or sets the alpha channel.
    /// </summary>
    /// <value>
    /// The alpha channel.
    /// </value>
    /// <remarks>Set to 0 if the channel holds the glyph data, 1 if it holds the outline, 2 if it holds the glyph and the outline, 3 if its set to zero, and 4 if its set to one.</remarks>
    public int AlphaChannel { get; set; }

    /// <summary>
    /// Gets or sets the number of pixels from the absolute top of the line to the base of the characters.
    /// </summary>
    /// <value>
    /// The number of pixels from the absolute top of the line to the base of the characters.
    /// </value>
    public int BaseHeight { get; set; }

    /// <summary>
    /// Gets or sets the blue channel.
    /// </summary>
    /// <value>
    /// The blue channel.
    /// </value>
    /// <remarks>Set to 0 if the channel holds the glyph data, 1 if it holds the outline, 2 if it holds the glyph and the outline, 3 if its set to zero, and 4 if its set to one.</remarks>
    public int BlueChannel { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the font is bold.
    /// </summary>
    /// <value>
    /// <c>true</c> if the font is bold, otherwise <c>false</c>.
    /// </value>
    public bool Bold { get; set; }

    /// <summary>
    /// Gets or sets the characters that comprise the font.
    /// </summary>
    /// <value>
    /// The characters that comprise the font.
    /// </value>
    public IDictionary<char, Character> Characters { get; set; }

    /// <summary>
    /// Gets or sets the name of the OEM charset used.
    /// </summary>
    /// <value>
    /// The name of the OEM charset used (when not unicode).
    /// </value>
    public string Charset { get; set; }

    /// <summary>
    /// Gets or sets the name of the true type font.
    /// </summary>
    /// <value>
    /// The font family name.
    /// </value>
    public string FamilyName { get; set; }

    /// <summary>
    /// Gets or sets the size of the font.
    /// </summary>
    /// <value>
    /// The size of the font.
    /// </value>
    public int FontSize { get; set; }

    /// <summary>
    /// Gets or sets the green channel.
    /// </summary>
    /// <value>
    /// The green channel.
    /// </value>
    /// <remarks>Set to 0 if the channel holds the glyph data, 1 if it holds the outline, 2 if it holds the glyph and the outline, 3 if its set to zero, and 4 if its set to one.</remarks>
    public int GreenChannel { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the font is italic.
    /// </summary>
    /// <value>
    /// <c>true</c> if the font is italic, otherwise <c>false</c>.
    /// </value>
    public bool Italic { get; set; }

    /// <summary>
    /// Indexer to get items within this collection using array index syntax.
    /// </summary>
    /// <param name="character">The character.</param>
    /// <returns>
    /// The indexed item.
    /// </returns>
    public Character this[char character]
    {
      get { return this.Characters[character]; }
    }

    /// <summary>
    /// Gets or sets the character kernings for the font.
    /// </summary>
    /// <value>
    /// The character kernings for the font.
    /// </value>
    public IDictionary<Kerning, int> Kernings { get; set; }

    /// <summary>
    /// Gets or sets the distance in pixels between each line of text.
    /// </summary>
    /// <value>
    /// The distance in pixels between each line of text.
    /// </value>
    public int LineHeight { get; set; }

    /// <summary>
    /// Gets or sets the outline thickness for the characters.
    /// </summary>
    /// <value>
    /// The outline thickness for the characters.
    /// </value>
    public int OutlineSize { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the monochrome characters have been packed into each of the texture channels.
    /// </summary>
    /// <value>
    /// <c>true</c> if the characters are packed, otherwise <c>false</c>.
    /// </value>
    /// <remarks>
    /// When packed, the <see cref="AlphaChannel"/> property describes what is stored in each channel.
    /// </remarks>
    public bool Packed { get; set; }

    /// <summary>
    /// Gets or sets the padding for each character.
    /// </summary>
    /// <value>
    /// The padding for each character.
    /// </value>
    public Padding Padding { get; set; }

    /// <summary>
    /// Gets or sets the texture pages for the font.
    /// </summary>
    /// <value>
    /// The pages.
    /// </value>
    public Page[] Pages { get; set; }

    /// <summary>
    /// Gets or sets the red channel.
    /// </summary>
    /// <value>
    /// The red channel.
    /// </value>
    /// <remarks>Set to 0 if the channel holds the glyph data, 1 if it holds the outline, 2 if it holds the glyph and the outline, 3 if its set to zero, and 4 if its set to one.</remarks>
    public int RedChannel { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the font is smoothed.
    /// </summary>
    /// <value>
    /// <c>true</c> if the font is smoothed, otherwise <c>false</c>.
    /// </value>
    public bool Smoothed { get; set; }

    /// <summary>
    /// Gets or sets the spacing for each character.
    /// </summary>
    /// <value>
    /// The spacing for each character.
    /// </value>
    public Point Spacing { get; set; }

    /// <summary>
    /// Gets or sets the font height stretch.
    /// </summary>
    /// <value>
    /// The font height stretch.
    /// </value>
    /// <remarks>100% means no stretch.</remarks>
    public int StretchedHeight { get; set; }

    /// <summary>
    /// Gets or sets the level of super sampling used by the font.
    /// </summary>
    /// <value>
    /// The super sampling level of the font.
    /// </value>
    /// <remarks>A value of 1 indicates no super sampling is in use.</remarks>
    public int SuperSampling { get; set; }

    /// <summary>
    /// Gets or sets the size of the texture images used by the font.
    /// </summary>
    /// <value>
    /// The size of the texture.
    /// </value>
    public Size TextureSize { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the font is unicode.
    /// </summary>
    /// <value>
    /// <c>true</c> if the font is unicode, otherwise <c>false</c>.
    /// </value>
    public bool Unicode { get; set; }

    #endregion

    #region Methods

    /// <summary>
    /// Gets the kerning for the specified character combination.
    /// </summary>
    /// <param name="previous">The previous character.</param>
    /// <param name="current">The current character.</param>
    /// <returns>
    /// The spacing between the specified characters.
    /// </returns>
    public int GetKerning(char previous, char current)
    {
      Kerning key;
      int result;

      key = new Kerning(previous, current, 0);

      if (!this.Kernings.TryGetValue(key, out result))
      {
        result = 0;
      }

      return result;
    }

    /// <summary>
    /// Load font information from the specified <see cref="Stream"/>.
    /// </summary>
    /// <exception cref="ArgumentNullException">Thrown when one or more required arguments are null.</exception>
    /// <exception cref="ArgumentException">Thrown when one or more arguments have unsupported or
    /// illegal values.</exception>
    /// <exception cref="InvalidDataException">Thrown when an Invalid Data error condition occurs.</exception>
    /// <param name="stream">The stream to load.</param>
    public virtual void Load(Stream stream)
    {
      byte[] buffer;
      string header;

      if (stream == null)
      {
        throw new ArgumentNullException("stream");
      }

      if (!stream.CanSeek)
      {
        throw new ArgumentException("Stream must be seekable in order to determine file format.", "stream");
      }

      // read the first five bytes so we can try and work out what the format is
      // then reset the position so the format loaders can work
      buffer = new byte[5];
      stream.Read(buffer, 0, 5);
      stream.Seek(0, SeekOrigin.Begin);
      header = Encoding.ASCII.GetString(buffer);

      switch (header)
      {
        case "info ":
          this.LoadText(stream);
          break;
        case "<?xml":
          this.LoadXml(stream);
          break;
        default: throw new InvalidDataException("Unknown file format.");
      }
    }

    /// <summary>
    /// Load font information from the specified file.
    /// </summary>
    /// <exception cref="ArgumentNullException">Thrown when one or more required arguments are null.</exception>
    /// <exception cref="FileNotFoundException">Thrown when the requested file is not present.</exception>
    /// <param name="fileName">The file name to load.</param>
    public void Load(string fileName)
    {
      if (string.IsNullOrEmpty(fileName))
      {
        throw new ArgumentNullException("fileName");
      }

      if (!File.Exists(fileName))
      {
        throw new FileNotFoundException(string.Format("Cannot find file '{0}'.", fileName), fileName);
      }

      using (Stream stream = File.OpenRead(fileName))
      {
        this.Load(stream);
      }

      BitmapFontLoader.QualifyResourcePaths(this, Path.GetDirectoryName(fileName));
    }

    /// <summary>
    /// Loads font information from the specified stream.
    /// </summary>
    /// <remarks>
    /// The source data must be in BMFont binary format.
    /// </remarks>
    /// <exception cref="ArgumentNullException">Thrown when one or more required arguments are null.</exception>
    /// <param name="stream">The stream containing the font to load.</param>
    public void LoadBinary(Stream stream)
    {
      byte[] buffer;

      if (stream == null)
      {
        throw new ArgumentNullException("stream");
      }

      buffer = new byte[1024];

      // The first three bytes are the file identifier and must always be 66, 77, 70, or "BMF". The fourth byte gives the format version, currently it must be 3.

      stream.Read(buffer, 0, 4);

      if (buffer[0] != 66 || buffer[1] != 77 || buffer[2] != 70)
      {
        throw new InvalidDataException("Source steam does not contain BMFont data.");
      }

      if (buffer[3] != 3)
      {
        throw new InvalidDataException("Only BMFont version 3 format data is supported.");
      }

      // Following the first four bytes is a series of blocks with information. Each block starts with a one byte block type identifier, followed by a 4 byte integer that gives the size of the block, not including the block type identifier and the size value.

      while (stream.Read(buffer, 0, 5) != 0)
      {
        int blockSize;
        byte blockType;

        blockType = buffer[0];

        blockSize = WordHelpers.MakeDWordLittleEndian(buffer, 1);
        if (blockSize > buffer.Length)
        {
          buffer = new byte[blockSize];
        }

        if (stream.Read(buffer, 0, blockSize) != blockSize)
        {
          throw new InvalidDataException("Failed to read enough data to fill block.");
        }

        switch (blockType)
        {
          case 1: // Block type 1: info
            this.LoadInfoBlock(buffer);
            break;
          case 2: // Block type 2: common
            this.LoadCommonBlock(buffer);
            break;
          case 3: // Block type 3: pages
            this.LoadPagesBlock(buffer);
            break;
          case 4: // Block type 4: chars
            this.LoadCharactersBlock(buffer, blockSize);
            break;
          case 5: // Block type 5: kerning pairs
            this.LoadKerningsBlock(buffer, blockSize);
            break;
          default: throw new InvalidDataException("Block type " + blockType + " is not a valid BMFont block");
        }
      }
    }

    /// <summary>
    /// Loads font information from the specified string.
    /// </summary>
    /// <param name="text">String containing the font to load.</param>
    /// <remarks>The source data must be in BMFont text format.</remarks>
    public void LoadText(string text)
    {
      using (StringReader reader = new StringReader(text))
      {
        this.LoadText(reader);
      }
    }

    /// <summary>
    /// Loads font information from the specified stream.
    /// </summary>
    /// <remarks>
    /// The source data must be in BMFont text format.
    /// </remarks>
    /// <exception cref="ArgumentNullException">Thrown when one or more required arguments are null.</exception>
    /// <param name="stream">The stream containing the font to load.</param>
    public void LoadText(Stream stream)
    {
      if (stream == null)
      {
        throw new ArgumentNullException("stream");
      }

      using (TextReader reader = new StreamReader(stream))
      {
        this.LoadText(reader);
      }
    }

    /// <summary>
    /// Loads font information from the specified <see cref="TextReader"/>.
    /// </summary>
    /// <remarks>
    /// The source data must be in BMFont text format.
    /// </remarks>
    /// <exception cref="ArgumentNullException">Thrown when one or more required arguments are null.</exception>
    /// <param name="reader">The <strong>TextReader</strong> used to feed the data into the font.</param>
    public virtual void LoadText(TextReader reader)
    {
      IDictionary<int, Page> pageData;
      IDictionary<Kerning, int> kerningDictionary;
      IDictionary<char, Character> charDictionary;
      string line;

      if (reader == null)
      {
        throw new ArgumentNullException("reader");
      }

      pageData = new SortedDictionary<int, Page>();
      kerningDictionary = new Dictionary<Kerning, int>();
      charDictionary = new Dictionary<char, Character>();

      do
      {
        line = reader.ReadLine();

        if (line != null)
        {
          string[] parts;

          parts = BitmapFontLoader.Split(line, ' ');

          if (parts.Length != 0)
          {
            switch (parts[0])
            {
              case "info":
                this.FamilyName = BitmapFontLoader.GetNamedString(parts, "face");
                this.FontSize = BitmapFontLoader.GetNamedInt(parts, "size");
                this.Bold = BitmapFontLoader.GetNamedBool(parts, "bold");
                this.Italic = BitmapFontLoader.GetNamedBool(parts, "italic");
                this.Charset = BitmapFontLoader.GetNamedString(parts, "charset");
                this.Unicode = BitmapFontLoader.GetNamedBool(parts, "unicode");
                this.StretchedHeight = BitmapFontLoader.GetNamedInt(parts, "stretchH");
                this.Smoothed = BitmapFontLoader.GetNamedBool(parts, "smooth");
                this.SuperSampling = BitmapFontLoader.GetNamedInt(parts, "aa");
                this.Padding = BitmapFontLoader.ParsePadding(BitmapFontLoader.GetNamedString(parts, "padding"));
                this.Spacing = BitmapFontLoader.ParsePoint(BitmapFontLoader.GetNamedString(parts, "spacing"));
                this.OutlineSize = BitmapFontLoader.GetNamedInt(parts, "outline");
                break;
              case "common":
                this.LineHeight = BitmapFontLoader.GetNamedInt(parts, "lineHeight");
                this.BaseHeight = BitmapFontLoader.GetNamedInt(parts, "base");
                this.TextureSize = new Size(BitmapFontLoader.GetNamedInt(parts, "scaleW"), BitmapFontLoader.GetNamedInt(parts, "scaleH"));
                this.Packed = BitmapFontLoader.GetNamedBool(parts, "packed");
                this.AlphaChannel = BitmapFontLoader.GetNamedInt(parts, "alphaChnl");
                this.RedChannel = BitmapFontLoader.GetNamedInt(parts, "redChnl");
                this.GreenChannel = BitmapFontLoader.GetNamedInt(parts, "greenChnl");
                this.BlueChannel = BitmapFontLoader.GetNamedInt(parts, "blueChnl");
                break;
              case "page":
                int id;
                string name;

                id = BitmapFontLoader.GetNamedInt(parts, "id");
                name = BitmapFontLoader.GetNamedString(parts, "file");

                pageData.Add(id, new Page(id, name));
                break;
              case "char":
                Character charData;

                charData = new Character
                           {
                             Char = (char)BitmapFontLoader.GetNamedInt(parts, "id"),
                             Bounds = new Rectangle(BitmapFontLoader.GetNamedInt(parts, "x"), BitmapFontLoader.GetNamedInt(parts, "y"), BitmapFontLoader.GetNamedInt(parts, "width"), BitmapFontLoader.GetNamedInt(parts, "height")),
                             Offset = new Point(BitmapFontLoader.GetNamedInt(parts, "xoffset"), BitmapFontLoader.GetNamedInt(parts, "yoffset")),
                             XAdvance = BitmapFontLoader.GetNamedInt(parts, "xadvance"),
                             TexturePage = BitmapFontLoader.GetNamedInt(parts, "page"),
                             Channel = BitmapFontLoader.GetNamedInt(parts, "chnl")
                           };
                charDictionary.Add(charData.Char, charData);
                break;
              case "kerning":
                Kerning key;

                key = new Kerning((char)BitmapFontLoader.GetNamedInt(parts, "first"), (char)BitmapFontLoader.GetNamedInt(parts, "second"), BitmapFontLoader.GetNamedInt(parts, "amount"));

                if (!kerningDictionary.ContainsKey(key))
                {
                  kerningDictionary.Add(key, key.Amount);
                }
                break;
            }
          }
        }
      } while (line != null);

      this.Pages = BitmapFontLoader.ToArray(pageData.Values);
      this.Characters = charDictionary;
      this.Kernings = kerningDictionary;
    }

    /// <summary>
    /// Loads font information from the specified string.
    /// </summary>
    /// <param name="xml">String containing the font to load.</param>
    /// <remarks>The source data must be in BMFont XML format.</remarks>
    public void LoadXml(string xml)
    {
      using (StringReader reader = new StringReader(xml))
      {
        this.LoadXml(reader);
      }
    }

    /// <summary>
    /// Loads font information from the specified <see cref="TextReader"/>.
    /// </summary>
    /// <remarks>
    /// The source data must be in BMFont XML format.
    /// </remarks>
    /// <exception cref="ArgumentNullException">Thrown when one or more required arguments are null.</exception>
    /// <param name="reader">The <strong>TextReader</strong> used to feed the data into the font.</param>
    public virtual void LoadXml(TextReader reader)
    {
      XmlDocument document;
      IDictionary<int, Page> pageData;
      IDictionary<Kerning, int> kerningDictionary;
      IDictionary<char, Character> charDictionary;
      XmlNode root;
      XmlNode properties;

      if (reader == null)
      {
        throw new ArgumentNullException("reader");
      }

      document = new XmlDocument();
      pageData = new SortedDictionary<int, Page>();
      kerningDictionary = new Dictionary<Kerning, int>();
      charDictionary = new Dictionary<char, Character>();

      document.Load(reader);
      root = document.DocumentElement;

      // load the basic attributes
      properties = root.SelectSingleNode("info");
      this.FamilyName = properties.Attributes["face"].Value;
      this.FontSize = Convert.ToInt32(properties.Attributes["size"].Value);
      this.Bold = Convert.ToInt32(properties.Attributes["bold"].Value) != 0;
      this.Italic = Convert.ToInt32(properties.Attributes["italic"].Value) != 0;
      this.Unicode = Convert.ToInt32(properties.Attributes["unicode"].Value) != 0;
      this.StretchedHeight = Convert.ToInt32(properties.Attributes["stretchH"].Value);
      this.Charset = properties.Attributes["charset"].Value;
      this.Smoothed = Convert.ToInt32(properties.Attributes["smooth"].Value) != 0;
      this.SuperSampling = Convert.ToInt32(properties.Attributes["aa"].Value);
      this.Padding = BitmapFontLoader.ParsePadding(properties.Attributes["padding"].Value);
      this.Spacing = BitmapFontLoader.ParsePoint(properties.Attributes["spacing"].Value);
      this.OutlineSize = Convert.ToInt32(properties.Attributes["outline"].Value);

      // common attributes
      properties = root.SelectSingleNode("common");
      this.BaseHeight = Convert.ToInt32(properties.Attributes["base"].Value);
      this.LineHeight = Convert.ToInt32(properties.Attributes["lineHeight"].Value);
      this.TextureSize = new Size(Convert.ToInt32(properties.Attributes["scaleW"].Value), Convert.ToInt32(properties.Attributes["scaleH"].Value));
      this.Packed = Convert.ToInt32(properties.Attributes["packed"].Value) != 0;
      this.AlphaChannel = Convert.ToInt32(properties.Attributes["alphaChnl"].Value);
      this.RedChannel = Convert.ToInt32(properties.Attributes["redChnl"].Value);
      this.GreenChannel = Convert.ToInt32(properties.Attributes["greenChnl"].Value);
      this.BlueChannel = Convert.ToInt32(properties.Attributes["blueChnl"].Value);

      // load texture information
      foreach (XmlNode node in root.SelectNodes("pages/page"))
      {
        Page page;

        page = new Page();
        page.Id = Convert.ToInt32(node.Attributes["id"].Value);
        page.FileName = node.Attributes["file"].Value;

        pageData.Add(page.Id, page);
      }
      this.Pages = BitmapFontLoader.ToArray(pageData.Values);

      // load character information
      foreach (XmlNode node in root.SelectNodes("chars/char"))
      {
        Character character;

        character = new Character();
        character.Char = (char)Convert.ToInt32(node.Attributes["id"].Value);
        character.Bounds = new Rectangle(Convert.ToInt32(node.Attributes["x"].Value), Convert.ToInt32(node.Attributes["y"].Value), Convert.ToInt32(node.Attributes["width"].Value), Convert.ToInt32(node.Attributes["height"].Value));
        character.Offset = new Point(Convert.ToInt32(node.Attributes["xoffset"].Value), Convert.ToInt32(node.Attributes["yoffset"].Value));
        character.XAdvance = Convert.ToInt32(node.Attributes["xadvance"].Value);
        character.TexturePage = Convert.ToInt32(node.Attributes["page"].Value);
        character.Channel = Convert.ToInt32(node.Attributes["chnl"].Value);

        charDictionary.Add(character.Char, character);
      }
      this.Characters = charDictionary;

      // loading kerning information
      foreach (XmlNode node in root.SelectNodes("kernings/kerning"))
      {
        Kerning key;

        key = new Kerning((char)Convert.ToInt32(node.Attributes["first"].Value), (char)Convert.ToInt32(node.Attributes["second"].Value), Convert.ToInt32(node.Attributes["amount"].Value));

        if (!kerningDictionary.ContainsKey(key))
        {
          kerningDictionary.Add(key, key.Amount);
        }
      }
      this.Kernings = kerningDictionary;
    }

    /// <summary>
    /// Loads font information from the specified stream.
    /// </summary>
    /// <remarks>
    /// The source data must be in BMFont XML format.
    /// </remarks>
    /// <exception cref="ArgumentNullException">Thrown when one or more required arguments are null.</exception>
    /// <param name="stream">The stream containing the font to load.</param>
    public void LoadXml(Stream stream)
    {
      if (stream == null)
      {
        throw new ArgumentNullException("stream");
      }

      using (TextReader reader = new StreamReader(stream))
      {
        this.LoadXml(reader);
      }
    }

    /// <summary>
    /// Provides the size, in pixels, of the specified text when drawn with this font.
    /// </summary>
    /// <param name="text">The text to measure.</param>
    /// <returns>
    /// The <see cref="Size"/>, in pixels, of <paramref name="text"/> drawn with this font.
    /// </returns>
    public Size MeasureFont(string text)
    {
      return this.MeasureFont(text, NoMaxWidth);
    }

    /// <summary>
    /// Provides the size, in pixels, of the specified text when drawn with this font, automatically wrapping to keep within the specified with.
    /// </summary>
    /// <param name="text">The text to measure.</param>
    /// <param name="maxWidth">The maximum width.</param>
    /// <returns>
    /// The <see cref="Size"/>, in pixels, of <paramref name="text"/> drawn with this font.
    /// </returns>
    /// <remarks>The MeasureText method uses the <paramref name="maxWidth"/> parameter to automatically wrap when determining text size.</remarks>
    public Size MeasureFont(string text, double maxWidth)
    {
      Size result;

      if (!string.IsNullOrEmpty(text))
      {
        char previousCharacter;
        int currentLineWidth;
        int currentLineHeight;
        int blockWidth;
        int blockHeight;
        int length;
        List<int> lineHeights;

        length = text.Length;
        previousCharacter = ' ';
        currentLineWidth = 0;
        currentLineHeight = this.LineHeight;
        blockWidth = 0;
        blockHeight = 0;
        lineHeights = new List<int>();

        for (int i = 0; i < length; i++)
        {
          char character;

          character = text[i];

          if (character == '\n' || character == '\r')
          {
            if (character == '\n' || i + 1 == length || text[i + 1] != '\n')
            {
              lineHeights.Add(currentLineHeight);
              blockWidth = Math.Max(blockWidth, currentLineWidth);
              currentLineWidth = 0;
              currentLineHeight = this.LineHeight;
            }
          }
          else
          {
            Character data;
            int width;

            data = this[character];
            width = data.XAdvance + this.GetKerning(previousCharacter, character);

            if (maxWidth != NoMaxWidth && currentLineWidth + width >= maxWidth)
            {
              lineHeights.Add(currentLineHeight);
              blockWidth = Math.Max(blockWidth, currentLineWidth);
              currentLineWidth = 0;
              currentLineHeight = this.LineHeight;
            }

            currentLineWidth += width;
            currentLineHeight = Math.Max(currentLineHeight, data.Bounds.Height + data.Offset.Y);
            previousCharacter = character;
          }
        }

        // finish off the current line if required
        if (currentLineHeight != 0)
        {
          lineHeights.Add(currentLineHeight);
        }

        // reduce any lines other than the last back to the base
        for (int i = 0; i < lineHeights.Count - 1; i++)
        {
          lineHeights[i] = this.LineHeight;
        }

        // calculate the final block height
        foreach (int lineHeight in lineHeights)
        {
          blockHeight += lineHeight;
        }

        result = new Size(Math.Max(currentLineWidth, blockWidth), blockHeight);
      }
      else
      {
        result = Size.Empty;
      }

      return result;
    }

    private string GetString(byte[] buffer, int index)
    {
      StringBuilder sb;

      sb = new StringBuilder();

      for (int i = index; i < buffer.Length; i++)
      {
        byte chr;

        chr = buffer[i];

        if (chr == 0)
        {
          break;
        }

        sb.Append((char)chr);
      }

      return sb.ToString();
    }

    private void LoadCharactersBlock(byte[] buffer, int blockSize)
    {
      IDictionary<char, Character> characters;
      int charCount;

      charCount = blockSize / 20; // The number of characters in the file can be computed by taking the size of the block and dividing with the size of the charInfo structure, i.e.: numChars = charsBlock.blockSize/20.
      characters = new Dictionary<char, Character>(charCount);

      for (int i = 0; i < charCount; i++)
      {
        int start;
        Character chr;

        start = i * 20;

        chr = new Character
              {
                Char = (char)WordHelpers.MakeDWordLittleEndian(buffer, start),
                Offset = new Point(WordHelpers.MakeWordLittleEndian(buffer, start + 12), WordHelpers.MakeWordLittleEndian(buffer, start + 14)),
                Bounds = new Rectangle(WordHelpers.MakeWordLittleEndian(buffer, start + 4), WordHelpers.MakeWordLittleEndian(buffer, start + 6), WordHelpers.MakeWordLittleEndian(buffer, start + 8), WordHelpers.MakeWordLittleEndian(buffer, start + 10)),
                XAdvance = WordHelpers.MakeWordLittleEndian(buffer, start + 16),
                TexturePage = buffer[start + 18],
                Channel = buffer[start + 19]
              };

        characters.Add(chr.Char, chr);
      }

      this.Characters = characters;
    }

    private void LoadCommonBlock(byte[] buffer)
    {
      this.LineHeight = WordHelpers.MakeWordLittleEndian(buffer, 0);
      this.BaseHeight = WordHelpers.MakeWordLittleEndian(buffer, 2);
      this.TextureSize = new Size(WordHelpers.MakeWordLittleEndian(buffer, 4), WordHelpers.MakeWordLittleEndian(buffer, 6));
      this.Pages = new Page[WordHelpers.MakeWordLittleEndian(buffer, 8)];
      this.AlphaChannel = buffer[11];
      this.RedChannel = buffer[12];
      this.GreenChannel = buffer[13];
      this.BlueChannel = buffer[14];
    }

    private void LoadInfoBlock(byte[] buffer)
    {
      byte bits;

      this.FontSize = WordHelpers.MakeWordLittleEndian(buffer, 0);
      bits = buffer[2]; // 	bit 0: smooth, bit 1: unicode, bit 2: italic, bit 3: bold, bit 4: fixedHeigth, bits 5-7: reserved
      this.Smoothed = (bits & (1 << 7)) != 0;
      this.Unicode = (bits & (1 << 6)) != 0;
      this.Italic = (bits & (1 << 5)) != 0;
      this.Bold = (bits & (1 << 4)) != 0;
      this.Charset = string.Empty; // TODO: buffer[3]
      this.StretchedHeight = WordHelpers.MakeWordLittleEndian(buffer, 4);
      this.SuperSampling = WordHelpers.MakeWordLittleEndian(buffer, 6);
      this.Padding = new Padding(buffer[10], buffer[7], buffer[8], buffer[9]);
      this.Spacing = new Point(buffer[11], buffer[12]);
      this.OutlineSize = buffer[13];
      this.FamilyName = this.GetString(buffer, 14);
    }

    private void LoadKerningsBlock(byte[] buffer, int blockSize)
    {
      Dictionary<Kerning, int> kernings;
      int pairCount;

      pairCount = blockSize / 10;
      kernings = new Dictionary<Kerning, int>(pairCount);

      for (int i = 0; i < pairCount; i++)
      {
        int start;
        Kerning kerning;

        start = i * 10;

        kerning = new Kerning
                  {
                    FirstCharacter = (char)WordHelpers.MakeDWordLittleEndian(buffer, start),
                    SecondCharacter = (char)WordHelpers.MakeDWordLittleEndian(buffer, start + 4),
                    Amount = WordHelpers.MakeWordLittleEndian(buffer, start + 8),
                  };

        kernings.Add(kerning, kerning.Amount);
      }

      this.Kernings = kernings;
    }

    private void LoadPagesBlock(byte[] buffer)
    {
      int nextStringStart;

      nextStringStart = 0;

      for (int i = 0; i < this.Pages.Length; i++)
      {
        Page page;
        string name;

        page = this.Pages[i];

        name = this.GetString(buffer, nextStringStart);
        nextStringStart += name.Length;

        page.Id = i;
        page.FileName = name;

        this.Pages[i] = page;
      }
    }

    #endregion

    #region IEnumerable<Character> Interface

    /// <summary>
    /// Returns an enumerator that iterates through the collection.
    /// </summary>
    /// <returns>
    /// A <see cref="T:System.Collections.Generic.IEnumerator`1" /> that can be used to iterate through
    /// the collection.
    /// </returns>
    /// <seealso cref="M:System.Collections.Generic.IEnumerable{Cyotek.Drawing.BitmapFont.Character}.GetEnumerator()"/>
    public IEnumerator<Character> GetEnumerator()
    {
      foreach (KeyValuePair<char, Character> pair in this.Characters)
      {
        yield return pair.Value;
      }
    }

    /// <summary>
    /// Gets the enumerator.
    /// </summary>
    /// <returns>
    /// The enumerator.
    /// </returns>
    IEnumerator IEnumerable.GetEnumerator()
    {
      return this.GetEnumerator();
    }

    #endregion
  }
}
