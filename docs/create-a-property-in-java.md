# Create a Property in Java

Java does not support the concept of properties as a code feature like C#. If you write a .NET library than it can be nice to have properties in your public API.

## Properties via map.xml


At compile time you can use a `map.xml` file to modify the compiled result. The follow sample demonstrate the use of `map.xml` to define a property.

```c#
public class IntList{
 private int[] list;
 public IntList(int size){
   list = new int[size];
 }
 public void setItem(int index, int value){
   list[index] = value;
 }
 public int getItem(int index){
   return list[index];
 }
}
```

### map.xml

```xml
<root>
  <assembly>
    <class name="IntList">
      <attribute type="System.Reflection.DefaultMemberAttribute, mscorlib" sig="(Ljava.lang.String;)V">
        <parameter>Item</parameter>
      </attribute>
      <property name="Item" sig="(I)I">
        <getter name="getItem" sig="(I)I" />
        <setter name="setItem" sig="(II)V" />
      </property>
    </class>
  </assembly>
</root>
```

When `IntList.java` is compiled to `IntList.class` and then IKVM compiled using:

```console
  ikvmc IntList.class -remap:map.xml
```

The resulting `IntList.dll` will be usable from C# like this:

```c#
  IntList l = new IntList(10);
  l[4] = 42;
  Console.WriteLine(l[4]);
```

Valdemar Mejstad created a tool to automatically generate the XML to define properties based on Java's java.beans.BeanInfo. The source is available here: [MapFileGenerator.java](https://web.archive.org/web/20161027171952/http://www.frijters.net/MapFileGenerator.java).

## Properties via Annotation

If you want create a property for a class member then you can use the annotation.

```c#
@ikvm.lang.Property(get = "get_Handle")
private long Handle;
```
