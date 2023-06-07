namespace IKVM.ByteCode.Parsing
{

    public sealed record ModuleAttributeRecord(ushort NameIndex, ModuleFlag Flags, ushort VersionIndex, ModuleAttributeRequiresRecord[] Requires, ModuleAttributeExportsRecord[] Exports, ModuleAttributeOpensRecord[] Opens, ushort[] Uses, ModuleAttributeProvidesRecord[] Provides) : AttributeRecord
    {

        public static bool TryReadModuleAttribute(ref ClassFormatReader reader, out AttributeRecord attribute)
        {
            attribute = null;

            if (reader.TryReadU2(out ushort moduleNameIndex) == false)
                return false;
            if (reader.TryReadU2(out ushort moduleFlags) == false)
                return false;
            if (reader.TryReadU2(out ushort moduleVersionIndex) == false)
                return false;

            if (reader.TryReadU2(out ushort requiresCount) == false)
                return false;

            var requires = new ModuleAttributeRequiresRecord[requiresCount];
            for (int i = 0; i < requiresCount; i++)
            {
                if (reader.TryReadU2(out ushort requiresIndex) == false)
                    return false;
                if (reader.TryReadU2(out ushort requiresFlags) == false)
                    return false;
                if (reader.TryReadU2(out ushort requiresVersionIndex) == false)
                    return false;

                requires[i] = new ModuleAttributeRequiresRecord(requiresIndex, (ModuleRequiresFlag)requiresFlags, requiresVersionIndex);
            }

            if (reader.TryReadU2(out ushort exportsCount) == false)
                return false;

            var exports = new ModuleAttributeExportsRecord[exportsCount];
            for (int i = 0; i < exportsCount; i++)
            {
                if (reader.TryReadU2(out ushort exportsIndex) == false)
                    return false;
                if (reader.TryReadU2(out ushort exportsFlags) == false)
                    return false;

                if (reader.TryReadU2(out ushort exportsModuleCount) == false)
                    return false;

                var exportsModules = new ushort[exportsModuleCount];
                for (int j = 0; j < exportsModuleCount; j++)
                {
                    if (reader.TryReadU2(out ushort exportsToModuleIndex) == false)
                        return false;

                    exportsModules[j] = exportsToModuleIndex;
                }

                exports[i] = new ModuleAttributeExportsRecord(exportsIndex, (ModuleExportsFlag)exportsFlags, exportsModules);
            }

            if (reader.TryReadU2(out ushort opensCount) == false)
                return false;

            var opens = new ModuleAttributeOpensRecord[opensCount];
            for (int i = 0; i < opensCount; i++)
            {
                if (reader.TryReadU2(out ushort opensIndex) == false)
                    return false;
                if (reader.TryReadU2(out ushort opensFlags) == false)
                    return false;

                if (reader.TryReadU2(out ushort opensModulesCount) == false)
                    return false;

                var opensModules = new ushort[opensModulesCount];
                for (int j = 0; j < opensModulesCount; j++)
                {
                    if (reader.TryReadU2(out ushort opensModuleIndex) == false)
                        return false;

                    opensModules[j] = opensModuleIndex;
                }

                opens[i] = new ModuleAttributeOpensRecord(opensIndex, (ModuleOpensFlag)opensFlags, opensModules);
            }

            if (reader.TryReadU2(out ushort usesCount) == false)
                return false;

            var uses = new ushort[usesCount];
            for (int i = 0; i < usesCount; i++)
            {
                if (reader.TryReadU2(out ushort usesIndex) == false)
                    return false;

                uses[i] = usesIndex;
            }

            if (reader.TryReadU2(out ushort providesCount) == false)
                return false;

            var provides = new ModuleAttributeProvidesRecord[providesCount];
            for (int i = 0; i < providesCount; i++)
            {
                if (reader.TryReadU2(out ushort providesIndex) == false)
                    return false;

                if (reader.TryReadU2(out ushort providesModulesCount) == false)
                    return false;

                var providesModules = new ushort[providesModulesCount];
                for (int j = 0; j < providesModulesCount; j++)
                {
                    if (reader.TryReadU2(out ushort providesModuleIndex) == false)
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
