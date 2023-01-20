package ikvm.tests.java.lang.annotation;

import java.util.*;
import java.lang.annotation.*;
import java.lang.reflect.*;
import java.io.Serializable;

@Target(ElementType.TYPE_USE)
@Retention(RetentionPolicy.RUNTIME)
public @interface TypeAnnotation2 {
    String value();
}
