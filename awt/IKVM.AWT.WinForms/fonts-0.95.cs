/*
  Copyright (C) 2002, 2004, 2005, 2006, 2007 Jeroen Frijters
  Copyright (C) 2006 Active Endpoints, Inc.
  Copyright (C) 2006, 2007, 2009 - 2011 Volker Berlin

  This software is provided 'as-is', without any express or implied
  warranty.  In no event will the authors be held liable for any damages
  arising from the use of this software.

  Permission is granted to anyone to use this software for any purpose,
  including commercial applications, and to alter it and redistribute it
  freely, subject to the following restrictions:

  1. The origin of this software must not be misrepresented; you must not
     claim that you wrote the original software. If you use this software
     in a product, an acknowledgment in the product documentation would be
     appreciated but is not required.
  2. Altered source versions must be plainly marked as such, and must not be
     misrepresented as being the original software.
  3. This notice may not be removed or altered from any source distribution.

  Jeroen Frijters
  jeroen@frijters.net 

*/

using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Text;
using java.awt.font;
using java.awt.geom;
using java.util;
#if WINFX
using System.Windows;
using System.Windows.Media;
#endif


namespace ikvm.awt
{

    /*class NetFontMetrics : java.awt.FontMetrics
    {
        private static readonly Bitmap defaultbitmap = new Bitmap(1, 1);
        [ThreadStatic]
        private static Graphics threadLocalDefaultGraphics;

		private static Graphics GetDefaultGraphics()
		{
			Graphics g = threadLocalDefaultGraphics;
			if (g == null)
			{
				g = threadLocalDefaultGraphics = Graphics.FromImage(defaultbitmap);
				g.SmoothingMode = SmoothingMode.None;
				g.PixelOffsetMode = PixelOffsetMode.None;
				g.TextRenderingHint = TextRenderingHint.SingleBitPerPixelGridFit;
			}
			return g;
		}

        public NetFontMetrics(java.awt.Font font) : base(font)
        {
        }

        private Font GetNetFont()
        {
            return font.getNetFont();
        }

        public override int getHeight()
        {
            return GetNetFont().Height;
        }

        public override int getLeading()
        {
            return (int)Math.Round(GetLeadingFloat());
        }

        public override int getMaxAdvance()
        {
            // HACK very lame
            return charWidth('M');
        }

        public override int charWidth(char ch)
        {
            // HACK we average 20 characters to decrease the influence of the pre/post spacing
            return stringWidth(new String(ch, 20)) / 20;
        }

        public override int charsWidth(char[] data, int off, int len)
        {
            return stringWidth(new String(data, off, len));
        }

        public override int getAscent()
        {
            return (int)Math.Round(GetAscentFloat());
        }

        public override int getDescent()
        {
            return (int)Math.Round(GetDescentFloat());
        }

        public override int stringWidth(string s) 
        {
            return (int)Math.Round(GetStringWidth(s, GetDefaultGraphics()));
        }

        public float GetAscentFloat()
        {
            Font f = GetNetFont();
            int ascent = f.FontFamily.GetCellAscent(f.Style);
            return f.Size * ascent / f.FontFamily.GetEmHeight(f.Style);
        }

        public float GetDescentFloat()
        {
            Font f = GetNetFont();
            int descent = f.FontFamily.GetCellDescent(f.Style);
            return f.Size * descent / f.FontFamily.GetEmHeight(f.Style);
        }

        public float GetLeadingFloat()
        {
            float leading = getHeight() - (GetAscentFloat() + GetDescentFloat());
            return Math.Max(0.0f, leading);
        }

        internal float GetStringWidth(String aString, System.Drawing.Graphics g)
        {
            if (aString.Length == 0)
            {
                return 0;
            }
            // System.Windows.Forms.TextRenderer#MeasureText seems to large
            // Graphics#MeasureString is many faster but work only correct with TextRenderingHint.AntiAlias
            bool rounding;
            StringFormat format;
            switch (g.TextRenderingHint)
            {
                // Fractional metrics
                case TextRenderingHint.AntiAlias:
                case TextRenderingHint.SingleBitPerPixel:
                    // this very mystic, if a StringFormat extends from GenericTypographic then the metric are different but like Java with fractional metrics
                    format = new StringFormat(StringFormat.GenericTypographic);
                    rounding = false;
                    break;
                default:
                    format = new StringFormat();
                    rounding = true;
                    break;
            }

            format.FormatFlags = StringFormatFlags.MeasureTrailingSpaces | StringFormatFlags.NoWrap |
                                 StringFormatFlags.FitBlackBox;
            format.Trimming = StringTrimming.None;
            format.SetMeasurableCharacterRanges(new CharacterRange[] {new CharacterRange(0, aString.Length)});
            Region[] regions = g.MeasureCharacterRanges(aString, GetNetFont(),
                                                        new RectangleF(0, 0, int.MaxValue, int.MaxValue), format);
            SizeF size = regions[0].GetBounds(g).Size;
            regions[0].Dispose();
            //with Arial 9.0 and only one character under Vista .NET does not round it, that we rounding manualy
            return rounding ? (int) Math.Round(size.Width) : size.Width;
        }

        internal java.awt.geom.Rectangle2D GetStringBounds(String aString, System.Drawing.Graphics g)
        {
            Font netFont = GetNetFont();
            FontFamily family = netFont.FontFamily;
            FontStyle style = netFont.Style;
            float factor = netFont.Size / family.GetEmHeight(style);
            float height = family.GetLineSpacing(style) * factor;
            float descent = family.GetCellDescent(style) * factor;
            float ascent = family.GetCellAscent(style) * factor;
            float leading = height - ascent - descent;

            return new java.awt.geom.Rectangle2D.Float(0, -ascent - leading / 2, GetStringWidth(aString, g), height);
        }

        public override java.awt.geom.Rectangle2D getStringBounds(String aString, java.awt.Graphics gr)
        {
            if (gr is NetGraphics)
            {
                return GetStringBounds(aString, ((NetGraphics)gr).JGraphics);
            }
            return GetStringBounds(aString, GetDefaultGraphics());
        }
    }*/

/*    class NetFontPeer : java.awt.peer.FontPeer, IDisposable
    {
        internal readonly Font netFont;

        internal NetFontPeer(string name, java.util.Map attrs)
            : base(name, attrs)
        {
            netFont = J2C.ConvertFont(name, getStyle(null), getSize(null));
        }

        public override bool canDisplay(int codePoint)
        {
            //HACK There is no equivalent in C# http://msdn2.microsoft.com/en-us/library/sf4dhbw8(VS.80).aspx
            return true;
        }

        public override int canDisplayUpTo(java.awt.Font font, java.text.CharacterIterator param2, int param3, int param4)
        {
            //HACK There is no equivalent in C# http://msdn2.microsoft.com/en-us/library/e8bh4szw(VS.80).aspx
            return -1;
        }

        public override GlyphVector createGlyphVector(java.awt.Font font, FontRenderContext frc, int[] glyphCodes)
        {
            char[] chars = new char[glyphCodes.Length];
            for (int i = 0; i < chars.Length; i++)
            {
                chars[i] = (char)glyphCodes[i];
            }
            return new NetGlyphVector(netFont, font, frc, chars);
        }

        public override GlyphVector createGlyphVector(java.awt.Font font, FontRenderContext frc, java.text.CharacterIterator text)
        {
            int count = text.getEndIndex() - text.getBeginIndex();
            char[] chars = new char[count];
            text.first();
            for (int i = 0; i < count; i++)
            {
                chars[i] = text.current();
                text.next();
            }
            return new NetGlyphVector(netFont, font, frc, chars);
        }

        public override byte getBaselineFor(java.awt.Font font, char param2)
        {
            throw new NotImplementedException();
        }

        public override java.awt.FontMetrics getFontMetrics(java.awt.Font font)
        {
            return new NetFontMetrics(font);
        }

        public override string getGlyphName(java.awt.Font font, int param2)
        {
            throw new NotImplementedException();
        }

        public override LineMetrics getLineMetrics(java.awt.Font font, java.text.CharacterIterator aCharacterIterator, int aBegin, int aLimit, FontRenderContext aFontRenderContext)
        {
            string s = ToString(aCharacterIterator, aBegin, aLimit);
            return new NetLineMetrics(font, s);
        }

        public override java.awt.geom.Rectangle2D getMaxCharBounds(java.awt.Font font, FontRenderContext param2)
        {
            throw new NotImplementedException();
        }

        public override int getMissingGlyphCode(java.awt.Font font)
        {
            throw new NotImplementedException();
        }

        public override int getNumGlyphs(java.awt.Font font)
        {
            throw new NotImplementedException();
        }

        public override string getPostScriptName(java.awt.Font font)
        {
            throw new NotImplementedException();
        }

        public override bool hasUniformLineMetrics(java.awt.Font font)
        {
            throw new NotImplementedException();
        }

        public override GlyphVector layoutGlyphVector(java.awt.Font font, FontRenderContext frc, char[] text, int beginIndex, int endIndex, int flags)
        {
            char[] chars = new char[endIndex - beginIndex];
            Array.Copy(text, beginIndex, chars, 0, chars.Length);
            return new NetGlyphVector(netFont, font, frc, chars);
        }

        public override string getSubFamilyName(java.awt.Font font, Locale param2)
        {
            throw new NotImplementedException();
        }

        private static string ToString(java.text.CharacterIterator aCharacterIterator, int aBegin, int aLimit)
        {
            aCharacterIterator.setIndex(aBegin);
            StringBuilder sb = new StringBuilder();

            for (int i = aBegin; i <= aLimit; ++i)
            {
                char c = aCharacterIterator.current();
                sb.Append(c);
                aCharacterIterator.next();
            }

            return sb.ToString();
        }

        #region IDisposable Members

        public void Dispose()
        {
            netFont.Dispose();
        }

        #endregion
    }*/

