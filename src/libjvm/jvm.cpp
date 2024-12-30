#include <jni.h>
#include <jvm.h>
#include <stdio.h>
#include <string.h>
#include <fcntl.h>

#include "ikvm.h"

#if defined WIN32
#include <windows.h>
#include <winsock2.h>
#include <ws2tcpip.h>
#include <stdint.h>
#include <io.h>
#endif

#if defined LINUX || defined MACOS || defined ANDROID || defined EMSCRIPTEN
#include <unistd.h>
#include <pthread.h>
#include <errno.h>
#include <dlfcn.h>
#include <sys/socket.h>
#include <sys/time.h>
#include <sys/ioctl.h>
#include <time.h>
#include <poll.h>
#include <stdint.h>
#include <sys/stat.h>
#endif

#ifdef __cplusplus
extern "C" {
#endif

JVMInvokeInterface *jvmii;

void JNICALL JVM_Init(JVMInvokeInterface *p_jvmii)
{
    jvmii = p_jvmii;
}

void JNICALL JVM_ThrowException(const char *name, const char *msg)
{
    jvmii->JVM_ThrowException(name, msg);
}

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

inline void set_low(jlong* value, jint low)
{
    *value &= (jlong)0xffffffff << 32;
    *value |= (jlong)(julong)(juint)low;
}

inline void set_high(jlong* value, jint high)
{
    *value &= (jlong)(julong)(juint)0xffffffff;
    *value |= (jlong)high << 32;
}

inline jlong jlong_from(jint h, jint l) 
{
    jlong result = 0; // initialization to avoid warning
    set_high(&result, h);
    set_low(&result, l);
    return result;
}

// Platform-independent error return values from OS functions
enum OSReturn
{
    OS_OK = 0,          // Operation was successful
    OS_ERR = -1,        // Operation failed
    OS_INTRPT = -2,     // Operation was interrupted
    OS_TIMEOUT = -3,    // Operation timed out
    OS_NOMEM = -5,      // Operation failed for lack of memory
    OS_NORESOURCE = -6  // Operation failed for lack of nonmemory resource
};

#if defined LINUX || defined MACOS || defined EMSCRIPTEN

// macro for restartable system calls
#define RESTARTABLE(_cmd, _result) do { \
    _result = _cmd; \
} while(((int)_result == OS_ERR) && (errno == EINTR))

// macro for restartable system calls
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

static jlong _offset = 116444736000000000; // Constant offset - calculated using offset()
static jlong fake_time = 0; // Fake time counter for reproducible results when debugging
inline jlong windows_to_java_time(FILETIME wt)
{
    jlong a = jlong_from(wt.dwHighDateTime, wt.dwLowDateTime);
    return (a - _offset) / 10000;
}

#endif

const jint NANOSECS_PER_MILLISEC = 1000000;

