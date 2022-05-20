

/*
 * Copyright (c) 1998, 2002, Oracle and/or its affiliates. All rights reserved.
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
 */

using System;

static partial class fdlibm
{
	static int __HI(double x)
	{
		return (int)(BitConverter.DoubleToInt64Bits(x) >> 32);
	}

	static int __LO(double x)
	{
		return (int)BitConverter.DoubleToInt64Bits(x);
	}

	static double __HI(double x, int i)
	{
		long l = BitConverter.DoubleToInt64Bits(x) & 0xFFFFFFFFL;
		long h = (long)i << 32;
		return BitConverter.Int64BitsToDouble(l | h);
	}

	static double __LO(double x, int i)
	{
		long h = BitConverter.DoubleToInt64Bits(x) & ~0xFFFFFFFFL;
		return BitConverter.Int64BitsToDouble(h | (uint)i);
	}

	static double fabs(double d)
	{
		return Math.Abs(d);
	}

	static double sqrt(double d)
	{
		return Math.Sqrt(d);
	}

	static double copysign(double x, double y)
	{
		x = __HI(x, (__HI(x) & 0x7fffffff) | (__HI(y) & unchecked((int)0x80000000)));
		return x;
	}
}
