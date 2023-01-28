package ikvm.tests.java.lang.annotation;

import java.util.*;
import java.lang.annotation.*;
import java.lang.reflect.*;
import java.io.Serializable;

public abstract class TypeAnnotationClassTypeVarAndField<T extends @TypeAnnotation1("Object1") Object & @TypeAnnotation1("Runnable1") @TypeAnnotation2("Runnable2") Runnable, @TypeAnnotation1("EE") EE extends @TypeAnnotation2("EEBound") Runnable> {

    @TypeAnnotation1("T1 field") @TypeAnnotation2("T2 field") T field1;
    T field2;
    @TypeAnnotation1("Object field") Object field3;

    public @TypeAnnotation1("t1") @TypeAnnotation2("t2") T foo(){
        return null;
    }

    public <M extends @TypeAnnotation1("M Runnable") Runnable> M foo2() {
        return null;
    }

    public <@TypeAnnotation1("K") K extends Cloneable> K foo3() {
        return null;
    }

}
