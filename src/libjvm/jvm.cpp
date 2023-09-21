#include <jni.h>
#include <jvm.h>
#include "jvm.h"
#include <stdio.h>
#include <string.h>

#if defined WIN32
#include <windows.h>
#include <winsock2.h>
#include <ws2tcpip.h>
#include <stdint.h>
#endif

#if defined LINUX || defined MACOS
#include <unistd.h>
#include <pthread.h>
#include <errno.h>
#include <dlfcn.h>
#include <sys/socket.h>
#include <sys/time.h>
#include <sys/ioctl.h>
#include <time.h>
#include <poll.h>
#endif

#define assert(condition, fmt, ...);

typedef uint32_t uint;

// Unsigned one, two, four and eigth byte quantities used for describing
// the .class file format. See JVM book chapter 4.

typedef uint8_t  jubyte;
typedef uint16_t jushort;
typedef uint32_t juint;
typedef uint64_t julong;

const jubyte  max_jubyte = (jubyte)-1;  // 0xFF       largest jubyte
const jushort max_jushort = (jushort)-1; // 0xFFFF     largest jushort
const juint   max_juint = (juint)-1;   // 0xFFFFFFFF largest juint
const julong  max_julong = (julong)-1;  // 0xFF....FF largest julong

// the fancy casts are a hopefully portable way
// to do unsigned 32 to 64 bit type conversion
inline void set_low(jlong* value, jint low) {
    *value &= (jlong)0xffffffff << 32;
    *value |= (jlong)(julong)(juint)low;
}

inline void set_high(jlong* value, jint high) {
    *value &= (jlong)(julong)(juint)0xffffffff;
    *value |= (jlong)high << 32;
}

inline jlong jlong_from(jint h, jint l) {
    jlong result = 0; // initialization to avoid warning
    set_high(&result, h);
    set_low(&result, l);
    return result;
}

// Platform-independent error return values from OS functions
enum OSReturn {
    OS_OK = 0,          // Operation was successful
    OS_ERR = -1,        // Operation failed
    OS_INTRPT = -2,     // Operation was interrupted
    OS_TIMEOUT = -3,    // Operation timed out
    OS_NOMEM = -5,      // Operation failed for lack of memory
    OS_NORESOURCE = -6  // Operation failed for lack of nonmemory resource
};

#if defined LINUX || defined MACOS
// macros for restartable system calls

#define RESTARTABLE(_cmd, _result) do { \
    _result = _cmd; \
} while(((int)_result == OS_ERR) && (errno == EINTR))

#define RESTARTABLE_RETURN_INT(_cmd) do { \
    int _result; \
    RESTARTABLE(_cmd, _result); \
    return _result; \
} while(false)

#endif

jint JNICALL JVM_GetInterfaceVersion()
{
    return JVM_INTERFACE_VERSION;
}

#ifdef WIN32

// Windows format:
//   The FILETIME structure is a 64-bit value representing the number of 100-nanosecond intervals since January 1, 1601.
// Java format:
//   Java standards require the number of milliseconds since 1/1/1970

// Constant offset - calculated using offset()
static jlong  _offset = 116444736000000000;
// Fake time counter for reproducible results when debugging
static jlong  fake_time = 0;

inline jlong windows_to_java_time(FILETIME wt) {
    jlong a = jlong_from(wt.dwHighDateTime, wt.dwLowDateTime);
    return (a - _offset) / 10000;
}

#endif

const jint NANOSECS_PER_MILLISEC = 1000000;

#ifdef WIN32
jlong os_javaTimeMillis() {
    FILETIME wt;
    GetSystemTimeAsFileTime(&wt);
    return windows_to_java_time(wt);
}
#endif
#ifdef LINUX
jlong os_javaTimeMillis() {
    timeval time;
    int status = gettimeofday(&time, NULL);
    assert(status != -1, "linux error");
    return jlong(time.tv_sec) * 1000 + jlong(time.tv_usec / 1000);
}
#endif
#ifdef MACOS
jlong os_javaTimeMillis() {
    timeval time;
    int status = gettimeofday(&time, NULL);
    assert(status != -1, "bsd error");
    return jlong(time.tv_sec) * 1000 + jlong(time.tv_usec / 1000);
}
#endif

jlong JNICALL JVM_CurrentTimeMillis(JNIEnv* env, jclass ignored)
{
    return os_javaTimeMillis();
}

