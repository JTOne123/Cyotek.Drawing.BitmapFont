namespace Cyotek.Drawing.BitmapFont
{
  internal static class WordHelpers
  {
    #region Static Methods

    public static int MakeDWordLittleEndian(byte[] buffer, int offset)
    {
      return (buffer[offset + 3] << 0x18) | (buffer[offset + 2] << 0x10) | (buffer[offset + 1] << 8) | buffer[offset];
    }

    public static short MakeWordLittleEndian(byte[] buffer, int offset)
    {
      return (short)((buffer[offset + 1] << 8) | buffer[offset]);
    }

    #endregion
  }
}
