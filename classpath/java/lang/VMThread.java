package java.lang;

// Note: stop() should take care not to stop a thread if it is
// executing code in this class.
final class VMThread
{
    private static cli.System.LocalDataStoreSlot localDataStoreSlot = cli.System.Threading.Thread.AllocateDataSlot();
    private cli.System.WeakReference nativeThreadReference;

    // Note: when this thread dies, this reference is *not* cleared
    volatile Thread thread;
    private volatile boolean running;

    private VMThread(Thread thread)
    {
	this.thread = thread;
    }

    private void run()
    {
	try
	{
	    try
	    {
		running = true;
		synchronized(thread)
		{
		    Throwable t = thread.stillborn;
		    if(t != null)
		    {
			thread.stillborn = null;
			throw t;
		    }
		}
		thread.run();
	    }
	    catch(Throwable t)
	    {
		try
		{
		    thread.group.uncaughtException(thread, t);
		}
		catch(Throwable ignore)
		{
		}
	    }
	}
	finally
	{
	    // Setting runnable to false is partial protection against stop
	    // being called while we're cleaning up. To be safe all code in
	    // VMThread be unstoppable.
	    running = false;
	    cleanup();
	}
    }

    // notify Thread that it is dead, this method can safely be called multiple times
    private synchronized void cleanup()
    {
	if(thread.vmThread != null)
	{
	    thread.die();
	}
    }

    static void create(Thread thread, long stacksize)
    {
	VMThread vmThread = new VMThread(thread);
	vmThread.start(stacksize);
	thread.vmThread = vmThread;
    }

    synchronized String getName()
    {
	return thread.name;
    }

    synchronized void setName(String name)
    {
	thread.name = name;
    }

    synchronized void setPriority(int priority)
    {
	thread.priority = priority;
	nativeSetPriority(priority);
    }

    synchronized int getPriority()
    {
        return thread.priority;
    }

    boolean isDaemon()
    {
        return thread.daemon;
    }

    int countStackFrames()
    {
	return 0;
    }

    void join(long ms, int ns) throws InterruptedException
    {
	cli.System.Threading.Thread nativeThread = (cli.System.Threading.Thread)nativeThreadReference.get_Target();
	if(nativeThread != null)
	{
	    if(ms == 0 && ns == 0)
	    {
		nativeThread.Join();
	    }
	    else
	    {
		// if nanoseconds are specified, round up to one millisecond
		if(ns != 0)
		{
		    nativeThread.Join(1);
		}
		for(long iter = ms / Integer.MAX_VALUE; iter != 0; iter--)
		{
		    nativeThread.Join(Integer.MAX_VALUE);
		}
		nativeThread.Join((int)(ms % Integer.MAX_VALUE));
	    }
	    // make sure the thread is marked as dead and removed from the thread group, before we
	    // return from a successful join
	    if(!nativeThread.get_IsAlive())
	    {
		cleanup();
	    }
	}
    }

    void stop(Throwable t)
    {
	// NOTE we assume that we own the lock on thread
	// (i.e. that Thread.stop() is synchronized)
	if(running)
	    nativeStop(t);
	else
	    thread.stillborn = t;
    }

    void start(long stacksize)
    {
	cli.System.Threading.ThreadStart starter = new cli.System.Threading.ThreadStart(
	    new cli.System.Threading.ThreadStart.Method()
	{
	    public void Invoke()
	    {
		cli.System.Threading.Thread.SetData(localDataStoreSlot, thread);
		run();
	    }
	});
	cli.System.Threading.Thread nativeThread = new cli.System.Threading.Thread(starter);
	nativeThreadReference = new cli.System.WeakReference(nativeThread);
	nativeThread.set_Name(thread.name);
	nativeThread.set_IsBackground(thread.daemon);
	nativeSetPriority(thread.priority);
	nativeThread.Start();
    }

    void interrupt()
    {
	cli.System.Threading.Thread nativeThread = (cli.System.Threading.Thread)nativeThreadReference.get_Target();
	if(nativeThread != null)
	{
	    nativeThread.Interrupt();
	}
    }

