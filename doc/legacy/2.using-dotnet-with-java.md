# Using .NET With Java

```c#
typeof operator
System.Type type = typeof(com.mypack.MyClass);
cli.System.Type type = ikvm.runtime.Util.getInstanceTypeFromClass(com.mypack.MyClass.class);

enum value
MyEnum.Value1
MyEnum.wrap( MyEnum.Value1 )

bool
bool a = true;

if ( a ) {

cli.System.Boolean a = kvm.lang.CIL.box_boolean(true); // add ikvm-api.jar to your classpath

if ( kvm.lang.CIL.unbox_boolean( a ) {

// property
obj.PropertyName
obj.get_PropertyName()

// events
obj.SomeChange += new MySomeChange( AnyMethod );

 obj.add_SomeChange(
   new cli.System.EventHandler (
     new cli.System.EventHandler.Method () {
       public void Invoke (Object sender, cli.System.EventArgs e) {
         ...
       }
     }
   )
 );

// annotation
[System.ThreadStaticAttribute]
@cli.System.ThreadStaticAttribute.Annotation
```

## Attributes as Annotations

With some limitations you can use .NET attributes as explained in Attribute Annotations post.

## Related

- [Using Java with .NET](using-java-with-dotnet.md)