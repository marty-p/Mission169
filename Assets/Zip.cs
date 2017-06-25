using System;
using System.IO;

public class Zip
{
    static int dictionary = 1 << 23; // 1 << 23;
    static bool eos = false;
    static SevenZip.CoderPropID[] propIDs =
    {
        SevenZip.CoderPropID.DictionarySize,
        SevenZip.CoderPropID.PosStateBits,
        SevenZip.CoderPropID.LitContextBits,
        SevenZip.CoderPropID.LitPosBits,
        SevenZip.CoderPropID.Algorithm,
        SevenZip.CoderPropID.NumFastBytes,
        SevenZip.CoderPropID.MatchFinder,
        SevenZip.CoderPropID.EndMarker
    };

    // these are the default properties, keeping it simple for now:
    static object[] properties =
    {
    (Int32)(dictionary),
    (Int32)(2), /* PosStateBits 2 */
    (Int32)(3), /* LitContextBits 3 */
    (Int32)(0), /* LitPosBits 0 */
    (Int32)(2), /*Algorithm  2 */
    (Int32)(128), /* NumFastBytes 128 */
    "bt4", /* MatchFinder "bt4" */
    eos   /* endMarker  eos */
};

    public static byte[] Compress(byte[] inputBytes)
    {
        using (MemoryStream inStream = new MemoryStream(inputBytes))
        {
            using (MemoryStream outStream = new MemoryStream())
            {
                SevenZip.Compression.LZMA.Encoder encoder = new SevenZip.Compression.LZMA.Encoder();
                encoder.SetCoderProperties(propIDs, properties);
                encoder.WriteCoderProperties(outStream);
                long fileSize = inStream.Length;
                for (int i = 0; i < 8; i++)
                    outStream.WriteByte((Byte)(fileSize >> (8 * i)));
                encoder.Code(inStream, outStream, -1, -1, null);
                return outStream.ToArray();
            }
        }
    }

    public static byte[] Decompress(byte[] inputBytes)
    {
        using (MemoryStream newInStream = new MemoryStream(inputBytes))
        {
            SevenZip.Compression.LZMA.Decoder decoder = new SevenZip.Compression.LZMA.Decoder();
            newInStream.Seek(0, 0);
            using (MemoryStream newOutStream = new MemoryStream())
            {
                byte[] properties2 = new byte[5];
                if (newInStream.Read(properties2, 0, 5) != 5)
                    throw (new Exception("input .lzma is too short"));
                long outSize = 0;
                for (int i = 0; i < 8; i++)
                {
                    int v = newInStream.ReadByte();
                    if (v < 0)
                        throw (new Exception("Can't Read 1"));
                    outSize |= ((long)(byte)v) << (8 * i);
                }
                decoder.SetDecoderProperties(properties2);
                long compressedSize = newInStream.Length - newInStream.Position;
                decoder.Code(newInStream, newOutStream, compressedSize, outSize, null);
                return newOutStream.ToArray();
            }
        }
    }
}
