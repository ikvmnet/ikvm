package ikvm.util;

import java.util.*;

import cli.System.Collections.IEnumerator;

public final class EnumeratorIterator<E> implements Iterator<E> {

    final IEnumerator enumerator;
    boolean didMove;
    boolean hasNext;

    public EnumeratorIterator(IEnumerator enumerator) {
        this.enumerator = enumerator;
    }

    @Override
    public boolean hasNext() {
        if (didMove == false) {
            hasNext = enumerator.MoveNext();
            didMove = true;
        }

        return hasNext;
    }

    @Override
    public E next() {
        if (hasNext() == false) {
            throw new NoSuchElementException();
        }

        didMove = false;
        return (E)enumerator.get_Current();
    }

}