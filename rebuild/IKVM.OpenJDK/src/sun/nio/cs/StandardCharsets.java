/*
 * Copyright (c) 2000, 2007, Oracle and/or its affiliates. All rights reserved.
 *
 * DO NOT ALTER OR REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
 *
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.  Oracle designates this
 * particular file as subject to the "Classpath" exception as provided
 * by Oracle in the LICENSE file that accompanied this code.
 *
 * This code is distributed in the hope that it will be useful, but WITHOUT
 * ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or
 * FITNESS FOR A PARTICULAR PURPOSE.  See the GNU General Public License
 * version 2 for more details (a copy is included in the LICENSE file that
 * accompanied this code).
 *
 * You should have received a copy of the GNU General Public License version
 * 2 along with this work; if not, write to the Free Software Foundation,
 * Inc., 51 Franklin St, Fifth Floor, Boston, MA 02110-1301 USA.
 *
 * Please contact Oracle, 500 Oracle Parkway, Redwood Shores, CA 94065 USA
 * or visit www.oracle.com if you need additional information or have any
 * questions.
 *
 */

// -- This file was mechanically generated: Do not edit! -- //

package sun.nio.cs;

import java.nio.charset.*;


public class StandardCharsets
    extends FastCharsetProvider
{

    static final String[] aliases_US_ASCII = new String[] {
        "iso-ir-6",
        "ANSI_X3.4-1986",
        "ISO_646.irv:1991",
        "ASCII",
        "ISO646-US",
        "us",
        "IBM367",
        "cp367",
        "csASCII",
        "default",
        "646",
        "iso_646.irv:1983",
        "ANSI_X3.4-1968",
        "ascii7",
    };

    static final String[] aliases_UTF_8 = new String[] {
        "UTF8",
        "unicode-1-1-utf-8",
    };

    static final String[] aliases_CESU_8 = new String[] {
        "CESU8",
        "csCESU-8",
    };

    static final String[] aliases_UTF_16 = new String[] {
        "UTF_16",
        "utf16",
        "unicode",
        "UnicodeBig",
    };

    static final String[] aliases_UTF_16BE = new String[] {
        "UTF_16BE",
        "ISO-10646-UCS-2",
        "X-UTF-16BE",
        "UnicodeBigUnmarked",
    };

    static final String[] aliases_UTF_16LE = new String[] {
        "UTF_16LE",
        "X-UTF-16LE",
        "UnicodeLittleUnmarked",
    };

    static final String[] aliases_UTF_16LE_BOM = new String[] {
        "UnicodeLittle",
    };

    static final String[] aliases_UTF_32 = new String[] {
        "UTF_32",
        "UTF32",
    };

    static final String[] aliases_UTF_32LE = new String[] {
        "UTF_32LE",
        "X-UTF-32LE",
    };

    static final String[] aliases_UTF_32BE = new String[] {
        "UTF_32BE",
        "X-UTF-32BE",
    };

    static final String[] aliases_UTF_32LE_BOM = new String[] {
        "UTF_32LE_BOM",
        "UTF-32LE-BOM",
    };

    static final String[] aliases_UTF_32BE_BOM = new String[] {
        "UTF_32BE_BOM",
        "UTF-32BE-BOM",
    };

    static final String[] aliases_ISO_8859_1 = new String[] {
        "iso-ir-100",
        "ISO_8859-1",
        "latin1",
        "l1",
        "IBM819",
        "cp819",
        "csISOLatin1",
        "819",
        "IBM-819",
        "ISO8859_1",
        "ISO_8859-1:1987",
        "ISO_8859_1",
        "8859_1",
        "ISO8859-1",
    };

    static final String[] aliases_ISO_8859_2 = new String[] {
        "iso8859_2",
        "8859_2",
        "iso-ir-101",
        "ISO_8859-2",
        "ISO_8859-2:1987",
        "ISO8859-2",
        "latin2",
        "l2",
        "ibm912",
        "ibm-912",
        "cp912",
        "912",
        "csISOLatin2",
    };

    static final String[] aliases_ISO_8859_4 = new String[] {
        "iso8859_4",
        "iso8859-4",
        "8859_4",
        "iso-ir-110",
        "ISO_8859-4",
        "ISO_8859-4:1988",
        "latin4",
        "l4",
        "ibm914",
        "ibm-914",
        "cp914",
        "914",
        "csISOLatin4",
    };

    static final String[] aliases_ISO_8859_5 = new String[] {
        "iso8859_5",
        "8859_5",
        "iso-ir-144",
        "ISO_8859-5",
        "ISO_8859-5:1988",
        "ISO8859-5",
        "cyrillic",
        "ibm915",
        "ibm-915",
        "cp915",
        "915",
        "csISOLatinCyrillic",
    };

    static final String[] aliases_ISO_8859_7 = new String[] {
        "iso8859_7",
        "8859_7",
        "iso-ir-126",
        "ISO_8859-7",
        "ISO_8859-7:1987",
        "ELOT_928",
        "ECMA-118",
        "greek",
        "greek8",
        "csISOLatinGreek",
        "sun_eu_greek",
        "ibm813",
        "ibm-813",
        "813",
        "cp813",
        "iso8859-7",
    };

    static final String[] aliases_ISO_8859_9 = new String[] {
        "iso8859_9",
        "8859_9",
        "iso-ir-148",
        "ISO_8859-9",
        "ISO_8859-9:1989",
        "ISO8859-9",
        "latin5",
        "l5",
        "ibm920",
        "ibm-920",
        "920",
        "cp920",
        "csISOLatin5",
    };

    static final String[] aliases_ISO_8859_13 = new String[] {
        "iso8859_13",
        "8859_13",
        "iso_8859-13",
        "ISO8859-13",
    };

    static final String[] aliases_ISO_8859_15 = new String[] {
        "ISO_8859-15",
        "8859_15",
        "ISO-8859-15",
        "ISO8859_15",
        "ISO8859-15",
        "IBM923",
        "IBM-923",
        "cp923",
        "923",
        "LATIN0",
        "LATIN9",
        "L9",
        "csISOlatin0",
        "csISOlatin9",
        "ISO8859_15_FDIS",
    };

    static final String[] aliases_KOI8_R = new String[] {
        "koi8_r",
        "koi8",
        "cskoi8r",
    };

    static final String[] aliases_KOI8_U = new String[] {
        "koi8_u",
    };

    static final String[] aliases_MS1250 = new String[] {
        "cp1250",
        "cp5346",
    };

    static final String[] aliases_MS1251 = new String[] {
        "cp1251",
        "cp5347",
        "ansi-1251",
    };

    static final String[] aliases_MS1252 = new String[] {
        "cp1252",
        "cp5348",
    };

    static final String[] aliases_MS1253 = new String[] {
        "cp1253",
        "cp5349",
    };

    static final String[] aliases_MS1254 = new String[] {
        "cp1254",
        "cp5350",
    };

    static final String[] aliases_MS1257 = new String[] {
        "cp1257",
        "cp5353",
    };

    static final String[] aliases_IBM437 = new String[] {
        "cp437",
        "ibm437",
        "ibm-437",
        "437",
        "cspc8codepage437",
        "windows-437",
    };

    static final String[] aliases_IBM737 = new String[] {
        "cp737",
        "ibm737",
        "ibm-737",
        "737",
    };

    static final String[] aliases_IBM775 = new String[] {
        "cp775",
        "ibm775",
        "ibm-775",
        "775",
    };

    static final String[] aliases_IBM850 = new String[] {
        "cp850",
        "ibm-850",
        "ibm850",
        "850",
        "cspc850multilingual",
    };

    static final String[] aliases_IBM852 = new String[] {
        "cp852",
        "ibm852",
        "ibm-852",
        "852",
        "csPCp852",
    };

    static final String[] aliases_IBM855 = new String[] {
        "cp855",
        "ibm-855",
        "ibm855",
        "855",
        "cspcp855",
    };

    static final String[] aliases_IBM857 = new String[] {
        "cp857",
        "ibm857",
        "ibm-857",
        "857",
        "csIBM857",
    };

    static final String[] aliases_IBM858 = new String[] {
        "cp858",
        "ccsid00858",
        "cp00858",
        "858",
        "PC-Multilingual-850+euro",
    };

    static final String[] aliases_IBM862 = new String[] {
        "cp862",
        "ibm862",
        "ibm-862",
        "862",
        "csIBM862",
        "cspc862latinhebrew",
    };

    static final String[] aliases_IBM866 = new String[] {
        "cp866",
        "ibm866",
        "ibm-866",
        "866",
        "csIBM866",
    };

    static final String[] aliases_IBM874 = new String[] {
        "cp874",
        "ibm874",
        "ibm-874",
        "874",
    };

    private static final class Aliases
        extends sun.util.PreHashedMap<String>
    {

        private static final int ROWS = 1024;
        private static final int SIZE = 211;
        private static final int SHIFT = 0;
        private static final int MASK = 0x3ff;

        private Aliases() {
            super(ROWS, SIZE, SHIFT, MASK);
        }

        protected void init(Object[] ht) {
            ht[1] = new Object[] { "csisolatin0", "iso-8859-15" };
            ht[2] = new Object[] { "csisolatin1", "iso-8859-1" };
            ht[3] = new Object[] { "csisolatin2", "iso-8859-2" };
            ht[5] = new Object[] { "csisolatin4", "iso-8859-4" };
            ht[6] = new Object[] { "csisolatin5", "iso-8859-9" };
            ht[10] = new Object[] { "csisolatin9", "iso-8859-15" };
            ht[19] = new Object[] { "unicodelittle", "x-utf-16le-bom" };
            ht[24] = new Object[] { "iso646-us", "us-ascii" };
            ht[25] = new Object[] { "iso_8859-7:1987", "iso-8859-7" };
            ht[26] = new Object[] { "912", "iso-8859-2" };
            ht[28] = new Object[] { "914", "iso-8859-4" };
            ht[29] = new Object[] { "915", "iso-8859-5" };
            ht[55] = new Object[] { "920", "iso-8859-9" };
            ht[58] = new Object[] { "923", "iso-8859-15" };
            ht[86] = new Object[] { "csisolatincyrillic", "iso-8859-5",
                         new Object[] { "8859_1", "iso-8859-1" } };
            ht[87] = new Object[] { "8859_2", "iso-8859-2" };
            ht[89] = new Object[] { "8859_4", "iso-8859-4" };
            ht[90] = new Object[] { "813", "iso-8859-7",
                         new Object[] { "8859_5", "iso-8859-5" } };
            ht[92] = new Object[] { "8859_7", "iso-8859-7" };
            ht[94] = new Object[] { "8859_9", "iso-8859-9" };
            ht[95] = new Object[] { "iso_8859-1:1987", "iso-8859-1" };
            ht[96] = new Object[] { "819", "iso-8859-1" };
            ht[106] = new Object[] { "unicode-1-1-utf-8", "utf-8" };
            ht[121] = new Object[] { "x-utf-16le", "utf-16le" };
            ht[125] = new Object[] { "ecma-118", "iso-8859-7" };
            ht[134] = new Object[] { "koi8_r", "koi8-r" };
            ht[137] = new Object[] { "koi8_u", "koi8-u" };
            ht[141] = new Object[] { "cp912", "iso-8859-2" };
            ht[143] = new Object[] { "cp914", "iso-8859-4" };
            ht[144] = new Object[] { "cp915", "iso-8859-5" };
            ht[170] = new Object[] { "cp920", "iso-8859-9" };
            ht[173] = new Object[] { "cp923", "iso-8859-15" };
            ht[177] = new Object[] { "utf_32le_bom", "x-utf-32le-bom" };
            ht[192] = new Object[] { "utf_16be", "utf-16be" };
            ht[199] = new Object[] { "cspc8codepage437", "ibm437",
                         new Object[] { "ansi-1251", "windows-1251" } };
            ht[205] = new Object[] { "cp813", "iso-8859-7" };
            ht[211] = new Object[] { "850", "ibm850",
                         new Object[] { "cp819", "iso-8859-1" } };
            ht[213] = new Object[] { "852", "ibm852" };
            ht[216] = new Object[] { "855", "ibm855" };
            ht[218] = new Object[] { "857", "ibm857",
                         new Object[] { "iso-ir-6", "us-ascii" } };
            ht[219] = new Object[] { "858", "ibm00858",
                         new Object[] { "737", "x-ibm737" } };
            ht[225] = new Object[] { "csascii", "us-ascii" };
            ht[244] = new Object[] { "862", "ibm862" };
            ht[248] = new Object[] { "866", "ibm866" };
            ht[253] = new Object[] { "x-utf-32be", "utf-32be" };
            ht[254] = new Object[] { "iso_8859-2:1987", "iso-8859-2" };
            ht[259] = new Object[] { "unicodebig", "utf-16" };
            ht[269] = new Object[] { "iso8859_15_fdis", "iso-8859-15" };
            ht[277] = new Object[] { "874", "x-ibm874" };
            ht[280] = new Object[] { "unicodelittleunmarked", "utf-16le" };
            ht[283] = new Object[] { "iso8859_1", "iso-8859-1" };
            ht[284] = new Object[] { "iso8859_2", "iso-8859-2" };
            ht[286] = new Object[] { "iso8859_4", "iso-8859-4" };
            ht[287] = new Object[] { "iso8859_5", "iso-8859-5" };
            ht[289] = new Object[] { "iso8859_7", "iso-8859-7" };
            ht[291] = new Object[] { "iso8859_9", "iso-8859-9" };
            ht[294] = new Object[] { "ibm912", "iso-8859-2" };
            ht[296] = new Object[] { "ibm914", "iso-8859-4" };
            ht[297] = new Object[] { "ibm915", "iso-8859-5" };
            ht[305] = new Object[] { "iso_8859-13", "iso-8859-13" };
            ht[307] = new Object[] { "iso_8859-15", "iso-8859-15" };
            ht[312] = new Object[] { "greek8", "iso-8859-7",
                         new Object[] { "646", "us-ascii" } };
            ht[321] = new Object[] { "ibm-912", "iso-8859-2" };
            ht[323] = new Object[] { "ibm920", "iso-8859-9",
                         new Object[] { "ibm-914", "iso-8859-4" } };
            ht[324] = new Object[] { "ibm-915", "iso-8859-5" };
            ht[325] = new Object[] { "l1", "iso-8859-1" };
            ht[326] = new Object[] { "cp850", "ibm850",
                         new Object[] { "ibm923", "iso-8859-15",
                             new Object[] { "l2", "iso-8859-2" } } };
            ht[327] = new Object[] { "cyrillic", "iso-8859-5" };
            ht[328] = new Object[] { "cp852", "ibm852",
                         new Object[] { "l4", "iso-8859-4" } };
            ht[329] = new Object[] { "l5", "iso-8859-9" };
            ht[331] = new Object[] { "cp855", "ibm855" };
            ht[333] = new Object[] { "cp857", "ibm857",
                         new Object[] { "l9", "iso-8859-15" } };
            ht[334] = new Object[] { "cp858", "ibm00858",
                         new Object[] { "cp737", "x-ibm737" } };
            ht[336] = new Object[] { "iso_8859_1", "iso-8859-1" };
            ht[339] = new Object[] { "koi8", "koi8-r" };
            ht[341] = new Object[] { "775", "ibm775" };
            ht[345] = new Object[] { "iso_8859-9:1989", "iso-8859-9" };
            ht[350] = new Object[] { "ibm-920", "iso-8859-9" };
            ht[353] = new Object[] { "ibm-923", "iso-8859-15" };
            ht[358] = new Object[] { "ibm813", "iso-8859-7" };
            ht[359] = new Object[] { "cp862", "ibm862" };
            ht[363] = new Object[] { "cp866", "ibm866" };
            ht[364] = new Object[] { "ibm819", "iso-8859-1" };
            ht[378] = new Object[] { "ansi_x3.4-1968", "us-ascii" };
            ht[385] = new Object[] { "ibm-813", "iso-8859-7" };
            ht[391] = new Object[] { "ibm-819", "iso-8859-1" };
            ht[392] = new Object[] { "cp874", "x-ibm874" };
            ht[405] = new Object[] { "iso-ir-100", "iso-8859-1" };
            ht[406] = new Object[] { "iso-ir-101", "iso-8859-2" };
            ht[408] = new Object[] { "437", "ibm437" };
            ht[421] = new Object[] { "iso-8859-15", "iso-8859-15" };
            ht[428] = new Object[] { "latin0", "iso-8859-15" };
            ht[429] = new Object[] { "latin1", "iso-8859-1" };
            ht[430] = new Object[] { "latin2", "iso-8859-2" };
            ht[432] = new Object[] { "latin4", "iso-8859-4" };
            ht[433] = new Object[] { "latin5", "iso-8859-9" };
            ht[436] = new Object[] { "iso-ir-110", "iso-8859-4" };
            ht[437] = new Object[] { "latin9", "iso-8859-15" };
            ht[438] = new Object[] { "ansi_x3.4-1986", "us-ascii" };
            ht[443] = new Object[] { "utf-32be-bom", "x-utf-32be-bom" };
            ht[456] = new Object[] { "cp775", "ibm775" };
            ht[473] = new Object[] { "iso-ir-126", "iso-8859-7" };
            ht[479] = new Object[] { "ibm850", "ibm850" };
            ht[481] = new Object[] { "ibm852", "ibm852" };
            ht[484] = new Object[] { "ibm855", "ibm855" };
            ht[486] = new Object[] { "ibm857", "ibm857" };
            ht[487] = new Object[] { "ibm737", "x-ibm737" };
            ht[502] = new Object[] { "utf_16le", "utf-16le" };
            ht[506] = new Object[] { "ibm-850", "ibm850" };
            ht[508] = new Object[] { "ibm-852", "ibm852" };
            ht[511] = new Object[] { "ibm-855", "ibm855" };
            ht[512] = new Object[] { "ibm862", "ibm862" };
            ht[513] = new Object[] { "ibm-857", "ibm857" };
            ht[514] = new Object[] { "ibm-737", "x-ibm737" };
            ht[516] = new Object[] { "ibm866", "ibm866" };
            ht[520] = new Object[] { "unicodebigunmarked", "utf-16be" };
            ht[523] = new Object[] { "cp437", "ibm437" };
            ht[524] = new Object[] { "utf16", "utf-16" };
            ht[533] = new Object[] { "iso-ir-144", "iso-8859-5" };
            ht[537] = new Object[] { "iso-ir-148", "iso-8859-9" };
            ht[539] = new Object[] { "ibm-862", "ibm862" };
            ht[543] = new Object[] { "ibm-866", "ibm866" };
            ht[545] = new Object[] { "ibm874", "x-ibm874" };
            ht[563] = new Object[] { "x-utf-32le", "utf-32le" };
            ht[572] = new Object[] { "ibm-874", "x-ibm874" };
            ht[573] = new Object[] { "iso_8859-4:1988", "iso-8859-4" };
            ht[577] = new Object[] { "default", "us-ascii" };
            ht[582] = new Object[] { "utf32", "utf-32" };
            ht[583] = new Object[] { "pc-multilingual-850+euro", "ibm00858" };
            ht[588] = new Object[] { "elot_928", "iso-8859-7" };
            ht[593] = new Object[] { "csisolatingreek", "iso-8859-7" };
            ht[598] = new Object[] { "csibm857", "ibm857" };
            ht[609] = new Object[] { "ibm775", "ibm775" };
            ht[617] = new Object[] { "cp1250", "windows-1250" };
            ht[618] = new Object[] { "cp1251", "windows-1251" };
            ht[619] = new Object[] { "cp1252", "windows-1252" };
            ht[620] = new Object[] { "cp1253", "windows-1253" };
            ht[621] = new Object[] { "cp1254", "windows-1254" };
            ht[624] = new Object[] { "csibm862", "ibm862",
                         new Object[] { "cp1257", "windows-1257" } };
            ht[628] = new Object[] { "csibm866", "ibm866",
                         new Object[] { "cesu8", "cesu-8" } };
            ht[632] = new Object[] { "iso8859_13", "iso-8859-13" };
            ht[634] = new Object[] { "iso8859_15", "iso-8859-15",
                         new Object[] { "utf_32be", "utf-32be" } };
            ht[635] = new Object[] { "utf_32be_bom", "x-utf-32be-bom" };
            ht[636] = new Object[] { "ibm-775", "ibm775" };
            ht[654] = new Object[] { "cp00858", "ibm00858" };
            ht[669] = new Object[] { "8859_13", "iso-8859-13" };
            ht[670] = new Object[] { "us", "us-ascii" };
            ht[671] = new Object[] { "8859_15", "iso-8859-15" };
            ht[676] = new Object[] { "ibm437", "ibm437" };
            ht[679] = new Object[] { "cp367", "us-ascii" };
            ht[686] = new Object[] { "iso-10646-ucs-2", "utf-16be" };
            ht[703] = new Object[] { "ibm-437", "ibm437" };
            ht[710] = new Object[] { "iso8859-13", "iso-8859-13" };
            ht[712] = new Object[] { "iso8859-15", "iso-8859-15" };
            ht[732] = new Object[] { "iso_8859-5:1988", "iso-8859-5" };
            ht[733] = new Object[] { "unicode", "utf-16" };
            ht[768] = new Object[] { "greek", "iso-8859-7" };
            ht[774] = new Object[] { "ascii7", "us-ascii" };
            ht[781] = new Object[] { "iso8859-1", "iso-8859-1" };
            ht[782] = new Object[] { "iso8859-2", "iso-8859-2" };
            ht[783] = new Object[] { "cskoi8r", "koi8-r" };
            ht[784] = new Object[] { "iso8859-4", "iso-8859-4" };
            ht[785] = new Object[] { "iso8859-5", "iso-8859-5" };
            ht[787] = new Object[] { "iso8859-7", "iso-8859-7" };
            ht[789] = new Object[] { "iso8859-9", "iso-8859-9" };
            ht[813] = new Object[] { "ccsid00858", "ibm00858" };
            ht[818] = new Object[] { "cspc862latinhebrew", "ibm862" };
            ht[832] = new Object[] { "ibm367", "us-ascii" };
            ht[834] = new Object[] { "iso_8859-1", "iso-8859-1" };
            ht[835] = new Object[] { "iso_8859-2", "iso-8859-2",
                         new Object[] { "x-utf-16be", "utf-16be" } };
            ht[836] = new Object[] { "sun_eu_greek", "iso-8859-7" };
            ht[837] = new Object[] { "iso_8859-4", "iso-8859-4" };
            ht[838] = new Object[] { "iso_8859-5", "iso-8859-5" };
            ht[840] = new Object[] { "cspcp852", "ibm852",
                         new Object[] { "iso_8859-7", "iso-8859-7" } };
            ht[842] = new Object[] { "iso_8859-9", "iso-8859-9" };
            ht[843] = new Object[] { "cspcp855", "ibm855" };
            ht[846] = new Object[] { "windows-437", "ibm437" };
            ht[849] = new Object[] { "ascii", "us-ascii" };
            ht[863] = new Object[] { "cscesu-8", "cesu-8" };
            ht[881] = new Object[] { "utf8", "utf-8" };
            ht[896] = new Object[] { "iso_646.irv:1983", "us-ascii" };
            ht[909] = new Object[] { "cp5346", "windows-1250" };
            ht[910] = new Object[] { "cp5347", "windows-1251" };
            ht[911] = new Object[] { "cp5348", "windows-1252" };
            ht[912] = new Object[] { "cp5349", "windows-1253" };
            ht[925] = new Object[] { "iso_646.irv:1991", "us-ascii" };
            ht[934] = new Object[] { "cp5350", "windows-1254" };
            ht[937] = new Object[] { "cp5353", "windows-1257" };
            ht[944] = new Object[] { "utf_32le", "utf-32le" };
            ht[957] = new Object[] { "utf_16", "utf-16" };
            ht[993] = new Object[] { "cspc850multilingual", "ibm850" };
            ht[1009] = new Object[] { "utf-32le-bom", "x-utf-32le-bom" };
            ht[1015] = new Object[] { "utf_32", "utf-32" };
        }

    }

    private static final class Classes
        extends sun.util.PreHashedMap<String>
    {

        private static final int ROWS = 32;
        private static final int SIZE = 39;
        private static final int SHIFT = 1;
        private static final int MASK = 0x1f;

        private Classes() {
            super(ROWS, SIZE, SHIFT, MASK);
        }

        protected void init(Object[] ht) {
            ht[0] = new Object[] { "ibm862", "IBM862" };
            ht[2] = new Object[] { "ibm866", "IBM866",
                         new Object[] { "utf-32", "UTF_32",
                             new Object[] { "utf-16le", "UTF_16LE" } } };
            ht[3] = new Object[] { "windows-1251", "MS1251",
                         new Object[] { "windows-1250", "MS1250" } };
            ht[4] = new Object[] { "windows-1253", "MS1253",
                         new Object[] { "windows-1252", "MS1252",
                             new Object[] { "utf-32be", "UTF_32BE" } } };
            ht[5] = new Object[] { "windows-1254", "MS1254",
                         new Object[] { "utf-16", "UTF_16" } };
            ht[6] = new Object[] { "windows-1257", "MS1257" };
            ht[7] = new Object[] { "utf-16be", "UTF_16BE" };
            ht[8] = new Object[] { "iso-8859-2", "ISO_8859_2",
                         new Object[] { "iso-8859-1", "ISO_8859_1" } };
            ht[9] = new Object[] { "iso-8859-4", "ISO_8859_4",
                         new Object[] { "utf-8", "UTF_8" } };
            ht[10] = new Object[] { "iso-8859-5", "ISO_8859_5" };
            ht[11] = new Object[] { "x-ibm874", "IBM874",
                         new Object[] { "iso-8859-7", "ISO_8859_7" } };
            ht[12] = new Object[] { "iso-8859-9", "ISO_8859_9" };
            ht[14] = new Object[] { "x-ibm737", "IBM737" };
            ht[15] = new Object[] { "ibm850", "IBM850" };
            ht[16] = new Object[] { "ibm852", "IBM852",
                         new Object[] { "ibm775", "IBM775" } };
            ht[17] = new Object[] { "iso-8859-13", "ISO_8859_13",
                         new Object[] { "us-ascii", "US_ASCII" } };
            ht[18] = new Object[] { "ibm855", "IBM855",
                         new Object[] { "ibm437", "IBM437",
                             new Object[] { "iso-8859-15", "ISO_8859_15" } } };
            ht[19] = new Object[] { "ibm00858", "IBM858",
                         new Object[] { "ibm857", "IBM857",
                             new Object[] { "x-utf-32le-bom", "UTF_32LE_BOM" } } };
            ht[22] = new Object[] { "x-utf-16le-bom", "UTF_16LE_BOM" };
            ht[23] = new Object[] { "cesu-8", "CESU_8" };
            ht[24] = new Object[] { "x-utf-32be-bom", "UTF_32BE_BOM" };
            ht[28] = new Object[] { "koi8-r", "KOI8_R" };
            ht[29] = new Object[] { "koi8-u", "KOI8_U" };
            ht[31] = new Object[] { "utf-32le", "UTF_32LE" };
        }

    }

    private static final class Cache
        extends sun.util.PreHashedMap<Charset>
    {

        private static final int ROWS = 32;
        private static final int SIZE = 39;
        private static final int SHIFT = 1;
        private static final int MASK = 0x1f;

        private Cache() {
            super(ROWS, SIZE, SHIFT, MASK);
        }

        protected void init(Object[] ht) {
            // [IKVM] we pre-populate some cache entries to avoid reflection when instantiating the common charsets
            ht[0] = new Object[] { "ibm862", null };
            ht[2] = new Object[] { "ibm866", null,
                         new Object[] { "utf-32", null,
                             new Object[] { "utf-16le", null } } };
            ht[3] = new Object[] { "windows-1251", null,
                         new Object[] { "windows-1250", null } };
            ht[4] = new Object[] { "windows-1253", null,
                         new Object[] { "windows-1252", new MS1252(),
                             new Object[] { "utf-32be", null } } };
            ht[5] = new Object[] { "windows-1254", null,
                         new Object[] { "utf-16", null } };
            ht[6] = new Object[] { "windows-1257", null };
            ht[7] = new Object[] { "utf-16be", null };
            ht[8] = new Object[] { "iso-8859-2", null,
                         new Object[] { "iso-8859-1", null } };
            ht[9] = new Object[] { "iso-8859-4", null,
                         new Object[] { "utf-8", new UTF_8() } };
            ht[10] = new Object[] { "iso-8859-5", null };
            ht[11] = new Object[] { "x-ibm874", null,
                         new Object[] { "iso-8859-7", null } };
            ht[12] = new Object[] { "iso-8859-9", null };
            ht[14] = new Object[] { "x-ibm737", null };
            ht[15] = new Object[] { "ibm850", null };
            ht[16] = new Object[] { "ibm852", null,
                         new Object[] { "ibm775", null } };
            ht[17] = new Object[] { "iso-8859-13", null,
                         new Object[] { "us-ascii", null } };
            ht[18] = new Object[] { "ibm855", null,
                         new Object[] { "ibm437", null,
                             new Object[] { "iso-8859-15", null } } };
            ht[19] = new Object[] { "ibm00858", null,
                         new Object[] { "ibm857", null,
                             new Object[] { "x-utf-32le-bom", null } } };
            ht[22] = new Object[] { "x-utf-16le-bom", null };
            ht[23] = new Object[] { "cesu-8", null };
            ht[24] = new Object[] { "x-utf-32be-bom", null };
            ht[28] = new Object[] { "koi8-r", null };
            ht[29] = new Object[] { "koi8-u", null };
            ht[31] = new Object[] { "utf-32le", null };
        }

    }

    public StandardCharsets() {
        super("sun.nio.cs", new Aliases(), new Classes(), new Cache());
    }

}
