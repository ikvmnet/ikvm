package java.lang;

// Note: stop() should take care not to stop a thread if it is
// executing code in this class.
final class VMThread
{
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
	    thread.die();
	    synchronized(this)
	    {
		// release the threads waiting to join us
		notifyAll();
	    }
	}
    }

    static void create(Thread thread, long stacksize)
    {
	VMThread vmThread = new VMThread(thread);
	vmThread.start(stacksize);
	thread.vmThread = vmThread;
    }

    String getName()
    {
	return thread.name;
    }

    void setName(String name)
    {
	thread.name = name;
    }

    void setPriority(int priority)
    {
	thread.priority = priority;
	nativeSetPriority(priority);
    }

    int getPriority()
    {
	synchronized(thread)
	{
	    return thread.priority;
	}
    }

    boolean isDaemon()
    {
        return thread.daemon;
    }

    int countStackFrames()
    {
	return 0;
    }

    synchronized void join(long ms, int ns) throws InterruptedException
    {
	// We use the VMThread object to wait on, because this is a private
	// object, so client code cannot call notify on us.
        wait(ms, ns);
    }

    void stop(Throwable t)
    {
	// Note: we assume that we own the lock on thread
	// (i.e. that Thread.stop() is synchronized)
	if(running)
	    nativeStop(t);
	else
	    thread.stillborn = t;
    }

    private static cli.System.LocalDataStoreSlot localDataStoreSlot = cli.System.Threading.Thread.AllocateDataSlot();
    private cli.System.Threading.Thread nativeThread;

    /*native*/ void start(long stacksize)
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
	nativeThread = new cli.System.Threading.Thread(starter);
	nativeThread.set_Name(thread.name);
	nativeThread.set_IsBackground(thread.daemon);
	nativeSetPriority(thread.priority);
	nativeThread.Start();
    }

    /*native*/ void interrupt()
    {
	nativeThread.Interrupt();
    }

    /*native*/ boolean isInterrupted()
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
		nativeThread.Interrupt();
		return true;
	    }
	}
	// HACK since quering the interrupted state of another thread is inherently racy, I hope
	// we can get away with always returning false, because I have no idea how to obtain this
	// information from the .NET runtime
	return false;
    }

    /*native*/ void suspend()
    {
	nativeThread.Suspend();
    }

    /*native*/ void resume()
    {
	nativeThread.Resume();
    }

    /*native*/ void nativeSetPriority(int priority)
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

    /*native*/ void nativeStop(Throwable t)
    {
	// NOTE we allow ThreadDeath to be thrown in every thread, but any other exception
	// is ignored, except if we're throwing it on the current Thread. This is done
	// to allow exception handlers to be type specific, otherwise every exception handler
	// would have to catch ThreadAbortException and look inside it to see if it contains
	// the real exception that we wish to handle.
	// I hope we can get away with this behavior, because Thread.stop() is deprecated
	// anyway. Note that we do allow arbitrary exceptions to be thrown on the current
	// thread, since this is harmless (because it isn't wrapped) and also because it
	// provides some real value, because it is the only way you can throw checked
	// exceptions from Java.
	if(currentThread().vmThread == this)
	{
	    VMClass.throwException(t);
	}
	else if(t instanceof ThreadDeath)
	{
	    nativeThread.Abort(t);
	}
    }

    /*native*/ static Thread currentThread()
    {
	Thread javaThread = (Thread)cli.System.Threading.Thread.GetData(localDataStoreSlot);
	if(javaThread == null)
	{
	    // threads created outside of Java always run in the root thread group
	    // TODO if the thread dies, it needs to be removed from the root ThreadGroup   
	    // and any other threads waiting to join it, should be released.
	    cli.System.Threading.Thread nativeThread = cli.System.Threading.Thread.get_CurrentThread();
	    VMThread vmThread = new VMThread(null);
	    vmThread.nativeThread = nativeThread;
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
	    javaThread.group = ThreadGroup.root;
	    javaThread.group.addThread(javaThread);
	    InheritableThreadLocal.newChildThread(javaThread);
	}
	return javaThread;
    }

    static /*native*/ void yield()
    {
	cli.System.Threading.Thread.Sleep(0);
    }

    static /*native*/ void sleep(long ms, int ns) throws InterruptedException
    {
	try
	{
	    if(false) throw new cli.System.Threading.ThreadInterruptedException();
	    // TODO guard against ms and ns overflowing
	    cli.System.Threading.Thread.Sleep(new cli.System.TimeSpan(ms * 10000 + (ns + 99) / 100));
	}
	catch(cli.System.Threading.ThreadInterruptedException x)
	{
	    throw new InterruptedException(x.getMessage());
	}
    }

    static /*native*/ boolean interrupted()
    {
	try
	{
	    synchronized(currentThread())
	    {
		if(false) throw new InterruptedException();
		cli.System.Threading.Thread.Sleep(0);
	    }
	    return false;
	}
	catch(InterruptedException x)
	{
	    return true;
	}
    }

    static /*native*/ boolean holdsLock(Object obj)
    {
	if(obj == null)
	{
	    throw new NullPointerException();
	}
	try
	{
	    // HACK this is a lame way of doing this, but I can't see any other way
	    // NOTE Wait causes the lock to be released temporarily, which isn't what we want
	    if(false) throw new IllegalMonitorStateException();
	    if(false) throw new InterruptedException();
	    cli.System.Threading.Monitor.Wait(obj, 0);
	    return true;
	}
	catch(IllegalMonitorStateException x)
	{
	    return false;
	}
	catch(InterruptedException x1)
	{
	    // Since we "consumed" the interrupt, we have to interrupt ourself again
	    cli.System.Threading.Thread.get_CurrentThread().Interrupt();
	    return true;
	}
    }
}
