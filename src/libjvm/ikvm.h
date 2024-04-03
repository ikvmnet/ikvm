#ifdef WIN32
#define NETEXPORT __declspec(dllexport)
#define NETCALL  __stdcall
#else
#define NETEXPORT
#define NETCALL
#endif

#ifdef __cplusplus
extern "C" {
#endif

typedef void IKVM_ThrowException_Func(const char *name, const char *msg);

void IKVM_ThrowException(const char *name, const char *msg);

#ifdef __cplusplus
}
#endif
