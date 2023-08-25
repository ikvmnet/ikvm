#if WIN32
#else
#include <dlfcn.h>
#endif

#if WIN32
#else

void* IKVM_dlopen(const char* name, int mode)
{
    return dlopen(name, mode);
}

void* IKVM_dlsym(void* handle, const char* symbol)
{
    return dlsym(handle, symbol);
}

int IKVM_dlclose(void* handle)
{
    return dlclose(handle);
}

#endif
