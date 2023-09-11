#ifdef WIN32
#define NETEXPORT __declspec(dllexport)
#define NETCALL __stdcall
#else
#define NETEXPORT
#define NETCALL
#endif

#if defined WIN32
#include <windows.h>
#endif

#if defined LINUX | MACOSX
#include <pthread.h>
#include <errno.h>
#include <dlfcn.h>
#endif

#if defined LINUX | MACOSX
static pthread_mutex_t dl_mutex;
#endif

NETEXPORT void* NETCALL IKVM_dl_open(const char* name)
{
#ifdef WIN32
    return LoadLibraryEx(name, 0, LOAD_LIBRARY_SEARCH_DEFAULT_DIRS | LOAD_LIBRARY_SEARCH_DLL_LOAD_DIR);
#else
    return dlopen(name, RTLD_NOW | RTLD_GLOBAL);
#endif
}

NETEXPORT void NETCALL IKVM_dl_close(void* handle)
{
#ifdef WIN32
    FreeLibrary((HMODULE)handle);
#else
    dlclose(handle);
#endif
}

NETEXPORT void* NETCALL IKVM_dl_sym(void* handle, const char* name)
{
#ifdef WIN32
    return (void*)GetProcAddress((HMODULE)handle, name);
#else
    pthread_mutex_lock(&dl_mutex);
    void* result = dlsym(handle, name);
    pthread_mutex_unlock(&dl_mutex);
    return result;
#endif
}
