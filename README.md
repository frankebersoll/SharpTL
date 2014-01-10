# SharpTL

Portable class library allows to serialize/deserialize objects defined in [Type Language](http://core.telegram.org/mtproto/TL) schemas.

Binaries are available via NuGet package:

    PM> Install-Package SharpTL

## Usage ##

Declare a class with a **TLObject** attribute with unique number for an objects schema. Properties that should be serialized are marked with a **TLProperty** attribute with order number.

    [TLObject(0xA1B2C3D4)]
    public class TestObject
    {
        [TLProperty(1)]
        public bool TestBoolean { get; set; }

        [TLProperty(2)]
        public double TestDouble { get; set; }

        [TLProperty(3)]
        public int TestInt { get; set; }

        [TLProperty(4)]
        public List<int> TestIntVector { get; set; }

        [TLProperty(5)]
        public long TestLong { get; set; }

        [TLProperty(6)]
        public string TestString { get; set; }

        [TLProperty(7)]
        public List<IUser> TestUsersVector { get; set; }
    }

The example **IUser** interface must be marked by a **TLType** attribute with types of derived classes which also must be marked by a **TLObject** attribute.

    [TLType(typeof (User), typeof (NoUser))]
    public interface IUser
    {
        int Id { get; set; }
    }

    [TLObject(0xD23C81A3)]
    public class User : IUser
    {
        [TLProperty(1)]
        public int Id { get; set; }

        [TLProperty(2)]
        public string FirstName { get; set; }

        [TLProperty(3)]
        public string LastName { get; set; }

        [TLProperty(4)]
        public byte[] Key { get; set; }
    }

    [TLObject(0xC67599D1)]
    public class NoUser : IUser
    {
        [TLProperty(1)]
        public int Id { get; set; }
    }

Serializing:

    var obj = new TestObject();
	// obj properties initializing.
    byte[] objBytes = TLRig.Default.Serialize(obj);

Deserializing:

    byte[] objBytes;
    // ...
    var obj = TLRig.Default.Deserialize<TestObject>(objBytes);
    

## TL-schema compiler ##
It is possible to automatically convert a TL-schema (json/tl) to C# object model using the **SharpTL.Compiler.CLI** tool.

Usage example:

    SharpTL.Compiler.CLI.exe compile json MTProto.json MTProto.cs MTProtoSchema