#ifdef WIN32
jlong os_javaTimeNanos() {
    return os_javaTimeMillis() * NANOSECS_PER_MILLISEC;
}
#endif
#ifdef LINUX
jlong os_javaTimeNanos() {
    struct timespec tp;
    int status = clock_gettime(CLOCK_MONOTONIC, &tp);
    jlong result = jlong(tp.tv_sec) * (1000 * 1000 * 1000) + jlong(tp.tv_nsec);
    return result;
}
#endif
#ifdef MACOS
jlong os_javaTimeNanos() {
    return os_javaTimeMillis() * NANOSECS_PER_MILLISEC;
}
#endif

jlong JNICALL JVM_NanoTime(JNIEnv* env, jclass ignored)
{
    return os_javaTimeNanos();
}

void JNICALL JVM_ArrayCopy(JNIEnv* env, jclass ignored, jobject src, jint src_pos, jobject dst, jint dst_pos, jint length)
{

}

jobject JNICALL JVM_InitProperties(JNIEnv* env, jobject properties)
{
    return 0;
}

jstring JNICALL JVM_GetTemporaryDirectory(JNIEnv* env)
{
    return 0;
}

#ifdef WIN32
size_t os_lasterror(char* buf, size_t len) {
    DWORD errval;

    if ((errval = GetLastError()) != 0) {
        // DOS error
        size_t n = (size_t)FormatMessage(
            FORMAT_MESSAGE_FROM_SYSTEM | FORMAT_MESSAGE_IGNORE_INSERTS,
            NULL,
            errval,
            0,
            buf,
            (DWORD)len,
            NULL);
        if (n > 3) {
            // Drop final '.', CR, LF
            if (buf[n - 1] == '\n') n--;
            if (buf[n - 1] == '\r') n--;
            if (buf[n - 1] == '.') n--;
            buf[n] = '\0';
        }
        return n;
    }

    if (errno != 0) {
        // C runtime error that has no corresponding DOS error code
        const char* s = strerror(errno);
        size_t n = strlen(s);
        if (n >= len) n = len - 1;
        strncpy(buf, s, n);
        buf[n] = '\0';
        return n;
    }

    return 0;
}
#endif
#ifdef LINUX
size_t os_lasterror(char* buf, size_t len) {

    if (errno == 0)  return 0;

    const char* s = ::strerror(errno);
    size_t n = ::strlen(s);
    if (n >= len) {
        n = len - 1;
    }
    ::strncpy(buf, s, n);
    buf[n] = '\0';
    return n;
}
#endif
#ifdef MACOS
size_t os_lasterror(char* buf, size_t len) {

    if (errno == 0)  return 0;

    const char* s = ::strerror(errno);
    size_t n = ::strlen(s);
    if (n >= len) {
        n = len - 1;
    }
    ::strncpy(buf, s, n);
    buf[n] = '\0';
    return n;
}
#endif

jint JNICALL JVM_GetLastErrorString(char* buf, int len)
{
    return (jint)os_lasterror(buf, len);
}

jclass JNICALL JVM_FindClassFromClass(JNIEnv* env, const char* name, jboolean init, jclass from)
{
    return 0;
}

jclass JNICALL JVM_FindClassFromClassLoader(JNIEnv* env, const char* name, jboolean init, jobject loader, jboolean throwError)
{
    return 0;
}

jboolean JNICALL JVM_IsInterface(JNIEnv* env, jclass cls)
{
    return 0;
}

const char* JNICALL JVM_GetClassNameUTF(JNIEnv* env, jclass cls)
{
    return 0;
}

void JNICALL JVM_GetClassCPTypes(JNIEnv* env, jclass cls, unsigned char* types)
{

}

jint JNICALL JVM_GetClassCPEntriesCount(JNIEnv* env, jclass cls)
{
    return 0;
}

jint JNICALL JVM_GetClassFieldsCount(JNIEnv* env, jclass cls)
{
    return 0;
}


jint JNICALL JVM_GetClassMethodsCount(JNIEnv* env, jclass cls)
{
    return 0;
}

void JNICALL JVM_GetMethodIxExceptionIndexes(JNIEnv* env, jclass cls, jint method_index, unsigned short* exceptions)
{

}

jint JNICALL JVM_GetMethodIxExceptionsCount(JNIEnv* env, jclass cls, jint method_index)
{
    return 0;
}

void JNICALL JVM_GetMethodIxByteCode(JNIEnv* env, jclass cls, jint method_index, unsigned char* code)
{

}

