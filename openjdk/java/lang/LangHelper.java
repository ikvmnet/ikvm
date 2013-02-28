/*
  Copyright (C) 2007-2011 Jeroen Frijters

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

package java.lang;

import sun.nio.ch.Interruptible;
import sun.reflect.annotation.AnnotationType;

@ikvm.lang.Internal
public class LangHelper
{
    public static sun.misc.JavaLangAccess getJavaLangAccess()
    {
        return new sun.misc.JavaLangAccess() {
            public sun.reflect.ConstantPool getConstantPool(Class klass) {
                return null;
            }
            public void setAnnotationType(Class klass, AnnotationType type) {
                klass.setAnnotationType(type);
            }
            public AnnotationType getAnnotationType(Class klass) {
                return klass.getAnnotationType();
            }
            public <E extends Enum<E>>
                    E[] getEnumConstantsShared(Class<E> klass) {
                return klass.getEnumConstantsShared();
            }
            public void blockedOn(Thread t, Interruptible b) {
                t.blockedOn(b);
            }
            public void registerShutdownHook(int slot, boolean registerShutdownInProgress, Runnable hook) {
                Shutdown.add(slot, registerShutdownInProgress, hook);
            }
            public int getStackTraceDepth(Throwable t) {
                return t.getStackTraceDepth();
            }
            public StackTraceElement getStackTraceElement(Throwable t, int i) {
                return t.getStackTraceElement(i);
            }
            public int getStringHash32(String string) {
                return StringHelper.hash32(string);
            }
        };
    }
}
