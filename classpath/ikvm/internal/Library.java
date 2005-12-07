package ikvm.internal;

public class Library
{
  private static final LibraryVMInterface impl = doGetImpl();

  private static native LibraryVMInterface doGetImpl();

  public static LibraryVMInterface getImpl()
  {
    return impl;
  }
}
