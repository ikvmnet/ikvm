package ikvm.lang;

import java.lang.annotation.*;

@Documented
@Retention(RetentionPolicy.CLASS)
@Target({ ElementType.FIELD })
public @interface Property
{
	String get();
	String set() default "";
}
