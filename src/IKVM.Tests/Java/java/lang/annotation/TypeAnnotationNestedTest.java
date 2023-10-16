package ikvm.tests.java.lang.annotation;

import java.util.*;
import java.lang.annotation.*;
import java.lang.reflect.*;
import java.io.Serializable;

abstract class TypeAnnotationNestedTest {

    public @TypeAnnotation1("Outer") TypeAnnotationNestedTestOuter.@TypeAnnotation1("Inner")Inner @TypeAnnotation1("array")[] foo() {
        return null;
    }

}
