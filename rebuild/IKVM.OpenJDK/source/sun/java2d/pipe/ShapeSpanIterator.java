/*
  Copyright (C) 2009 Volker Berlin (i-net software)

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
package sun.java2d.pipe;

import java.awt.geom.PathIterator;
import ikvm.internal.NotYetImplementedError;

/**
 * Replacement for compiling only 
 */
public class ShapeSpanIterator implements SpanIterator{

    public ShapeSpanIterator(boolean normalize){
        throw new NotYetImplementedError();
    }

    public void setOutputArea(Region devBounds){
        // TODO Auto-generated method stub
        
    }

    public void appendPath(PathIterator pathIterator){
        // TODO Auto-generated method stub
        
    }

    public void getPathBox(int[] box){
        // TODO Auto-generated method stub
        
    }

    @Override
    public long getNativeIterator(){
        // TODO Auto-generated method stub
        return 0;
    }

    @Override
    public void intersectClipBox(int lox, int loy, int hix, int hiy){
        // TODO Auto-generated method stub
        
    }

    @Override
    public boolean nextSpan(int[] spanbox){
        // TODO Auto-generated method stub
        return false;
    }

    @Override
    public void skipDownTo(int y){
        // TODO Auto-generated method stub
        
    }

    public void dispose(){
        // TODO Auto-generated method stub
        
    }

}
