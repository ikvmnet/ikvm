#ifdef WIN32
#define NETEXPORT __declspec(dllexport)
#define NETCALL __stdcall
#else
#define NETEXPORT
#define NETCALL
#endif

#if defined WIN32
#include <winsock2.h>
#endif

#if defined LINUX || defined MACOS
#define _LARGEFILE64_SOURCE
#include <errno.h>
#include <unistd.h>
#include <pthread.h>
#include <sys/types.h>
#include <sys/stat.h>
#endif

#if defined LINUX
#include <sys/stat.h>
#endif

NETEXPORT int NETCALL IKVM_io_is_file(long long handle)
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

NETEXPORT int NETCALL IKVM_io_is_socket(long long handle)
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

NETEXPORT long long NETCALL IKVM_io_duplicate_file(long long handle)
{
    if (handle > -1)
    {
#ifdef WIN32
        HANDLE newHandle;
        DuplicateHandle(GetCurrentProcess(), (HANDLE)handle, GetCurrentProcess(), &newHandle, 0, FALSE, DUPLICATE_SAME_ACCESS);
        return (long long)newHandle;
#else
        return (long long)dup((int)handle);
#endif
    }
    else
    {
        return -1;
    }
}

NETEXPORT long long NETCALL IKVM_io_duplicate_socket(long long handle)
{
    if (handle > -1)
    {
#ifdef WIN32
        WSAPROTOCOL_INFOW protocolInfo;
        WSADuplicateSocketW((SOCKET)handle, GetCurrentProcessId(), &protocolInfo);
        return (long long)WSASocketW(-1, -1, -1, &protocolInfo, 0, 0);
#else
        return (long long)dup((int)handle);
#endif
    }
    else
    {
        return -1;
    }
}

NETEXPORT void NETCALL IKVM_io_close_file(long long handle)
{
    if (handle > -1)
    {
#ifdef WIN32
        CloseHandle((HANDLE)handle);
#else
        close((int)handle);
#endif
    }
}

NETEXPORT void NETCALL IKVM_io_close_socket(long long handle)
{
    if (handle > -1)
    {
#ifdef WIN32
        closesocket((SOCKET)handle);
#else
        close((int)handle);
#endif
    }
}
