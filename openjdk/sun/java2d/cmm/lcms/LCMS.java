/*
  Copyright (C) 2009 Jeroen Frijters

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
package sun.java2d.cmm.lcms;

import java.awt.color.CMMException;
import java.awt.color.ICC_Profile;
import java.awt.image.BufferedImage;
import java.awt.image.Raster;
import java.awt.image.WritableRaster;
import java.util.ArrayList;
import sun.java2d.cmm.ColorTransform;
import sun.java2d.cmm.PCMM;

// dummy color management implementation
public class LCMS implements PCMM
{
    private final ArrayList<byte[]> profiles = new ArrayList<byte[]>();

    public synchronized long loadProfile(byte[] data)
    {
        int free = profiles.indexOf(null);
        if (free != -1)
        {
            profiles.set(free, data.clone());
            return free;
        }
        else
        {
            long id = profiles.size();
            profiles.add(data.clone());
            return id;
        }
    }

    public synchronized void freeProfile(long profileID)
    {
        profiles.set((int)profileID, null);
    }

    public synchronized int getProfileSize(long profileID)
    {
        return profiles.get((int)profileID).length;
    }

    public synchronized void getProfileData(long profileID, byte[] data)
    {
        byte[] src = profiles.get((int)profileID);
        System.arraycopy(src, 0, data, 0, src.length);
    }

    public void getTagData(long profileID, int tagSignature, byte[] data)
    {
        if (tagSignature == ICC_Profile.icSigHead)
        {
            byte[] src = profiles.get((int)profileID);
            System.arraycopy(src, 0, data, 0, 128);
            return;
        }
        throw new CMMException("Not implemented");
    }

    public int getTagSize(long profileID, int tagSignature)
    {
        if (tagSignature == ICC_Profile.icSigHead)
        {
            return 128;
        }
        throw new CMMException("Not implemented");
    }

    public void setTagData(long profileID, int tagSignature, byte[] data)
    {
        throw new CMMException("Not implemented");
    }

    public ColorTransform createTransform(ICC_Profile profile, int renderType, int transformType)
    {
        return new DummyColorTransform();
    }

    public ColorTransform createTransform(ColorTransform[] transforms)
    {
        return new DummyColorTransform();
    }
    
    static class DummyColorTransform implements ColorTransform
    {
        public int getNumInComponents()
        {
            throw new CMMException("Not implemented");
        }

        public int getNumOutComponents()
        {
            throw new CMMException("Not implemented");
        }

        public void colorConvert(BufferedImage src, BufferedImage dst)
        {
            throw new CMMException("Not implemented");
        }

        public void colorConvert(Raster src, WritableRaster dst, float[] srcMinVal, float[] srcMaxVal, float[] dstMinVal, float[]dstMaxVal)
        {
            throw new CMMException("Not implemented");
        }

        public void colorConvert(Raster src, WritableRaster dst)
        {
            throw new CMMException("Not implemented");
        }

        public short[] colorConvert(short[] src, short[] dest)
        {
            throw new CMMException("Not implemented");
        }

        public byte[] colorConvert(byte[] src, byte[] dest)
        {
            throw new CMMException("Not implemented");
        }
    }
}
