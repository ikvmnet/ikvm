#ifdef WIN32
#define NETEXPORT __declspec(dllexport)
#define NETCALL __stdcall
#else
#define NETEXPORT __attribute__((visibility("default")))
#define NETCALL
#endif

#if defined LINUX | MACOSX
#include <stdio.h>
#include <signal.h>
#include <stdlib.h>

NETEXPORT int NETCALL IKVM_sig_get_size_sigaction()
{
    return sizeof(struct sigaction);
}

NETEXPORT int NETCALL IKVM_sig_get_chld_action(struct sigaction* sig)
{
    return sigaction(SIGCHLD, NULL, sig);
}

NETEXPORT int NETCALL IKVM_sig_set_chld_action(struct sigaction* sig)
{
    return sigaction(SIGCHLD, sig, NULL);
}

#endif