    /*    class NetGlyphVector : GlyphVector
        {
            private readonly Font netFont;
            private readonly java.awt.Font font;
            private readonly FontRenderContext frc;
            private readonly char[] text;
            private NetFontMetrics metrics;


            internal NetGlyphVector(Font netFont, java.awt.Font font, FontRenderContext frc, char[] text)
            {
                this.netFont = netFont;
                this.font = font;
                this.frc = frc;
                this.text = text;
            }

            private NetFontMetrics getMetrics()
            {
                if(metrics == null)
                {
                    metrics = new NetFontMetrics(font);
                }
                return metrics;
            }

            public override bool equals(GlyphVector gv)
            {
                throw new NotImplementedException();
            }

            public override java.awt.Font getFont()
            {
                return font;
            }

            public override FontRenderContext getFontRenderContext()
            {
                return frc;
            }

            public override int getGlyphCode(int i)
            {
                throw new NotImplementedException();
            }

            public override int[] getGlyphCodes(int i1, int i2, int[] iarr)
            {
                throw new NotImplementedException();
            }

            public override GlyphJustificationInfo getGlyphJustificationInfo(int i)
            {
                throw new NotImplementedException();
            }

            public override java.awt.Shape getGlyphLogicalBounds(int i)
            {
                throw new NotImplementedException();
            }

            public override GlyphMetrics getGlyphMetrics(int i)
            {
                throw new NotImplementedException();
            }

            public override java.awt.Shape getGlyphOutline(int i)
            {
                throw new NotImplementedException();
            }

            public override Point2D getGlyphPosition(int i)
            {
                throw new NotImplementedException();
            }

            public override float[] getGlyphPositions(int i1, int i2, float[] farr)
            {
                throw new NotImplementedException();
            }

            public override AffineTransform getGlyphTransform(int i)
            {
                throw new NotImplementedException();
            }

            public override java.awt.Shape getGlyphVisualBounds(int index)
            {
                return getMetrics().GetStringBounds(new String(text, index, 1));
            }

            public override Rectangle2D getLogicalBounds()
            {
                return getMetrics().GetStringBounds(new String(text));
            }

            public override int getNumGlyphs()
            {
                return text.Length;
            }

            public override java.awt.Shape getOutline()
            {
                throw new NotImplementedException();
            }

            public override java.awt.Shape getOutline(float f1, float f2)
            {
                throw new NotImplementedException();
            }

            public override Rectangle2D getVisualBounds()
            {
                return new NetFontMetrics(font).GetStringBounds(new String(text));
            }

            public override void performDefaultLayout()
            {
                throw new NotImplementedException();
            }

            public override void setGlyphPosition(int i, Point2D pd)
            {
                throw new NotImplementedException();
            }

            public override void setGlyphTransform(int i, AffineTransform at)
            {
                throw new NotImplementedException();
            }

            public char[] getText()
            {
                return text;
            }
    }*/