    boolean isInterrupted()
    {
	// NOTE special case for current thread, because then we can use the .NET interrupted status
	if(thread == currentThread())
	{
	    try
	    {
		if(false) throw new InterruptedException();
		cli.System.Threading.Thread.Sleep(0);
		return false;
	    }
	    catch(InterruptedException x)
	    {
		// because we "consumed" the interrupt, we need to interrupt ourself again
		cli.System.Threading.Thread nativeThread = (cli.System.Threading.Thread)nativeThreadReference.get_Target();
		if(nativeThread != null)
		{
		    nativeThread.Interrupt();
		}
		return true;
	    }
	}
	// HACK since quering the interrupted state of another thread is inherently racy, I hope
	// we can get away with always returning false, because I have no idea how to obtain this
	// information from the .NET runtime
	return false;
    }

    void suspend()
    {
	cli.System.Threading.Thread nativeThread = (cli.System.Threading.Thread)nativeThreadReference.get_Target();
	if(nativeThread != null)
	{
	    nativeThread.Suspend();
	}
    }

    void resume()
    {
	cli.System.Threading.Thread nativeThread = (cli.System.Threading.Thread)nativeThreadReference.get_Target();
	if(nativeThread != null)
	{
	    nativeThread.Resume();
	}
    }

    void nativeSetPriority(int priority)
    {
	cli.System.Threading.Thread nativeThread = (cli.System.Threading.Thread)nativeThreadReference.get_Target();
	if(nativeThread != null)
	{
	    if(priority == Thread.MIN_PRIORITY)
	    {
		nativeThread.set_Priority(cli.System.Threading.ThreadPriority.wrap(cli.System.Threading.ThreadPriority.Lowest));
	    }
	    else if(priority > Thread.MIN_PRIORITY && priority < Thread.NORM_PRIORITY)
	    {
		nativeThread.set_Priority(cli.System.Threading.ThreadPriority.wrap(cli.System.Threading.ThreadPriority.BelowNormal));
	    }
	    else if(priority == Thread.NORM_PRIORITY)
	    {
		nativeThread.set_Priority(cli.System.Threading.ThreadPriority.wrap(cli.System.Threading.ThreadPriority.Normal));
	    }
	    else if(priority > Thread.NORM_PRIORITY && priority < Thread.MAX_PRIORITY)
	    {
		nativeThread.set_Priority(cli.System.Threading.ThreadPriority.wrap(cli.System.Threading.ThreadPriority.AboveNormal));
	    }
	    else if(priority == Thread.MAX_PRIORITY)
	    {
		nativeThread.set_Priority(cli.System.Threading.ThreadPriority.wrap(cli.System.Threading.ThreadPriority.Highest));
	    }
	}
    }

    void nativeStop(Throwable t)
    {
	// NOTE we allow ThreadDeath (and its subclasses) to be thrown on every thread, but any
	// other exception is ignored, except if we're throwing it on the current Thread. This
	// is done to allow exception handlers to be type specific, otherwise every exception
	// handler would have to catch ThreadAbortException and look inside it to see if it
	// contains the real exception that we wish to handle.
	// I hope we can get away with this behavior, because Thread.stop() is deprecated
	// anyway. Note that we do allow arbitrary exceptions to be thrown on the current
	// thread, since this is harmless (because they aren't wrapped) and also because it
	// provides some real value, because it is the only way you can throw arbitrary checked
	// exceptions from Java.
	if(currentThread().vmThread == this)
	{
	    VMClass.throwException(t);
	}
	else if(t instanceof ThreadDeath)
	{
	    cli.System.Threading.Thread nativeThread = (cli.System.Threading.Thread)nativeThreadReference.get_Target();
	    if(nativeThread != null)
	    {
		nativeThread.Abort(t);
	    }
	}
    }

    private static class CleanupHack
    {
	private Thread thread;

	CleanupHack(Thread thread)
	{
	    this.thread = thread;
	}

