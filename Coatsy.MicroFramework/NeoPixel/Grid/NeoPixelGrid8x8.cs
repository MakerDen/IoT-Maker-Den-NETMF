using System;
using Microsoft.SPOT;
using System.Threading;
using Glovebox.IoT;

namespace Coatsy.Netduino.NeoPixel.Grid {
    public class NeoPixelGrid8x8 : NeoPixelGrid {


        #region Font Definition
        ulong[] fontSimple = new ulong[] { 
            0x0000000000000000, // space
            0x0008000808080808, // !            
            0x0000000000001414, // "
            0x0014143E143E1414, // #
            0x00143E543E153E14, // $
            0x0032220408102226, // %  
            0x002C122C0C12120C, // &
            0x0000000000000808, // '            
            0x0008040202020408, // (
            0x0002040808080402, // )
            0x00412A142A142A41, // *
            0x000808083E080808, // +
            0x0408080000000000, // ,
            0x000000003E000000, // -
            0x0002000000000000, // .
            0x0001020408102040, // / 
            0x001c22262a32221c, //0
            0x003e080808080a0c, //1
            0x003e04081020221c, //2
            0x001c22201820221c, //3
            0x0008083e0a020202, //4
            0x001c2220201e023e, //5
            0x001c22221e02023c, //6
            0x000202040810203e, //7
            0x001c22221c22221c, //8
            0x002020203c22221c, //9
            0x00000C0C000C0C00, // :
            0x0004080800080800, // ;
            0x0010080402040810, // <
            0x0000003E003E0000, // =
            0x0002040810080402, // >
            0x000808081020221C, // ?
            0x001C02122A32221C, // @
            0x002222223e22221c, //A
            0x001e22221e22221e, //B
            0x001c22020202221c, //C
            0x001e22222222221e, //D
            0x003e02021e02023e, //E
            0x000202021e02023e, //F
            0x001c22223a02221c, //G
            0x002222223e222222, //H
            0x003e08080808083e, //I
            0x000608080808083e, //J
            0x0022120a060a1222, //K
            0x003e020202020202, //L
            0x00222222222a3622, //M
            0x002222322a262222, //N
            0x001c22222222221c, //O
            0x000202021e22221e, //P
            0x002c122a2222221c, //Q
            0x0022120a1e22221e, //R
            0x001c22100804221c, //S
            0x000808080808083e, //T
            0x001c222222222222, //U
            0x0008142222222222, //V
            0x0022362a22222222, //W
            0x0022221408142222, //X
            0x0008080808142222, //Y
            0x003e02040810203e, //Z
            0x000E02020202020E, //[
            0x0020201008040202, // \
            0x000E08080808080E, //]
            0x0000000000221408, // ^
            0x003E000000000000, // _
            0x0000000000001008, // '
            0x0012121E120C0000, // a
            0x000E120E120E0000, // b
            0x000C1202120C0000, // c
            0x000E1212120E0000, // d
            0x001E020E021E0000, // e
            0x0002020E021E0000, // f
            0x000C121A021C0000, // g
            0x0012121E12120000, // h
            0x001C0808081C0000, // i
            0x000C121010100000, // j
            0x00120A060A120000, // k
            0x001E020202020000, // l
            0x0022222A36220000, // m
            0x0022322A26220000, // n
            0x000C1212120C0000, // o
            0x0002020E120E0000, // p
            0x201C1212120C0000, // q
            0x0012120E120E0000, // r
            0x000E100C021C0000, // s
            0x00080808081C0000, // t
            0x000C121212120000, // u
            0x0008142222220000, // v
            0x0022362A22220000, // w
            0x0022140814220000, // x
            0x0008080814220000, // y
            0x003E0408103E0000, // z

            };

        #endregion 

        public enum Symbols : ulong {
            Heart = 0x00081C3E7F7F3600, // heart   
        }

        public NeoPixelGrid8x8(string name, ushort panels = 1)
            : base(8, 8, panels, name) {
        }

        #region Scroll string primatives

        public void ScrollStringInFromRight(string characters, Pixel colour, int pause) {
            ScrollStringInFromRight(characters, new Pixel[] { colour }, pause);
        }

        public void ScrollStringInFromLeft(string characters, Pixel colour, int pause) {
            ScrollStringInFromLeft(characters, new Pixel[] { colour }, pause);
        }

        public void ScrollStringInFromRight(string characters, Pixel[] colour, int pause) {
            ushort cycleColour = 0;

            // loop through each chacter
            for (int ch = 0; ch < characters.Length; ch++) {

                char charactor = characters.Substring(ch, 1)[0];
                if (charactor >= ' ' && charactor <= 'z') {
                    ScrollBitmapInFromRight(fontSimple[charactor - 32], colour[cycleColour % colour.Length], pause);
                    cycleColour++;
                }
            }
        }

        public void ScrollStringInFromLeft(string characters, Pixel[] colour, int pause) {
            ushort cycleColour = 0;

            // loop through each chacter
            for (int ch = characters.Length - 1; ch >= 0; ch--) {

                char charactor = characters.Substring(ch, 1)[0];
                if (charactor >= ' ' && charactor <= 'z') {
                    ScrollBitmapInFromLeft(fontSimple[charactor - 32], colour[cycleColour % colour.Length], pause);
                    cycleColour++;
                }
            }
        }

        #endregion

        #region Scroll Character primatives
        public void ScrollCharacterFromRight(char charactor, Pixel colour, int pause) {
            if (charactor >= ' ' && charactor <= 'z') {
                ScrollBitmapInFromRight(fontSimple[charactor - 32], colour, pause);
            }
        }

