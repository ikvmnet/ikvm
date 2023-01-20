package ikvm.tests.java.lang.annotation;

import java.util.*;
import java.lang.annotation.*;
import java.lang.reflect.*;
import java.io.Serializable;

abstract class TypeAnnotationWildcardTest {

    public <T> List<? super T> foo() {
        return null;
    }

    public Class<@TypeAnnotation1("1") ? extends @TypeAnnotation1("2") Annotation> f1;

    public Class<@TypeAnnotation1("3") ? super @TypeAnnotation1("4") Annotation> f2;

}
