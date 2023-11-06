# Using Java with .NET

Most Java classes can be accessed directly. There is no package prefix. The article [C# from a Java Developer's Perspective](http://www.25hoursaday.com/CsharpVsJava.html) can be helpful.

Some Java syntax is not compatible and requires some hacks and has some specific notation.

```c#
class operator
Class clazz = com.mypack.MyClass.class;
Class clazz = typeof(com.mypack.MyClass);

// enum

MyEnum value;
switch(value){
    case VALUE1:
    ...

MyEnum value;
switch(value.ordinal()){
    case (int)MyEnum.__Enum.VALUE1:
    ...
```

## Related

- [Installation](installation.md)
- [Using .NET with Java](using-dotnet-with-java.md)

