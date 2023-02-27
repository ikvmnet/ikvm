package ikvm.util;

import java.util.*;

import cli.System.Collections.IEnumerator;

public final class EnumeratorIterator<E> implements Iterator<E> {

    final IEnumerator enumerator;
    boolean ready;
    boolean withNext;

    public EnumeratorIterator(IEnumerator enumerator) {
        this.enumerator = enumerator;
    }

    @Override
    public boolean hasNext() {
        if (ready) {
            withNext = enumerator.MoveNext();
            ready = false;
        }

        return withNext;
    }

    @Override
    public E next() {
        if (hasNext() == false) {
            throw new NoSuchElementException();
        }

        ready = true;
        return (E)enumerator.get_Current();
    }

}