jint JNICALL JVM_GetMethodIxByteCodeLength(JNIEnv* env, jclass cls, jint method_index)
{
    return 0;
}

void JNICALL JVM_GetMethodIxExceptionTableEntry(JNIEnv* env, jclass cls, jint method_index, jint entry_index, JVM_ExceptionTableEntryType* entry)
{

}

jint JNICALL JVM_GetMethodIxExceptionTableLength(JNIEnv* env, jclass cls, int method_index)
{
    return 0;
}

jint JNICALL JVM_GetMethodIxModifiers(JNIEnv* env, jclass cls, int method_index)
{
    return 0;
}

jint JNICALL JVM_GetFieldIxModifiers(JNIEnv* env, jclass cls, int field_index)
{
    return 0;
}

jint JNICALL  JVM_GetMethodIxLocalsCount(JNIEnv* env, jclass cls, int method_index)
{
    return 0;
}

jint JNICALL JVM_GetMethodIxArgsSize(JNIEnv* env, jclass cls, int method_index)
{
    return 0;
}

jint JNICALL JVM_GetMethodIxMaxStack(JNIEnv* env, jclass cls, int method_index)
{
    return 0;
}

jboolean JNICALL JVM_IsConstructorIx(JNIEnv* env, jclass cls, int method_index)
{
    return 0;
}

jboolean JNICALL JVM_IsVMGeneratedMethodIx(JNIEnv* env, jclass cls, int method_index)
{
    return 0;
}

const char* JNICALL JVM_GetMethodIxNameUTF(JNIEnv* env, jclass cls, jint method_index)
{
    return 0;
}

const char* JVM_GetMethodIxSignatureUTF(JNIEnv* env, jclass cls, jint method_index)
{
    return 0;
}

const char* JNICALL JVM_GetCPMethodNameUTF(JNIEnv* env, jclass cls, jint cp_index)
{
    return 0;
}

const char* JNICALL JVM_GetCPMethodSignatureUTF(JNIEnv* env, jclass cls, jint cp_index)
{
    return 0;
}

const char* JNICALL JVM_GetCPFieldSignatureUTF(JNIEnv* env, jclass cls, jint cp_index)
{
    return 0;
}

const char* JNICALL JVM_GetCPClassNameUTF(JNIEnv* env, jclass cls, jint cp_index)
{
    return 0;
}

const char* JNICALL JVM_GetCPFieldClassNameUTF(JNIEnv* env, jclass cls, jint cp_index)
{
    return 0;
}

const char* JNICALL JVM_GetCPMethodClassNameUTF(JNIEnv* env, jclass cls, jint cp_index)
{
    return 0;
}

jint JNICALL JVM_GetCPFieldModifiers(JNIEnv* env, jclass cls, int cp_index, jclass called_cls)
{
    return 0;
}

jint JVM_GetCPMethodModifiers(JNIEnv* env, jclass cls, int cp_index, jclass called_cls)
{
    return 0;
}

void JNICALL JVM_ReleaseUTF(const char* utf)
{

}

jboolean JNICALL JVM_IsSameClassPackage(JNIEnv* env, jclass class1, jclass class2)
{
    return 0;
}

// Networking library support ////////////////////////////////////////////////////////////////////

jint JNICALL JVM_InitializeSocketLibrary()
{
    return 0;
}

jint JNICALL JVM_Socket(jint domain, jint type, jint protocol)
{
    return socket(domain, type, protocol);
}

#ifdef WIN32
int os_socket_close(int fd) {
    return ::closesocket(fd);
}
#endif
#ifdef LINUX
inline int os_socket_close(int fd) {
    return ::close(fd);
}
#endif
#ifdef MACOS
inline int os_socket_close(int fd) {
    return ::close(fd);
}
#endif

jint JNICALL JVM_SocketClose(jint fd)
{
    return os_socket_close(fd);
}

#ifdef WIN32
int os_socket_shutdown(int fd, int howto) {
    return ::shutdown(fd, howto);
}
#endif
#ifdef LINUX
inline int os_socket_shutdown(int fd, int howto) {
    return ::shutdown(fd, howto);
}
#endif
#ifdef MACOS
inline int os_socket_shutdown(int fd, int howto) {
    return ::shutdown(fd, howto);
}
#endif

jint JNICALL JVM_SocketShutdown(jint fd, jint howto)
{
    return os_socket_shutdown(fd, howto);
}

