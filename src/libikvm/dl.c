#ifdef WIN32
#define NETEXPORT __declspec(dllexport)
#define NETCALL __stdcall
#else
#define NETEXPORT __attribute__((visibility("default")))
#define NETCALL
#endif

#if defined WIN32
#include <windows.h>
#endif

#if defined LINUX | MACOSX | EMSCRIPTEN
#include <pthread.h>
#include <errno.h>
#include <dlfcn.h>
#endif

#if defined EMSCRIPTEN
#include <emscripten.h>
#endif

#if defined LINUX | MACOSX
static pthread_mutex_t dl_mutex;
#endif

NETEXPORT void* NETCALL IKVM_dl_open(const char* name)
{
#ifdef WIN32
    size_t sname = (size_t)MultiByteToWideChar(CP_UTF8, 0, name, -1, NULL, 0);
    LPWSTR wname = (LPWSTR)malloc(sname * sizeof(WCHAR));
    MultiByteToWideChar(CP_UTF8, 0, name, -1, wname, sname);

    void* result = LoadLibraryExW(wname, 0, LOAD_LIBRARY_SEARCH_DEFAULT_DIRS | LOAD_LIBRARY_SEARCH_DLL_LOAD_DIR);
    free(wname);
    return result;
#else
    return dlopen(name, RTLD_LAZY | RTLD_GLOBAL);
#endif
}

NETEXPORT void NETCALL IKVM_dl_close(void* handle)
{
#ifdef WIN32
    FreeLibrary((HMODULE)handle);
#elif EMSCRIPTEN
#else
    dlclose(handle);
#endif
}

NETEXPORT void* NETCALL IKVM_dl_sym(void* handle, const char* name)
{
#ifdef WIN32
    return (void*)GetProcAddress((HMODULE)handle, name);
#else
#ifndef EMSCRIPTEN
    pthread_mutex_lock(&dl_mutex);
#endif
    void* result = dlsym(handle, name);
#ifndef EMSCRIPTEN
    pthread_mutex_unlock(&dl_mutex);
#endif
    return result;
#endif
}
