import java.util.*;
import java.lang.annotation.*;
import java.lang.reflect.*;
import java.io.Serializable;

abstract class TypeAnnotationArrayTest extends @TypeAnnotation1("extends") @TypeAnnotation2("extends2") Object
    implements @TypeAnnotation1("implements serializable") @TypeAnnotation2("implements2 serializable") Serializable,
    Readable,
    @TypeAnno("implements cloneable") @TypeAnnotation2("implements2 cloneable") Cloneable {
    public @TypeAnnotation1("return4") Object @TypeAnnotation1("return1") [][] @TypeAnnotation1("return3")[] foo() { return null; }
}
