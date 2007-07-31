package sun.awt;

public final class AppContext extends java.util.Hashtable
{
  private static final AppContext instance = new AppContext();

  private AppContext() {}

  public static AppContext getAppContext()
  {
    return instance;
  }
}