#ifdef WIN32
int os_recv(int fd, char* buf, size_t nBytes, uint flags) {
    return ::recv(fd, buf, (int)nBytes, flags);
}
#endif
#ifdef LINUX
inline int os_recv(int fd, char* buf, size_t nBytes, uint flags) {
    RESTARTABLE_RETURN_INT(::recv(fd, buf, nBytes, flags));
}
#endif
#ifdef MACOS
inline int os_recv(int fd, char* buf, size_t nBytes, uint flags) {
    RESTARTABLE_RETURN_INT(::recv(fd, buf, nBytes, flags));
}
#endif

jint JNICALL JVM_Recv(jint fd, char* buf, jint nBytes, jint flags)
{
    return os_recv(fd, buf, (size_t)nBytes, (uint)flags);
}

#ifdef WIN32
int os_send(int fd, char* buf, size_t nBytes, uint flags) {
    return ::send(fd, buf, (int)nBytes, flags);
}
#endif
#ifdef LINUX
inline int os_send(int fd, char* buf, size_t nBytes, uint flags) {
    RESTARTABLE_RETURN_INT(::send(fd, buf, nBytes, flags));
}
#endif
#ifdef MACOS
inline int os_send(int fd, char* buf, size_t nBytes, uint flags) {
    RESTARTABLE_RETURN_INT(::send(fd, buf, nBytes, flags));
}
#endif

jint JNICALL JVM_Send(jint fd, char* buf, jint nBytes, jint flags)
{
    return os_send(fd, buf, (size_t)nBytes, (uint)flags);
}

#ifdef WIN32
int os_timeout(int fd, long timeout) {
    fd_set tbl;
    struct timeval t;

    t.tv_sec = timeout / 1000;
    t.tv_usec = (timeout % 1000) * 1000;

    tbl.fd_count = 1;
    tbl.fd_array[0] = fd;

    return ::select(1, &tbl, 0, 0, &t);
}
#endif
#ifdef LINUX
inline int os_timeout(int fd, long timeout) {
    julong prevtime, newtime;
    struct timeval t;

    gettimeofday(&t, NULL);
    prevtime = ((julong)t.tv_sec * 1000) + t.tv_usec / 1000;

    for (;;) {
        struct pollfd pfd;

        pfd.fd = fd;
        pfd.events = POLLIN | POLLERR;

        int res = ::poll(&pfd, 1, timeout);

        if (res == OS_ERR && errno == EINTR) {

            // On Linux any value < 0 means "forever"

            if (timeout >= 0) {
                gettimeofday(&t, NULL);
                newtime = ((julong)t.tv_sec * 1000) + t.tv_usec / 1000;
                timeout -= newtime - prevtime;
                if (timeout <= 0)
                    return OS_OK;
                prevtime = newtime;
            }
        }
        else
            return res;
    }
}
#endif
#ifdef MACOS
inline int os_timeout(int fd, long timeout) {
    julong prevtime, newtime;
    struct timeval t;

    gettimeofday(&t, NULL);
    prevtime = ((julong)t.tv_sec * 1000) + t.tv_usec / 1000;

    for (;;) {
        struct pollfd pfd;

        pfd.fd = fd;
        pfd.events = POLLIN | POLLERR;

        int res = ::poll(&pfd, 1, timeout);

        if (res == OS_ERR && errno == EINTR) {

            // On Bsd any value < 0 means "forever"

            if (timeout >= 0) {
                gettimeofday(&t, NULL);
                newtime = ((julong)t.tv_sec * 1000) + t.tv_usec / 1000;
                timeout -= newtime - prevtime;
                if (timeout <= 0)
                    return OS_OK;
                prevtime = newtime;
            }
        }
        else
            return res;
    }
}
#endif

jint JNICALL JVM_Timeout(int fd, long timeout)
{
    return os_timeout(fd, timeout);
}

#ifdef WIN32
int os_listen(int fd, int count) {
    return ::listen(fd, count);
}
#endif
#ifdef LINUX
inline int os_listen(int fd, int count) {
    return ::listen(fd, count);
}
#endif
#ifdef MACOS
inline int os_listen(int fd, int count) {
    return ::listen(fd, count);
}
#endif

jint JNICALL JVM_Listen(jint fd, jint count)
{
    return os_listen(fd, count);
}