    /*class NetLineMetrics : LineMetrics
    {
        private java.awt.Font mFont;
        private String mString;
        private FontFamily fontFamily;
        private FontStyle style;
        private float factor;

        public NetLineMetrics(java.awt.Font aFont, String aString)
        {
            mFont = aFont;
            mString = aString;
            fontFamily = J2C.CreateFontFamily(aFont.getName());
            style = (FontStyle)mFont.getStyle();
            factor = aFont.getSize2D() / fontFamily.GetEmHeight(style);
        }

        public override float getAscent()
        {
            return fontFamily.GetCellAscent(style) * factor;
        }

        public override int getBaselineIndex()
        {
            return 0; //I have no font see that return another value.
        }

        public override float[] getBaselineOffsets()
        {
            float ascent = getAscent();
            return new float[] { 0, (getDescent() / 2f - ascent) / 2f, -ascent };
        }

        public override float getDescent()
        {
            return fontFamily.GetCellDescent(style) * factor;
        }

        public override float getHeight()
        {
            return fontFamily.GetLineSpacing(style) * factor;
        }

        public override float getLeading()
        {
            return getHeight() - getAscent() - getDescent();
        }

        public override int getNumChars()
        {
            return mString.Length;
        }

#if WINFX
        private Typeface GetTypeface()
        {
            return new Typeface(fontFamily, style, FontWeight.Normal, FontStretch.Medium);
        }
#endif

        public override float getStrikethroughOffset()
        {
#if WINFX              
            return GetTypeface().StrikethroughPosition * factor;
#else
            return getAscent() / -3;
#endif
        }

        public override float getStrikethroughThickness()
        {
#if WINFX              
            return GetTypeface().StrikethroughThickness * factor;
#else
            return mFont.getSize2D() / 18;
#endif
        }

        public override float getUnderlineOffset()
        {
#if WINFX              
            return GetTypeface().UnderlinePosition * factor;
#else
            return mFont.getSize2D() / 8.7F;
#endif
        }

        public override float getUnderlineThickness()
        {
#if WINFX              
            return GetTypeface().UnderlineThickness * factor;
#else
            return mFont.getSize2D() / 18;
#endif
        }
    }*/

}
