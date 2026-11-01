using System;
using System.Collections;
using System.Collections.Generic;

using IKVM.ByteCode;
using IKVM.CoreLib.Jvm;

namespace IKVM.CoreLib.Linking
{

    /// <summary>
    /// Abstracts a class constant pool. We wrap the class file constants from the underlying decoder so we can track
    /// linking status of them, and so we can support constant pool patching and live objects.
    /// </summary>
    /// <typeparam name="TType"></typeparam>
    /// <typeparam name="TMember"></typeparam>
    /// <typeparam name="TField"></typeparam>
    /// <typeparam name="TMethod"></typeparam>
    /// <typeparam name="TTypeSystem"></typeparam>
    internal struct ConstantPool<TType, TMember, TField, TMethod, TTypeSystem> : IEnumerable<ConstantPoolItem<TType, TMember, TField, TMethod, TTypeSystem>>
        where TType : class
        where TMember : class
        where TField : class, TMember
        where TMethod : class, TMember
        where TTypeSystem : struct, IJvmTypeSystem<TType, TMember, TField, TMethod>
    {

        readonly ClassFile<TType, TMember, TField, TMethod, TTypeSystem> _classFile;
        readonly ConstantPoolItem<TType, TMember, TField, TMethod, TTypeSystem>[] _items;
        readonly string[] _utf8items;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="classFile"></param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ClassFormatException"></exception>
        public ConstantPool(ClassFile<TType, TMember, TField, TMethod, TTypeSystem> classFile)
        {
            _classFile = classFile ?? throw new ArgumentNullException(nameof(classFile));
            _items = new ConstantPoolItem<TType, TMember, TField, TMethod, TTypeSystem>[_classFile.Decoder.Constants.SlotCount];
            _utf8items = new string[_classFile.Decoder.Constants.SlotCount];

            Read();
        }

        /// <summary>
        /// Reads the constants in.
        /// </summary>
        /// <exception cref="ClassFormatException"></exception>
        readonly void Read()
        {
            foreach (var handle in _classFile.Decoder.Constants)
            {
                switch (handle.Kind)
                {
                    case ConstantKind.Class:
                        _items[handle.Slot] = new ConstantPoolItemClass<TType, TMember, TField, TMethod, TTypeSystem>(_classFile, _classFile.Decoder.Constants.Read((ClassConstantHandle)handle));
                        break;
                    case ConstantKind.Double:
                        _items[handle.Slot] = new ConstantPoolItemDouble<TType, TMember, TField, TMethod, TTypeSystem>(_classFile, _classFile.Decoder.Constants.Read((DoubleConstantHandle)handle));
                        break;
                    case ConstantKind.Fieldref:
                        _items[handle.Slot] = new ConstantPoolItemFieldref<TType, TMember, TField, TMethod, TTypeSystem>(_classFile, _classFile.Decoder.Constants.Read((FieldrefConstantHandle)handle));
                        break;
                    case ConstantKind.Float:
                        _items[handle.Slot] = new ConstantPoolItemFloat<TType, TMember, TField, TMethod, TTypeSystem>(_classFile, _classFile.Decoder.Constants.Read((FloatConstantHandle)handle));
                        break;
                    case ConstantKind.Integer:
                        _items[handle.Slot] = new ConstantPoolItemInteger<TType, TMember, TField, TMethod, TTypeSystem>(_classFile, _classFile.Decoder.Constants.Read((IntegerConstantHandle)handle));
                        break;
                    case ConstantKind.InterfaceMethodref:
                        _items[handle.Slot] = new ConstantPoolItemInterfaceMethodref<TType, TMember, TField, TMethod, TTypeSystem>(_classFile, _classFile.Decoder.Constants.Read((InterfaceMethodrefConstantHandle)handle));
                        break;
                    case ConstantKind.Long:
                        _items[handle.Slot] = new ConstantPoolItemLong<TType, TMember, TField, TMethod, TTypeSystem>(_classFile, _classFile.Decoder.Constants.Read((LongConstantHandle)handle));
                        break;
                    case ConstantKind.Methodref:
                        _items[handle.Slot] = new ConstantPoolItemMethodref<TType, TMember, TField, TMethod, TTypeSystem>(_classFile, _classFile.Decoder.Constants.Read((MethodrefConstantHandle)handle));
                        break;
                    case ConstantKind.NameAndType:
                        _items[handle.Slot] = new ConstantPoolItemNameAndType<TType, TMember, TField, TMethod, TTypeSystem>(_classFile, _classFile.Decoder.Constants.Read((NameAndTypeConstantHandle)handle));
                        break;
                    case ConstantKind.MethodHandle:
                        if (_classFile.Decoder.Version < 51)
                            goto default;

                        _items[handle.Slot] = new ConstantPoolItemMethodHandle<TType, TMember, TField, TMethod, TTypeSystem>(_classFile, _classFile.Decoder.Constants.Read((MethodHandleConstantHandle)handle));
                        break;
                    case ConstantKind.MethodType:
                        if (_classFile.Decoder.Version < 51)
                            goto default;

                        _items[handle.Slot] = new ConstantPoolItemMethodType<TType, TMember, TField, TMethod, TTypeSystem>(_classFile, _classFile.Decoder.Constants.Read((MethodTypeConstantHandle)handle));
                        break;
                    case ConstantKind.InvokeDynamic:
                        if (_classFile.Decoder.Version < 51)
                            goto default;

                        _items[handle.Slot] = new ConstantPoolItemInvokeDynamic<TType, TMember, TField, TMethod, TTypeSystem>(_classFile, _classFile.Decoder.Constants.Read((InvokeDynamicConstantHandle)handle));
                        break;
                    case ConstantKind.String:
                        _items[handle.Slot] = new ConstantPoolItemString<TType, TMember, TField, TMethod, TTypeSystem>(_classFile, _classFile.Decoder.Constants.Read((StringConstantHandle)handle));
                        break;
                    case ConstantKind.Utf8:
                        _utf8items[handle.Slot] = _classFile.Decoder.Constants.Read((Utf8ConstantHandle)handle).Value;
                        break;
                    default:
                        throw new ClassFormatException("Unknown constant type.");
                }
            }
        }

        /// <summary>
        /// Applies the given patches to the constants.
        /// </summary>
        /// <param name="inputClassName"></param>
        /// <param name="patches"></param>
        /// <exception cref="ClassFormatException"></exception>
        /// <exception cref="NotImplementedException"></exception>
        public readonly void Patch(string inputClassName, object[] patches)
        {
            for (int i = 1; i < patches.Length; i++)
            {
                if (patches[i] is { } cpp)
                {
                    if (_utf8items[i] != null)
                    {
                        if (cpp is not string stringV)
                            throw new ClassFormatException("Illegal utf8 patch at {0} in class file {1}", i, inputClassName);

                        _utf8items[i] = stringV;
                        continue;
                    }

                    if (_items[i] != null)
                    {
                        switch (_items[i].ConstantType)
                        {
                            case ConstantType.String:
                                _items[i] = new ConstantPoolItemLiveObject<TType, TMember, TField, TMethod, TTypeSystem>(_classFile, cpp);
                                break;
                            case ConstantType.Class:
                                if (cpp is TType clazz)
                                    _items[i] = new ConstantPoolItemClass<TType, TMember, TField, TMethod, TTypeSystem>(_classFile, _classFile.TypeSystem.GetName(clazz), clazz);
                                else if (cpp is string name)
                                    _items[i] = new ConstantPoolItemClass<TType, TMember, TField, TMethod, TTypeSystem>(_classFile, string.Intern(name.Replace('/', '.')), null);
                                else
                                    throw new ClassFormatException("Illegal class patch at {0} in class file {1}", i, inputClassName);

                                break;
                            case ConstantType.Integer:
                                if (cpp is int intV)
                                    ((ConstantPoolItemInteger<TType, TMember, TField, TMethod, TTypeSystem>)_items[i])._value = intV;
                                else
                                    throw new ClassFormatException("Illegal class patch at {0} in class file {1}", i, inputClassName);
                                break;
                            case ConstantType.Long:
                                if (cpp is long longV)
                                    ((ConstantPoolItemLong<TType, TMember, TField, TMethod, TTypeSystem>)_items[i])._value = longV;
                                else
                                    throw new ClassFormatException("Illegal class patch at {0} in class file {1}", i, inputClassName);
                                break;
                            case ConstantType.Float:
                                if (cpp is float floatV)
                                    ((ConstantPoolItemFloat<TType, TMember, TField, TMethod, TTypeSystem>)_items[i])._value = floatV;
                                else
                                    throw new ClassFormatException("Illegal class patch at {0} in class file {1}", i, inputClassName);
                                break;
                            case ConstantType.Double:
                                if (cpp is double doubleV)
                                    ((ConstantPoolItemDouble<TType, TMember, TField, TMethod, TTypeSystem>)_items[i])._value = doubleV;
                                else
                                    throw new ClassFormatException("Illegal class patch at {0} in class file {1}", i, inputClassName);
                                break;
                            default:
                                throw new NotImplementedException("ConstantPoolPatch: " + cpp);
                        }

                        continue;
                    }
                }
            }
        }

        /// <summary>
        /// Initiates a resolve of all of the constants.
        /// </summary>
        /// <exception cref="ClassFormatException"></exception>
        public readonly void Resolve(string inputClassName)
        {
            foreach (var handle in _classFile.Decoder.Constants)
            {
                if (_items[handle.Slot] != null)
                {
                    try
                    {
                        _items[handle.Slot].Resolve();
                    }
                    catch (ClassFormatException e)
                    {
                        // HACK at this point we don't yet have the class name, so any exceptions throw are missing the class name
                        throw new ClassFormatException("{0} ({1})", inputClassName, e.Message);
                    }
                    catch (IndexOutOfRangeException)
                    {
                        throw new ClassFormatException("{0} (Invalid constant pool item #{1})", inputClassName, handle.Slot);
                    }
                    catch (InvalidCastException)
                    {
                        throw new ClassFormatException("{0} (Invalid constant pool item #{1})", inputClassName, handle.Slot);
                    }
                }
            }
        }

        /// <summary>
        /// Gets the class file that owns this constant pool.
        /// </summary>
        public readonly ClassFile<TType, TMember, TField, TMethod, TTypeSystem> ClassFile => _classFile;

        /// <summary>
        /// Gets the total count of constants.
        /// </summary>
        public readonly int Count => _classFile.Decoder.Constants.Count;

        /// <summary>
        /// Gets the total count of constant slots.
        /// </summary>
        public readonly int SlotCount => _classFile.Decoder.Constants.SlotCount;

        /// <summary>
        /// Gets the constant pool item associated with the given handle.
        /// </summary>
        /// <param name="handle"></param>
        /// <returns></returns>
        public readonly ConstantPoolItem<TType, TMember, TField, TMethod, TTypeSystem> this[ConstantHandle handle] => _items[handle.Slot];

        /// <summary>
        /// Gets the UTF8 constant value.
        /// </summary>
        /// <param name="handle"></param>
        /// <returns></returns>
        public readonly string GetUtf8(Utf8ConstantHandle handle) => _utf8items[handle.Slot];

        /// <summary>
        /// Creates explicit constant pool items for cached UTF8 items.
        /// </summary>
        internal readonly void CreateUtf8ConstantPoolItems()
        {
            foreach (var handle in _classFile.Decoder.Constants)
                if (_items[handle.Slot] == null && _utf8items[handle.Slot] != null)
                    _items[handle.Slot] = new ConstantPoolItemUtf8<TType, TMember, TField, TMethod, TTypeSystem>(_classFile, _utf8items[handle.Slot]);
        }

        /// <summary>
        /// Gets an enumerator over the constant items.
        /// </summary>
        /// <returns></returns>
        public readonly IEnumerator<ConstantPoolItem<TType, TMember, TField, TMethod, TTypeSystem>> GetEnumerator()
        {
            foreach (var handle in _classFile.Decoder.Constants)
                yield return this[handle];
        }

        /// <inheritdoc />
        readonly IEnumerator IEnumerable.GetEnumerator()
        {
            return _items.GetEnumerator();
        }

    }

}