#ifdef WIN32
int os_connect(int fd, struct sockaddr* him, socklen_t len) {
    return ::connect(fd, him, len);
}
#endif
#ifdef LINUX
inline int os_connect(int fd, struct sockaddr* him, socklen_t len) {
    RESTARTABLE_RETURN_INT(::connect(fd, him, len));
}
#endif
#ifdef MACOS
inline int os_connect(int fd, struct sockaddr* him, socklen_t len) {
    RESTARTABLE_RETURN_INT(::connect(fd, him, len));
}
#endif

jint JNICALL JVM_Connect(jint fd, struct sockaddr* him, jint len)
{
    return os_connect(fd, him, (socklen_t)len);
}

#ifdef WIN32
int os_bind(int fd, struct sockaddr* him, socklen_t len) {
    return ::bind(fd, him, len);
}
#endif
#ifdef LINUX
inline int os_bind(int fd, struct sockaddr* him, socklen_t len) {
    return ::bind(fd, him, len);
}
#endif
#ifdef MACOS
inline int os_bind(int fd, struct sockaddr* him, socklen_t len) {
    return ::bind(fd, him, len);
}
#endif

jint JNICALL JVM_Bind(jint fd, struct sockaddr* him, jint len)
{
    return os_bind(fd, him, (socklen_t)len);
}

#ifdef WIN32
int os_accept(int fd, struct sockaddr* him, socklen_t* len) {
    return ::accept(fd, him, len);
}
#endif
#ifdef LINUX
inline int os_accept(int fd, struct sockaddr* him, socklen_t* len) {
    // Linux doc says this can't return EINTR, unlike accept() on Solaris.
    // But see attachListener_linux.cpp, LinuxAttachListener::dequeue().
    return (int)::accept(fd, him, len);
}
#endif
#ifdef MACOS
inline int os_accept(int fd, struct sockaddr* him, socklen_t* len) {
    // At least OpenBSD and FreeBSD can return EINTR from accept.
    RESTARTABLE_RETURN_INT(::accept(fd, him, len));
}
#endif

jint JNICALL JVM_Accept(jint fd, struct sockaddr* him, jint* len)
{
    socklen_t socklen = (socklen_t)(*len);
    jint result = os_accept(fd, him, &socklen);
    *len = (jint)socklen;
    return result;
}

#ifdef WIN32
int os_recvfrom(int fd, char* buf, size_t nBytes, uint flags,
    sockaddr* from, socklen_t* fromlen) {

    return ::recvfrom(fd, buf, (int)nBytes, flags, from, fromlen);
}
#endif
#ifdef LINUX
inline int os_recvfrom(int fd, char* buf, size_t nBytes, uint flags,
    sockaddr* from, socklen_t* fromlen) {
    RESTARTABLE_RETURN_INT((int)::recvfrom(fd, buf, nBytes, flags, from, fromlen));
}
#endif
#ifdef MACOS
inline int os_recvfrom(int fd, char* buf, size_t nBytes, uint flags,
    sockaddr* from, socklen_t* fromlen) {
    RESTARTABLE_RETURN_INT((int)::recvfrom(fd, buf, nBytes, flags, from, fromlen));
}
#endif

jint JNICALL JVM_RecvFrom(jint fd, char* buf, int nBytes, int flags, struct sockaddr* from, int* fromlen)
{
    socklen_t socklen = (socklen_t)(*fromlen);
    jint result = os_recvfrom(fd, buf, (size_t)nBytes, (uint)flags, from, &socklen);
    *fromlen = (int)socklen;
    return result;
}

#ifdef WIN32
int os_get_sock_name(int fd, struct sockaddr* him, socklen_t* len) {
    return ::getsockname(fd, him, len);
}
#endif
#ifdef LINUX
inline int os_get_sock_name(int fd, struct sockaddr* him, socklen_t* len) {
    return ::getsockname(fd, him, len);
}
#endif
#ifdef MACOS
inline int os_get_sock_name(int fd, struct sockaddr* him, socklen_t* len) {
    return ::getsockname(fd, him, len);
}
#endif

jint JNICALL JVM_GetSockName(jint fd, struct sockaddr* him, int* len)
{
    socklen_t socklen = (socklen_t)(*len);
    jint result = os_get_sock_name(fd, him, &socklen);
    *len = (int)socklen;
    return result;
}

