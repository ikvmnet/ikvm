#ifdef WIN32
#define EXPORT __declspec(dllexport)
#else
#define EXPORT
#endif

#if defined WIN32
#include <windows.h>
#endif

#if defined LINUX | MACOSX
#include <pthread.h>
#include <errno.h>
#include <sys/stat.h>
#endif

EXPORT int IKVM_io_is_file(long handle)
{
#ifdef WIN32
    return GetFileType((HANDLE)handle) == FILE_TYPE_DISK;
#else
    struct stat st;
    if (fstat(handle, &st) < 0)
        return 1;

    return S_ISSOCK(st.st_mode) == 0;
#endif
}

EXPORT int IKVM_io_is_socket(long handle)
{
#ifdef WIN32
    return GetFileType((HANDLE)handle) == FILE_TYPE_PIPE && GetNamedPipeInfo((HANDLE)handle, NULL, NULL, NULL, NULL) == 0;
#else
    struct stat st;
    if (fstat(handle, &st) < 0)
        return 1;

    return S_ISSOCK(st.st_mode) != 0;
#endif
}