	protected void finalize()
	{
	    VMThread vmThread = thread.vmThread;
	    if(vmThread != null)
	    {
		vmThread.cleanup();
	    }
	}
    }

    static Thread currentThread()
    {
	Thread javaThread = (Thread)cli.System.Threading.Thread.GetData(localDataStoreSlot);
	if(javaThread == null)
	{
	    // threads created outside of Java always run in the root thread group
	    // TODO if the thread dies, it needs to be removed from the root ThreadGroup
	    cli.System.Threading.Thread nativeThread = cli.System.Threading.Thread.get_CurrentThread();
	    VMThread vmThread = new VMThread(null);
	    vmThread.nativeThreadReference = new cli.System.WeakReference(nativeThread);
	    vmThread.running = true;
	    int priority = Thread.NORM_PRIORITY;
	    switch(nativeThread.get_Priority().Value)
	    {
		case cli.System.Threading.ThreadPriority.Lowest:
		    priority = Thread.MIN_PRIORITY;
		    break;
		case cli.System.Threading.ThreadPriority.BelowNormal:
		    priority = 3;
		    break;
		case cli.System.Threading.ThreadPriority.Normal:
		    priority = Thread.NORM_PRIORITY;
		    break;
		case cli.System.Threading.ThreadPriority.AboveNormal:
		    priority = 7;
		    break;
		case cli.System.Threading.ThreadPriority.Highest:
		    priority = Thread.MAX_PRIORITY;
		    break;
	    }
	    javaThread = new Thread(vmThread, nativeThread.get_Name(), priority, nativeThread.get_IsBackground());
	    vmThread.thread = javaThread;
	    cli.System.Threading.Thread.SetData(localDataStoreSlot, javaThread);
	    cli.System.Threading.Thread.SetData(cli.System.Threading.Thread.GetNamedDataSlot("ikvm-thread-hack"), new CleanupHack(javaThread));
	    javaThread.group = ThreadGroup.root;
	    javaThread.group.addThread(javaThread);
	    InheritableThreadLocal.newChildThread(javaThread);
	}
	return javaThread;
    }

    static void yield()
    {
	try
	{
	    if(false) throw new InterruptedException();
	    cli.System.Threading.Thread.Sleep(0);
	}
	catch(InterruptedException x)
	{
	    // since we "consumed" the interrupt, we have to interrupt ourself again
	    cli.System.Threading.Thread.get_CurrentThread().Interrupt();
	}
    }

    static void sleep(long ms, int ns) throws InterruptedException
    {
	// NOTE sleep(0) doesn't trigger a pending interrupt on the Sun JDK,
	// so we duplicate that behavior.
	if(ms == 0 && ns == 0)
	{
	    yield();
	}
	else
	{
	    // if nanoseconds are specified, round up to one millisecond
	    if(ns != 0)
	    {
		cli.System.Threading.Thread.Sleep(1);
	    }
	    for(long iter = ms / Integer.MAX_VALUE; iter != 0; iter--)
	    {
		cli.System.Threading.Thread.Sleep(Integer.MAX_VALUE);
	    }
	    cli.System.Threading.Thread.Sleep((int)(ms % Integer.MAX_VALUE));
	}
    }

    static boolean interrupted()
    {
	try
	{
	    if(false) throw new InterruptedException();
	    cli.System.Threading.Thread.Sleep(0);
	    return false;
	}
	catch(InterruptedException x)
	{
	    return true;
	}
    }

    static boolean holdsLock(Object obj)
    {
	if(obj == null)
	{
	    throw new NullPointerException();
	}
	try
	{
	    // The new 1.5 memory model explicitly allows spurious wake-ups from Object.wait,
	    // so we abuse Pulse to check if we own the monitor.
	    if(false) throw new IllegalMonitorStateException();
	    cli.System.Threading.Monitor.Pulse(obj);
	    return true;
	}
	catch(IllegalMonitorStateException x)
	{
	    return false;
	}
    }
}
