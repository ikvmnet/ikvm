package ikvm.java.tests.java.lang.reflect;

import java.lang.*;
import java.lang.reflect.*;
import java.util.*;

public class MethodInvokeTests {

    public static class CovariantReturn_Base {

        public Object method() {
            return null;
        }

    }

    public static class CovariantReturn extends CovariantReturn_Base {

        @Override
        public String method() {
            return null;
        }

    }

}
