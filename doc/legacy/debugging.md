# Debugging

In some cases it is needed to debug a Java program that run with IKVM because there there are some things other us with the Sun Java VM. Currently IKVM does not have a Java debug interface. If you want contribute your help is very welcome.

## Run in Eclipse

You can not debug a IKVM application but you can run it with Eclipse. Then you can add debug output to your program with System.out. If you want run a Java application with Eclipse then you need:

- Copy ikvm.exe to java.exe. If you update your IKVM then you should repeat this.
- Create a lib directory parallel to the bin directory.
- Copy a rt.jar from an existing Sun Java VM in the lib directory.
- In the Eclipse "Preference | Java | Installed JREs" add a standard VM. This should be possible now. If not then you need to restart Eclipse. Eclipse check one directory only once.


Change the launch of you your program that it use the IKVM. It can be helpful to copy the launch and use one for the Sun VM and one for IKVM.

## Debugging with Visual Studio

Because IKVM is a .NET application that it is possible to debug it with Visual Studio or Visual Studio Express for C#.

- First you must checkout IKVM from GitHub.
- Then you must build it or copy a compiled version in the bin directory
- In the file source file vfs.cs you need to replace the line:

  ```c#
  return Assembly.GetExecutingAssembly().GetManifestResourceStream("vfs.zip");
  ```

  with:

  ```c#
  return new System.IO.FileStream("c:\\ikvm\\openjdk\\vfs.zip", System.IO.FileMode.Open);
  ```

- Change the path to your `vfs.zip`

An alternative solution is to add `vfs.zip` to the project IKVM.Runtime and set the build property to embedded. Because Visual Studio copy the file in the project root that you need to copy from time to time a new version of vfs.zip in the project root.

If you want debug inside the original Java sources then you need to compile it with debug. This can you do in the build file `response.txt`. Add the flag `-debug` to the block of every dll that you want debug.

## Related

- [Installation](installation.md)