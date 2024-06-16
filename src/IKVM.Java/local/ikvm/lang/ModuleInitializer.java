package ikvm.lang;

import java.lang.annotation.*;

/**
 * Used to indicate to the compiler that a method should be called in its containing module's initializer.
 */

@Documented
@Retention(RetentionPolicy.CLASS)
@Target({ ElementType.METHOD })
public @interface ModuleInitializer
{
    
}
