package ikvm.internal;

public class Library
{
  private static final LibraryVMInterface impl = new java.lang.LibraryVMInterfaceImpl();

  public static LibraryVMInterface getImpl()
  {
    return impl;
  }
}
