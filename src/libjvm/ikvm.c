#include "ikvm.h"

static IKVM_ThrowException_Func *IKVM_ThrowException_Ptr;

NETEXPORT void NETCALL Set_IKVM_ThrowException(IKVM_ThrowException_Func *func)
{
    IKVM_ThrowException_Ptr = func;
}

void IKVM_ThrowException(const char *name, const char *msg)
{
    (*IKVM_ThrowException_Ptr)(name, msg);
}