#ifdef WIN32
jlong os_javaTimeMillis()
{
    FILETIME wt;
    GetSystemTimeAsFileTime(&wt);
    return windows_to_java_time(wt);
}
#endif
#if defined LINUX || defined EMSCRIPTEN
jlong os_javaTimeMillis()
{
    timeval time;
    int status = gettimeofday(&time, NULL);
    assert(status != -1, "linux error");
    return jlong(time.tv_sec) * 1000 + jlong(time.tv_usec / 1000);
}
#endif
#ifdef MACOS
jlong os_javaTimeMillis()
{
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
jlong os_javaTimeNanos()
{
    return os_javaTimeMillis() * NANOSECS_PER_MILLISEC;
}
#endif
#if defined LINUX || defined EMSCRIPTEN
jlong os_javaTimeNanos()
{
    struct timespec tp;
    int status = clock_gettime(CLOCK_MONOTONIC, &tp);
    jlong result = jlong(tp.tv_sec) * (1000 * 1000 * 1000) + jlong(tp.tv_nsec);
    return result;
}
#endif
#ifdef MACOS
jlong os_javaTimeNanos()
{
    return os_javaTimeMillis() * NANOSECS_PER_MILLISEC;
}
#endif

jlong JNICALL JVM_NanoTime(JNIEnv *env, jclass ignored)
{
    return os_javaTimeNanos();
}

void JNICALL JVM_ArrayCopy(JNIEnv *env, jclass ignored, jobject src, jint src_pos, jobject dst, jint dst_pos, jint length)
{
    jvmii->JVM_ArrayCopy(env, ignored, src, src_pos, dst, dst_pos, length);
}

void JNICALL JVM_CopySwapMemory(JNIEnv *env, jobject srcObj, jlong srcOffset, jobject dstObj, jlong dstOffset, jlong size, jlong elemSize)
{
    jvmii->JVM_CopySwapMemory(env, srcObj, srcOffset, dstObj, dstOffset, size, elemSize);
}

jobject JNICALL JVM_InitProperties(JNIEnv *env, jobject properties)
{
    return jvmii->JVM_InitProperties(env, properties);
}

jstring JNICALL JVM_GetTemporaryDirectory(JNIEnv *env)
{
    JVM_ThrowException("java/lang/InternalError", "Unsupported JVM method: JVM_GetTemporaryDirectory");
    return 0;
}

#ifdef WIN32
size_t os_lasterror(char* buf, size_t len)
{
    DWORD errval;

    if ((errval = GetLastError()) != 0) {
        LPWSTR w = new wchar_t[len];
        size_t n = (size_t)::FormatMessageW(
            FORMAT_MESSAGE_FROM_SYSTEM | FORMAT_MESSAGE_IGNORE_INSERTS,
            NULL,
            errval,
            0,
            w,
            (DWORD)len,
            NULL);
        w[n] = '\0';

        n = ::WideCharToMultiByte(CP_UTF8, 0, w, -1, buf, 0, NULL, NULL);
        if (n >= len) n = len - 1;
        ::WideCharToMultiByte(CP_UTF8, 0, w, -1, buf, n, NULL, NULL);
        buf[n] = '\0';

        delete[] w;
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
#if defined LINUX || defined EMSCRIPTEN
size_t os_lasterror(char* buf, size_t len)
{
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
size_t os_lasterror(char* buf, size_t len)
{
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

jint JNICALL JVM_ActiveProcessorCount()
{
    return jvmii->JVM_ActiveProcessorCount();
}

jboolean JNICALL JVM_IsUseContainerSupport()
{
    return JNI_FALSE;
}

jclass JNICALL JVM_FindClassFromClass(JNIEnv* env, const char* name, jboolean init, jclass from)
{
    JVM_ThrowException("java/lang/InternalError", "Unsupported JVM method: JVM_FindClassFromClass");
    return 0;
}

jclass JNICALL JVM_FindClassFromClassLoader(JNIEnv* env, const char* name, jboolean init, jobject loader, jboolean throwError)
{
    JVM_ThrowException("java/lang/InternalError", "Unsupported JVM method: JVM_FindClassFromClassLoader");
    return 0;
}

jboolean JNICALL JVM_IsInterface(JNIEnv* env, jclass cls)
{
    JVM_ThrowException("java/lang/InternalError", "Unsupported JVM method: JVM_IsInterface");
    return 0;
}

const char* JNICALL JVM_GetClassNameUTF(JNIEnv* env, jclass cls)
{
    JVM_ThrowException("java/lang/InternalError", "Unsupported JVM method: JVM_GetClassNameUTF");
    return 0;
}

void JNICALL JVM_GetClassCPTypes(JNIEnv* env, jclass cls, unsigned char* types)
{
    JVM_ThrowException("java/lang/InternalError", "Unsupported JVM method: JVM_GetClassCPTypes");
}

jint JNICALL JVM_GetClassCPEntriesCount(JNIEnv* env, jclass cls)
{
    JVM_ThrowException("java/lang/InternalError", "Unsupported JVM method: JVM_GetClassCPEntriesCount");
    return 0;
}

jint JNICALL JVM_GetClassFieldsCount(JNIEnv* env, jclass cls)
{
    JVM_ThrowException("java/lang/InternalError", "Unsupported JVM method: JVM_GetClassFieldsCount");
    return 0;
}


jint JNICALL JVM_GetClassMethodsCount(JNIEnv* env, jclass cls)
{
    JVM_ThrowException("java/lang/InternalError", "Unsupported JVM method: JVM_GetClassMethodsCount");
    return 0;
}

void JNICALL JVM_GetMethodIxExceptionIndexes(JNIEnv* env, jclass cls, jint method_index, unsigned short* exceptions)
{
    JVM_ThrowException("java/lang/InternalError", "Unsupported JVM method: JVM_GetMethodIxExceptionIndexes");
}

jint JNICALL JVM_GetMethodIxExceptionsCount(JNIEnv* env, jclass cls, jint method_index)
{
    JVM_ThrowException("java/lang/InternalError", "Unsupported JVM method: JVM_GetMethodIxExceptionsCount");
    return 0;
}

void JNICALL JVM_GetMethodIxByteCode(JNIEnv* env, jclass cls, jint method_index, unsigned char* code)
{
    JVM_ThrowException("java/lang/InternalError", "Unsupported JVM method: JVM_GetMethodIxByteCode");
}

jint JNICALL JVM_GetMethodIxByteCodeLength(JNIEnv* env, jclass cls, jint method_index)
{
    JVM_ThrowException("java/lang/InternalError", "Unsupported JVM method: JVM_GetMethodIxByteCodeLength");
    return 0;
}

void JNICALL JVM_GetMethodIxExceptionTableEntry(JNIEnv* env, jclass cls, jint method_index, jint entry_index, JVM_ExceptionTableEntryType* entry)
{
    JVM_ThrowException("java/lang/InternalError", "Unsupported JVM method: JVM_GetMethodIxExceptionTableEntry");
}

jint JNICALL JVM_GetMethodIxExceptionTableLength(JNIEnv* env, jclass cls, int method_index)
{
    JVM_ThrowException("java/lang/InternalError", "Unsupported JVM method: JVM_GetMethodIxExceptionTableLength");
    return 0;
}

jint JNICALL JVM_GetMethodIxModifiers(JNIEnv* env, jclass cls, int method_index)
{
    JVM_ThrowException("java/lang/InternalError", "Unsupported JVM method: JVM_GetMethodIxModifiers");
    return 0;
}

jint JNICALL JVM_GetFieldIxModifiers(JNIEnv* env, jclass cls, int field_index)
{
    JVM_ThrowException("java/lang/InternalError", "Unsupported JVM method: JVM_GetFieldIxModifiers");
    return 0;
}

jint JNICALL  JVM_GetMethodIxLocalsCount(JNIEnv* env, jclass cls, int method_index)
{
    JVM_ThrowException("java/lang/InternalError", "Unsupported JVM method: JVM_GetMethodIxLocalsCount");
    return 0;
}

jint JNICALL JVM_GetMethodIxArgsSize(JNIEnv* env, jclass cls, int method_index)
{
    JVM_ThrowException("java/lang/InternalError", "Unsupported JVM method: JVM_GetMethodIxArgsSize");
    return 0;
}

jint JNICALL JVM_GetMethodIxMaxStack(JNIEnv* env, jclass cls, int method_index)
{
    JVM_ThrowException("java/lang/InternalError", "Unsupported JVM method: JVM_GetMethodIxMaxStack");
    return 0;
}

jboolean JNICALL JVM_IsConstructorIx(JNIEnv* env, jclass cls, int method_index)
{
    JVM_ThrowException("java/lang/InternalError", "Unsupported JVM method: JVM_IsConstructorIx");
    return 0;
}

jboolean JNICALL JVM_IsVMGeneratedMethodIx(JNIEnv* env, jclass cls, int method_index)
{
    JVM_ThrowException("java/lang/InternalError", "Unsupported JVM method: JVM_IsVMGeneratedMethodIx");
    return 0;
}

const char* JNICALL JVM_GetMethodIxNameUTF(JNIEnv* env, jclass cls, jint method_index)
{
    JVM_ThrowException("java/lang/InternalError", "Unsupported JVM method: JVM_GetMethodIxNameUTF");
    return 0;
}

const char* JVM_GetMethodIxSignatureUTF(JNIEnv* env, jclass cls, jint method_index)
{
    JVM_ThrowException("java/lang/InternalError", "Unsupported JVM method: JVM_GetMethodIxSignatureUTF");
    return 0;
}

const char* JNICALL JVM_GetCPMethodNameUTF(JNIEnv* env, jclass cls, jint cp_index)
{
    JVM_ThrowException("java/lang/InternalError", "Unsupported JVM method: JVM_GetCPMethodNameUTF");
    return 0;
}

const char* JNICALL JVM_GetCPMethodSignatureUTF(JNIEnv* env, jclass cls, jint cp_index)
{
    JVM_ThrowException("java/lang/InternalError", "Unsupported JVM method: JVM_GetCPMethodSignatureUTF");
    return 0;
}

const char* JNICALL JVM_GetCPFieldSignatureUTF(JNIEnv* env, jclass cls, jint cp_index)
{
    JVM_ThrowException("java/lang/InternalError", "Unsupported JVM method: JVM_GetCPFieldSignatureUTF");
    return 0;
}

const char* JNICALL JVM_GetCPClassNameUTF(JNIEnv* env, jclass cls, jint cp_index)
{
    JVM_ThrowException("java/lang/InternalError", "Unsupported JVM method: JVM_GetCPClassNameUTF");
    return 0;
}

const char* JNICALL JVM_GetCPFieldClassNameUTF(JNIEnv* env, jclass cls, jint cp_index)
{
    JVM_ThrowException("java/lang/InternalError", "Unsupported JVM method: JVM_GetCPFieldClassNameUTF");
    return 0;
}

const char* JNICALL JVM_GetCPMethodClassNameUTF(JNIEnv* env, jclass cls, jint cp_index)
{
    JVM_ThrowException("java/lang/InternalError", "Unsupported JVM method: JVM_GetCPMethodClassNameUTF");
    return 0;
}

jint JNICALL JVM_GetCPFieldModifiers(JNIEnv* env, jclass cls, int cp_index, jclass called_cls)
{
    JVM_ThrowException("java/lang/InternalError", "Unsupported JVM method: JVM_GetCPFieldModifiers");
    return 0;
}

jint JVM_GetCPMethodModifiers(JNIEnv* env, jclass cls, int cp_index, jclass called_cls)
{
    JVM_ThrowException("java/lang/InternalError", "Unsupported JVM method: JVM_GetCPMethodModifiers");
    return 0;
}

void JNICALL JVM_ReleaseUTF(const char* utf)
{
    JVM_ThrowException("java/lang/InternalError", "Unsupported JVM method: JVM_ReleaseUTF");
}

jboolean JNICALL JVM_IsSameClassPackage(JNIEnv* env, jclass class1, jclass class2)
{
    JVM_ThrowException("java/lang/InternalError", "Unsupported JVM method: JVM_IsSameClassPackage");
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
int os_socket_close(int fd)
{
    return ::closesocket(fd);
}
#endif
#if defined LINUX || defined EMSCRIPTEN
inline int os_socket_close(int fd)
{
    return ::close(fd);
}
#endif
#ifdef MACOS
inline int os_socket_close(int fd)
{
    return ::close(fd);
}
#endif

jint JNICALL JVM_SocketClose(jint fd)
{
    return os_socket_close(fd);
}

#ifdef WIN32
int os_socket_shutdown(int fd, int howto)
{
    return ::shutdown(fd, howto);
}
#endif
#if defined LINUX || defined EMSCRIPTEN
inline int os_socket_shutdown(int fd, int howto)
{
    return ::shutdown(fd, howto);
}
#endif
#ifdef MACOS
inline int os_socket_shutdown(int fd, int howto)
{
    return ::shutdown(fd, howto);
}
#endif

jint JNICALL JVM_SocketShutdown(jint fd, jint howto)
{
    return os_socket_shutdown(fd, howto);
}

#ifdef WIN32
int os_recv(int fd, char* buf, size_t nBytes, uint flags)
{
    return ::recv(fd, buf, (int)nBytes, flags);
}
#endif
#if defined LINUX || defined EMSCRIPTEN
inline int os_recv(int fd, char* buf, size_t nBytes, uint flags)
{
    RESTARTABLE_RETURN_INT(::recv(fd, buf, nBytes, flags));
}
#endif
#ifdef MACOS
inline int os_recv(int fd, char* buf, size_t nBytes, uint flags)
{
    RESTARTABLE_RETURN_INT(::recv(fd, buf, nBytes, flags));
}
#endif

jint JNICALL JVM_Recv(jint fd, char* buf, jint nBytes, jint flags)
{
    return os_recv(fd, buf, (size_t)nBytes, (uint)flags);
}

#ifdef WIN32
int os_send(int fd, char* buf, size_t nBytes, uint flags)
{
    return ::send(fd, buf, (int)nBytes, flags);
}
#endif
#if defined LINUX || defined EMSCRIPTEN
inline int os_send(int fd, char* buf, size_t nBytes, uint flags)
{
    RESTARTABLE_RETURN_INT(::send(fd, buf, nBytes, flags));
}
#endif
#ifdef MACOS
inline int os_send(int fd, char* buf, size_t nBytes, uint flags)
{
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
#if defined LINUX || defined EMSCRIPTEN
inline int os_timeout(int fd, long timeout)
{
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
inline int os_timeout(int fd, long timeout)
{
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
int os_listen(int fd, int count)
{
    return ::listen(fd, count);
}
#endif
#if defined LINUX || defined EMSCRIPTEN
inline int os_listen(int fd, int count)
{
    return ::listen(fd, count);
}
#endif
#ifdef MACOS
inline int os_listen(int fd, int count)
{
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
#if defined LINUX || defined EMSCRIPTEN
inline int os_connect(int fd, struct sockaddr* him, socklen_t len)
{
    RESTARTABLE_RETURN_INT(::connect(fd, him, len));
}
#endif
#ifdef MACOS
inline int os_connect(int fd, struct sockaddr* him, socklen_t len)
{
    RESTARTABLE_RETURN_INT(::connect(fd, him, len));
}
#endif

jint JNICALL JVM_Connect(jint fd, struct sockaddr* him, jint len)
{
    return os_connect(fd, him, (socklen_t)len);
}

#ifdef WIN32
int os_bind(int fd, struct sockaddr* him, socklen_t len)
{
    return ::bind(fd, him, len);
}
#endif
#if defined LINUX || defined EMSCRIPTEN
inline int os_bind(int fd, struct sockaddr* him, socklen_t len)
{
    return ::bind(fd, him, len);
}
#endif
#ifdef MACOS
inline int os_bind(int fd, struct sockaddr* him, socklen_t len)
{
    return ::bind(fd, him, len);
}
#endif

jint JNICALL JVM_Bind(jint fd, struct sockaddr* him, jint len)
{
    return os_bind(fd, him, (socklen_t)len);
}

#ifdef WIN32
int os_accept(int fd, struct sockaddr* him, socklen_t* len)
{
    return ::accept(fd, him, len);
}
#endif
#if defined LINUX || defined EMSCRIPTEN
inline int os_accept(int fd, struct sockaddr* him, socklen_t* len)
{
    // Linux doc says this can't return EINTR, unlike accept() on Solaris.
    // But see attachListener_linux.cpp, LinuxAttachListener::dequeue().
    return (int)::accept(fd, him, len);
}
#endif
#ifdef MACOS
inline int os_accept(int fd, struct sockaddr* him, socklen_t* len)
{
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
int os_recvfrom(int fd, char* buf, size_t nBytes, uint flags, sockaddr* from, socklen_t* fromlen)
{
    return ::recvfrom(fd, buf, (int)nBytes, flags, from, fromlen);
}
#endif
#if defined LINUX || defined EMSCRIPTEN
inline int os_recvfrom(int fd, char* buf, size_t nBytes, uint flags, sockaddr* from, socklen_t* fromlen)
{
    RESTARTABLE_RETURN_INT((int)::recvfrom(fd, buf, nBytes, flags, from, fromlen));
}
#endif
#ifdef MACOS
inline int os_recvfrom(int fd, char* buf, size_t nBytes, uint flags, sockaddr* from, socklen_t* fromlen)
{
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
int os_get_sock_name(int fd, struct sockaddr* him, socklen_t* len)
{
    return ::getsockname(fd, him, len);
}
#endif
#if defined LINUX || defined EMSCRIPTEN
inline int os_get_sock_name(int fd, struct sockaddr* him, socklen_t* len)
{
    return ::getsockname(fd, him, len);
}
#endif
#ifdef MACOS
inline int os_get_sock_name(int fd, struct sockaddr* him, socklen_t* len)
{
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
int os_sendto(int fd, char* buf, size_t len, uint flags, struct sockaddr* to, socklen_t tolen)
{
    return ::sendto(fd, buf, (int)len, flags, to, tolen);
}
#endif
#if defined LINUX || defined EMSCRIPTEN
inline int os_sendto(int fd, char* buf, size_t len, uint flags, struct sockaddr* to, socklen_t tolen)
{
    RESTARTABLE_RETURN_INT((int)::sendto(fd, buf, len, flags, to, tolen));
}
#endif
#ifdef MACOS
inline int os_sendto(int fd, char* buf, size_t len, uint flags, struct sockaddr* to, socklen_t tolen)
{
    RESTARTABLE_RETURN_INT((int)::sendto(fd, buf, len, flags, to, tolen));
}
#endif

jint JNICALL JVM_SendTo(jint fd, char* buf, int len, int flags, struct sockaddr* to, int tolen)
{
    return os_sendto(fd, buf, (size_t)len, (uint)flags, to, (socklen_t)tolen);
}

#ifdef WIN32
int os_socket_available(int fd, jint* pbytes)
{
    int ret = ::ioctlsocket(fd, FIONREAD, (u_long*)pbytes);
    return (ret < 0) ? 0 : 1;
}
#endif
#if defined LINUX || defined EMSCRIPTEN
int os_socket_available(int fd, jint* pbytes)
{
    // Linux doc says EINTR not returned, unlike Solaris
    int ret = ::ioctl(fd, FIONREAD, pbytes);

    //%% note ioctl can return 0 when successful, JVM_SocketAvailable
    // is expected to return 0 on failure and 1 on success to the jdk.
    return (ret < 0) ? 0 : 1;
}
#endif
#ifdef MACOS
int os_socket_available(int fd, jint* pbytes)
{
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
int os_get_sock_opt(int fd, int level, int optname, char* optval, socklen_t* optlen)
{
    return ::getsockopt(fd, level, optname, optval, optlen);
}
#endif
#if defined LINUX || defined EMSCRIPTEN
inline int os_get_sock_opt(int fd, int level, int optname, char* optval, socklen_t* optlen)
{
    return ::getsockopt(fd, level, optname, optval, optlen);
}
#endif
#ifdef MACOS
inline int os_get_sock_opt(int fd, int level, int optname, char* optval, socklen_t* optlen)
{
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
int os_set_sock_opt(int fd, int level, int optname, const char* optval, socklen_t optlen)
{
    return ::setsockopt(fd, level, optname, optval, optlen);
}
#endif
#if defined LINUX || defined EMSCRIPTEN
inline int os_set_sock_opt(int fd, int level, int optname, const char* optval, socklen_t optlen)
{
    return ::setsockopt(fd, level, optname, optval, optlen);
}
#endif
#ifdef MACOS
inline int os_set_sock_opt(int fd, int level, int optname, const char* optval, socklen_t optlen)
{
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

#ifdef WIN32
void* os_dll_load(const char* filename, char* ebuf, int ebuflen)
{
    size_t sfilename = (size_t)::MultiByteToWideChar(CP_UTF8, 0, filename, -1, NULL, 0);
    LPWSTR wfilename = new WCHAR[sfilename];
    ::MultiByteToWideChar(CP_UTF8, 0, filename, -1, wfilename, sfilename);

    void* result = ::LoadLibraryExW(wfilename, 0, LOAD_LIBRARY_SEARCH_DEFAULT_DIRS | LOAD_LIBRARY_SEARCH_DLL_LOAD_DIR);
    delete[] wfilename;
    if (result != NULL) {
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
#if defined LINUX || defined EMSCRIPTEN
void* os_dll_load(const char* filename, char* ebuf, int ebuflen)
{
    void* result = ::dlopen(filename, RTLD_LAZY);
    if (result == NULL) {
        ::strncpy(ebuf, ::dlerror(), ebuflen - 1);
        ebuf[ebuflen - 1] = '\0';
    }

    return result;
}
#endif
#ifdef MACOS
void* os_dll_load(const char* filename, char* ebuf, int ebuflen)
{
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
        JVM_ThrowException("java/lang/UnsatisfiedLinkError", (const char*)msg);
        return NULL;
    }

    return load_result;
}

#ifdef WIN32
inline char* os_native_path(char* path)
{
  char *src = path, *dst = path, *end = path;
  char *colon = NULL;           /* If a drive specifier is found, this will
                                        point to the colon following the drive
                                        letter */

  /* Assumption: '/', '\\', ':', and drive letters are never lead bytes */
  assert(((!::IsDBCSLeadByte('/'))
    && (!::IsDBCSLeadByte('\\'))
    && (!::IsDBCSLeadByte(':'))),
    "Illegal lead byte");

  /* Check for leading separators */
#define isfilesep(c) ((c) == '/' || (c) == '\\')
  while (isfilesep(*src)) {
    src++;
  }

  if (::isalpha(*src) && !::IsDBCSLeadByte(*src) && src[1] == ':') {
    /* Remove leading separators if followed by drive specifier.  This
      hack is necessary to support file URLs containing drive
      specifiers (e.g., "file://c:/path").  As a side effect,
      "/c:/path" can be used as an alternative to "c:/path". */
    *dst++ = *src++;
    colon = dst;
    *dst++ = ':';
    src++;
  } else {
    src = path;
    if (isfilesep(src[0]) && isfilesep(src[1])) {
      /* UNC pathname: Retain first separator; leave src pointed at
         second separator so that further separators will be collapsed
         into the second separator.  The result will be a pathname
         beginning with "\\\\" followed (most likely) by a host name. */
      src = dst = path + 1;
      path[0] = '\\';     /* Force first separator to '\\' */
    }
  }

  end = dst;

  /* Remove redundant separators from remainder of path, forcing all
      separators to be '\\' rather than '/'. Also, single byte space
      characters are removed from the end of the path because those
      are not legal ending characters on this operating system.
  */
  while (*src != '\0') {
    if (isfilesep(*src)) {
      *dst++ = '\\'; src++;
      while (isfilesep(*src)) src++;
      if (*src == '\0') {
        /* Check for trailing separator */
        end = dst;
        if (colon == dst - 2) break;                      /* "z:\\" */
        if (dst == path + 1) break;                       /* "\\" */
        if (dst == path + 2 && isfilesep(path[0])) {
          /* "\\\\" is not collapsed to "\\" because "\\\\" marks the
            beginning of a UNC pathname.  Even though it is not, by
            itself, a valid UNC pathname, we leave it as is in order
            to be consistent with the path canonicalizer as well
            as the win32 APIs, which treat this case as an invalid
            UNC pathname rather than as an alias for the root
            directory of the current drive. */
          break;
        }
        end = --dst;  /* Path does not denote a root directory, so
                                    remove trailing separator */
        break;
      }
      end = dst;
    } else {
      if (::IsDBCSLeadByte(*src)) { /* Copy a double-byte character */
        *dst++ = *src++;
        if (*src) *dst++ = *src++;
        end = dst;
      } else {         /* Copy a single-byte character */
        char c = *src++;
        *dst++ = c;
        /* Space is not a legal ending character */
        if (c != ' ') end = dst;
      }
    }
  }

  *end = '\0';

  /* For "z:", add "." to work around a bug in the C runtime library */
  if (colon == dst - 1) {
          path[2] = '.';
          path[3] = '\0';
  }

  return path;
}
#endif
#if defined LINUX || defined EMSCRIPTEN
inline char* os_native_path(char* path)
{
    return path;
}
#endif
#ifdef MACOS
inline char* os_native_path(char* path)
{
    return path;
}
#endif

char* JNICALL JVM_NativePath(char* path)
{
    return os_native_path(path);
}

#ifdef WIN32
inline int os_open(const char *path, int oflag, int mode) 
{
  char pathbuf[MAX_PATH];

  if (strlen(path) > MAX_PATH - 1) {
    errno = ENAMETOOLONG;
          return -1;
  }
  os_native_path(strcpy(pathbuf, path));
  return open(pathbuf, oflag | O_BINARY | O_NOINHERIT, mode);
}
#endif
#ifdef LINUX
#ifndef O_DELETE
#define O_DELETE 0x10000
#endif

inline int os_open(const char *path, int oflag, int mode) 
{
  if (strlen(path) > FILENAME_MAX - 1) {
    errno = ENAMETOOLONG;
    return -1;
  }
  int fd;
  int o_delete = (oflag & O_DELETE);
  oflag = oflag & ~O_DELETE;

  fd = ::open64(path, oflag, mode);
  if (fd == -1) return -1;

  //If the open succeeded, the file might still be a directory
  {
    struct stat64 buf64;
    int ret = ::fstat64(fd, &buf64);
    int st_mode = buf64.st_mode;

    if (ret != -1) {
      if ((st_mode & S_IFMT) == S_IFDIR) {
        errno = EISDIR;
        ::close(fd);
        return -1;
      }
    } else {
      ::close(fd);
      return -1;
    }
  }

    /*
     * All file descriptors that are opened in the JVM and not
     * specifically destined for a subprocess should have the
     * close-on-exec flag set.  If we don't set it, then careless 3rd
     * party native code might fork and exec without closing all
     * appropriate file descriptors (e.g. as we do in closeDescriptors in
     * UNIXProcess.c), and this in turn might:
     *
     * - cause end-of-file to fail to be detected on some file
     *   descriptors, resulting in mysterious hangs, or
     *
     * - might cause an fopen in the subprocess to fail on a system
     *   suffering from bug 1085341.
     *
     * (Yes, the default setting of the close-on-exec flag is a Unix
     * design flaw)
     *
     * See:
     * 1085341: 32-bit stdio routines should support file descriptors >255
     * 4843136: (process) pipe file descriptor from Runtime.exec not being closed
     * 6339493: (process) Runtime.exec does not close all file descriptors on Solaris 9
     */
#ifdef FD_CLOEXEC
    {
        int flags = ::fcntl(fd, F_GETFD);
        if (flags != -1)
            ::fcntl(fd, F_SETFD, flags | FD_CLOEXEC);
    }
#endif

  if (o_delete != 0) {
    ::unlink(path);
  }
  return fd;
}
#endif
#if defined MACOS || defined EMSCRIPTEN
#ifndef O_DELETE
#define O_DELETE 0x10000
#endif

inline int os_open(const char *path, int oflag, int mode) 
{
  if (strlen(path) > FILENAME_MAX - 1) {
    errno = ENAMETOOLONG;
    return -1;
  }
  int fd;
  int o_delete = (oflag & O_DELETE);
  oflag = oflag & ~O_DELETE;

  fd = ::open(path, oflag, mode);
  if (fd == -1) return -1;

  //If the open succeeded, the file might still be a directory
  {
    struct stat buf;
    int ret = ::fstat(fd, &buf);
    int st_mode = buf.st_mode;

    if (ret != -1) {
      if ((st_mode & S_IFMT) == S_IFDIR) {
        errno = EISDIR;
        ::close(fd);
        return -1;
      }
    } else {
      ::close(fd);
      return -1;
    }
  }

    /*
     * All file descriptors that are opened in the JVM and not
     * specifically destined for a subprocess should have the
     * close-on-exec flag set.  If we don't set it, then careless 3rd
     * party native code might fork and exec without closing all
     * appropriate file descriptors (e.g. as we do in closeDescriptors in
     * UNIXProcess.c), and this in turn might:
     *
     * - cause end-of-file to fail to be detected on some file
     *   descriptors, resulting in mysterious hangs, or
     *
     * - might cause an fopen in the subprocess to fail on a system
     *   suffering from bug 1085341.
     *
     * (Yes, the default setting of the close-on-exec flag is a Unix
     * design flaw)
     *
     * See:
     * 1085341: 32-bit stdio routines should support file descriptors >255
     * 4843136: (process) pipe file descriptor from Runtime.exec not being closed
     * 6339493: (process) Runtime.exec does not close all file descriptors on Solaris 9
     */
#ifdef FD_CLOEXEC
    {
        int flags = ::fcntl(fd, F_GETFD);
        if (flags != -1)
            ::fcntl(fd, F_SETFD, flags | FD_CLOEXEC);
    }
#endif

  if (o_delete != 0) {
    ::unlink(path);
  }
  return fd;
}
#endif

jint JNICALL JVM_Open(const char *fname, jint flags, jint mode)
{
    int result = os_open(fname, flags, mode);
    if (result >= 0) {
        return result;
    } else {
        switch (errno) {
            case EEXIST:
                return JVM_EEXIST;
            default:
                return -1;
        }
    }
}

inline int os_close(int fd)
{
    return close(fd);
}

jint JNICALL JVM_Close(jint fd)
{
    return os_close(fd);
}

#ifdef WIN32
inline void  os_dll_unload(void* lib)
{
    ::FreeLibrary((HMODULE)lib);
}
#endif
#ifdef LINUX
inline void os_dll_unload(void* lib)
{
    ::dlclose(lib);
}
#endif
#ifdef MACOS
inline void os_dll_unload(void* lib)
{
    ::dlclose(lib);
}
#endif
#ifdef EMSCRIPTEN
inline void os_dll_unload(void* lib)
{
    // no-op: emscripten cannot unload
}
#endif

void JNICALL JVM_UnloadLibrary(void* handle)
{
    os_dll_unload(handle);
}

#ifdef WIN32
inline void* os_dll_lookup(void* lib, const char* name)
{
    return (void*)::GetProcAddress((HMODULE)lib, name);
}
#endif
#ifdef LINUX
static pthread_mutex_t dl_mutex;

void* os_dll_lookup(void* handle, const char* name)
{
    pthread_mutex_lock(&dl_mutex);
    void* res = dlsym(handle, name);
    pthread_mutex_unlock(&dl_mutex);
    return res;
}
#endif
#ifdef MACOS
void *os_dll_lookup(void *handle, const char *name) {
  return dlsym(handle, name);
}
#endif
#ifdef EMSCRIPTEN
void *os_dll_lookup(void *handle, const char *name)
{
    return dlsym(handle, name);
}
#endif

void* JNICALL JVM_FindLibraryEntry(void* handle, const char* name)
{
    return os_dll_lookup(handle, name);
}

#ifdef WIN32
void* JNICALL JVM_GetThreadInterruptEvent()
{
    return jvmii->JVM_GetThreadInterruptEvent();
}
#endif

void* JNICALL JVM_RegisterSignal(jint sig, void* handler)
{
    JVM_ThrowException("java/lang/InternalError", "Unsupported JVM method: JVM_RegisterSignal");
    return 0;
}

jboolean JNICALL JVM_RaiseSignal(jint sig)
{
    JVM_ThrowException("java/lang/InternalError", "Unsupported JVM method: JVM_RaiseSignal");
    return JNI_FALSE;
}

jint JNICALL JVM_IHashCode(JNIEnv *pEnv, jobject handle)
{
    return jvmii->JVM_IHashCode(pEnv, handle);
}

void* JNICALL JVM_RawMonitorCreate()
{
    return jvmii->JVM_RawMonitorCreate();
}

void JNICALL JVM_RawMonitorDestroy(void* mon)
{
    jvmii->JVM_RawMonitorDestroy(mon);
}

jint JNICALL JVM_RawMonitorEnter(void* mon)
{
    return jvmii->JVM_RawMonitorEnter(mon);
}

void JNICALL JVM_RawMonitorExit(void* mon)
{
    jvmii->JVM_RawMonitorExit(mon);
}

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

int jio_snprintf(char* str, size_t count, const char* fmt, ...)
{
    va_list args;
    int len;
    va_start(args, fmt);
    len = jio_vsnprintf(str, count, fmt, args);
    va_end(args);
    return len;
}

int jio_fprintf(FILE* f, const char* fmt, ...)
{
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

int jio_printf(const char* fmt, ...)
{
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