#ifdef WIN32
int os_sendto(int fd, char* buf, size_t len, uint flags,
    struct sockaddr* to, socklen_t tolen) {

    return ::sendto(fd, buf, (int)len, flags, to, tolen);
}
#endif
#ifdef LINUX
inline int os_sendto(int fd, char* buf, size_t len, uint flags,
    struct sockaddr* to, socklen_t tolen) {
    RESTARTABLE_RETURN_INT((int)::sendto(fd, buf, len, flags, to, tolen));
}
#endif
#ifdef MACOS
inline int os_sendto(int fd, char* buf, size_t len, uint flags,
    struct sockaddr* to, socklen_t tolen) {
    RESTARTABLE_RETURN_INT((int)::sendto(fd, buf, len, flags, to, tolen));
}
#endif

jint JNICALL JVM_SendTo(jint fd, char* buf, int len, int flags, struct sockaddr* to, int tolen)
{
    return os_sendto(fd, buf, (size_t)len, (uint)flags, to, (socklen_t)tolen);
}

#ifdef WIN32
int os_socket_available(int fd, jint* pbytes) {
    int ret = ::ioctlsocket(fd, FIONREAD, (u_long*)pbytes);
    return (ret < 0) ? 0 : 1;
}
#endif
#ifdef LINUX
int os_socket_available(int fd, jint* pbytes) {
    // Linux doc says EINTR not returned, unlike Solaris
    int ret = ::ioctl(fd, FIONREAD, pbytes);

    //%% note ioctl can return 0 when successful, JVM_SocketAvailable
    // is expected to return 0 on failure and 1 on success to the jdk.
    return (ret < 0) ? 0 : 1;
}
#endif
#ifdef MACOS
int os_socket_available(int fd, jint* pbytes) {
    if (fd < 0)
        return OS_OK;

    int ret;

    RESTARTABLE(::ioctl(fd, FIONREAD, pbytes), ret);

    //%% note ioctl can return 0 when successful, JVM_SocketAvailable
    // is expected to return 0 on failure and 1 on success to the jdk.

    return (ret == OS_ERR) ? 0 : 1;
}
#endif

jint JNICALL JVM_SocketAvailable(jint fd, jint* pbytes)
{
    return os_socket_available(fd, pbytes);
}

#ifdef WIN32
int os_get_sock_opt(int fd, int level, int optname,
    char* optval, socklen_t* optlen) {
    return ::getsockopt(fd, level, optname, optval, optlen);
}
#endif
#ifdef LINUX
inline int os_get_sock_opt(int fd, int level, int optname,
    char* optval, socklen_t* optlen) {
    return ::getsockopt(fd, level, optname, optval, optlen);
}
#endif
#ifdef MACOS
inline int os_get_sock_opt(int fd, int level, int optname,
    char* optval, socklen_t* optlen) {
    return ::getsockopt(fd, level, optname, optval, optlen);
}
#endif

jint JNICALL JVM_GetSockOpt(jint fd, int level, int optname, char* optval, int* optlen)
{
    socklen_t socklen = (socklen_t)(*optlen);
    jint result = os_get_sock_opt(fd, level, optname, optval, &socklen);
    *optlen = (int)socklen;
    return result;
}

#ifdef WIN32
int os_set_sock_opt(int fd, int level, int optname,
    const char* optval, socklen_t optlen) {
    return ::setsockopt(fd, level, optname, optval, optlen);
}
#endif
#ifdef LINUX
inline int os_set_sock_opt(int fd, int level, int optname,
    const char* optval, socklen_t optlen) {
    return ::setsockopt(fd, level, optname, optval, optlen);
}
#endif
#ifdef MACOS
inline int os_set_sock_opt(int fd, int level, int optname,
    const char* optval, socklen_t optlen) {
    return ::setsockopt(fd, level, optname, optval, optlen);
}
#endif

jint JNICALL JVM_SetSockOpt(jint fd, int level, int optname, const char* optval, int optlen)
{
    return os_set_sock_opt(fd, level, optname, optval, (socklen_t)optlen);
}

int JNICALL JVM_GetHostName(char* name, int namelen)
{
    return gethostname(name, namelen);
}

// Library support ///////////////////////////////////////////////////////////////////////////

