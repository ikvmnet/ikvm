/*
  Copyright (C) 2004, 2005 Jeroen Frijters

  This software is provided 'as-is', without any express or implied
  warranty.  In no event will the authors be held liable for any damages
  arising from the use of this software.

  Permission is granted to anyone to use this software for any purpose,
  including commercial applications, and to alter it and redistribute it
  freely, subject to the following restrictions:

  1. The origin of this software must not be misrepresented; you must not
     claim that you wrote the original software. If you use this software
     in a product, an acknowledgment in the product documentation would be
     appreciated but is not required.
  2. Altered source versions must be plainly marked as such, and must not be
     misrepresented as being the original software.
  3. This notice may not be removed or altered from any source distribution.

  Jeroen Frijters
  jeroen@frijters.net
  
*/
.module JVM.DLL

.method public static int32 JNI_CreateJavaVM(void* ppvm, void* ppenv, void* args)
{
  .vtentry 1:1
  .export [1] as JNI_CreateJavaVM
  ldarg.0
  ldarg.1
  ldarg.2
  call int32 [IKVM.Runtime.JNI]IKVM.Runtime.JNI::CreateJavaVM(void*, void*, void*)
  ret
}

.method public static int32 JNI_GetDefaultJavaVMInitArgs(void *vm_args)
{
  .vtentry 1:2
  .export [2] as JNI_GetDefaultJavaVMInitArgs
  ldarg.0
  call int32 [IKVM.Runtime.JNI]IKVM.Runtime.JNI::GetDefaultJavaVMInitArgs(void*)
  ret
}

.method public static int32 JNI_GetCreatedJavaVMs(void* ppvmBuf, int32 bufLen, int32* nVMs)
{
  .vtentry 1:3
  .export [3] as JNI_GetCreatedJavaVMs
  ldarg.0
  ldarg.1
  ldarg.2
  call int32 [IKVM.Runtime.JNI]IKVM.Runtime.JNI::GetCreatedJavaVMs(void*, int32, int32*)
  ret
}
