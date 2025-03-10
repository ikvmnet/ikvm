package ikvm.runtime;

import cli.System.Collections.IEnumerable;
import cli.System.Collections.IEnumerator;

import java.util.Enumeration;
import java.util.NoSuchElementException;

public final class EnumerationWrapper<T> implements Enumeration<T> {

    private final IEnumerator enumerator;
    private boolean next;

    public EnumerationWrapper(IEnumerable enumerable) {
        this.enumerator = enumerable.GetEnumerator();
        next = enumerator.MoveNext();
    }

    public boolean hasMoreElements() {
        return next;
    }

    public T nextElement() {
        if (!next) {
            throw new NoSuchElementException();
        }

        Object value = enumerator.get_Current();
        next = enumerator.MoveNext();
        return (T)value;
    }

}