#ifdef WIN32
void* os_dll_load(const char* filename, char* ebuf, int ebuflen)
{
    void* result = ::LoadLibraryEx(filename, 0, LOAD_LIBRARY_SEARCH_DEFAULT_DIRS | LOAD_LIBRARY_SEARCH_DLL_LOAD_DIR);
    if (result != NULL)
    {
        return result;
    }

    DWORD errcode = GetLastError();
    if (errcode == ERROR_MOD_NOT_FOUND) {
        strncpy(ebuf, "Can't find dependent libraries", ebuflen - 1);
        ebuf[ebuflen - 1] = '\0';
        return NULL;
    }

    os_lasterror(ebuf, (size_t)ebuflen);
    ebuf[ebuflen - 1] = '\0';
    return NULL;
}
#endif
#ifdef LINUX
void* os_dll_load(const char* filename, char* ebuf, int ebuflen) {
    void* result = ::dlopen(filename, RTLD_LAZY);
    if (result == NULL) {
        ::strncpy(ebuf, ::dlerror(), ebuflen - 1);
        ebuf[ebuflen - 1] = '\0';
    }
    return result;
}
#endif
#ifdef MACOS
void* os_dll_load(const char* filename, char* ebuf, int ebuflen) {
    void* result = ::dlopen(filename, RTLD_LAZY);
    if (result != NULL) {
        // Successful loading
        return result;
    }

    // Read system error message into ebuf
    ::strncpy(ebuf, ::dlerror(), ebuflen - 1);
    ebuf[ebuflen - 1] = '\0';

    return NULL;
}
#endif

void* JNICALL JVM_LoadLibrary(const char* name)
{
    char ebuf[1024];
    void* load_result;
    load_result = os_dll_load(name, ebuf, sizeof ebuf);
    if (load_result == NULL) {
        char msg[1024];
        jio_snprintf(msg, sizeof msg, "%s: %s", name, ebuf);
        return NULL;
    }

    return load_result;
}

#ifdef WIN32
inline void  os_dll_unload(void* lib) {
    ::FreeLibrary((HMODULE)lib);
}
#endif
#ifdef LINUX
inline void os_dll_unload(void* lib) {
    ::dlclose(lib);
}
#endif
#ifdef MACOS
inline void os_dll_unload(void* lib) {
    ::dlclose(lib);
}
#endif

void JNICALL JVM_UnloadLibrary(void* handle)
{
    os_dll_unload(handle);
}

#ifdef WIN32
inline void* os_dll_lookup(void* lib, const char* name) {
    return (void*)::GetProcAddress((HMODULE)lib, name);
}
#endif
#ifdef LINUX
static pthread_mutex_t dl_mutex;

/*
 * glibc-2.0 libdl is not MT safe.  If you are building with any glibc,
 * chances are you might want to run the generated bits against glibc-2.0
 * libdl.so, so always use locking for any version of glibc.
 */
void* os_dll_lookup(void* handle, const char* name) {
    pthread_mutex_lock(&dl_mutex);
    void* res = dlsym(handle, name);
    pthread_mutex_unlock(&dl_mutex);
    return res;
}
#endif
#ifdef MACOS
void* os_dll_lookup(void* handle, const char* name) {
    return dlsym(handle, name);
}
#endif

void* JNICALL JVM_FindLibraryEntry(void* handle, const char* name)
{
    return os_dll_lookup(handle, name);
}

#ifdef __cplusplus
extern "C" {
#endif

    int jio_vsnprintf(char* str, size_t count, const char* fmt, va_list args)
    {
        // Reject count values that are negative signed values converted to
        // unsigned; see bug 4399518, 4417214
        if ((intptr_t)count <= 0) return -1;

        int result = vsnprintf(str, count, fmt, args);
        if (result > 0 && (size_t)result >= count) {
            result = -1;
        }

        return result;
    }

    int jio_snprintf(char* str, size_t count, const char* fmt, ...) {
        va_list args;
        int len;
        va_start(args, fmt);
        len = jio_vsnprintf(str, count, fmt, args);
        va_end(args);
        return len;
    }

    int jio_fprintf(FILE* f, const char* fmt, ...) {
        int len;
        va_list args;
        va_start(args, fmt);
        len = jio_vfprintf(f, fmt, args);
        va_end(args);
        return len;
    }

    int jio_vfprintf(FILE* f, const char* fmt, va_list args)
    {
        return vfprintf(f, fmt, args);
    }

    int jio_printf(const char* fmt, ...) {
        int len;
        va_list args;
        va_start(args, fmt);
        len = jio_vfprintf(stdout, fmt, args);
        va_end(args);
        return len;
    }

#ifdef __cplusplus
}
#endif
