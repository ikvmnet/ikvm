using System.Buffers;

using IKVM.ByteCode.Buffers;

namespace IKVM.ByteCode.Parsing
{

    public sealed record ModuleAttributeRecord(ushort NameIndex, ModuleFlag Flags, ushort VersionIndex, ModuleAttributeRequiresRecord[] Requires, ModuleAttributeExportsRecord[] Exports, ModuleAttributeOpensRecord[] Opens, ushort[] Uses, ModuleAttributeProvidesRecord[] Provides) : AttributeRecord
    {

        public static bool TryReadModuleAttribute(ref SequenceReader<byte> reader, out AttributeRecord attribute)
        {
            attribute = null;

            if (reader.TryReadBigEndian(out ushort moduleNameIndex) == false)
                return false;
            if (reader.TryReadBigEndian(out ushort moduleFlags) == false)
                return false;
            if (reader.TryReadBigEndian(out ushort moduleVersionIndex) == false)
                return false;

            if (reader.TryReadBigEndian(out ushort requiresCount) == false)
                return false;

            var requires = new ModuleAttributeRequiresRecord[requiresCount];
            for (int i = 0; i < requiresCount; i++)
            {
                if (reader.TryReadBigEndian(out ushort requiresIndex) == false)
                    return false;
                if (reader.TryReadBigEndian(out ushort requiresFlags) == false)
                    return false;
                if (reader.TryReadBigEndian(out ushort requiresVersionIndex) == false)
                    return false;

                requires[i] = new ModuleAttributeRequiresRecord(requiresIndex, (ModuleRequiresFlag)requiresFlags, requiresVersionIndex);
            }

            if (reader.TryReadBigEndian(out ushort exportsCount) == false)
                return false;

            var exports = new ModuleAttributeExportsRecord[exportsCount];
            for (int i = 0; i < exportsCount; i++)
            {
                if (reader.TryReadBigEndian(out ushort exportsIndex) == false)
                    return false;
                if (reader.TryReadBigEndian(out ushort exportsFlags) == false)
                    return false;

                if (reader.TryReadBigEndian(out ushort exportsModuleCount) == false)
                    return false;

                var exportsModules = new ushort[exportsModuleCount];
                for (int j = 0; j < exportsModuleCount; j++)
                {
                    if (reader.TryReadBigEndian(out ushort exportsToModuleIndex) == false)
                        return false;

                    exportsModules[j] = exportsToModuleIndex;
                }

                exports[i] = new ModuleAttributeExportsRecord(exportsIndex, (ModuleExportsFlag)exportsFlags, exportsModules);
            }

            if (reader.TryReadBigEndian(out ushort opensCount) == false)
                return false;

            var opens = new ModuleAttributeOpensRecord[opensCount];
            for (int i = 0; i < opensCount; i++)
            {
                if (reader.TryReadBigEndian(out ushort opensIndex) == false)
                    return false;
                if (reader.TryReadBigEndian(out ushort opensFlags) == false)
                    return false;

                if (reader.TryReadBigEndian(out ushort opensModulesCount) == false)
                    return false;

                var opensModules = new ushort[opensModulesCount];
                for (int j = 0; j < opensModulesCount; j++)
                {
                    if (reader.TryReadBigEndian(out ushort opensModuleIndex) == false)
                        return false;

                    opensModules[j] = opensModuleIndex;
                }

                opens[i] = new ModuleAttributeOpensRecord(opensIndex, (ModuleOpensFlag)opensFlags, opensModules);
            }

            if (reader.TryReadBigEndian(out ushort usesCount) == false)
                return false;

            var uses = new ushort[usesCount];
            for (int i = 0; i < usesCount; i++)
            {
                if (reader.TryReadBigEndian(out ushort usesIndex) == false)
                    return false;

                uses[i] = usesIndex;
            }

            if (reader.TryReadBigEndian(out ushort providesCount) == false)
                return false;

            var provides = new ModuleAttributeProvidesRecord[providesCount];
            for (int i = 0; i < providesCount; i++)
            {
                if (reader.TryReadBigEndian(out ushort providesIndex) == false)
                    return false;

                if (reader.TryReadBigEndian(out ushort providesModulesCount) == false)
                    return false;

                var providesModules = new ushort[providesModulesCount];
                for (int j = 0; j < providesModulesCount; j++)
                {
                    if (reader.TryReadBigEndian(out ushort providesModuleIndex) == false)
                        return false;

                    providesModules[j] = providesModuleIndex;
                }

                provides[i] = new ModuleAttributeProvidesRecord(providesIndex, providesModules);
            }

            attribute = new ModuleAttributeRecord(moduleNameIndex, (ModuleFlag)moduleFlags, moduleVersionIndex, requires, exports, opens, uses, provides);
            return true;
        }

    }

}