        public void ScrollCharacterFromLeft(char charactor, Pixel colour, int pause) {
            if (charactor >= ' ' && charactor <= 'z') {
                ScrollBitmapInFromLeft(fontSimple[charactor - 32], colour, pause);
            }
        }

        #endregion

        #region Scroll symbol primatives

        public void ScrollSymbolInFromRight(Symbols sym, Pixel colour, int pause) {
            ScrollBitmapInFromRight((ulong)sym, colour, pause);
        }

        public void ScrollSymbolInFromLeft(Symbols sym, Pixel colour, int pause) {
            ScrollBitmapInFromLeft((ulong)sym, colour, pause);
        }

        public void ScrollSymbolInFromRight(Symbols[] sym, Pixel colour, int pause) {
            foreach (var item in sym) {
                ScrollBitmapInFromRight((ulong)item, colour, pause);
            }
        }

        public void ScrollSymbolInFromLeft(Symbols[] sym, Pixel colour, int pause) {
            foreach (var item in sym) {
                ScrollBitmapInFromLeft((ulong)item, colour, pause);
            }
        }

        public void ScrollSymbolInFromRight(Symbols[] sym, Pixel[] colourPalette, int pause) {
            ushort cycleColour = 0;
            foreach (var item in sym) {
                ScrollBitmapInFromRight((ulong)item, colourPalette[cycleColour % colourPalette.Length], pause);
                cycleColour++;
            }
        }

        public void ScrollSymbolInFromLeft(Symbols[] sym, Pixel[] colourPalette, int pause) {
            ushort cycleColour = 0;
            foreach (var item in sym) {
                ScrollBitmapInFromLeft((ulong)item, colourPalette[cycleColour % colourPalette.Length], pause);
                cycleColour++;
            }
        }

        #endregion

        #region Scroll Bitmaps left and right

        public void ScrollBitmapInFromRight(ulong bitmap, Pixel colour, int pause) {
            ushort pos = 0;
            ulong mask;
            bool pixelFound = false;
            int panelOffset = (Panels - 1) * Columns * Rows;

            // space character ?
            if (bitmap == 0) {
                ShiftFrameLeft();
                FrameDraw();
                Util.Delay(pause);
                return;
            }

            // fetch vertical slice of character font
            for (int col = 0; col < Columns; col++) {
                pixelFound = false;

                for (int row = 0; row < Rows; row++) {
                    mask = (ulong)1 << row * Columns + col;
                    pos = (ushort)(row * Columns + (Columns - 1) + panelOffset);

                    if ((bitmap & mask) != 0) {
                        FrameSet(colour, pos);
                        pixelFound = true;
                    }
                }
                if (pixelFound) {
                    FrameDraw();
                    ShiftFrameLeft();
                    Util.Delay(pause);
                }
            }
            //blank character space
            ShiftFrameLeft();
        }

        public void ScrollBitmapInFromLeft(ulong bitmap, Pixel colour, int pause) {
            ushort pos = 0;
            ulong mask;
            bool pixelFound = false;


            // space character ?
            if (bitmap == 0) {
                ShiftFrameRight();
                FrameDraw();
                Util.Delay(pause);
                return;
            }

            // fetch vertical slice of character font
            for (int col = Columns - 1; col >= 0; col--) {
                pixelFound = false;

                for (int row = 0; row < Rows; row++) {
                    mask = (ulong)1 << row * Columns + col;
                    pos = (ushort)(row * Columns);

                    if ((bitmap & mask) != 0) {
                        FrameSet(colour, pos);
                        pixelFound = true;
                    }
                }
                if (pixelFound) {
                    FrameDraw();
                    ShiftFrameRight();
                    Util.Delay(pause);
                }
            }
            //blank character space
            ShiftFrameRight();
        }

        #endregion

        #region Draw Primatives


        public void DrawString(string characters, Pixel colour, int pause, ushort panel = 0) {
            DrawString(characters, new Pixel[] { colour }, pause, panel);
        }

        public void DrawString(string characters, Pixel[] colour, int pause, ushort panel = 0) {
            ushort cycleColour = 0;
            char c;
            for (int i = 0; i < characters.Length; i++) {
                c = characters.Substring(i, 1)[0];
                if (c >= ' ' && c <= 'z') {
                    DrawLetter(c, colour[cycleColour % colour.Length], panel);
                    FrameDraw();
                    Util.Delay(pause);
                    cycleColour++;
                }
            }
        }

        public void DrawLetter(char character, Pixel colour, ushort panel = 0) {
            ulong letter = 0;

            if (character >= ' ' && character <= 'z') {
                //calc ascii offset
                byte charValue = (byte)(character - 32);
                letter = fontSimple[charValue];
            }
            else { return; }

            DrawBitmap(letter, colour, panel);
        }

        public void DrawSymbol(Symbols[] sym, Pixel[] colour, int pause, ushort panel = 0) {
            ushort cycleColour = 0;
            foreach (var item in sym) {
                DrawBitmap((ulong)item, colour[cycleColour], panel);
                FrameDraw();
                Util.Delay(pause);
                cycleColour++;                
            }            
        }

        public void DrawSymbol(Symbols sym, Pixel colour, ushort panel = 0) {
            DrawBitmap((ulong)sym, colour, panel);
        }

        public void DrawBitmap(ulong bitmap, Pixel colour, ushort panel = 0) {
            ulong mask;
            ushort pos = (ushort)(panel * Rows * Columns);
            int len = (ushort)(panel * Rows * Columns) + (Rows * Columns);

            while (pos < len) {
                mask = (ulong)1 << pos;
                if ((bitmap & mask) == 0) {
                    FrameSet(Pixel.Colour.Black, pos);
                }
                else {
                    FrameSet(colour, pos);
                }
                pos++;
            }
        }
        #endregion
    }
}
